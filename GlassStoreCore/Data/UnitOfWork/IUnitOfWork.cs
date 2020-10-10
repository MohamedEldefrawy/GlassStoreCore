using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using GlassStoreCore.Services.WholeSaleProductsService;

namespace GlassStoreCore.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersService UsersService { get; }
        IRolesService RolesService { get; }
        IUsersRolesService UsersRolesService { get; }

        IWholeSaleProductsService WholeSaleProductsService { get; }

        Task<int> Complete();
    }
}
