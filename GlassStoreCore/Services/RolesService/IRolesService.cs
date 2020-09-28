using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IRolesService
    {
        public Task<List<IdentityRole>> GetAllRoles();

        public Task<IdentityRole> GetRole(string id);

        public Task<int> DeleteRole(IdentityRole role);

        public Task<int> AddRole(IdentityRole user);

        public Task<int> UpdateRole(IdentityRole role, string id);
    }
}
