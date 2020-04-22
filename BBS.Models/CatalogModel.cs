using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class CatalogModel:BaseModel
    {
        public string Title { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }

        #region 附加属性
        /// <summary>
        /// 主题总数
        /// </summary>
        public int TopicCount { get; set; }
        #endregion
    }
}
