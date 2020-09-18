using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService
    {
        public Task<List<ApplicationUser>> GetAllUsers();

        public ApplicationUser GetUser(string id);

        public void DeleteUser(ApplicationUser user);

        public Task<IdentityResult> AddUser(ApplicationUser user, string pw);

        public void UpdateUser(ApplicationUser user, string id);

        public void Dispose();
    }
}
