using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BBS.Web.Infrastructure
{
    public abstract class BBSRazorPage<TModel> : RazorPage<TModel>
    {
        protected UserModel UserModel
        {
            get
            {
                try
                {
                    int id = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
                    string name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value,
                        account = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    avatar = User.Claims.First(c => c.Type == ClaimTypes.Actor).Value;
                    return new UserModel
                    {
                        Id = id,
                        Account = account,
                        NickName = name,
                        Avatar = avatar,
                        IsLogin  = true
                    };
                }
                catch
                {
                    return new UserModel();
                }

            }
        }
    }
}
