using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlassStoreCore.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        public Task<(List<TEntity>, int)> GetAll(int pageNumber, int pageSize);

        public Task<TEntity> Get(string id);

        public void Delete(string id);

        public void Add(TEntity entity);
    }
}
