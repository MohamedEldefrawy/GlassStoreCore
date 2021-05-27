using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Helpers;
using GlassStoreCore.JsonResponses;
using GlassStoreCore.Services;
using GlassStoreCore.Services.PaginationUowService;
using GlassStoreCore.Services.UserService;
using GlassStoreCore.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ObjectMapper _mapper;
        private readonly IPaginationUow _paginationUow;
        private readonly IService<IdentityUserRole<string>> _userRolesService;
        private readonly IUsersService _usersService;
        private Singleton singleton;

        public UsersController(IPaginationUow paginationUow, ObjectMapper mapper)
        {
            _paginationUow = paginationUow;
            _mapper = mapper;
            _usersService = _paginationUow.GetUsersService();
            _userRolesService = _paginationUow.Service<IdentityUserRole<string>>();
            singleton = Singleton.GetInstance;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<ApplicationUser> GetUsers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (users, totalRecords) = _usersService.GetAll(filter.PageNumber, filter.PageSize);

            if (singleton.JwtToken == string.Empty)
            {
                return (BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Please Login first",
                    Success = false
                }));
            }

            if (users == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Find users.",
                    Success = false
                });
            }

            var usersDtos = users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>).ToList();

            foreach (var user in usersDtos)
            {
                var userRoles = _userRolesService.GetAll(u => u.UserId == user.Id);
                user.Roles = new List<UserRoleDto>();

                foreach (var role in userRoles)
                {
                    user.Roles.Add(new UserRoleDto
                    {
                        RoleId = role.RoleId,
                        UserId = role.UserId
                    });
                }
            }

            var pageResponse = PaginationHelper.CreatePagedResponse(usersDtos, filter, totalRecords, _paginationUow, route);
            pageResponse.Succeeded = true;
            return Ok(pageResponse);
        }

        [HttpPost]
        public ActionResult<ApplicationUser> Login(LoginUserDto loginUserDto)
        {

            var result = _usersService.Authenticate(loginUserDto);
            var ApiToken = JwtToken.GenerateJwtToken(result);

            if (result == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Wrong UserName or password.",
                    Success = false,
                });
            }
            return Ok(new LoginJsonResponse
            {
                StatusMessage = "User has logged in successfully.",
                Success = true,
                Token = ApiToken,
                Data = result
            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult<ApplicationUser> LogOut()
        {
            var result = _usersService.SignOut();
            if (!result)
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Logout.",
                    Success = false
                });
            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Logged out successfully.",
                Success = true
            });
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationUser> GetUser(string id)
        {
            var user = _usersService.FindById(id);

            if (user == null)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Selected user not found.",
                    Success = false
                });
            }

            var userRoles = _userRolesService.GetAll(u => u.UserId == id);
            var userDto = _mapper.Mapper.Map<ApplicationUser, UserDto>(user);

            userDto.Roles = new List<UserRoleDto>();
            foreach (var role in userRoles)
            {
                userDto.Roles.Add(new UserRoleDto
                {
                    UserId = role.UserId,
                    RoleId = role.RoleId
                });
            }
            return Ok(new GetJsonResponse
            {
                StatusMessage = "User has been selected successfully.",
                Success = true,
                Data = userDto
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationUser> DeleteUser(string id)
        {
            var userService = _usersService;

            var selectedUser = userService.FindById(id);
            if (selectedUser == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user not found.",
                    Success = false
                });
            }

            var result = userService.Delete(selectedUser);

            if (result == 0)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Faild to Delete selected user.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Selected user has been deleted succesfully.",
                Success = true
            });
        }

        [HttpPost]
        public ActionResult<ApplicationUser> CreateUser(CreateUserDto userDto)
        {
            AesDecrypt.keyAndIvBytes = Encoding.UTF8.GetBytes("8080808080808080");
            userDto.Password = AesDecrypt.DecodeAndDecrypt(userDto.Password);
            userDto.ConfirmPassword = AesDecrypt.DecodeAndDecrypt(userDto.ConfirmPassword);

            UsersValidator validator = new UsersValidator();
            var validationResult = validator.Validate(userDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = _usersService.CreateUser(userDto);

            foreach (var role in userDto.Roles)
            {
                role.UserId = result.Result.Id;
                _userRolesService.Add(new IdentityUserRole<string>()
                {
                    UserId = role.UserId,
                    RoleId = role.RoleId
                });
                _paginationUow.Complete();
            }
            return Ok(new OtherJsonResponse
            {
                StatusMessage = "User Created Successfully.",
                Success = true
            });
        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationUser> UpdateUser(UserDto userDto, string id)
        {
            var user = _usersService.FindById(id);
            if (user == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Find Selected user",
                    Success = false
                });
            }

            _mapper.Mapper.Map(userDto, user);
            user.Id = id;

            var userRoles = _userRolesService.GetAll(u => u.UserId == userDto.Id);

            foreach (var userRole in userRoles)
            {
                _userRolesService.Delete(userRole);
            }

            foreach (var role in userDto.Roles)
            {
                var updatedUserRole = _mapper.Mapper.Map<UserRoleDto, IdentityUserRole<string>>(role);
                updatedUserRole.UserId = userDto.Id;
                _userRolesService.Add(updatedUserRole);
            }

            var result = _usersService.Update(user);

            if (result == 0)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't update selected user.",
                    Success = false
                });
            }
            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Selected user has been updated successfully.",
                Success = true
            });
        }

        [HttpGet]
        public ActionResult<ApplicationUser> FindUserByName(UserNameDto nameDto)
        {
            if (nameDto is null)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Faild to find user.",
                    Success = false
                });
            }

            var users = _usersService.GetAll(u => u.UserName.Contains(nameDto.Name)).ToList();

            if (users == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "No users found.",
                    Success = false
                });
            }

            return Ok(new GetJsonResponse
            {
                StatusMessage = "User has been found successfully.",
                Success = true,
                Data = users.Select(_mapper.Mapper.Map<ApplicationUser, UserDto>)
            });
        }
    }
}
