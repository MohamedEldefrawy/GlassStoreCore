using FluentValidation;
using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs.UsersDtos;
using GlassStoreCore.BL.DTOs.WholeSaleProductsDtos;
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
            services.AddTransient<ObjectMapper>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<IUsersRolesService, UsersRolesService>();
            services.AddTransient<IWholeSaleProductsService, WholeSaleProductsService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPaginationUow, PaginationUow>();
            services.AddTransient<IValidator<CreateUserDto>, UsersValidator>();
            services.AddTransient<IValidator<WholeSaleProductsDto>, WholeSaleProductsValidator>();


            return services;
        }
    }
}
