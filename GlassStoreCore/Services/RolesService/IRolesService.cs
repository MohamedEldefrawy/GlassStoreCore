using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GlassStoreCore.Services.RolesService
{
    public interface IRolesService : IService<ApplicationRole>
    {
        public Task<IdentityResult> AddRole(ApplicationRole role);
        public Task<IdentityResult> UpdateRole(ApplicationRole role);
        public Task<IdentityResult> DeleteRole(ApplicationRole role);


    }
}
