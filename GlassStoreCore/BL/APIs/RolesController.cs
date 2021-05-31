using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.RolesDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Helpers;
using GlassStoreCore.JsonResponses;
using GlassStoreCore.Services;
using GlassStoreCore.Services.PaginationUowService;
using GlassStoreCore.Services.RolesService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IPaginationUow _paginationUow;
        private readonly IRolesService _rolesService;
        private readonly ObjectMapper _mapper;

        public RolesController(IPaginationUow paginationUow, ObjectMapper mapper)
        {
            _mapper = mapper;
            _paginationUow = paginationUow;
            _rolesService = _paginationUow.GetRolesService();
        }

        [HttpGet]
        public ActionResult<ApplicationRole> GetRoles([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (roles, totalRecords) = _rolesService.GetAll(filter.PageNumber, filter.PageSize);

            if (roles == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "No roles found.",
                    Success = false
                });
            }

            var rolesDtos = roles.Select(_mapper.Mapper.Map<ApplicationRole, RoleDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(rolesDtos, filter, totalRecords, _paginationUow, route);
            pageResponse.Message = "request has completed successfully.";
            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationRole> GetRole(string id)
        {
            var role = _rolesService.FindById(id);

            if (role == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            return Ok(new GetJsonResponse
            {
                StatusMessage = "Role has been selected successfully.",
                Success = true,
                Data = _mapper.Mapper.Map<ApplicationRole, RoleDto>(role)
            });
        }

        [HttpPost]
        public ActionResult<ApplicationRole> CreateRole(RoleDto roleDto)
        {
            var role = _mapper.Mapper.Map<RoleDto, ApplicationRole>(roleDto);
            var result = _rolesService.AddRole(role);

            if (!result.Result.Succeeded)
            {
                return BadRequest(new GetJsonResponse
                {
                    StatusMessage = "Couldn't create role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Role has been created successfully.",
                Success = true,
            });
        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var role = _rolesService.FindById(id);

            if (role == null)
            {
                return NotFound(new GetJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            _mapper.Mapper.Map(roleDto, role);

            var result = _rolesService.UpdateRole(role);

            if (!result.Result.Succeeded)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't update selected role.",
                    Success = false
                });
            }

            return Ok(new GetJsonResponse
            {
                StatusMessage = "Selected role has been updated.",
                Success = true

            });
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationRole> DeleteRole(string id)
        {
            var selectedRole = _rolesService.FindById(id);
            if (selectedRole == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            var result = _rolesService.DeleteRole(selectedRole);
            if (!result.Result.Succeeded)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't delete selected role.",
                    Success = false

                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Role has been deleted successfully.",
                Success = true
            });
        }
    }
}
