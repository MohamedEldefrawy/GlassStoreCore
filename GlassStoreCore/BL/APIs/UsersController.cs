using System;
using System.Linq;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ObjectMapper _mapper = new ObjectMapper();
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public ActionResult<ApplicationUser> GetUsers()
        {
            var users = _usersService.GetAllUsers().Result;

            if (users == null)
            {
                return BadRequest();
            }

            var usersDtos = users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>).ToList();

            //foreach (var user in usersDtos)
            //{
            //    foreach (var role in user.Roles)
            //    {
            //        role.RoleName = _glassStoreContext.Roles.SingleOrDefault(r => r.Id == role.RoleId)?.Name;
            //    }
            //}

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

            var userDto = _mapper.Mapper.Map<ApplicationUser, UserDto>(user);

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

            _usersService.DeleteUser(user);
            _usersService.Dispose();

            return Ok("User Has been deleted successfully");
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
                _usersService.Dispose();
                return Ok();
            }

            else
            {
                foreach (var error in result.Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                _usersService.Dispose();
                return BadRequest("Please Enter valid data");
            }
        }
    }
}
