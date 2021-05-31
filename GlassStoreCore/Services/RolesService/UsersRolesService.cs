using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;

namespace GlassStoreCore.Services.RolesService
{
    public class UsersRolesService : Service<ApplicationUserRole>, IUsersRolesService
    {
        public UsersRolesService(GlassStoreContext context)
            : base(context)
        {
        }
    }
}

