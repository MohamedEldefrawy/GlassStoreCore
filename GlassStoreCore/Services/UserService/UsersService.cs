using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.UserService
{
    public class UsersService : Service<ApplicationUser>, IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ObjectMapper _mapper = new ObjectMapper();
        private readonly GlassStoreContext _context;

        public UsersService(GlassStoreContext context, UserManager<ApplicationUser> userManager)
            : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> CreateUser(CreateUserDto user)
        {
            var newUser = _mapper.Mapper.Map<CreateUserDto, ApplicationUser>(user);
            newUser.NormalizedEmail = newUser.Email.ToUpper();
            newUser.NormalizedUserName = newUser.UserName.ToUpper();
            await _userManager.CreateAsync(newUser, user.Password);
            return newUser;
        }

        public void Update(UserDto userDto, string id)
        {
            var selectedUser = _userManager.FindByIdAsync(id).Result;
            selectedUser.Email = userDto.Email;
            selectedUser.UserName = userDto.UserName;
            selectedUser.PhoneNumber = userDto.PhoneNumber;
            selectedUser.NormalizedUserName = userDto.UserName.ToUpper();
            selectedUser.NormalizedEmail = userDto.Email.ToUpper();

            _context.Entry(selectedUser).State = EntityState.Modified;

        }
    }
}
