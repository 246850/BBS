using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBS.Web.Infrastructure
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }

        public bool IsLogin { get; set; }
    }
}
