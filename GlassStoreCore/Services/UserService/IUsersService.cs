using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService
    {
        public Task<List<ApplicationUser>> GetAllUsers();

        public Task<ApplicationUser> GetUser(string id);

        public Task<IdentityResult> DeleteUser(ApplicationUser user);

        public Task<IdentityResult> AddUser(ApplicationUser user, string pw);

        public Task<IdentityResult> UpdateUser(UserDto user, string id);

    }
}
