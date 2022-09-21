using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            return condition ? queryable.Where(predicate) : queryable;
        }

        public static IQueryable<T> OrderByIf<T>(this IQueryable<T> queryable, bool condition, string sorting)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            return condition
            ? Dynamic.Core.DynamicQueryableExtensions.OrderBy(queryable, sorting)
            : queryable;
        }

        public static IQueryable<T> PageBy<T>(this IQueryable<T> queryable, int skipCount, int maxResultCount)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            return queryable.Skip(skipCount).Take(maxResultCount);
        }
    }
}
