using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IUsersRolesService : IService<IdentityUserRole<string>>
    {

        public Task<List<UserRoleDto>> GetUserRoles(string userId);
        public Task<int> Delete(string userId, string roleId);
    }
}
