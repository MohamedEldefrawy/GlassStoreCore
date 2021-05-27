using System.Threading.Tasks;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.Services.UserService
{
    public interface IUsersService : IService<ApplicationUser>
    {
        public Task<ApplicationUser> CreateUser(CreateUserDto user);
        public LoggedInUserDto Authenticate(LoginUserDto userDto);
        public bool SignOut();
    }
}
