using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Extensions
{
    public static class CodeResultExtensions
    {
        public static CodeResult ToResult(this bool flag)
        {
            return ToResult(flag, string.Empty);
        }

        public static CodeResult ToResult(this bool flag, string msg)
        {
            int code = flag ? 1 : -1;
            if (string.IsNullOrWhiteSpace(msg))
            {
                msg = flag ? "成功 - Success" : "失败 - Failed";
            }

            return new CodeResult
            {
                Code = code,
                Msg = msg
            };
        }

        public static CodeResult<T> ToResult<T>(this T source)
        {
            return new CodeResult<T>
            {
                Code = 1,
                Msg = "成功 - Success",
                Data = source
            };
        }

        public static CodeResult<T> Success<T>(this T source)
        {
            return new CodeResult<T>
            {
                Code = 1,
                Msg = "成功 - Success",
                Data = source
            };
        }
    }
}
