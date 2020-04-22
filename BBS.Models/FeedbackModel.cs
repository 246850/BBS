using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class FeedbackModel:BaseModel
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Contents { get; set; }
    }
}
