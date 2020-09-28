using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IUsersRolesService
    {
        public Task<List<IdentityUserRole<string>>> GetAllUsersRoles();

        public Task<List<IdentityUserRole<string>>> GetUserRoles(string userId);

        public Task<int> DeleteUserRole(IdentityUserRole<string> userRole);

        public Task<int> AddUserRole(IdentityUserRole<string> user);
    }
}
