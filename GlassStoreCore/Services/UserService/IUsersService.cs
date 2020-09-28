using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService
    {
        public Task<List<UserDto>> GetAllUsers();

        public Task<UserDto> GetUser(string id);

        public Task<IdentityResult> DeleteUser(string id);

        public Task<ApplicationUser> AddUser(CreateUserDto user, string pw);

        public Task<int> UpdateUser(UserDto user, string id);

    }
}
