using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs.RolesDtos;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ObjectMapper _mapper = new ObjectMapper();
        private readonly GlassStoreContext _context;

        public UsersService(GlassStoreContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
            : base(context)
        {
            _signInManager = signInManager;
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

            var signInResult = _signInManager.PasswordSignInAsync(user, password, true, false);

            // check if password is correct
            if (!signInResult.Result.Succeeded)
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
            var rolesNames = new List<string>();
            var newUser = _mapper.Mapper.Map<CreateUserDto, ApplicationUser>(user);

            newUser.NormalizedEmail = newUser.Email.ToUpper();
            newUser.NormalizedUserName = newUser.UserName.ToUpper();

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                newUser = null;
            }

            foreach (var role in user.Roles)
            {
                rolesNames.Add(role.Name);
            }

            await _userManager.AddToRolesAsync(newUser, rolesNames);


            return newUser;
        }

        public async Task<List<RoleNameDto>> GetUserRoles(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var rolesDtos = new List<RoleNameDto>();
            foreach (var role in roles)
            {
                rolesDtos.Add(new RoleNameDto
                {
                    Name = role
                });
            }
            return rolesDtos;
        }

        public bool SignOut()
        {
            _signInManager.SignOutAsync();
            JwtToken.RemoveCurrnetToken();
            Singleton singleton = Singleton.GetInstance;

            if (singleton.JwtToken == string.Empty)
                return true;
            else
                return false;

        }

        public async Task<IdentityResult> AssignUserRole(string userId, RoleNameDto role)
        {
            var slectedUser = _userManager.FindByIdAsync(userId).Result;

            return await _userManager.AddToRoleAsync(slectedUser, role.Name);
        }

        public async Task<IdentityResult> AssignUserRoles(string userId, ICollection<RoleNameDto> roles)
        {
            var slectedUser = _userManager.FindByIdAsync(userId).Result;
            var rolNames = new List<string>();
            foreach (var role in roles)
            {
                rolNames.Add(role.Name);
            }
            return await _userManager.AddToRolesAsync(slectedUser, rolNames);
        }
        public async Task<IdentityResult> UnAssignUserRole(string userId, RoleNameDto role)
        {
            var slectedUser = _userManager.FindByIdAsync(userId).Result;
            return await _userManager.RemoveFromRoleAsync(slectedUser, role.Name);
        }

        public async Task<IdentityResult> UnAssignUserRoles(string userId, ICollection<RoleNameDto> roles)
        {
            var rolNames = new List<string>();
            foreach (var role in roles)
            {
                rolNames.Add(role.Name);
            }
            var slectedUser = _userManager.FindByIdAsync(userId).Result;
            return await _userManager.RemoveFromRolesAsync(slectedUser, rolNames);
        }
    }
}
