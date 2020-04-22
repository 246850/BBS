using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class AdminModel:BaseModel
    {
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
    }
}
