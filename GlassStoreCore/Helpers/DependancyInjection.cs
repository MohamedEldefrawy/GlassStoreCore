using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.Data;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using GlassStoreCore.Services.WholeSaleProductsService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GlassStoreCore.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IUsersRolesService, UsersRolesService>();
            services.AddScoped<IWholeSaleProductsService, WholeSaleProductsService>();
            return services;
        }
    }
}
