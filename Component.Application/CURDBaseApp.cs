using Component.Data.Repository;
using Component.Domain.Service;
using Component.Infrastructure;
using Component.Model.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Component.Application
{
    public class CURDBaseApp<TEntity> : BaseAppService where TEntity : class
    {
        BaseService<TEntity> _service = new BaseService<TEntity>();
        BaseRepository<TEntity> _repository = new BaseRepository<TEntity>();
        public virtual ResponseResultBase Create(TEntity t)
        {
            var result = new ResponseResultBase();
            result.Code = ResultCode.Success;
            BeginTransaction();
            var validationResult = _service.Add(t);
            if (validationResult.IsValid) Commit();
            else
            {
                result.ErrorMessages = validationResult.ErrorMessages;
            }

            return result;
        }

        public virtual ResponseResultBase Update(TEntity t)
        {
            var result = new ResponseResultBase();
            result.Code = ResultCode.Success;
            BeginTransaction();
            var validationResult = _service.Update(t);
            if (validationResult.IsValid) Commit();
            else
            {
                result.ErrorMessages = validationResult.ErrorMessages;
            }

            return result;
        }

        public virtual ResponseResultBase Remove(TEntity t)
        {
            var result = new ResponseResultBase();
            result.Code = ResultCode.Success;
            BeginTransaction();
            var validationResult = _service.Delete(t);
            if (validationResult.IsValid) Commit();
            else
            {
                result.ErrorMessages = validationResult.ErrorMessages;
            }

            return result;
        }


        public virtual TEntity Get(int id, bool @readonly = false)
        {
            return this._repository.Get(id);
        }


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false)
        {
            return _repository.Find(predicate, @readonly);
        }

        #region Read Methods

        public virtual TEntity Get(Guid id)
        {
            return _repository.Get(id);
        }
        
        public virtual IEnumerable<TEntity> All()
        {
            return _repository.All();
        }

        public virtual IEnumerable<TViewModel> All<TViewModel>()
        {
            return _repository.All().JsonAutoMapTo<List<TViewModel>>();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Find(predicate);
        }

        public virtual IEnumerable<TViewModel> Find<TViewModel>(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Find(predicate).JsonAutoMapTo<List<TViewModel>>();
        }

        public DataTablesResult<TViewModel> AllDataTableResult<TViewModel>()
        {
            var data = this.All<TViewModel>().ToList();

            var dataTablesResult = new DataTablesResult<TViewModel>(0, data.Count(), data.Count(), data);
            return dataTablesResult;
        }

        public DataTablesResult<TViewModel> FindEntireEntityDataTableResult<TViewModel>(Expression<Func<TEntity, bool>> predicate, Func<TEntity, TViewModel> selector)
        {
            var data = this._repository.FindEntireEntity(predicate).Select(selector).ToList()
                .JsonAutoMapTo<List<TViewModel>>();

            var dataTablesResult = new DataTablesResult<TViewModel>(0, data.Count(), data.Count(), data);
            return dataTablesResult;
        }

        public DataTablesResult<TViewModel> FindDataTableResult<TViewModel>(Expression<Func<TEntity, bool>> predicate, Func<TEntity, TViewModel> selector = null)
        {
            var data = this._repository.Find(predicate).Select(selector).ToList()
                .JsonAutoMapTo<List<TViewModel>>();

            var dataTablesResult = new DataTablesResult<TViewModel>(0, data.Count(), data.Count(), data);
            return dataTablesResult;
        }

        public DataTablesResult<TViewModel> FindDataTableResult<TViewModel>(Expression<Func<TEntity, bool>> predicate)
        {
            var data = this._repository.Find(predicate).ToList()
                .JsonAutoMapTo<List<TViewModel>>();

            var dataTablesResult = new DataTablesResult<TViewModel>(0, data.Count(), data.Count(), data);
            return dataTablesResult;
        }

        public DataTablesResult<TViewModel> AllDataTableResult<TViewModel>(Expression<Func<TEntity, TViewModel>> selector)
        {
            var data = this.All<TViewModel>().ToList();

            var dataTablesResult = new DataTablesResult<TViewModel>(0, data.Count(), data.Count(), data);
            return dataTablesResult;
        }

        #endregion

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
