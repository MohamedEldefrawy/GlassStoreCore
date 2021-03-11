using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.RolesDtos;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services;
using GlassStoreCore.Services.PaginationUowService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IPaginationUow _paginationUow;
        private readonly IService<IdentityRole> _rolesService;
        private readonly ObjectMapper _mapper;


        public RolesController(IPaginationUow paginationUow, ObjectMapper mapper)
        {
            _mapper = mapper;
            _paginationUow = paginationUow;
            _rolesService = _paginationUow.Service<IdentityRole>();
        }

        [HttpGet]
        public ActionResult<IdentityRole> GetRoles([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (roles, totalRecords) = _rolesService.GetAll(filter.PageNumber, filter.PageSize);

            if (roles == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "No roles found."
                });
            }

            var rolesDtos = roles.Select(_mapper.Mapper.Map<IdentityRole, RoleDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(rolesDtos, filter, totalRecords, _paginationUow, route);
            pageResponse.Message = "request has completed successfully.";
            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<IdentityRole> GetRole(string id)
        {
            var role = _rolesService.FindById(id);

            if (role == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected role not found."
                });
            }

            return Ok(_mapper.Mapper.Map<IdentityRole, RoleDto>(role));
        }

        [HttpPost]
        public ActionResult<IdentityRole> CreateRole(RoleDto roleDto)
        {
            var role = _mapper.Mapper.Map<RoleDto, IdentityRole>(roleDto);
            var result = _rolesService.Add(role);

            if (result <= 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't create role."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Role has been created successfully."
            });
        }

        [HttpPut("{id}")]
        public ActionResult<IdentityRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var role = _rolesService.FindById(id);

            if (role == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected role not found."
                });
            }

            _mapper.Mapper.Map(roleDto, role);

            var result = _rolesService.Update(role);

            if (result == 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't update selected role."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Selected user not found."
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<IdentityRole> DeleteRole(string id)
        {
            var selectedRole = _rolesService.FindById(id);
            if (selectedRole == null)
            {
                return NotFound(new JsonResults
                {
                    StatusCode = 404,
                    StatusMessage = "Selected role not found."
                });
            }

            var result = _rolesService.Delete(selectedRole);
            if (result == 0)
            {
                return BadRequest(new JsonResults
                {
                    StatusCode = 400,
                    StatusMessage = "Couldn't delete selected role."
                });
            }

            return Ok(new JsonResults
            {
                StatusCode = 200,
                StatusMessage = "Role has been deleted successfully."
            });
        }
    }
}
