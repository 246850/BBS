using BBS.Domain;
using BBS.Framework.Extensions;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BBS.Web.Components
{
    public class MyInfoViewComponent : ViewComponent
    {
        private readonly BBSDbContext _context;
        public MyInfoViewComponent(BBSDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(AccountModel model = null)
        {
            model = model ?? _context.Account.Find(Convert.ToInt32(UserClaimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Sid).Value)).MapTo<Account, AccountModel>();
            return View(model);
        }
    }
}
