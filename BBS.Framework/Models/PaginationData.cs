using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Models
{
    /// <summary>
    /// 分页结果包装类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationData<T> where T:class
    {
        public PaginationData():this(0)
        {
        }
        public PaginationData(int total)
        {
            Total = total;
            List = new List<T>();
        }
        /// <summary>
        /// 数据列表
        /// </summary>
        public virtual List<T> List { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int Total { get; }
    }
}
