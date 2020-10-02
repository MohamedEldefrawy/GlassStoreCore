using System.Collections.Generic;
using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService : IService<ApplicationUser>
    {
        public Task<ApplicationUser> CreateUser(CreateUserDto user);
    }
}
