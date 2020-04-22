using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS.Framework.Extensions
{
    public static class PagedListExtensions
    {
        public static PagedList<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> queryable, int page, int pageSize) where TEntity : class
        {
            int total = queryable.Count();
            if (page < 1) { page = 1; }
            if (pageSize < 1) { pageSize = 1; }
            var models = queryable.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PagedList<TEntity> list = new PagedList<TEntity>(page, pageSize, total);
            list.AddRange(models);
            return list;
        }

        public static PagedList<TModel> ToPagedList<TEntity, TModel>(this IQueryable<TEntity> queryable, int page, int pageSize) where TEntity : class
            where TModel : class
        {
            int total = queryable.Count();
            if (page < 1) { page = 1; }
            if (pageSize < 1) { pageSize = 1; }
            var models = queryable.Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(x => x.MapTo<TEntity, TModel>());

            PagedList<TModel> list = new PagedList<TModel>(page, pageSize, total);
            list.AddRange(models);
            return list;
        }

        public static PagedList<TModel> ToPagedList<TEntity, TModel>(this IQueryable<TEntity> queryable, int page, int pageSize, Func<TEntity, TModel> mapTo) where TEntity : class
            where TModel : class
        {
            int total = queryable.Count();
            if (page < 1) { page = 1; }
            if (pageSize < 1) { pageSize = 1; }
            var models = queryable.Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(x => mapTo(x));

            PagedList<TModel> list = new PagedList<TModel>(page, pageSize, total);
            list.AddRange(models);
            return list;
        }

        public static PaginationModel ToPagination<T>(this PagedList<T> page, string action, string controller) where T:class
        {
            return new PaginationModel(page.Page, page.PageSize, page.Total)
            {
                Action = action,
                Controller = controller
            };
        }

        public static PaginationModel ToPagination<T>(this PagedList<T> page, string action, string controller, object param) where T : class
        {
            var pagination = new PaginationModel(page.Page, page.PageSize, page.Total)
            {
                Action = action,
                Controller = controller
            };
            var type = param.GetType();
            var properties = type.GetProperties();
            string queryString = string.Join("&", properties.Select(x=> x.Name +"=" + x.GetValue(param)));

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                pagination.QueryString += "&" + queryString;
            }
            return pagination;
        }
    }
}
