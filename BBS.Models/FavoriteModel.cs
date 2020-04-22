using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class FavoriteModel:BaseModel
    {
        public int ItemId { get; set; }
        public int AccountId { get; set; }
    }
}
