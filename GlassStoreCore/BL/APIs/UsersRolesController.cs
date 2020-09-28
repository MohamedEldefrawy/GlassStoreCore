﻿using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Services.RolesService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUsersRolesService _usersRolesService;

        public UsersRolesController(IUsersRolesService usersRolesService)
        {
            _usersRolesService = usersRolesService;
        }

        [HttpGet]
        public ActionResult<IdentityUserRole<string>> GetUsersRoles()
        {
            var userRoles = _usersRolesService.GetAllUsersRoles().Result;
            if (userRoles == null)
            {
                return NotFound();
            }

            return Ok(userRoles);
        }

        [HttpGet("{userId}")]
        public ActionResult<IdentityUserRole<string>> GetUserRoles(string userId)
        {
            var selectedUserRoles = _usersRolesService.GetUserRoles(userId).Result;

            if (selectedUserRoles == null)
            {
                return NotFound("Please enter a valid id");
            }

            return Ok(selectedUserRoles);
        }

        [HttpPost]
        public ActionResult<IdentityUserRole<string>> CreateUserRole(UserRoleDto userRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = _usersRolesService.AddUserRole(userRoleDto).Result;

            if (result == 0)
            {
                return BadRequest("something wrong");
            }

            return Ok();
        }


        [HttpDelete("{userId}/{roleId}")]
        public ActionResult<IdentityUserRole<string>> DeleteUserRole(string userId, string roleId)
        {
            var selectedUserRoles = _usersRolesService.GetUserRoles(userId).Result;
            if (selectedUserRoles == null)
            {
                return NotFound("Please Enter a valid userId and roleId");
            }
            var result = _usersRolesService.DeleteUserRole(userId, roleId).Result;
            if (result == 0)
            {
                return BadRequest("something wrong");
            }
            return Ok();
        }

   }
}
