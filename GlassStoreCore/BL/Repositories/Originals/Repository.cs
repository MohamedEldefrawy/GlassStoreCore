using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GlassStoreCore.Data;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.BL.Repositories.Originals
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add(TEntity entity)
        {
            this._context.Set<TEntity>().Add(entity);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, string children = "NO")
        {
            if (children.ToUpper() == "NO")
            {
                return this._context.Set<TEntity>().Where<TEntity>(predicate);

            }
            else
            {
                return this._context.Set<TEntity>().Include(children).Where<TEntity>(predicate);

            }
        }

        public IEnumerable<TEntity> FindAsNoTracking(Expression<Func<TEntity, bool>> predicate, string children = "NO")
        {
            if (children.ToUpper() == "NO")
            {
                return this._context.Set<TEntity>().AsNoTracking().Where<TEntity>(predicate);

            }
            else
            {
                return this._context.Set<TEntity>().Include(children).AsNoTracking().Where<TEntity>(predicate);

            }
        }


        public List<TEntity> GetAll(string children)
        {
            if (children.ToUpper() == "NO")
            {
                return this._context.Set<TEntity>().ToList<TEntity>();

            }

            else
            {
                return this._context.Set<TEntity>().Include(children).ToList<TEntity>();

            }
        }

        public void Remove(TEntity entity)
        {
            this._context.Set<TEntity>().Remove(entity);
        }

    }

}