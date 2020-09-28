using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly GlassStoreContext _context;
        private readonly ObjectMapper _mapper;

        public RolesService(GlassStoreContext context, ObjectMapper mapper)
        {
            this._context = context;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync<IdentityRole>();
            return roles.Select(_mapper.Mapper.Map<IdentityRole, RoleDto>).ToList();
        }

        public async Task<RoleDto> GetRole(string id)
        {
            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

            return _mapper.Mapper.Map<IdentityRole, RoleDto>(role);

        }

        public async Task<int> DeleteRole(string id)
        {
            _context.Roles.Remove(_context.Roles.FindAsync(id).Result);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddRole(RoleDto role)
        {

            await _context.Roles.AddAsync(_mapper.Mapper.Map<RoleDto, IdentityRole>(role));
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRole(UpdateRoleDto role, string id)
        {
            var updatedRole = _context.Roles.FindAsync(id).Result;
            updatedRole.Name = role.Name;
            _context.Entry(updatedRole).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
