using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GlassStoreCore.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        public (List<TEntity>, int) GetAll(int pageNumber, int pageSize);
        public TEntity FindById(params object[] primaryKeys);
        public TEntity FindByIdWithRelatedEntites(string relatedEntityName, Expression<Func<TEntity, bool>> match);
        public int Delete(TEntity entity);
        public int Add(TEntity entity);
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);
        public int Update(TEntity entity);
    }
}
