using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Models
{
    public class PagedList<T> : List<T> where T : class
    {
        public PagedList(int page, int pageSize, int total)
        {
            if (page < 1) throw new Exception("页码小于1");
            if (pageSize < 1) throw new Exception("页记录数小于1");
            if (total < 0) throw new Exception("总记录数小于0");

            Page = page;
            PageSize = pageSize;
            Total = total;
        }
        /// <summary>
        /// 前一页码
        /// </summary>
        public int Prev
        {
            get
            {
                return Page - 1;
            }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; }
        /// <summary>
        /// 下一页码
        /// </summary>
        public int Next
        {
            get
            {
                return Page + 1;
            }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize <= 0) return 0;
                var i = (Total / PageSize);
                if (Total % PageSize == 0)
                {
                    return i;
                }
                return i + 1;
            }
        }

        /// <summary>
        /// 第几行
        /// </summary>
        public int RowNumber
        {
            get
            {
                return (Page - 1) * PageSize + 1;
            }
        }
    }

    public class PaginationModel
    {
        public PaginationModel(int page, int pageSize, int total)
        {
            if (page < 1) throw new Exception("页码小于1");
            if (pageSize < 1) throw new Exception("页记录数小于1");
            if (total < 0) throw new Exception("总记录数小于0");

            Page = page;
            PageSize = pageSize;
            Total = total;
            QueryString = string.Empty;
        }
        /// <summary>
        /// 前一页码
        /// </summary>
        public int Prev
        {
            get
            {
                return Page - 1;
            }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; }
        /// <summary>
        /// 下一页码
        /// </summary>
        public int Next
        {
            get
            {
                return Page + 1;
            }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize <= 0) return 0;
                var i = (Total / PageSize);
                if (Total % PageSize == 0)
                {
                    return i;
                }
                return i + 1;
            }
        }
        /// <summary>
        /// 第几行
        /// </summary>
        public int RowNumber
        {
            get
            {
                return (Page - 1) * PageSize + 1;
            }
        }
        /// <summary>
        /// 导航Action
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 导航Controller
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 查询参数
        /// </summary>
        public string QueryString { get; internal set; }

    }
}
