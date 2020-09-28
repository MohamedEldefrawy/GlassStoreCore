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
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public ActionResult<IdentityRole> GetRoles()
        {
            var roles = _rolesService.GetAllRoles().Result;
            if (roles == null)
            {
                return NotFound();
            }

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public ActionResult<IdentityRole> GetRole(string id)
        {
            var role = _rolesService.GetRole(id).Result;

            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost]
        public ActionResult<IdentityRole> CreateRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter a valid role data");
            }


            var result = _rolesService.AddRole(roleDto).Result;
            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<IdentityRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter valid role data");

            }

            var result = _rolesService.UpdateRole(roleDto, id).Result;
            if (result == 0)
            {
                return BadRequest("Something wrong");
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<IdentityRole> DeleteRole(string id)
        {
            var role = _rolesService.GetRole(id).Result;
            if (role == null)
            {
                return NotFound("please select a valid role");
            }

            var result = _rolesService.DeleteRole(id).Result;
            if (result == 0)
            {
                return BadRequest("Something wrong");
            }
            return Ok();
        }
    }
}
