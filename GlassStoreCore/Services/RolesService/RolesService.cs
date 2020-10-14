using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public class RolesService : Service<IdentityRole>, IRolesService
    {
        public RolesService(GlassStoreContext context) :
            base(context)
        {
        }
    }
}
