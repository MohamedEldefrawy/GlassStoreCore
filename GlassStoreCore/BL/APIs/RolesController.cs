using System.Linq;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Services.RolesService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ObjectMapper _mapper = new ObjectMapper();
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            this._rolesService = rolesService;
        }

        [HttpGet]
        public ActionResult<IdentityRole> GetRoles()
        {
            var roles = _rolesService.GetAllRoles().Result;
            if (roles == null)
            {
                return NotFound();
            }

            var rolesDto = roles.Select(_mapper.Mapper.Map<IdentityRole, RoleDto>).ToList();

            return Ok(rolesDto);
        }

        [HttpGet("{id}")]
        public ActionResult<IdentityRole> GetRole(string id)
        {
            var role = _rolesService.GetRole(id);

            if (role == null)
            {
                return NotFound();
            }
            var roleDto = _mapper.Mapper.Map<IdentityRole, RoleDto>(role);
            return Ok(roleDto);
        }

        [HttpPost]
        public ActionResult<IdentityRole> CreateRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter a valid role data");
            }

            var role = _mapper.Mapper.Map<RoleDto, IdentityRole>(roleDto);

            _rolesService.AddRole(role);
            _rolesService.Dispose();
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<IdentityRole> UpdateRole(RoleDto roleDto, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter valid role data");

            }

            var role = _rolesService.GetRole(id);
            if (role == null)
            {
                return BadRequest("Please Select a valid role id");
            }

            role = _mapper.Mapper.Map(roleDto, role);

            _rolesService.UpdateRole(role, id);
            _rolesService.Dispose();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<IdentityRole> DeleteRole(string id)
        {
            var role = _rolesService.GetRole(id);
            if (role == null)
            {
                return NotFound("please select a valid role");
            }

            _rolesService.DeleteRole(role);
            _rolesService.Dispose();
            return Ok();
        }
    }
}
