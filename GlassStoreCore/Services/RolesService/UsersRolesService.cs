using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            return await _context.UserRoles.ToListAsync();
        }

        public IdentityUserRole<string> GetUserRole(string userId, string roleId)
        {
            return _context.UserRoles.AsNoTracking()
                                   .FirstOrDefault(u => u.UserId == userId && u.RoleId == roleId);
        }

        public void DeleteUserRole(IdentityUserRole<string> userRole)
        {
            _context.UserRoles.Remove(userRole);
        }

        public async void AddUserRole(IdentityUserRole<string> userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
        }

        public async void UpdateUserRole(IdentityUserRole<string> userRole, string userId, string roleId)
        {
            _context.Entry(userRole).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
