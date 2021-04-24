using System;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
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
            var PasswordHasher = new PasswordHasher<ApplicationUser>();
            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == userDto.UserName);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Failed)
                return null;

            // authentication successful
            return new LoggedInUserDto
            {
                UserID = user.Id,
                UserName = user.UserName
            };

        }

        //private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        //{
        //    if (password == null) throw new ArgumentNullException("password");
        //    if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
        //    if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
        //    if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

        //    using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        //    {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        for (int i = 0; i < computedHash.Length; i++)
        //        {
        //            if (computedHash[i] != storedHash[i]) return false;
        //        }
        //    }

        //    return true;
        //}

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
