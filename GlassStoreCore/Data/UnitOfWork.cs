using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL;
using GlassStoreCore.BL.Repositories;

namespace GlassStoreCore.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUsersRepository UsersRepository { get; }

        private ApplicationDbContext dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.UsersRepository = new UsersRepository(dbContext);
        }

        public void Dispose()
        {
            this.dbContext.DisposeAsync();
        }

        public Task<int> Complete()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
