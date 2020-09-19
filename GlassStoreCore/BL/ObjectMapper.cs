using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.BL
{
    public class ObjectMapper
    {
        public IMapper Mapper { get; set; }

        public ObjectMapper()
        {
            var config = new MapperConfiguration(cfg =>
                                                 {
                                                     cfg.CreateMap<ApplicationUser, CreateUserDto>();
                                                     cfg.CreateMap<CreateUserDto, ApplicationUser>();
                                                     cfg.CreateMap<ApplicationUser, UserDto>();
                                                     cfg.CreateMap<UserDto, ApplicationUser>();
                                                     cfg.CreateMap<IdentityUserRole<string>, UserRoleDto>();
                                                     cfg.CreateMap<UserRoleDto, IdentityUserRole<string>>();
                                                     cfg.CreateMap<RoleDto, IdentityRole>();
                                                     cfg.CreateMap<IdentityRole, RoleDto>();
                                                     cfg.CreateMap<UserRoleDto, IdentityUserRole<string>>();
                                                     cfg.CreateMap<IdentityUserRole<string>, UserRoleDto>();

                                                 });

            this.Mapper = config.CreateMapper();

        }
    }

}
