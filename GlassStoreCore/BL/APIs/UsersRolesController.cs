using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services;
using GlassStoreCore.Services.PaginationUowService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersRolesController : ControllerBase
    {
        private readonly IPaginationUow _paginationUow;
        private readonly ObjectMapper _mapper;
        private readonly IService<IdentityUserRole<string>> _usersRolesService;


        public UsersRolesController(IPaginationUow paginationUow, ObjectMapper mapper)
        {
            _paginationUow = paginationUow;
            _mapper = mapper;
            _usersRolesService = _paginationUow.Service<IdentityUserRole<string>>();
        }

        [HttpGet]
        public ActionResult<IdentityUserRole<string>> GetUsersRoles([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (userRoles, totalRecords) = _usersRolesService.GetAllAsync(filter.PageNumber, filter.PageSize).Result;

            if (userRoles == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected user's role not found."
                });
            }

            var pageResponse = PaginationHelper.CreatePagedResponse(userRoles, filter, totalRecords, _paginationUow, route);
            pageResponse.Message = "Request has been completed successfully.";
            return Ok(pageResponse);
        }

        [HttpGet("{userId}")]
        public ActionResult<IdentityUserRole<string>> GetUserRoles(string userId)
        {
            var selectedUserRoles = _usersRolesService.GetAllAsync(u => u.UserId == userId).Result;

            if (selectedUserRoles == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected user's role not found."
                });
            }
            return Ok(selectedUserRoles);
        }

        [HttpPost]
        public ActionResult<IdentityUserRole<string>> CreateUserRole(UserRoleDto userRoleDto)
        {
            var result = _usersRolesService
                                  .AddAsync(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(userRoleDto));

            if (result.Result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't create user's role."
                });
            }

            return Ok();
        }

        [HttpPut("{userId}")]
        public ActionResult<IdentityUserRole<string>> UpdateUserRoles(List<UserRoleDto> userRolesDto, string userId)
        {
            var userRole = _usersRolesService.GetAllAsync(u => u.UserId == userId).Result;
            foreach (var role in userRole)
            {
                _usersRolesService.DeleteAsync(role);
            }

            var result = 0;
            foreach (var role in userRolesDto)
            {
                result = _usersRolesService.AddAsync(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(role)).Result;
            }

            if (result == 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't update selected user's role."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "User's role has been updated successfully."
            });
        }

        [HttpDelete("{userId}/{roleId}")]
        public ActionResult<IdentityUserRole<string>> DeleteUserRole(string userId, string roleId)
        {
            var selectedUserRoles = _usersRolesService.GetAllAsync(u => u.UserId == userId && u.RoleId == roleId).Result.SingleOrDefault();

            if (selectedUserRoles == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected user's role not found."
                });
            }

            var result = _usersRolesService.DeleteAsync(selectedUserRoles).Result;

            if (result == 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't Delete selected user's role."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 404,
                StatusMessage = "Selected user's role has been deleted successfully."
            });
        }

    }
}
