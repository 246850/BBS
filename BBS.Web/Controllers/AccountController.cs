using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Helpers;
using BBS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly BBSDbContext _context;
        public AccountController(BBSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await HttpContext.SignOutAsync();
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountModel model)
        {
            if(!Regex.IsMatch(model.Account1, @"^[a-z|A-z|_]\w{3,29}$") || !Regex.IsMatch(model.Password, @"^\w{6,20}$") || string.IsNullOrWhiteSpace(model.NickName) || Encoding.UTF8.GetByteCount(model.NickName) > 16)
            {
                return View(model);
            }

            var entity = new Account();
            entity.Account1 = model.Account1.ToLower();
            entity.NickName = model.NickName;
            entity.Avatar = "/images/avatar_none.png";
            entity.Password = Md5Helper.Encrypt(model.Password);
            entity.CreateTime = DateTime.Now;
            _context.Account.Add(entity);

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult CheckAccount(string account)
        {
            account = account.Trim();
            if (string.IsNullOrWhiteSpace(account))
            {
                return Json(true);
            }
            return Json(_context.Account.Any(x=> x.Account1 == account));
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            await HttpContext.SignOutAsync();

            string refererUrl = Request.Headers["Referer"];
            if (string.IsNullOrWhiteSpace(returnUrl) && !string.IsNullOrWhiteSpace(refererUrl) && !refererUrl.Contains("/account/login", StringComparison.InvariantCultureIgnoreCase) && !refererUrl.Contains("/account/register", StringComparison.InvariantCultureIgnoreCase))
            {
                returnUrl = refererUrl;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountModel model, string returnUrl = "")
        {
            string account = model.Account1.ToLower(),
                password = Md5Helper.Encrypt(model.Password.ToLower());
            var entity = _context.Account.FirstOrDefault(x => x.Account1 == account && x.Password == password);
            if(entity == null)
            {
                ModelState.AddModelError("login.error", "账号密码不正确");
                return View(model);
            }

            // 写入cookie
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Sid, entity.Id.ToString()),
                 new Claim(ClaimTypes.NameIdentifier, entity.Account1),
                 new Claim(ClaimTypes.Name, entity.NickName),
                 new Claim(ClaimTypes.Actor, entity.Avatar)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut(string returnUrl = "")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            string refererUrl = Request.Headers["Referer"];
            if (!string.IsNullOrWhiteSpace(refererUrl))
            {
                return Redirect(refererUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}