﻿using System.Collections.Generic;
using GlassStoreCore.BL.DTOs.RolesDtos;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<RoleNameDto> Roles { get; set; }

    }
}
