using System;
using System.Collections.Generic;
using System.Linq;
using BBS.Domain;
using BBS.Framework.Configs;
using BBS.Framework.Enums;
using BBS.Framework.Extensions;
using BBS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BBS.Web.Controllers
{
    public class TopicController : BBSController
    {
        private readonly ILogger<TopicController> _logger;
        private readonly BBSDbContext _context;
        public TopicController(BBSDbContext context, ILogger<TopicController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Detail(int id, int page = 1)
        {
            var model = _context.Topic.Find(id).MapTo<Topic, TopicModel>(); // 主题
            model.Catalog = _context.Catalog.Find(model.CatalogId).MapTo<Catalog, CatalogModel>();  // 话题
            model.Catalog.TopicCount = _context.Topic.Where(x => x.CatalogId == model.CatalogId).Count();   // 主题总数
            model.Account = _context.Account.Find(model.AccountId).MapTo<Account, AccountModel>();  // 用户

            /*****标签*****/
            //string tagSql = @"SELECT T1.Id, T1.TagName, T1.CreateTime FROM tag T1
            //    INNER JOIN topic_tag T2 ON T1.Id = T2.TagId
            //    WHERE T2.TopicId = @TopicId ORDER BY T1.Id ASC";
            //var parameter = _context.Database.GetDbConnection().CreateParameter("@TopicId", id);
            //model.TagList = _context.Tag.FromSqlRaw(tagSql, parameter).ToList().MapTo<List<Tag>, List<TagModel>>();   // 原生SQL写法
            //model.TagList = (from tag in _context.Tag join topictag in _context.TopicTag on tag.Id equals topictag.TagId where topictag.TopicId == id select tag).OrderBy(x => x.Id).ToList().MapTo<List<Tag>, List<TagModel>>();    // Linq写法
            model.TagList = _context.Tag.Join(_context.TopicTag.Where(x => x.TopicId == id), tag => tag.Id, topictag => topictag.TagId, (tag, topictag) => tag).OrderBy(x => x.Id).ToList().MapTo<List<Tag>, List<TagModel>>();  // Lamda写法

            model.IsThumbsUp = _context.ThumbsUp.Any(x => x.ItemId == model.Id && x.IsThumb && x.AccountId == UserModel.Id && x.ItemType == ThumbsUpItemType.Topic.ToValue());  // 当前登录人主题是否赞
            model.IsThumbsDown = _context.ThumbsUp.Any(x => x.ItemId == model.Id && !x.IsThumb && x.AccountId == UserModel.Id && x.ItemType == ThumbsUpItemType.Topic.ToValue());  // 当前登录人主题是否踩
            model.IsFavorite = _context.Favorite.Any(x => x.ItemId == model.Id && x.AccountId == UserModel.Id);    // 当前登录人主题是否收藏

            // 评论列表，默认取15条
            model.CommentList = _context.Comment.Where(x => x.TopicId == id).OrderBy(x => x.Id).ToPagedList<Comment, CommentModel>(page, 15);
            // 评论所属用户
            var accountIdList = model.CommentList.Select(x => x.AccountId).ToList();
            var accounts = _context.Account.Where(x => accountIdList.Contains(x.Id)).ToList().MapTo<List<Account>, List<AccountModel>>();
            // 评论引用用户
            var quoteIdList = model.CommentList.Select(x => x.QuoteAccountId).Distinct().ToList();
            var quotes = _context.Account.Where(x => quoteIdList.Contains(x.Id)).ToList().MapTo<List<Account>, List<AccountModel>>();
            // 当前登录人评论是否赞/踩
            var commentIdList = model.CommentList.Select(x => x.Id).ToList();
            var thumbsUpList = _context.ThumbsUp.Where(x => commentIdList.Contains(x.ItemId) && x.AccountId == UserModel.Id && x.ItemType == ThumbsUpItemType.Comment.ToValue()).ToList(); // 当前登录人评论赞/踩列表
            model.CommentList.ForEach(item =>
            {
                item.Account = accounts.FirstOrDefault(x => x.Id == item.AccountId); // 评论所属用户

                var quoteAccount = quotes.FirstOrDefault(x => x.Id == item.QuoteAccountId);
                if (quoteAccount != null)
                {
                    item.QuoteAccount = quoteAccount;   // 评论引用用户
                }
                item.IsThumbsUp = thumbsUpList.Any(x => x.ItemId == item.Id && x.IsThumb);  // 当前登录人评论是否赞
                item.IsThumbsDown = thumbsUpList.Any(x => x.ItemId == item.Id && !x.IsThumb);  // 当前登录人评论是否踩
            });

            _context.Database.ExecuteSqlRaw($"UPDATE topic SET TrailCount = TrailCount + 1 where Id = {id}");   // 更新浏览数
            _context.SaveChanges();
            // 插入浏览记录
            if (UserModel.IsLogin && UserModel.Id > 0)
            {
                _context.Trail.Add(new Trail
                {
                    AccountId = UserModel.Id,
                    ItemId = id,
                    CreateTime = DateTime.Now
                });
                _context.SaveChanges();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Publish()
        {
            TopicModel model = new TopicModel();
            PrepareCatalog(model);
            return View(model);
        }

        /// <summary>
        /// 发表主题
        /// </summary>
        [HttpPost]
        public IActionResult Publish(TopicModel model)
        {
            IDbContextTransaction tran = _context.Database.BeginTransaction();
            try
            {
                // 主题
                var entity = model.MapTo<TopicModel, Topic>();
                var now = DateTime.Now;
                if (entity.CatalogId <= 0) { entity.CatalogId = 1; }
                entity.AccountId = UserModel.Id;
                entity.CreateTime = now;
                entity.LastUpdateTime = now;
                _context.Topic.Add(entity);
                _context.SaveChanges();

                if (model.TagList != null) model.TagList.RemoveAll(x => string.IsNullOrWhiteSpace(x.TagName));
                if (model.TagList != null && model.TagList.Count > 0)
                {
                    // 标签
                    var tags = model.TagList.MapTo<List<TagModel>, List<Tag>>();
                    tags.ForEach(x => x.CreateTime = now);
                    _context.Tag.AddRange(tags);
                    _context.SaveChanges();

                    // 主题 - 标签 多对多
                    var topicTags = tags.Select(x => new TopicTag { TopicId = entity.Id, TagId = x.Id }).ToList();
                    topicTags.ForEach(x => x.CreateTime = now);
                    _context.TopicTag.AddRange(topicTags);
                    _context.SaveChanges();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发表错误");
                tran?.Rollback();
            }
            finally
            {
                tran?.Dispose();
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 赞/踩/取消
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ThumbsUp(ThumbsUpModel model)
        {
            var deleteObj = _context.ThumbsUp.FirstOrDefault(x => x.ItemId == model.ItemId && x.IsThumb == model.IsThumb && x.ItemType == ThumbsUpItemType.Topic.ToValue() && x.AccountId == UserModel.Id);
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (deleteObj != null)
                {
                    /***已点赞，执行移除操作***/
                    _context.ThumbsUp.Remove(deleteObj);    // 删除点赞记录
                    _context.SaveChanges();

                    string sql = $"UPDATE topic T1 SET ThumbsUpCount = ThumbsUpCount - 1 WHERE T1.Id = {model.ItemId} AND ThumbsUpCount > 0";
                    if (!model.IsThumb)
                    {
                        // 踩
                        sql = $"UPDATE topic T1 SET ThumbsDownCount = ThumbsDownCount - 1 WHERE T1.Id = {model.ItemId} AND ThumbsDownCount > 0";

                    }
                    _context.Database.ExecuteSqlRaw(sql);   // 赞数/踩数减1
                }
                else
                {
                    /***未点赞，执行新增操作***/
                    var entity = model.MapTo<ThumbsUpModel, ThumbsUp>();
                    entity.ItemType = ThumbsUpItemType.Topic.ToValue();
                    entity.AccountId = UserModel.Id;
                    entity.CreateTime = DateTime.Now;
                    _context.ThumbsUp.Add(entity);  // 插入点赞记录
                    _context.SaveChanges();

                    string sql = $"UPDATE topic SET ThumbsUpCount = ThumbsUpCount + 1 WHERE Id = {model.ItemId}";
                    if (!model.IsThumb)
                    {
                        sql = $"UPDATE topic SET ThumbsDownCount = ThumbsDownCount + 1 WHERE Id = {model.ItemId}";
                    }
                    _context.Database.ExecuteSqlRaw(sql);   // 赞数/踩数加1
                }
                transaction.Commit();   // 提交事务
                return Json(true.ToResult());
            }
            catch (DbUpdateException ex)
            {
                transaction?.Rollback();
                return Json(false.ToResult("请勿重复操作"));
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return Json(false.ToResult("未知错误，请联系管理人员"));
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        /// <summary>
        /// 收藏主题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Favorite(FavoriteModel model)
        {
            try
            {
                var entity = model.MapTo<FavoriteModel, Favorite>();
                entity.AccountId = UserModel.Id;
                entity.CreateTime = DateTime.Now;
                _context.Favorite.Add(entity);
                _context.SaveChanges();
                return Json(true.ToResult());
            }
            catch (DbUpdateException ex)
            {
                return Json(false.ToResult("请勿重复操作"));
            }
            catch (Exception ex)
            {
                return Json(false.ToResult("未知错误，请联系管理人员"));
            }
        }

        #region 准备自定义属性
        void PrepareCatalog(TopicModel model)
        {
            model.CustomProperties.Add(PropertiesKey.Key_1, _context.Catalog.ToList().ToSelectListItem(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }, true));
        }
        #endregion 
    }
}