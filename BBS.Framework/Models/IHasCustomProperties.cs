using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Models
{
    public interface IHasCustomProperties<T> where T:class
    {
        /// <summary>
        /// 附加属性
        /// </summary>
        IDictionary<string, T> CustomProperties { get;}
    }
}
