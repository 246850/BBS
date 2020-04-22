using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Requests
{
    public class TopicCommetListRequest
    {
        /// <summary>
        /// 主题ID
        /// </summary>
        public int TopicId { get; set; }
        public PageRequest Page { get; set; }
    }
}
