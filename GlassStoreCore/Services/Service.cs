using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.Data;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services
{
    public class Service<TEntity> :  IService<TEntity> where TEntity : class
    {
        private readonly GlassStoreContext _glassStoreContext;

        public Service(GlassStoreContext glassStoreContext)
        {
            _glassStoreContext = glassStoreContext;
        }

        public async Task<(List<TEntity>, int)> GetAll(int pageNumber, int pageSize)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);
            var result = await _glassStoreContext.Set<TEntity>().AsNoTracking().
                                            Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                            .Take(validFilter.PageSize)
                                            .ToListAsync();
            var totalRecords = await _glassStoreContext.Set<TEntity>().CountAsync();
            return (result, totalRecords);

        }

        public async Task<TEntity> Get(string id)
        {
            return await _glassStoreContext.Set<TEntity>().FindAsync(id);
        }

        public async void Delete(string id)
        {
            var entity = await _glassStoreContext.Set<TEntity>().FindAsync(id);
            _glassStoreContext.Set<TEntity>().Remove(entity);
        }

        public async void Add(TEntity entity)
        {
            await _glassStoreContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _glassStoreContext.Set<TEntity>().Update(entity);
        }
    }
}
