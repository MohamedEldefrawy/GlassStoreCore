using GlassStoreCore.BL.DTOs.RolesDtos;
using System.Collections.Generic;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class LoggedInUserDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public ICollection<RoleNameDto> Roles { get; set; }
    }
}
