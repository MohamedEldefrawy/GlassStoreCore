using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs.RolesDtos;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService : IService<ApplicationUser>
    {
        public Task<ApplicationUser> CreateUser(CreateUserDto user);
        public LoggedInUserDto Authenticate(LoginUserDto userDto);
        public Task<List<RoleNameDto>> GetUserRoles(ApplicationUser user);
        public Task<IdentityResult> AssignUserRole(string userId, RoleNameDto role);
        public Task<IdentityResult> AssignUserRoles(string userId, ICollection<RoleNameDto> roles);
        public Task<IdentityResult> UnAssignUserRole(string userId, RoleNameDto role);
        public Task<IdentityResult> UnAssignUserRoles(string userId, ICollection<RoleNameDto> roles);

        public bool SignOut();
    }
}
