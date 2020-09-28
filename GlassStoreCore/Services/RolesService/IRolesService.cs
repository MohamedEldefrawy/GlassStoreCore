using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.RolesService
{
    public interface IRolesService
    {
        public Task<List<RoleDto>> GetAllRoles();

        public Task<RoleDto> GetRole(string id);

        public Task<int> DeleteRole(string id);

        public Task<int> AddRole(RoleDto user);

        public Task<int> UpdateRole(UpdateRoleDto role, string id);
    }
}
