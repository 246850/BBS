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
    public class UserController : Controller
    {
        private readonly BBSDbContext _context;
        public UserController(BBSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Topic(int id, int page = 1)
        {
            var list = _context.Topic.Where(x => x.AccountId == id).OrderByDescending(x => x.Id).ToPagedList<Topic, TopicModel>(page, 20);   // 主题列表，默认取20条

            // 话题
            var catalogIdList = list.Select(x => x.CatalogId).Distinct();
            var catalogs = _context.Catalog.Where(x => catalogIdList.Contains(x.Id)).ToList();

            // 评论数
            var topicIdList = list.Select(x => x.Id);
            var commentCounts = _context.Comment.Where(x => topicIdList.Contains(x.TopicId)).GroupBy(x => x.TopicId).Select(x => new { TopicId = x.Key, Count = x.Count() }).ToList();

            foreach (var item in list)
            {
                item.Catalog = catalogs.FirstOrDefault(x => x.Id == item.CatalogId).MapTo<Catalog, CatalogModel>(); // 话题

                var commentCount = commentCounts.FirstOrDefault(x => x.TopicId == item.Id);
                if (commentCount != null)
                {
                    item.CommentCount = commentCount.Count; // 评论数
                }
            }

            ViewBag.Id = id;
            return View(list);
        }

        [HttpGet]
        public IActionResult Follow(int id, int page = 1)
        {
            var queryable = from a in _context.Account
                            join f in _context.Follow on a.Id equals f.FollowAccountId
                            where f.AccountId == id
                            orderby f.Id descending
                            select a;
            var list = queryable.ToPagedList<Account, AccountModel>(page, 30);

            ViewBag.Id = id;
            return View(list);
        }

        [HttpGet]
        public IActionResult Fans(int id, int page = 1)
        {
            var queryable = from a in _context.Account
                            join f in _context.Follow on a.Id equals f.AccountId
                            where f.FollowAccountId == id
                            orderby f.Id descending
                            select a;
            var list = queryable.ToPagedList<Account, AccountModel>(page, 30);

            ViewBag.Id = id;
            return View(list);
        }

    }
}