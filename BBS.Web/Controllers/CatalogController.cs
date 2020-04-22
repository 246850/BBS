using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Extensions;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly BBSDbContext _context;
        public CatalogController(BBSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _context.Catalog.ToList().MapTo<List<Catalog>, List<CatalogModel>>(); //话题
            var catalogIdList = list.Select(x => x.Id);
            var counts = _context.Topic.Where(x => catalogIdList.Any(y => y == x.CatalogId)).GroupBy(x => x.CatalogId).Select(x => new { CatalogId = x.Key, Count = x.Count() }).ToList();  // 主题数量

            list.ForEach(item =>
            {
                var topicCount = counts.FirstOrDefault(x => x.CatalogId == item.Id);
                if (topicCount != null)
                {
                    item.TopicCount = topicCount.Count; // 主题数量
                }
            });

            return View(list);
        }

        [HttpGet]
        public IActionResult List(int id, int page = 1)
        {
            var model = _context.Catalog.Find(id).MapTo<Catalog, CatalogModel>();
            var list = _context.Topic.Where(x=> x.CatalogId == id).OrderByDescending(x => x.Id).ToPagedList<Topic, TopicModel>(page, 20);   // 话题下主题列表，默认取20条
            model.TopicCount = list.Total;   // 主题总数

            // 用户
            var accountIdList = list.Select(x => x.AccountId).Distinct();
            var accounts = _context.Account.Where(x => accountIdList.Contains(x.Id)).ToList();
            // 评论数
            var topicIdList = list.Select(x => x.Id);
            var commentCounts = _context.Comment.Where(x => topicIdList.Contains(x.TopicId)).GroupBy(x => x.TopicId).Select(x => new { TopicId = x.Key, Count = x.Count() }).ToList();

            foreach (var item in list)
            {
                item.Catalog = model; // 话题
                item.Account = accounts.FirstOrDefault(x => x.Id == item.AccountId).MapTo<Account, AccountModel>(); // 用户

                var commentCount = commentCounts.FirstOrDefault(x => x.TopicId == item.Id);
                if (commentCount != null)
                {
                    item.CommentCount = commentCount.Count; // 评论数
                }
            }

            ViewBag.TopicList = list;
            return View(model);
        }
    }
}