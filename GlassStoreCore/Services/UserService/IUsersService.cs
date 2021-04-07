using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService : IService<ApplicationUser>
    {
        public Task<ApplicationUser> CreateUser(CreateUserDto user);
        public ApplicationUser Authenticate(string username, string password);
    }
}
