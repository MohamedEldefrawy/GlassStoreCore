using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GlassStoreContext _glassStoreContext;
        private readonly ObjectMapper _mapper = new ObjectMapper();

        public UsersController(GlassStoreContext context)
        {
            this._glassStoreContext = context;
        }

        public async Task<ActionResult<ApplicationUser>> GetUsers()
        {
            var users = await _glassStoreContext.Users.ToListAsync<ApplicationUser>();

            if (users == null)
            {
                return BadRequest();
            }

            var usersDtos = users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>).ToList();

            //foreach (var user in usersDtos)
            //{
            //    foreach (var role in user.Roles)
            //    {
            //        role.RoleName = _glassStoreContext.Roles.SingleOrDefault(r => r.Id == role.RoleId)?.Name;
            //    }
            //}

            return Ok(usersDtos);
        }


    }
}
