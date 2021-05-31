using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Helpers;
using GlassStoreCore.JsonResponses;
using GlassStoreCore.Services;
using GlassStoreCore.Services.PaginationUowService;
using GlassStoreCore.Services.RolesService;
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
        private readonly IUsersService _usersService;
        private readonly Singleton _singleton;
        private readonly IService<IdentityUserRole<string>> _usersRolesService;
        private readonly IRolesService _roleService;

        public UsersController(IPaginationUow paginationUow, ObjectMapper mapper)
        {
            _paginationUow = paginationUow;
            _mapper = mapper;
            _usersService = _paginationUow.GetUsersService();
            _singleton = Singleton.GetInstance;
            _usersRolesService = _paginationUow.Service<IdentityUserRole<string>>();
            _roleService = _paginationUow.GetRolesService();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<ApplicationUser> GetUsers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (users, totalRecords) = _usersService.GetAll(filter.PageNumber, filter.PageSize);

            if (_singleton.JwtToken == string.Empty)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Please Login first",
                    Success = false
                });
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

            for (int i = 0; i < usersDtos.Count; i++)
            {
                UserDto user = usersDtos[i];
                user.Roles = _usersService.GetUserRoles(users[i]).Result;
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

            var userDto = _mapper.Mapper.Map<ApplicationUser, UserDto>(user);
            userDto.Roles = _usersService.GetUserRoles(user).Result;

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
            var selectedUser = _usersService.FindById(id);
            if (selectedUser == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user not found.",
                    Success = false
                });
            }

            var result = _usersService.Delete(selectedUser);

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

            if (result.Result == null)
            {
                return BadRequest(new OtherJsonResponse { StatusMessage = "Couldn't create user", Success = false });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "User Created Successfully.",
                Success = true
            });
        }

        [HttpPut]
        public ActionResult<ApplicationUser> UpdateUser(UserDto userDto)
        {
            var user = _usersService.FindById(userDto.Id);

            if (user == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Find Selected user",
                    Success = false
                });
            }

            _mapper.Mapper.Map(userDto, user);

            var selectedUserRoles = _usersRolesService.GetAll(r => r.UserId == userDto.Id).ToList();

            if (selectedUserRoles.Count > 0)
            {
                foreach (var role in selectedUserRoles)
                {
                    _usersRolesService.Delete(new ApplicationUserRole
                    {
                        UserId = userDto.Id,
                        RoleId = role.RoleId
                    });
                }
            }

            var newRoles = new List<ApplicationRole>();
            foreach (var role in userDto.Roles)
            {
                newRoles.Add(_roleService.GetAll(r => r.Name == role.Name).SingleOrDefault());
            }


            try
            {
                foreach (var role in newRoles)
                {
                    _usersRolesService.Add(new ApplicationUserRole { UserId = userDto.Id, RoleId = role.Id });
                }
            }
            catch (System.AggregateException e)
            {
                foreach (var role in selectedUserRoles)
                {
                    _usersRolesService.Add(new ApplicationUserRole
                    {
                        UserId = userDto.Id,
                        RoleId = role.RoleId
                    });
                }
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = e.Message,
                    Success = false
                });
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
