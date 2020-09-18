using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.UserService
{
    public class UsersService : IUsersService
    {
        private readonly GlassStoreContext _glassStoreContext;

        public UsersService(GlassStoreContext context)
        {
            _glassStoreContext = context;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _glassStoreContext.Users.ToListAsync<ApplicationUser>();
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _glassStoreContext.Users.FindAsync(id);
        }

        public async void DeleteUser(ApplicationUser user)
        {
            _glassStoreContext.Users.Remove(user);
            await _glassStoreContext.SaveChangesAsync();
            await _glassStoreContext.DisposeAsync();
        }
    }
}
