using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GlassStoreCore.Services.RolesService
{
    public class RolesService : Service<ApplicationRole>, IRolesService
    {
        private readonly RoleManager<ApplicationRole> _roleManger;
        public RolesService(GlassStoreContext context, RoleManager<ApplicationRole> roleManager) :
            base(context)
        {
            _roleManger = roleManager;
        }

        public async Task<IdentityResult> AddRole(ApplicationRole role)
        {
            return await _roleManger.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRole(ApplicationRole role)
        {
            return await _roleManger.DeleteAsync(role);
        }

        public async Task<IdentityResult> UpdateRole(ApplicationRole role)
        {
            return await _roleManger.UpdateAsync(role);
        }
    }
}
