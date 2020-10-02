using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using GlassStoreCore.Data.Response;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UriService;
using GlassStoreCore.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IUsersRolesService _usersRolesService;
        private readonly IUriService _uriService;

        public UsersController(IUsersService usersService, IUsersRolesService usersRolesService, IUriService uriService)
        {
            _usersService = usersService;
            _usersRolesService = usersRolesService;
            _uriService = uriService;
        }

        public ActionResult<ApplicationUser> GetUsers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (users, totalRecords) = _usersService.GetAllUsers(filter.PageNumber, filter.PageSize).Result;

            if (users == null)
            {
                return BadRequest(new Response<List<UserDto>>(users));
            }

            foreach (var user in users)
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

            var pageResponse = PaginationHelper.CreatePagedResponse(users, filter, totalRecords, _uriService, route);
            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationUser> GetUser(string id)
        {
            var user = _usersService.GetUser(id).Result;

            if (user == null)
            {
                return BadRequest("User not found please use valid id");
            }

            var userRoles = _usersRolesService.GetUserRoles(id).Result;


            user.Roles = new List<UserRoleDto>();
            foreach (var role in userRoles)
            {

                user.Roles.Add(new UserRoleDto
                {
                    UserId = role.UserId,
                    RoleId = role.RoleId
                });
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationUser> DeleteUser(string id)
        {
            var result = _usersService.DeleteUser(id).Result;
            if (result.Succeeded)
            {
                return Ok("User Has been deleted successfully");

            }

            return BadRequest("Something wrong");
        }

        [HttpPost]
        public ActionResult<ApplicationUser> CreateUser(CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = _usersService.AddUser(userDto, userDto.Password);

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
            var result = _usersService.UpdateUser(userDto, id);
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

            if (result.Result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();

        }
    }
}
