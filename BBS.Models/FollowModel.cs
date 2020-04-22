using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class FollowModel:BaseModel
    {
        public int AccountId { get; set; }
        public int FollowAccountId { get; set; }
    }
}
