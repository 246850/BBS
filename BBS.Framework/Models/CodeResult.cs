using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Framework.Models
{
    public class CodeResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
    }

    public class CodeResult<T>: CodeResult
    {
        public T Data { get; set; }
    }
}
