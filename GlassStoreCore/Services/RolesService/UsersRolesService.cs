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
    public class UsersRolesService : IUsersRolesService
    {
        private readonly GlassStoreContext _context;
        private readonly ObjectMapper _mapper;

        public UsersRolesService(GlassStoreContext context, ObjectMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserRoleDto>> GetAllUsersRoles()
        {
            var userRoles = await _context.UserRoles.AsNoTracking().ToListAsync();
            return userRoles.Select(_mapper.Mapper.Map<IdentityUserRole<string>, UserRoleDto>).ToList();
        }

        public async Task<List<UserRoleDto>> GetUserRoles(string userId)
        {
            var userRoles = await _context.UserRoles.Where(u => u.UserId == userId).ToListAsync();
            return userRoles.Select(_mapper.Mapper.Map<IdentityUserRole<string>, UserRoleDto>).ToList();
        }

        public async Task<int> DeleteUserRole(string userId, string roleId)
        {
            var userRole = await _context.UserRoles.FindAsync(userId, roleId);
            _context.UserRoles.Remove(userRole);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddUserRole(UserRoleDto userRole)
        {
            await _context.UserRoles.AddAsync(_mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(userRole));
            return await _context.SaveChangesAsync();
        }
    }
}
