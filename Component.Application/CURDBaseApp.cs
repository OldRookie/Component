using Component.Data.Repository;
using Component.Domain.Service;
using Component.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Component.Application
{
    public class CURDBaseApp<T> : BaseAppService where T : class
    {
        BaseService<T> _service = new BaseService<T>();
        BaseRepository<T> _repository = new BaseRepository<T>();
        public virtual ResponseResultBase Create(T t)
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

        public virtual ResponseResultBase Update(T t)
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

        public virtual ResponseResultBase Remove(T t)
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


        public virtual T Get(int id, bool @readonly = false)
        {
            return this._repository.Get(id);
        }


        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, bool @readonly = false)
        {
            return _repository.Find(predicate, @readonly);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
