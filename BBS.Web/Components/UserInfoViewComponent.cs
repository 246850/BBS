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
    public class UserInfoViewComponent: ViewComponent
    {
        private readonly BBSDbContext _context;
        public UserInfoViewComponent(BBSDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(AccountModel model = null)
        {
            int id = Convert.ToInt32(Request.RouteValues["id"]);
            model = model ?? _context.Account.Find(id).MapTo<Account, AccountModel>();

            ViewBag.Followed = false;   // 是否已关注
            if (UserClaimsPrincipal.Identity.IsAuthenticated)   // 登录情况下检测是否关注当前用户
            {
                var loginId = Convert.ToInt32(UserClaimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
                ViewBag.Followed = _context.Follow.Any(x => x.AccountId == loginId && x.FollowAccountId == model.Id);
            }
            return View(model);
        }
    }
}
