using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IUsersRolesService
    {
        public List<IdentityUserRole<string>> GetAllUsersRoles();

        public List<IdentityUserRole<string>> GetUserRoles(string userId);

        public void DeleteUserRole(IdentityUserRole<string> userRole);

        public void AddUserRole(IdentityUserRole<string> user);

        public void Dispose();

    }
}
