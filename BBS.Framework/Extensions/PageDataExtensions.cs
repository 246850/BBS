using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS.Framework.Extensions
{
    public static class PageDataExtensions
    {
        public static PaginationData<TModel> ToPageData<TEntity, TModel>(this IQueryable<TEntity> queryable, int page, int pageSize) 
            where TEntity : class
            where TModel :class
        {
            int total = queryable.Count();
            if (page < 1) { page = 1; }
            if (pageSize < 1) { pageSize = 1; }
            var entities = queryable.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var models = entities.MapTo<List<TEntity>, List<TModel>>();
            
            var result = new PaginationData<TModel>(total);
            result.List.AddRange(models);
            return result;
        }

        public static PaginationData<TModel> ToPageData<TModel>(this PagedList<TModel> source)
            where TModel : class
        {
            var result = new PaginationData<TModel>(source.Total);
            result.List.AddRange(source);
            return result;
        }
        public static PaginationData<TModel> ToPageData<TEntity, TModel>(this PagedList<TEntity> source)
            where TEntity : class
            where TModel : class
        {
            var models = source.MapTo<List<TEntity>, List<TModel>>();

            var result = new PaginationData<TModel>(source.Total);
            result.List.AddRange(models);
            return result;
        }
    }
}
