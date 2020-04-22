using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class ThumbsUpModel:BaseModel
    {
        public int ItemId { get; set; }
        public bool IsThumb { get; set; }
        public int AccountId { get; set; }
        public int ItemType { get; set; }
    }
}
