using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IUsersRolesService : IService<IdentityUserRole<string>>
    {

    }
}
