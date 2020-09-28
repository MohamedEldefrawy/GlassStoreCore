using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IUsersRolesService
    {
        public Task<List<UserRoleDto>> GetAllUsersRoles();

        public Task<List<UserRoleDto>> GetUserRoles(string userId);

        public Task<int> DeleteUserRole(string userId, string roleId);

        public Task<int> AddUserRole(UserRoleDto user);
    }
}
