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

        public void DeleteRole(IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> AddRole(IdentityRole user)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(IdentityRole role, string id)
        {
            throw new NotImplementedException();
        }
    }
}
