using BBS.Domain;
using BBS.Framework.Extensions;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBS.Web.Components
{
    public class Top10NewAccountViewComponent : ViewComponent
    {
        private readonly BBSDbContext _context;
        public Top10NewAccountViewComponent(BBSDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var list = _context.Account.OrderByDescending(x => x.Id).Take(10).ToList().MapTo<List<Account>, List<AccountModel>>();
            return View(list);
        }
    }
}
