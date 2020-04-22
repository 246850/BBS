using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Enums;
using BBS.Framework.Extensions;
using BBS.Framework.Models;
using BBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers
{
    public class MyController : BBSController
    {
        private readonly BBSDbContext _context;
        public MyController(BBSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Info()
        {
            var model = _context.Account.Find(UserModel.Id).MapTo<Account, AccountModel>();
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(AccountModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.NickName) && Encoding.UTF8.GetByteCount(model.NickName) <= 16)
            {
                var entity = _context.Account.Find(model.Id);
                entity.NickName = model.NickName;
                _context.Account.Update(entity);
                _context.SaveChanges();
            }

            return RedirectToAction("Info", "My");
        }

        [HttpGet]
        public IActionResult Topic(int page = 1)
        {
            var list = _context.Topic.Where(x => x.AccountId == UserModel.Id).OrderByDescending(x => x.Id).ToPagedList<Topic, TopicModel>(page, 20);   // 主题列表，默认取20条

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

            return View(list);
        }

        [HttpGet]
        public IActionResult Favorite(int page = 1)
        {
            //我的收藏主题列表，默认取20条
            var queryable = from t in _context.Topic
                            join f in _context.Favorite on t.Id equals f.ItemId
                            where f.AccountId == UserModel.Id
                            orderby f.Id descending
                            select new TopicModel
                            {
                                Id = t.Id,
                                Title = t.Title,
                                AccountId = t.AccountId,
                                CatalogId = t.CatalogId,
                                Contents = t.Contents,
                                ThumbsUpCount = t.ThumbsUpCount,
                                ThumbsDownCount = t.ThumbsDownCount,
                                TrailCount = t.TrailCount,
                                CreateTime = t.CreateTime,
                                LastUpdateTime = t.LastUpdateTime,
                                HappenTime = f.CreateTime
                            };
            var list = queryable.ToPagedList(page, 20);

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

            return View(list);
        }

        [HttpPost]
        public IActionResult CancelFavorite(int id)
        {
            var entity = _context.Favorite.FirstOrDefault(x => x.ItemId == id && x.AccountId == UserModel.Id);
            if (entity != null)
            {
                _context.Favorite.Remove(entity);
                _context.SaveChanges();
            }
            return Json(true.ToResult());
        }

        [HttpGet]
        public IActionResult Comment(int page = 1)
        {
            //我的评论主题列表，默认取20条
            var queryable = from t in _context.Topic
                            join f in _context.Comment on t.Id equals f.TopicId
                            where f.AccountId == UserModel.Id
                            orderby f.Id descending
                            select new TopicModel
                            {
                                Id = t.Id,
                                Title = t.Title,
                                AccountId = t.AccountId,
                                CatalogId = t.CatalogId,
                                Contents = t.Contents,
                                ThumbsUpCount = t.ThumbsUpCount,
                                ThumbsDownCount = t.ThumbsDownCount,
                                TrailCount = t.TrailCount,
                                CreateTime = t.CreateTime,
                                LastUpdateTime = t.LastUpdateTime
                            };
            var list = queryable.DistinctBy(x=> x.Id).AsQueryable().ToPagedList(page, 20);
            //var list = _context.Topic.Where(x => _context.Comment.Where(y => y.AccountId == UserModel.Id).Any(y => y.TopicId == x.Id)).ToPagedList<Topic, TopicModel>(page, 20);

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

            return View(list);
        }

        [HttpGet]
        public IActionResult ThumbsUp(int page = 1)
        {
            //我的点赞主题列表，默认取20条
            var queryable = from t in _context.Topic
                            join f in _context.ThumbsUp on t.Id equals f.ItemId
                            where f.AccountId == UserModel.Id && f.IsThumb && f.ItemType == ThumbsUpItemType.Topic.ToValue()
                            orderby f.Id descending
                            select new TopicModel
                            {
                                Id = t.Id,
                                Title = t.Title,
                                AccountId = t.AccountId,
                                CatalogId = t.CatalogId,
                                Contents = t.Contents,
                                ThumbsUpCount = t.ThumbsUpCount,
                                ThumbsDownCount = t.ThumbsDownCount,
                                TrailCount = t.TrailCount,
                                CreateTime = t.CreateTime,
                                LastUpdateTime = t.LastUpdateTime,
                                HappenTime = f.CreateTime
                            };
            var list = queryable.ToPagedList(page, 20);

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

            return View(list);
        }

        [HttpGet]
        public IActionResult ThumbsDown(int page = 1)
        {
            //我的点踩主题列表，默认取20条
            var queryable = from t in _context.Topic
                            join f in _context.ThumbsUp on t.Id equals f.ItemId
                            where f.AccountId == UserModel.Id && !f.IsThumb && f.ItemType == ThumbsUpItemType.Topic.ToValue()
                            orderby f.Id descending
                            select new TopicModel
                            {
                                Id = t.Id,
                                Title = t.Title,
                                AccountId = t.AccountId,
                                CatalogId = t.CatalogId,
                                Contents = t.Contents,
                                ThumbsUpCount = t.ThumbsUpCount,
                                ThumbsDownCount = t.ThumbsDownCount,
                                TrailCount = t.TrailCount,
                                CreateTime = t.CreateTime,
                                LastUpdateTime = t.LastUpdateTime,
                                HappenTime = f.CreateTime
                            };
            var list = queryable.ToPagedList(page, 20);

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

            return View(list);
        }

        [HttpGet]
        public IActionResult Follow(int page = 1)
        {
            var queryable = from a in _context.Account
                           join f in _context.Follow on a.Id equals f.FollowAccountId
                           where f.AccountId == UserModel.Id
                           orderby f.Id descending
                           select a;
            var list = queryable.ToPagedList<Account, AccountModel>(page, 30);
            return View(list);
        }

        [HttpGet]
        public IActionResult Fans(int page = 1)
        {
            var queryable = from a in _context.Account
                            join f in _context.Follow on a.Id equals f.AccountId
                            where f.FollowAccountId == UserModel.Id
                            orderby f.Id descending
                            select a;
            var list = queryable.ToPagedList<Account, AccountModel>(page, 30);
            return View(list);
        }
    }

    public class TopicModelComparer : IEqualityComparer<TopicModel>
    {
        public bool Equals([AllowNull] TopicModel x, [AllowNull] TopicModel y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] TopicModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}