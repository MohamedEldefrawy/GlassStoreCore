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

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync<ApplicationUser>();
            return users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>).ToList();
        }

        public async Task<UserDto> GetUser(string id)
        {
            var user = await _userManager.Users.AsNoTracking().
                                           FirstOrDefaultAsync(u => u.Id == id);
            return _mapper.Mapper.Map<ApplicationUser, UserDto>(user);
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var selectedUser = _userManager.FindByIdAsync(id).Result;
            return await _userManager.DeleteAsync(selectedUser);
        }

        public async Task<ApplicationUser> AddUser(CreateUserDto user, string pw)
        {
            var newUser = _mapper.Mapper.Map<CreateUserDto, ApplicationUser>(user);
            await _userManager.CreateAsync(newUser, pw);
            return newUser;
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
