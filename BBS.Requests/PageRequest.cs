using System;

namespace BBS.Requests
{
    public class PageRequest
    {
        public PageRequest()
        {
            Page = 1;
            PageSize = 20;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 页记录数
        /// </summary>
        public int PageSize { get; set; }
    }
}
