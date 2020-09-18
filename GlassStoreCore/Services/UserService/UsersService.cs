using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.UserService
{
    public class UsersService : IUsersService
    {
        private readonly GlassStoreContext _glassStoreContext;
        private readonly UserManager<ApplicationUser> _userManager;


        public UsersService(GlassStoreContext context, UserManager<ApplicationUser> userManager)
        {
            _glassStoreContext = context;
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _glassStoreContext.Users.ToListAsync<ApplicationUser>();
        }

        public ApplicationUser GetUser(string id)
        {
            return _glassStoreContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }

        public async void DeleteUser(ApplicationUser user)
        {
            _glassStoreContext.Users.Remove(user);
            await _glassStoreContext.SaveChangesAsync();

        }

        public async Task<IdentityResult> AddUser(ApplicationUser user, string pw)
        {
            return await _userManager.CreateAsync(user, pw);
        }

        public async void UpdateUser(ApplicationUser user, string id)
        {

            _glassStoreContext.Entry(user).State = EntityState.Modified;
            await _glassStoreContext.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _glassStoreContext.DisposeAsync();
        }
    }
}
