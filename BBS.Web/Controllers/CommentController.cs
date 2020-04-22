using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Enums;
using BBS.Framework.Extensions;
using BBS.Models;
using BBS.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BBS.Web.Controllers
{
    public class CommentController : BBSController
    {
        private readonly BBSDbContext _context;
        public CommentController(BBSDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Publish(CommentModel model)
        {
            model.AccountId = UserModel.Id;

            if (model.AccountId == model.QuoteAccountId)
                return Json(false.ToResult("不能引用自己"));

            // 发表评论
            var entity = model.MapTo<CommentModel, Comment>();
            entity.CreateTime = DateTime.Now;
            _context.Comment.Add(entity);
            int affect = _context.SaveChanges();

            if (affect > 0) return Json(true.ToResult());
            return Json(false.ToResult());
        }

        /// <summary>
        /// 赞/踩/取消
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ThumbsUp(ThumbsUpModel model)
        {
            var deleteObj = _context.ThumbsUp.FirstOrDefault(x => x.ItemId == model.ItemId && x.IsThumb == model.IsThumb && x.ItemType == ThumbsUpItemType.Comment.ToValue() && x.AccountId == UserModel.Id);
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (deleteObj!=null)
                {
                    /***已点赞，执行移除操作***/
                    _context.ThumbsUp.Remove(deleteObj);    // 删除点赞记录
                    _context.SaveChanges();

                    string sql = $"UPDATE comment T1 SET ThumbsUpCount = ThumbsUpCount - 1 WHERE T1.Id = {model.ItemId} AND ThumbsUpCount > 0";
                    if (!model.IsThumb)
                    {
                        // 踩
                        sql = $"UPDATE comment T1 SET ThumbsDownCount = ThumbsDownCount - 1 WHERE T1.Id = {model.ItemId} AND ThumbsDownCount > 0";

                    }
                    _context.Database.ExecuteSqlRaw(sql);   // 赞数/踩数减1
                }
                else
                {
                    /***未点赞，执行新增操作***/
                    var entity = model.MapTo<ThumbsUpModel, ThumbsUp>();
                    entity.ItemType = ThumbsUpItemType.Comment.ToValue();
                    entity.AccountId = UserModel.Id;
                    entity.CreateTime = DateTime.Now;
                    _context.ThumbsUp.Add(entity);  // 插入点赞记录
                    _context.SaveChanges();

                    string sql = $"UPDATE comment SET ThumbsUpCount = ThumbsUpCount + 1 WHERE Id = {model.ItemId}";
                    if (!model.IsThumb)
                    {
                        sql = $"UPDATE comment SET ThumbsDownCount = ThumbsDownCount + 1 WHERE Id = {model.ItemId}";
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

    }
}