using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.UserService
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ObjectMapper _mapper;


        public UsersService(UserManager<ApplicationUser> userManager, ObjectMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.AsNoTracking().ToListAsync<ApplicationUser>();
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _userManager.Users.AsNoTracking().
                                                FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddUser(ApplicationUser user, string pw)
        {
            return await _userManager.CreateAsync(user, pw);
        }

        public async Task<IdentityResult> UpdateUser(UserDto user, string id)
        {
            var selectedUser = _mapper.Mapper.Map<UserDto, ApplicationUser>(user);
            selectedUser.Id = id;
            selectedUser.NormalizedEmail = selectedUser.Email.ToUpper();
            selectedUser.NormalizedUserName = selectedUser.UserName.ToUpper();
            return await _userManager.UpdateAsync(selectedUser);
        }
    }
}
