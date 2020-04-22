using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BBS.Framework.Enums
{
    /// <summary>
    /// 点赞项类型枚举
    /// </summary>
    public enum ThumbsUpItemType
    {
        [Description("主题")]
        Topic = 2,
        [Description("评论")]
        Comment = 4
    }
}
