using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GlassStoreCore.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        public Task<(List<TEntity>, int)> GetAll(int pageNumber, int pageSize);
        public Task<TEntity> FindById(params object[] primaryKeys);
        public Task<TEntity> FindByIdWithRelatedEntites(string relatedEntityName, Expression<Func<TEntity, bool>> match);

        public Task<int> DeleteAsync(TEntity entity);
        public Task<int> Add(TEntity entity);
        public Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter);
        public Task<int> UpdateAsync(TEntity entity);
        public void DetachEntity(TEntity entity);
    }
}
