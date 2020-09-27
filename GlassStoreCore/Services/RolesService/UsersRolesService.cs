using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.RolesService
{
    public class UsersRolesService : IUsersRolesService
    {
        private readonly GlassStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRolesService(GlassStoreContext context, UserManager<ApplicationUser> userManger)
        {
            _context = context;
            _userManager = userManger;
        }

        public List<IdentityUserRole<string>> GetAllUsersRoles()
        {
            return _context.UserRoles.AsNoTracking().ToList();
        }

        public List<IdentityUserRole<string>> GetUserRoles(string userId)
        {
            return _context.UserRoles.Where(u => u.UserId == userId).ToList();
        }

        public void DeleteUserRole(IdentityUserRole<string> userRole)
        {
            _context.UserRoles.Remove(userRole);
            _context.SaveChanges();

        }

        public void AddUserRole(IdentityUserRole<string> userRole)
        {
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
        }
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
