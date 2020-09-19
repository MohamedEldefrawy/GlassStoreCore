using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IRolesService
    {
        public Task<List<IdentityRole>> GetAllRoles();

        public IdentityRole GetRole(string id);

        public void DeleteRole(IdentityRole role);

        public void AddRole(IdentityRole user);

        public void UpdateRole(IdentityRole role, string id);

    }
}
