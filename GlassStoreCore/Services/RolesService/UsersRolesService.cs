using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.RolesService
{
    public class UsersRolesService : Service<IdentityUserRole<string>>, IUsersRolesService
    {
        private readonly GlassStoreContext _context;
        private readonly ObjectMapper _mapper = new ObjectMapper();

        public UsersRolesService(GlassStoreContext context)
            : base(context)
        {
            _context = context;
        }


        public async Task<List<UserRoleDto>> GetUserRoles(string userId)
        {
            var userRoles = await _context.UserRoles.Where(u => u.UserId == userId).ToListAsync();
            return userRoles.Select(_mapper.Mapper.Map<IdentityUserRole<string>, UserRoleDto>).ToList();
        }

        public async Task<int> Delete(string userId, string roleId)
        {
            var userRole = await _context.UserRoles.FindAsync(userId, roleId);
            _context.UserRoles.Remove(userRole);
            return await _context.SaveChangesAsync();
        }
    }
}

