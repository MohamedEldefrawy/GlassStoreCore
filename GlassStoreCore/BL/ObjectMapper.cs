using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.BL.DTOs.RolesDtos;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.DTOs.UsersRolesDtos;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
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
                                                     cfg.CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
                                                     cfg.CreateMap<ApplicationUser, UserDto>().ReverseMap();
                                                     cfg.CreateMap<ApplicationUserRole, UserRoleDto>().ReverseMap();
                                                     cfg.CreateMap<RoleDto, ApplicationRole>().ReverseMap();
                                                     cfg.CreateMap<UserRoleDto, ApplicationUserRole>().ReverseMap();
                                                     cfg.CreateMap<UpdateRoleDto, ApplicationRole>().ReverseMap();
                                                     cfg.CreateMap<ApplicationUserRole, UpdateUserRole>().ReverseMap();
                                                     cfg.CreateMap<WholeSaleProductsDto, WholeSaleProducts>().ReverseMap();
                                                     cfg.CreateMap<WholeSaleProductsOrderDetailsDto, WholeSaleSellingOrderDetails>().ReverseMap();
                                                     cfg.CreateMap<WholeSaleSellingOrder, WholeSaleSellingOrdersDto>().ReverseMap();
                                                     cfg.CreateMap<UpdateWholeSaleSellingOrderDto, WholeSaleProductsOrderDetailsDto>().ReverseMap();
                                                     cfg.CreateMap<CreateWholeSaleSellingOrdersDto, WholeSaleSellingOrdersDto>().ReverseMap();
                                                 });

            this.Mapper = config.CreateMapper();


        }
    }

}
