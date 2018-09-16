﻿using Component.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.IRepository
{
    public interface IBaseDataSource<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        IEnumerable<TEntity> All(bool @readonly = false);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false);


        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, List<string> includeEntities = null);

        TEntity FirstOrDefaultFullEntity(Expression<Func<TEntity, bool>> predicate, List<string> includeEntities = null);

        IEnumerable<TEntity> IQueryable(Pagination pagination);
    }
}