using System;
using System.Collections;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Services;
using GlassStoreCore.Services.UserService;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUsersService UsersService { get; }

        private readonly GlassStoreContext _context;
        private Hashtable _services;

        public UnitOfWork(GlassStoreContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            UsersService = new UsersService(_context, userManager, signInManager);
        }

        public IService<TEntity> Service<TEntity>() where TEntity : class
        {
            _services ??= new Hashtable();
            var type = typeof(Service<>).MakeGenericType(typeof(TEntity));
            if (_services.ContainsKey(typeof(TEntity).Name)) return (Service<TEntity>)_services[typeof(TEntity)];

            var service = (Service<TEntity>)Activator.CreateInstance(type, _context);
            _services.Add(typeof(TEntity).Name, service);

            return service;
        }

        public IUsersService GetUsersService()
        {
            return UsersService;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async Task<int> Complete()
        {
            return await this._context.SaveChangesAsync();
        }
    }
}
