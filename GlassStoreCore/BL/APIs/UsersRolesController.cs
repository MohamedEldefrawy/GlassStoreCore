using System.Linq;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.JsonResponses;
using GlassStoreCore.Services.PaginationUowService;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly IRolesService _rolesService;


        public UsersRolesController(IRolesService rolesService, IUsersService usersService)
        {
            _userService = usersService;
            _rolesService = rolesService;
        }

        [HttpGet("{userId}")]
        public ActionResult<ApplicationUserRole> GetUserRoles(string userId)
        {
            var selectedUser = _userService.FindById(userId);

            if (selectedUser == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user's role not found.",
                    Success = false
                });
            }
            var selectedUserRoles = _userService.GetUserRoles(selectedUser).Result.ToList();

            return Ok(new GetJsonResponse
            {
                StatusMessage = "USer role has been selected successfully.",
                Success = true,
                Data = selectedUserRoles
            });
        }

        [HttpPost("{userId}")]
        public ActionResult<ApplicationUserRole> CreateUserRole(UserRoleDto userRoleDto, string userId)
        {
            var selectedUser = _userService.FindById(userId);
            var selectedRole = _rolesService.GetAll(r => r.Name.ToUpper() == userRoleDto.RoleName.Name.ToUpper()).SingleOrDefault();

            if (selectedUser == null)
            {
                return NotFound(new OtherJsonResponse { StatusMessage = "Coldn't find selected user", Success = false });
            }

            if (selectedRole == null)
            {
                return NotFound(new OtherJsonResponse { StatusMessage = string.Format("Coldn't find Role {0}", userRoleDto.RoleName), Success = false });

            }

            var result = _userService.AssignUserRole(selectedUser.Id, userRoleDto.RoleName);



            if (!result.Result.Succeeded)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't assign user with selected role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Role assigned to selected user successfully.",
                Success = true
            });
        }


        [HttpDelete("{id}")]
        public ActionResult<ApplicationUserRole> DeleteUserRole(UserRoleDto userRoleDto, string userId)
        {
            var selectedUser = _userService.FindById(userId);
            var selectedRole = _rolesService.GetAll(r => r.Name.ToUpper() == userRoleDto.RoleName.Name.ToUpper()).SingleOrDefault();

            if (selectedUser == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't find selected user.",
                    Success = false
                });
            }

            if (selectedRole == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = string.Format("Coldn't find Role {0}", userRoleDto.RoleName),
                    Success = false
                });

            }


            var result = _userService.UnAssignUserRole(selectedUser.Id, userRoleDto.RoleName);

            if (!result.Result.Succeeded)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Unassign selected role from  user.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "selected role has been removed from selected user successfully.",
                Success = true
            });
        }
    }
}
