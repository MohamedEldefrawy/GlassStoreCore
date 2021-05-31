using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using GlassStoreCore.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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
            var result = await _roleManger.CreateAsync(role);
            await _roleManger.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, role.Name));

            return result;
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
