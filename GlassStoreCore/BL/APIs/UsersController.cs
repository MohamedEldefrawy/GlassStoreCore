using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ObjectMapper _mapper = new ObjectMapper();
        private readonly IUsersService _usersService;
        private readonly IUsersRolesService _usersRolesService;

        public UsersController(IUsersService usersService, IUsersRolesService usersRolesService)
        {
            _usersService = usersService;
            _usersRolesService = usersRolesService;
        }

        public ActionResult<ApplicationUser> GetUsers()
        {
            var users = _usersService.GetAllUsers().Result;

            if (users == null)
            {
                return BadRequest();
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
            return Ok(users);
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

            if (result.Result.Succeeded)
            {
                return Ok();

            }

            return BadRequest("Something wrong");
        }
    }
}
