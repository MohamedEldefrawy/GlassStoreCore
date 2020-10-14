using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public class UsersRolesService : Service<IdentityUserRole<string>>, IUsersRolesService
    {
        public UsersRolesService(GlassStoreContext context)
            : base(context)
        {
        }
    }
}

