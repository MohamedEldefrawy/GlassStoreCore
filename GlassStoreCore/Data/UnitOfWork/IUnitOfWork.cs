using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.Services.UserService;

namespace GlassStoreCore.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersService UsersService { get; }
        Task<int> Complete();
    }
}
