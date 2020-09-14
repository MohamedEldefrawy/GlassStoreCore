using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Models;

namespace GlassStoreCore.BL.Repositories
{
    public interface IUsersRepository
    {
        void Update(ApplicationUser user);

    }
}
