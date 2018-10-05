using Component.Data.Repository;
using Component.Infrastructure;
using Component.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Component.Domain.Service
{
    public class BaseService<TEntity> 
       where TEntity : class
    {
        #region Constructor

        private readonly BaseRepository<TEntity> _repository;

        public BaseService()
        {
            _repository = new BaseRepository<TEntity>();
        }

        #endregion

        public virtual ValidationBaseInfo Add(TEntity entity)
        {
            var validation = entity.ValidateBaseInfo();

            if (validation.IsValid)
            {
                _repository.Add(entity);
            }
            
            return validation;
        }

        public virtual ValidationBaseInfo Update(TEntity entity)
        {
            var validation = entity.ValidateBaseInfo();

            if (validation.IsValid)
            {
                _repository.Update(entity);
            }

            return validation;
        }

        public virtual ValidationBaseInfo Delete(TEntity entity)
        {
            var validation = entity.ValidateBaseInfo();

            if (validation.IsValid)
            {
                _repository.Delete(entity);
            }

            return validation;
        }
    }

}
