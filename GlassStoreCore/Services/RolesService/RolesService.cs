using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly GlassStoreContext _context;

        public RolesService(GlassStoreContext context)
        {
            this._context = context;
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync<IdentityRole>();
        }

        public async Task<IdentityRole> GetRole(string id)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> DeleteRole(IdentityRole role)
        {
            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddRole(IdentityRole role)
        {
            await _context.Roles.AddAsync(role);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRole(IdentityRole role, string id)
        {
            _context.Entry(role).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
