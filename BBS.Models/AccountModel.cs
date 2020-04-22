using BBS.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Models
{
    public class AccountModel:BaseModel
    {
        public string Account1 { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public int Integral { get; set; }
    }
}
