﻿using GlassStoreCore.BL.DTOs.RolesDtos;
using System.Collections.Generic;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<RoleNameDto> Roles { get; set; }
    }
}
