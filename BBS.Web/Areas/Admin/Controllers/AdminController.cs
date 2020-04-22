using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBS.Web.Areas.Admin.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.Web.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = AdminAuthenticationScheme.AdminScheme)]
    [Area("Admin")]
    public abstract class AdminController : Controller
    {
    }
}