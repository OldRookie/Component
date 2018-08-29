using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Component.Data.Repository
{
    public class BaseRepository<TEntity> : IDisposable
        where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository()
        {
            _dbContext = DBContextManager.DBContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        protected DbContext Context
        {
            get { return _dbContext; }
        }

        protected IDbSet<TEntity> DbSet
        {
            get { return _dbSet; }
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public virtual IEnumerable<TEntity> All(bool @readonly = false)
        {
            return @readonly
                ? DbSet.AsNoTracking().ToList()
                : DbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false)
        {
            return @readonly
                ? DbSet.Where(predicate).AsNoTracking()
                : DbSet.Where(predicate);
        }


        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.FirstOrDefault(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, List<string> includeEntities = null)
        {
            var query = this.DbSet.Where(predicate);
            if (includeEntities != null)
            {
                foreach (var item in includeEntities)
                {
                    query = query.Include(item);
                }
            }

            return query.FirstOrDefault(predicate);
        }

        public TEntity FirstOrDefaultFullEntity(Expression<Func<TEntity, bool>> predicate, List<string> includeEntities = null)
        {
            var query = this.DbSet.Where(predicate);

            foreach (var prop in typeof(TEntity).GetProperties())
            {
                if (prop.PropertyType.IsClass && prop.GetSetMethod().IsVirtual)
                {
                    query = query.Include(prop.Name);
                }
            }

            if (includeEntities != null)
            {
                foreach (var item in includeEntities)
                {
                    query = query.Include(item);
                }
            }

            return query.FirstOrDefault(predicate);
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Context == null) return;
            Context.Dispose();
        }

        #endregion
    }
}
