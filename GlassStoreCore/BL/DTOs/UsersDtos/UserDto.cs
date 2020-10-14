using System.Collections.Generic;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserRoleDto> Roles { get; set; }
    }
}
