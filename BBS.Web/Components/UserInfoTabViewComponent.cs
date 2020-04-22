using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBS.Web.Components
{
    public class UserInfoTabViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(int id)
        {
            return View(id);
        }
    }
}
