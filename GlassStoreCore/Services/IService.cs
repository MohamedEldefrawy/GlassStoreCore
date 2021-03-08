using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GlassStoreCore.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        public Task<(List<TEntity>, int)> GetAllAsync(int pageNumber, int pageSize);
        public Task<TEntity> FindByIdAsync(params object[] primaryKeys);
        public Task<TEntity> FindByIdWithRelatedEntitesAsync(string relatedEntityName, Expression<Func<TEntity, bool>> match);
        public Task<int> DeleteAsync(TEntity entity);
        public Task<int> AddAsync(TEntity entity);
        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        public Task<int> UpdateAsync(TEntity entity);
        public (List<TEntity>, int) GetAll(int pageNumber, int pageSize);
        public TEntity FindById(params object[] primaryKeys);
        public TEntity FindByIdWithRelatedEntites(string relatedEntityName, Expression<Func<TEntity, bool>> match);
        public int Delete(TEntity entity);
        public int Add(TEntity entity);
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);
        public int Update(TEntity entity);
    }
}
