using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Component.Data
{
    public static class QueryableExtension {
        /// <summary>
        /// Filter by RecordStatus
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <returns></returns>
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate = null)
        {
            if (typeof(TSource).GetProperty("RecordStatus") != null)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TSource), "status");

                Expression left = Expression.Property(parameter, typeof(TSource).GetProperty("RecordStatus"));
                Expression right = Expression.Constant((int)-1);
                Expression statusFilterExp = Expression.NotEqual(left, right);

                source = source.Provider.CreateQuery<TSource>(
                     Expression.Call(
                         typeof(Queryable),
                         "Where",
                         new Type[] { source.ElementType },
                         source.Expression,
                         Expression.Lambda<Func<TSource, bool>>(statusFilterExp, new ParameterExpression[] { parameter }))
                         );
            }

            if (predicate != null)
            {
                source = source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { source.ElementType },
                    new Expression[] { source.Expression, Expression.Quote(predicate) }));
            }
            return source;
        }

        /// <summary>
        ///  Filter by RecordStatus
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = null)
        {
            if (typeof(TSource).GetProperty("RecordStatus") != null)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TSource), "status");

                Expression left = Expression.Property(parameter, typeof(TSource).GetProperty("RecordStatus"));
                Expression right = Expression.Constant((int)-1);
                Expression statusFilterExp = Expression.NotEqual(left, right);
                Expression<Func<TSource, bool>> statusFilterExpDel =
                    Expression.Lambda<Func<TSource, bool>>(statusFilterExp, new ParameterExpression[] { parameter });

                Func<TSource, bool> statusFilteFunc = statusFilterExpDel.Compile();

                source = source.Where(statusFilteFunc);
            }

            if (predicate != null)
            {
                source = source.Where(predicate);
            }
            return source;
        }
    }
}
