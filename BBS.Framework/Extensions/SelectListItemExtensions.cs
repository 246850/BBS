using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS.Framework.Extensions
{
    public static class SelectListItemExtensions
    {
        public static List<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> source, Func<T, SelectListItem> func) where T : class, new()
        {
            return ToSelectListItem(source, func, false);
        }

        public static List<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> source, Func<T, SelectListItem> func, bool defaulted) where T : class, new()
        {
            var list = source.Select(x => func(x)).ToList();
            if (!defaulted) return list;

            list.Insert(0, new SelectListItem()
            {
                Text = "请选择",
                Value = "-1"
            });
            return list;
        }
    }
}
