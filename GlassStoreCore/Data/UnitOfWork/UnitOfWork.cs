using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;
using GlassStoreCore.Services.UserService;
using Microsoft.AspNetCore.Identity;

namespace GlassStoreCore.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUsersService UsersService { get; }

        private readonly GlassStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnitOfWork(GlassStoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.UsersService = new UsersService(_context, _userManager);
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
