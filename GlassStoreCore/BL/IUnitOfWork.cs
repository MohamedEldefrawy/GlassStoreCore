using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.Repositories;

namespace GlassStoreCore.BL
{
    interface IUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
        Task<int> Complete();
    }
}
