using System.Collections.Generic;
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
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UsersService(GlassStoreContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _glassStoreContext = context;
            _userManager = userManager;
            _signInManager = signManager;
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
        }

        public async Task<IdentityResult> AddUser(ApplicationUser user, string pw)
        {
            return await _userManager.CreateAsync(user, pw);
        }

        public async void Dispose()
        {
            await _glassStoreContext.DisposeAsync();
        }
    }
}
