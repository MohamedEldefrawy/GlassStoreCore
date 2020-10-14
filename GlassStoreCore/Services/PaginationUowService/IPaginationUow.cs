using System;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.Data.UnitOfWork;

namespace GlassStoreCore.Services.PaginationUowService
{
    public interface IPaginationUow : IUnitOfWork
    {
        public Uri GetPageUri(PaginationFilter paginationFilter, string route);
    }
}
