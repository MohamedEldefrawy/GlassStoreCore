using System;
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
            var userRoles = _usersRolesService.GetAllUsersRoles().Result;
            if (userRoles == null)
            {
                return NotFound();
            }
            var userRolesDto = userRoles.Select(_mapper.Mapper.Map<IdentityUserRole<string>, UserRoleDto>);


            return Ok(userRolesDto);
        }

        [HttpGet("({userId}:{roleId})")]
        public ActionResult<IdentityUserRole<string>> GetUserRoles(string userId, string roleId)
        {
            var userRoles = _usersRolesService.GetUserRole(userId, roleId);

            if (userRoles == null)
            {
                return NotFound("Please Enter a valid userId and roleId");
            }

            return Ok(userRoles);
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

        [HttpPut("({roleId}:{userId})")]
        public ActionResult<IdentityUserRole<string>> UpdateUserRole(UserRoleDto userRoleDto, string roleId, string userId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var selectedUserRole = _usersRolesService.GetUserRole(roleId, userId);

            if (selectedUserRole == null)
            {
                return NotFound("Please Enter a valid userId and roleId");
            }

            var userRole = _mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(userRoleDto);
            _usersRolesService.UpdateUserRole(userRole, userId, roleId);

            return Ok();
        }

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
