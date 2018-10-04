using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using Component.ViewModel;
using System.Text.RegularExpressions;
using Component.Model;
using System.Linq.Dynamic;

namespace Component.Data.Repository
{
    public class BaseRepository<TEntity> : IBaseDataSource<TEntity>, IDisposable
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


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, string predicateText, object[] parameters, bool @readonly = false)
        {
            return @readonly
                ? DbSet.Where(predicate).Where(predicateText, parameters).AsNoTracking()
                : DbSet.Where(predicate).Where(predicateText, parameters);
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

        public IEnumerable<TEntity> IQueryable(Pagination pagination) {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = _dbContext.Set<TEntity>().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData;
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
