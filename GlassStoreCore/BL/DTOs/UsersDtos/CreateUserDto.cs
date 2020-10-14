using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public ICollection<UserRoleDto> Roles { get; set; }

    }
}
