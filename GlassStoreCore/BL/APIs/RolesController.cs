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
            var (roles, totalRecords) = _rolesService.GetAll(filter.PageNumber, filter.PageSize).Result;

            if (roles == null)
            {
                return NotFound();
            }

            var rolesDtos = roles.Select(_mapper.Mapper.Map<IdentityRole, RoleDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(rolesDtos, filter, totalRecords, _paginationUow, route);

            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<IdentityRole> GetRole(string id)
        {
            var role = _rolesService.FindById(id).Result;

            if (role == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Mapper.Map<IdentityRole, RoleDto>(role));
        }

        [HttpPost]
        public ActionResult<IdentityRole> CreateRole(RoleDto roleDto)
        {
            var role = _mapper.Mapper.Map<RoleDto, IdentityRole>(roleDto);
            var result = _rolesService.Add(role);

            if (result.Result <= 0)
            {
                return BadRequest("Something wrong!");
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<IdentityRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var role = _rolesService.FindById(id).Result;

            if (role == null)
            {
                return NotFound("Cannot find the selected role id");
            }

            _mapper.Mapper.Map(roleDto, role);

            var result = _rolesService.UpdateAsync(role).Result;

            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<IdentityRole> DeleteRole(string id)
        {
            var roleService = _rolesService;
            var selectedRole = roleService.FindById(id).Result;
            if (selectedRole == null)
            {
                return NotFound("please select a valid role");
            }

            var result = roleService.DeleteAsync(selectedRole).Result;
            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }
    }
}
