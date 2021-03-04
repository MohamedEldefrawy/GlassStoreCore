using FluentValidation;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Services.PaginationUowService;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using GlassStoreCore.Services.WholeSaleProductsService;
using GlassStoreCore.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GlassStoreCore.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<ObjectMapper>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IUsersRolesService, UsersRolesService>();
            services.AddScoped<IWholeSaleProductsService, WholeSaleProductsService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPaginationUow, PaginationUow>();
            services.AddTransient<IValidator<ApplicationUser>, UsersValidator>();

            return services;
        }
    }
}
