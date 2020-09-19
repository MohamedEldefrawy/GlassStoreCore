using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IUsersRolesService
    {
        public Task<List<IdentityUserRole<string>>> GetAllUsersRoles();

        public IdentityUserRole<string> GetUserRole(string userId, string roleId);

        public void DeleteUserRole(IdentityUserRole<string> userRole);

        public void AddUserRole(IdentityUserRole<string> user);

        public void UpdateUserRole(IdentityUserRole<string> userRole);

        public void Dispose();

    }
}
