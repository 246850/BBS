using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Requests.Admin
{
    public class BeginEndRequest:PageRequest
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    }
}
