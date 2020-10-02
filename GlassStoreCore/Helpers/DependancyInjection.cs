using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.Data;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GlassStoreCore.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<IUsersRolesService, UsersRolesService>();
            return services;
        }
    }
}
