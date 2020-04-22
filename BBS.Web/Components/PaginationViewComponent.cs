using BBS.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBS.Web.Components
{
    /// <summary>
    /// 分页组件
    /// </summary>
    public class PaginationViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(PaginationModel pagination)
        {
            return View(pagination);
        }
    }
}
