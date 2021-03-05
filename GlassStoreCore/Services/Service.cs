using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.Data;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private readonly GlassStoreContext _context;

        public Service(GlassStoreContext context)
        {
            _context = context;
        }

        public async Task<(List<TEntity>, int)> GetAll(int pageNumber, int pageSize)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);
            var result = await _context.Set<TEntity>().AsNoTracking().
                                            Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                            .Take(validFilter.PageSize)
                                            .ToListAsync();
            var totalRecords = await _context.Set<TEntity>().CountAsync();
            return (result, totalRecords);

        }

        public async Task<TEntity> FindById(params object[] primaryKeys)
        {
            var result = await _context.Set<TEntity>().FindAsync(primaryKeys);
            _context.Entry(result).State = EntityState.Unchanged;
            return result;
        }

        public async Task<int> Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public void DetachEntity(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(filter).ToListAsync();
        }


        public async Task<int> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
