using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using GlassStoreCore.Helpers;
using Microsoft.AspNetCore.Identity;

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
            _userManager = userManager;
            _context = context;
        }

        public LoggedInUserDto Authenticate(LoginUserDto userDto)
        {
            AesDecrypt.keyAndIvBytes = UTF8Encoding.UTF8.GetBytes("Secret Passphrase");

            var PasswordHasher = new PasswordHasher<ApplicationUser>();

            var password = AesDecrypt.DecodeAndDecrypt(userDto.Password);

            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == userDto.UserName);
            var hashedPassword = PasswordHasher.HashPassword(user, password);


            // check if username exists
            if (user == null)
                return null;

            var isAuthunticated = UserVerification.VerifyHashedPassword(user, hashedPassword);

            // check if password is correct

            if (!isAuthunticated)
                return null;

            // authentication successful
            return new LoggedInUserDto
            {
                UserID = user.Id,
                UserName = user.UserName
            };
        }

        public async Task<ApplicationUser> CreateUser(CreateUserDto user)
        {
            var newUser = _mapper.Mapper.Map<CreateUserDto, ApplicationUser>(user);
            newUser.NormalizedEmail = newUser.Email.ToUpper();
            newUser.NormalizedUserName = newUser.UserName.ToUpper();
            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                newUser = null;
            }
            return newUser;
        }
    }
}
