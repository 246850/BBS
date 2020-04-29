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

        /// <summary>
        /// 动态排序 - 根据字段名转表达式
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="queryable">查询表达式</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="asc">true:升序，false:降序</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> queryable, string propertyName, bool asc)
             where TSource : class
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var propertyInfo = queryable.ElementType.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"{nameof(propertyName)}不是类{queryable.ElementType.FullName}的属性字段", propertyName);

            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "x");
            MemberExpression property = Expression.Property(parameter, propertyInfo);
            LambdaExpression sort = Expression.Lambda(property, parameter);

            MethodCallExpression call = Expression.Call(
                                    typeof(Queryable),
                                    asc ?"OrderBy" : "OrderByDescending",
                                    new[] { typeof(TSource), property.Type },
                                    queryable.Expression,
                                    Expression.Quote(sort));

            return (IOrderedQueryable<TSource>)queryable.Provider.CreateQuery<TSource>(call);
        }

        public static Expression<Func<TSource, TKey>> BuildExpression<TSource, TKey>(this string propertyName)
            where TSource:class
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(TKey));   //important to use the Expression.Convert
            return Expression.Lambda<Func<TSource, TKey>>(conversion, param);
        }
    }
}
