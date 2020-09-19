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
            return await this._context.Roles.ToListAsync<IdentityRole>();
        }

        public IdentityRole GetRole(string id)
        {
            return this._context.Roles.AsNoTracking().FirstOrDefault(r => r.Id == id);
        }

        public async void DeleteRole(IdentityRole role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async void AddRole(IdentityRole role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async void UpdateRole(IdentityRole role, string id)
        {
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
