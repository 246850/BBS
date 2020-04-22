using BBS.Framework.Extensions;
using BBS.Framework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace BBS.Web.Infrastructure
{

    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public AdminAuthorizeAttribute()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var action = context.ActionDescriptor as ControllerActionDescriptor;
            if (action.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute))) return;

            if (!user.Identity.IsAuthenticated && context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                context.HttpContext.Response.StatusCode = HttpStatusCode.Unauthorized.ToValue();
                context.Result = new JsonResult(new CodeResult()
                {
                    Code = 401,
                    Msg = "请登录"
                });
            }
        }
    }
}
