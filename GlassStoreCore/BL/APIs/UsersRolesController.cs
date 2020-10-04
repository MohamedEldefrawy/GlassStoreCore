using System.Collections.Generic;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UriService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUriService _uriService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ObjectMapper _mapper;


        public UsersRolesController(IUnitOfWork unitOfWork, IUriService uriService, ObjectMapper mapper)
        {
            _uriService = uriService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IdentityUserRole<string>> GetUsersRoles([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (userRoles, totalRecords) = _unitOfWork.UsersRolesService.GetAll(filter.PageNumber, filter.PageSize).Result;

            if (userRoles == null)
            {
                return NotFound();
            }

            var pageResponse = PaginationHelper.CreatePagedResponse(userRoles, filter, totalRecords, _uriService, route);

            return Ok(pageResponse);
        }

        [HttpGet("{userId}")]
        public ActionResult<IdentityUserRole<string>> GetUserRoles(string userId)
        {
            var selectedUserRoles = _unitOfWork.UsersRolesService.GetUserRoles(userId).Result;

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

            _unitOfWork.UsersRolesService
                                    .Add(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(userRoleDto));
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpPut("{userId}")]
        public ActionResult<IdentityUserRole<string>> UpdateUserRoles(List<UserRoleDto> userRolesDto, string userId)
        {
            var userRole = _unitOfWork.UsersRolesService.GetUserRoles(userId).Result;
            foreach (var role in userRole)
            {
                _unitOfWork.UsersRolesService.Delete(role.UserId, role.RoleId);
            }

            foreach (var role in userRolesDto)
            {
                _unitOfWork.UsersRolesService.Add(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(role));
            }

            _unitOfWork.Complete();
            return Ok();

        }

        [HttpDelete("{userId}/{roleId}")]
        public ActionResult<IdentityUserRole<string>> DeleteUserRole(string userId, string roleId)
        {
            var selectedUserRoles = _unitOfWork.UsersRolesService.GetUserRoles(userId).Result;
            if (selectedUserRoles == null)
            {
                return NotFound("Please Enter a valid userId and roleId");
            }
            var result = _unitOfWork.UsersRolesService.Delete(userId, roleId).Result;
            if (result == 0)
            {
                return BadRequest("something wrong");
            }
            return Ok();
        }

    }
}
