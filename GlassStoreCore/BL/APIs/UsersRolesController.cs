using System.Collections.Generic;
using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;
using GlassStoreCore.Helpers;
using GlassStoreCore.JsonResponses;
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
            var (userRoles, totalRecords) = _usersRolesService.GetAll(filter.PageNumber, filter.PageSize);

            if (userRoles == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user's role not found.",
                    Success = false
                });
            }

            var pageResponse = PaginationHelper.CreatePagedResponse(userRoles, filter, totalRecords, _paginationUow, route);
            pageResponse.Message = "Request has been completed successfully.";
            pageResponse.Succeeded = true;
            return Ok(pageResponse);
        }

        [HttpGet("{userId}")]
        public ActionResult<IdentityUserRole<string>> GetUserRoles(string userId)
        {
            var selectedUserRoles = _usersRolesService.GetAll(u => u.UserId == userId);

            if (selectedUserRoles == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user's role not found.",
                    Success = false
                });
            }
            return Ok(new GetJsonResponse
            {
                StatusMessage = "USer role has been selected successfully.",
                Success = true,
                Data = selectedUserRoles
            });
        }

        [HttpPost]
        public ActionResult<IdentityUserRole<string>> CreateUserRole(UserRoleDto userRoleDto)
        {
            var result = _usersRolesService
                                  .Add(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(userRoleDto));

            if (result <= 0)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't create user's role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "User's Role has been created successfully.",
                Success = true
            });
        }

        [HttpPut("{userId}")]
        public ActionResult<IdentityUserRole<string>> UpdateUserRoles(List<UserRoleDto> userRolesDto, string userId)
        {
            var userRole = _usersRolesService.GetAll(u => u.UserId == userId);
            foreach (var role in userRole)
            {
                _usersRolesService.Delete(role);
            }

            var result = 0;
            foreach (var role in userRolesDto)
            {
                result = _usersRolesService.Add(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(role));
            }

            if (result == 0)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't update selected user's role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "User's role has been updated successfully.",
                Success = true
            });
        }

        [HttpDelete("{userId}/{roleId}")]
        public ActionResult<IdentityUserRole<string>> DeleteUserRole(string userId, string roleId)
        {
            var selectedUserRoles = _usersRolesService.GetAll(u => u.UserId == userId && u.RoleId == roleId).SingleOrDefault();

            if (selectedUserRoles == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user's role not found.",
                    Success = false
                });
            }

            var result = _usersRolesService.Delete(selectedUserRoles);

            if (result == 0)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Delete selected user's role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "selected user's role has been deleted successfully.",
                Success = true
            });
        }
    }
}
