using System;
using System.Threading.Tasks;
using GlassStoreCore.Services;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;

namespace GlassStoreCore.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IService<TEntity> Service<TEntity>() where TEntity : class;
        public IUsersService GetUsersService();
        public IRolesService GetRolesService();
        public Task<int> Complete();
    }
}
