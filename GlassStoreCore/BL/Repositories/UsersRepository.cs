using GlassStoreCore.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.Data;
using GlassStoreCore.BL.Repositories.Originals;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.BL.Repositories
{
    public class UsersRepository : Repository<ApplicationUser>, IUsersRepository
    {
        private ApplicationDbContext _dbContext;

        public UsersRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(ApplicationUser user)
        {
            this._dbContext.Users.Attach(user);

            this._dbContext.Entry(user).State = EntityState.Modified;
        }
    }

}
