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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var user = await _glassStoreContext.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest("User not found please use valid id");
            }

            var userDto = _mapper.Mapper.Map<ApplicationUser, UserDto>(user);

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationUser>> DeleteUser(string id)
        {
            var user = await _glassStoreContext.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest("Please Select a Valid user id");

            }

            _glassStoreContext.Users.Remove(user);
            await _glassStoreContext.SaveChangesAsync();
            await _glassStoreContext.DisposeAsync();

            return Ok("User Has been deleted successfully");
        }

        []

    }
}
