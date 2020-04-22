using BBS.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BBS.Web.Areas.Admin.Infrastructure
{
    public abstract class BBSRazorPage<TModel> : RazorPage<TModel>
    {
        protected AdminModel AdminModel
        {
            get
            {
                try
                {
                    int id = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
                    string name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value,
                        account = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    return new AdminModel
                    {
                        Id = id,
                        Account = account,
                        NickName = name
                    };
                }
                catch
                {
                    return new AdminModel();
                }

            }
        }
    }
}
