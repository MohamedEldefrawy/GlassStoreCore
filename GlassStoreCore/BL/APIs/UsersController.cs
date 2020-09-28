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
        private readonly IRolesService _rolesService;
        private readonly IUsersRolesService _usersRolesService;
        private readonly GlassStoreContext _glassStoreContext;

        public UsersController(IUsersService usersService, IRolesService rolesService, IUsersRolesService usersRolesService, GlassStoreContext glassStoreContext)
        {
            _usersService = usersService;
            _rolesService = rolesService;
            _usersRolesService = usersRolesService;
            _glassStoreContext = glassStoreContext;
        }

        public ActionResult<ApplicationUser> GetUsers()
        {
            var users = _usersService.GetAllUsers().Result;

            if (users == null)
            {
                return BadRequest();
            }

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

            return Ok(usersDtos);
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
            var user = _usersService.GetUser(id).Result;

            if (user == null)
            {
                return BadRequest("Please Select a Valid user id");

            }

            var result = _usersService.DeleteUser(user);
            if (result.Result.Succeeded)
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

            var newUser = _mapper.Mapper.Map<CreateUserDto, ApplicationUser>(userDto);
            var result = _usersService.AddUser(newUser, userDto.Password);

            if (result.Result.Succeeded)
            {
                foreach (var role in userDto.Roles)
                {
                    role.UserId = newUser.Id;
                    _usersRolesService.AddUserRole(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(role));
                }
                return Ok();
            }

            foreach (var error in result.Result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest("Please Enter valid data");
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
