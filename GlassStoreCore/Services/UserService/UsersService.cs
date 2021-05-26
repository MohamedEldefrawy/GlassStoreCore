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
            string password = DecryptPasswordAES(userDto.Password);

            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == userDto.UserName);


            // check if username exists
            if (user == null)
                return null;

            var signInResult = _userManager.CheckPasswordAsync(user, password);

            // check if password is correct
            if (!signInResult.Result)
                return null;

            // authentication successful
            return new LoggedInUserDto
            {
                UserID = user.Id,
                UserName = user.UserName
            };
        }

        public string DecryptPasswordAES(string text)
        {
            AesDecrypt.keyAndIvBytes = Encoding.UTF8.GetBytes("8080808080808080");

            var password = AesDecrypt.DecodeAndDecrypt(text);
            return password;
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
