using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToValue<TEnum>(this TEnum source) where TEnum : struct
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
                throw new ArgumentException("非枚举类型不能调用ToValue()方法", source.ToString());
            return Convert.ToInt32(source);
        }
    }
}
