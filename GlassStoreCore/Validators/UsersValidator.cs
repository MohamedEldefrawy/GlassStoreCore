using FluentValidation;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlassStoreCore.Validators
{
    public class UsersValidator : AbstractValidator<ApplicationUser>
    {
        public UsersValidator()
        {
            
        }
    }
}
