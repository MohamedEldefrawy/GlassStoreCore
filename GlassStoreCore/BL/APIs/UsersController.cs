using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UriService;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRolesService _usersRolesService;
        private readonly ObjectMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, IUsersRolesService usersRolesService, IUriService uriService, ObjectMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _usersRolesService = usersRolesService;
            _uriService = uriService;
            _mapper = mapper;
        }

        public ActionResult<ApplicationUser> GetUsers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (users, totalRecords) = _unitOfWork.UsersService.GetAll(filter.PageNumber, filter.PageSize).Result;

            var usersDtos = users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>).ToList();

            foreach (var user in usersDtos)
            {
                var userRoles = _usersRolesService.GetUserRoles(user.Id).Result;
                user.Roles = new List<UserRoleDto>();

                foreach (var role in userRoles)
                {
                    user.Roles.Add(new UserRoleDto
                    {
                        RoleId = role.RoleId,
                        UserId = role.UserId
                    });
                }
            }

            var pageResponse = PaginationHelper.CreatePagedResponse(usersDtos, filter, totalRecords, _uriService, route);
            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationUser> GetUser(string id)
        {
            var user = _unitOfWork.UsersService.Get(id).Result;

            if (user == null)
            {
                return BadRequest("User not found please use valid id");
            }

            var userRoles = _usersRolesService.GetUserRoles(id).Result;
            var userDto = _mapper.Mapper.Map<ApplicationUser, UserDto>(user);

            userDto.Roles = new List<UserRoleDto>();
            foreach (var role in userRoles)
            {

                userDto.Roles.Add(new UserRoleDto
                {
                    UserId = role.UserId,
                    RoleId = role.RoleId
                });
            }

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationUser> DeleteUser(string id)
        {
            if (_unitOfWork.UsersService.Get(id).Result == null)
            {
                return NotFound("Please select a valid user");
            }

            _unitOfWork.UsersService.Delete(id);

            return Ok("Done");
        }

        [HttpPost]
        public ActionResult<ApplicationUser> CreateUser(CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = _unitOfWork.UsersService.CreateUser(userDto);

            if (result.Result == null)
                return BadRequest("Please Enter valid data");

            foreach (var role in userDto.Roles)
            {
                role.UserId = result.Result.Id;
                _usersRolesService.AddUserRole(role);
            }

            return Ok();

        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationUser> UpdateUser(UserDto userDto, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _mapper.Mapper.Map<UserDto, ApplicationUser>(userDto);
            user.Id = id;
            _unitOfWork.UsersService.Update(user);
            var updateUserRoles = userDto.Roles;

            foreach (var role in updateUserRoles)
            {
                _usersRolesService.AddUserRole(role);
            }

            var userRole = _usersRolesService.GetUserRoles(id).Result;

            foreach (var role in userRole)
            {
                _usersRolesService.DeleteUserRole(role.UserId, role.RoleId);
            }

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
