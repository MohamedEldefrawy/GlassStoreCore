using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.APIs.Filters;
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
        private readonly GlassStoreContext _context;
        private readonly ObjectMapper _mapper;


        public UsersService(UserManager<ApplicationUser> userManager, ObjectMapper mapper, GlassStoreContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<(List<UserDto>, int)> GetAllUsers(int pageNumber, int pageSize)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);
            var users = await _userManager.Users.AsNoTracking()
                                          .Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize)
                                          .ToListAsync();
            var totalRecords = await _userManager.Users.CountAsync();
            return (users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>).ToList(), totalRecords);
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

        public async Task<int> UpdateUser(UserDto user, string id)
        {

            var selectedUser = _context.Users.FindAsync(id).Result;
            selectedUser.UserName = user.UserName;
            selectedUser.Email = user.Email;
            selectedUser.NormalizedEmail = user.Email.ToUpper();
            selectedUser.NormalizedUserName = user.UserName.ToUpper();
            selectedUser.PhoneNumber = user.PhoneNumber;

            _context.Entry(selectedUser).State = EntityState.Modified;
            return await _context.SaveChangesAsync();

        }
    }
}
