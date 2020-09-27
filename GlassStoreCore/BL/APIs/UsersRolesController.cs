﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Services.RolesService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUsersRolesService _usersRolesService;
        private readonly IRolesService _rolesService;
        private readonly ObjectMapper _mapper = new ObjectMapper();

        public UsersRolesController(IUsersRolesService usersRolesService, IRolesService rolesService)
        {
            _usersRolesService = usersRolesService;
            _rolesService = rolesService;
        }

        [HttpGet]
        public ActionResult<IdentityUserRole<string>> GetUsersRoles()
        {
            var userRoles = _usersRolesService.GetAllUsersRoles();
            if (userRoles == null)
            {
                return NotFound();
            }
            var userRolesDto = userRoles.Select(_mapper.Mapper.Map<IdentityUserRole<string>, UserRoleDto>);


            return Ok(userRolesDto);
        }

        [HttpGet("{userId}")]
        public ActionResult<IdentityUserRole<string>> GetUserRoles(string userId)
        {
            var selectedUserRoles = _usersRolesService.GetAllUsersRoles()
                                                      .Where(u => u.UserId == userId).ToList();

            var userRolesDto = selectedUserRoles.Select(_mapper.Mapper.Map<IdentityUserRole<string>, UserRoleDto>);

            return Ok(userRolesDto);
        }

        [HttpPost]
        public ActionResult<IdentityUserRole<string>> CreateUserRole(UserRoleDto userRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userRole = _mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(userRoleDto);
            _usersRolesService.AddUserRole(userRole);
            return Ok();
        }


        [HttpDelete("{userId}/{roleId}")]
        public ActionResult<IdentityUserRole<string>> DeleteUserRole(string userId, string roleId)
        {
            var selectedUser = _usersRolesService.GetUserRole(userId, roleId);

            if (selectedUser == null)
            {
                return NotFound("Please Enter a valid userId and roleId");
            }
            _usersRolesService.DeleteUserRole(selectedUser);

            return Ok();
        }
    }
}
