using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Configs;
using BBS.Framework.Extensions;
using BBS.Framework.Helpers;
using BBS.Framework.Models;
using BBS.Models;
using BBS.Requests;
using BBS.Requests.Admin;
using BBS.Web.Areas.Admin.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BBS.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        private readonly BBSDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public HomeController(BBSDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            DateTime end = DateTime.Now,
                begin = end.AddDays(-7);
            BeginEndRequest model = new BeginEndRequest()
            {
                Begin = begin,
                End = end
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(BeginEndRequest request)
        {
            var userCount = _context.Account.WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue).Count();
            var topicCount = _context.Topic.WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue).Count();
            var commentCount = _context.Comment.WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue).Count();
            var data = new { userCount , topicCount, commentCount};
            return Json(data.Success());

        }

        #region 用户管理
        [HttpGet]
        public IActionResult Account()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Account(BeginEndRequest request)
        {
            var data = _context.Account.WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue)
                .OrderByDescending(x => x.Id)
                .ToPageData<Account, AccountModel>(request.Page, request.PageSize);
            return Json(data.Success());
        }
        #endregion

        #region 话题管理

        [HttpGet]
        public IActionResult Catalog()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Catalog(PageRequest request)
        {
            var list = _context.Catalog.OrderByDescending(x => x.Id).ToPagedList<Catalog, CatalogModel>(request.Page, request.PageSize);
            var catalogIdList = list.Select(x => x.Id);
            var topicCounts = _context.Topic.Where(x => catalogIdList.Contains(x.CatalogId)).GroupBy(x => x.CatalogId).Select(x => new { CatalogId = x.Key, Count = x.Count() }).ToList();
            list.ForEach(item =>
            {
                var topicCount = topicCounts.FirstOrDefault(x => x.CatalogId == item.Id);
                if (topicCount != null)
                {
                    item.TopicCount = topicCount.Count;
                }
            });
            var data = list.ToPageData();
            return Json(data.Success());
        }
        [HttpGet]
        public IActionResult CatalogAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CatalogAdd(CatalogModel model)
        {
            var entity = model.MapTo<CatalogModel, Catalog>();
            entity.CreateTime = DateTime.Now;
            entity.Cover = model.Cover.Upload(_hostEnvironment.WebRootPath, "catalog");
            _context.Catalog.Add(entity);
            _context.SaveChanges();
            return Json(true.ToResult());
        }
        [HttpPost]
        public IActionResult CatalogDelete(int id)
        {
            var entity = _context.Catalog.Find(id);
            if(entity!=null)
            {
                _context.Catalog.Remove(entity);
                _context.SaveChanges();
            }

            return Json(true.ToResult());
        }
        #endregion

        #region 主题管理
        [HttpGet]
        public IActionResult Topic()
        {
            TopicListRequest model = new TopicListRequest();
            PrepareCatalog(model);
            return View(model);
        }
        [HttpPost]
        public IActionResult Topic(TopicListRequest request)
        {
            var list = _context.Topic.WhereIf(x => x.Title.Contains(request.Title), !string.IsNullOrWhiteSpace(request.Title))
                .WhereIf(x=> x.AccountId == request.AccountId, request.AccountId > 0)
                .WhereIf(x=> x.CatalogId == request.CatalogId, request.CatalogId > 0)
                .WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue)
                .OrderByDescending(x=> x.Id).ToPagedList<Topic, TopicModel>(request.Page, request.PageSize);

            return Json(list.ToPageData().Success());
        }
        #endregion

        #region 评论管理
        [HttpGet]
        public IActionResult Comment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Comment(CommentListRequest request)
        {
            var data = _context.Comment.WhereIf(x => x.TopicId == request.TopicId, request.TopicId > 0)
                .WhereIf(x => x.AccountId == request.AccountId, request.AccountId > 0)
                .WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue)
                .OrderByDescending(x => x.Id).ToPageData<Comment, CommentModel>(request.Page, request.PageSize);

            return Json(data.Success());
        }
        #endregion

        #region 后台账号
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Admin(PageRequest request)
        {
            var data = _context.Admin.OrderByDescending(x => x.Id).ToPageData<Domain.Admin, AdminModel>(request.Page, request.PageSize);
            return Json(data.Success());
        }
        [HttpGet]
        public IActionResult AdminAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminAdd(AdminModel model)
        {
            var entity = model.MapTo<AdminModel, Domain.Admin>();
            entity.Account = entity.Account.Trim().ToLower();
            entity.CreateTime = DateTime.Now;
            entity.Password = Md5Helper.Encrypt(model.Password.Trim());
            _context.Admin.Add(entity);
            _context.SaveChanges();
            return Json(true.ToResult());
        }
        [HttpPost]
        public IActionResult AdminDelete(int id)
        {
            var entity = _context.Admin.Find(id);
            if (entity != null)
            {
                _context.Admin.Remove(entity);
                _context.SaveChanges();
            }

            return Json(true.ToResult());
        }
        #endregion

        #region 后台登录
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(AdminModel model)
        {
            string account = model.Account.ToLower(),
                password = Md5Helper.Encrypt(model.Password.ToLower());
            var entity = _context.Admin.FirstOrDefault(x => x.Account == account && x.Password == password);
            if (entity == null)
            {
                ModelState.AddModelError("login.error", "账号密码不正确");
                return View(model);
            }

            // 写入cookie
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Sid, entity.Id.ToString()),
                 new Claim(ClaimTypes.NameIdentifier, entity.Account),
                 new Claim(ClaimTypes.Name, entity.NickName)
            }, AdminAuthenticationScheme.AdminScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(AdminAuthenticationScheme.AdminScheme, claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(AdminAuthenticationScheme.AdminScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 反馈留言
        [HttpGet]
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Feedback(BeginEndRequest request)
        {
            var data = _context.Feedback.WhereIf(x => x.CreateTime >= request.Begin.Value.Date, request.Begin.HasValue)
                .WhereIf(x => x.CreateTime < request.End.Value.Date.AddDays(1), request.End.HasValue)
                .OrderByDescending(x => x.Id)
                .ToPageData<Feedback, FeedbackModel>(request.Page, request.PageSize);
            return Json(data.Success());
        }
        #endregion

        #region 准备SelectListItem
        void PrepareCatalog(IHasCustomProperties<List<SelectListItem>> model)
        {
            var list = _context.Catalog.OrderByDescending(x => x.Id).ToList();
             var items = list.ToSelectListItem(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }, true);
            model.CustomProperties.Add(PropertiesKey.Key_1, items);
        }
        #endregion
    }
}