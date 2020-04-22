using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Requests.Admin
{
    public class CommentListRequest: BeginEndRequest
    {
        public int TopicId { get; set; }
        public int AccountId { get; set; }
    }
}
