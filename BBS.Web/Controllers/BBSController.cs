using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BBS.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public abstract class BBSController : Controller
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
                        Avatar = avatar
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