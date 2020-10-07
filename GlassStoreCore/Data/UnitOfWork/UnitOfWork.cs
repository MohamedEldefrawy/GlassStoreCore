using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Services.RolesService;
using GlassStoreCore.Services.UserService;
using GlassStoreCore.Services.WholeSaleProductsService;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUsersService UsersService { get; }
        public IRolesService RolesService { get; }
        public IUsersRolesService UsersRolesService { get; }

        public IWholeSaleProductsService WholeSaleProductsService { get; }

        private readonly GlassStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnitOfWork(GlassStoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.UsersService = new UsersService(_context, _userManager);
            RolesService = new RolesService(_context);
            UsersRolesService = new UsersRolesService(_context);
            WholeSaleProductsService = new WholeSaleProductsService(_context);
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
