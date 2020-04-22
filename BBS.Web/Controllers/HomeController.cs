using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Configs;
using BBS.Framework.Extensions;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace BBS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly BBSDbContext _context;
        public HomeController(BBSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string s = "", int page = 1)
        {
            var list = _context.Topic.WhereIf(x=> x.Title.Contains(s) || x.Contents.Contains(s), !string.IsNullOrWhiteSpace(s)).OrderByDescending(x => x.Id).ToPagedList<Topic, TopicModel>(page, 20);   // 主题列表，默认取20条

            // 话题
            var catalogIdList = list.Select(x => x.CatalogId).Distinct();
            var catalogs = _context.Catalog.Where(x => catalogIdList.Contains(x.Id)).ToList();
            // 用户
            var accountIdList = list.Select(x => x.AccountId).Distinct();
            var accounts = _context.Account.Where(x => accountIdList.Contains(x.Id)).ToList();
            // 评论数
            var topicIdList = list.Select(x => x.Id);
            var commentCounts = _context.Comment.Where(x => topicIdList.Contains(x.TopicId)).GroupBy(x => x.TopicId).Select(x=> new { TopicId = x.Key, Count = x.Count() }).ToList();

            foreach (var item in list)
            {
                item.Catalog = catalogs.FirstOrDefault(x => x.Id == item.CatalogId).MapTo<Catalog, CatalogModel>(); // 话题
                item.Account = accounts.FirstOrDefault(x => x.Id == item.AccountId).MapTo<Account, AccountModel>(); // 用户

                var commentCount = commentCounts.FirstOrDefault(x => x.TopicId == item.Id);
                if (commentCount != null)
                {
                    item.CommentCount = commentCount.Count; // 评论数
                }
            }

            ViewBag.S = s;
            return View(list);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Feedback(FeedbackModel model)
        {
            var entity = model.MapTo<FeedbackModel, Feedback>();
            entity.CreateTime = DateTime.Now;
            _context.Feedback.Add(entity);
            int affect = _context.SaveChanges();
            if(affect >0) return RedirectToAction("Success", "Home");

            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
