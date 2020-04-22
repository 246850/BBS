using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Models
{
    public abstract class BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; } 
    }

    public abstract class BaseModel<T>: BaseModel, IHasCustomProperties<T> where T:class
    {
        public BaseModel()
        {
            CustomProperties = new Dictionary<string, T>();
        }

        /// <summary>
        /// 附加属性
        /// </summary>
        public virtual IDictionary<string, T> CustomProperties { get; }
    }
}
