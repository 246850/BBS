using BBS.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class TopicModel: BaseModel<List<SelectListItem>>
    {
        public string Title { get; set; }
        public int AccountId { get; set; }
        public int CatalogId { get; set; }
        public string Contents { get; set; }
        /// <summary>
        /// 赞数
        /// </summary>
        public int ThumbsUpCount { get; set; }
        /// <summary>
        /// 踩数
        /// </summary>
        public int ThumbsDownCount { get; set; }
        /// <summary>
        /// 浏览数
        /// </summary>
        public int TrailCount { get; set; }
        public DateTime LastUpdateTime { get; set; }

        #region 附加属性
        public CatalogModel Catalog { get; set; }
        public AccountModel Account { get; set; }
        public List<TagModel> TagList { get; set; }
        public PagedList<CommentModel> CommentList { get; set; }
        /// <summary>
        /// 当前登录人是否 赞
        /// </summary>
        public bool IsThumbsUp { get; set; }
        /// <summary>
        /// 当前登录人是否 踩
        /// </summary>
        public bool IsThumbsDown { get; set; }
        /// <summary>
        /// 是否被当前登录人收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime HappenTime { get; set; }
        #endregion
    }
}
