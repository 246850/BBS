using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BBS.Framework.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TSource : class
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> queryable, Expression<Func<TSource, bool>> predicate, bool condition)
            where TSource : class
        {
            return condition ? queryable.Where(predicate) : queryable;
        }
    }
}
