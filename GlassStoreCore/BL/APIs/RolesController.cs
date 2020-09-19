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
    [Route("api/[controller]")]
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

    }
}
