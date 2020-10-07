using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.UriService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ObjectMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, IUriService uriService, ObjectMapper mapper)
        {
            _unitOfWork = unitOfWork;
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
                var userRoles = _unitOfWork.UsersRolesService.GetUserRoles(user.Id).Result;
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

            var userRoles = _unitOfWork.UsersRolesService.GetUserRoles(id).Result;
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
            var result = _unitOfWork.Complete().Result;

            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

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
                _unitOfWork.UsersRolesService.Add(new IdentityUserRole<string>()
                {
                    UserId = role.UserId,
                    RoleId = role.RoleId
                });
                _unitOfWork.Complete();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationUser> UpdateUser(UserDto userDto, string id)
        {
            _unitOfWork.UsersService.Update(userDto, id);
            _unitOfWork.Complete();
            return Ok();
        }
    }
}
