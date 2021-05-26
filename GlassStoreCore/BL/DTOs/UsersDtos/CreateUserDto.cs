using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        [StringLength(255)]
        public string Password { get; set; }
        [StringLength(255)]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserRoleDto> Roles { get; set; }

    }
}
