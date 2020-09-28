using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GlassStoreCore.Services.RolesService
{
    public class UsersRolesService : IUsersRolesService
    {
        private readonly GlassStoreContext _context;

        public UsersRolesService(GlassStoreContext context)
        {
            _context = context;
        }

        public async Task<List<IdentityUserRole<string>>> GetAllUsersRoles()
        {
            return await _context.UserRoles.AsNoTracking().ToListAsync();
        }

        public async Task<List<IdentityUserRole<string>>> GetUserRoles(string userId)
        {
            return await _context.UserRoles.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<int> DeleteUserRole(IdentityUserRole<string> userRole)
        {
            _context.UserRoles.Remove(userRole);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddUserRole(IdentityUserRole<string> userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            return await _context.SaveChangesAsync();
        }
    }
}
