using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class CommentModel:BaseModel
    {
        public int TopicId { get; set; }
        public int AccountId { get; set; }
        public int QuoteAccountId { get; set; }
        public string Contents { get; set; }
        /// <summary>
        /// 赞数
        /// </summary>
        public int ThumbsUpCount { get; set; }
        /// <summary>
        /// 踩数
        /// </summary>
        public int ThumbsDownCount { get; set; }

        #region 附加属性
        /// <summary>
        /// 所属用户
        /// </summary>
        public AccountModel Account { get; set; }
        /// <summary>
        /// 引用用户
        /// </summary>
        public AccountModel QuoteAccount { get; set; }
        /// <summary>
        /// 当前登录人是否 赞
        /// </summary>
        public bool IsThumbsUp { get; set; }
        /// <summary>
        /// 当前登录人是否 踩
        /// </summary>
        public bool IsThumbsDown { get; set; }
        #endregion
    }
}
