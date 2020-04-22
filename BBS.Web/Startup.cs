using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using BBS.Domain;
using BBS.Framework.Configs;
using BBS.Framework.Jsons;
using BBS.Web.Areas.Admin.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace BBS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());  // json 日期格式
            }); // MVC

            // 登录授权验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>    // 前台登录
                {
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                    options.Cookie = new CookieBuilder { HttpOnly = true };
                })
                .AddCookie(AdminAuthenticationScheme.AdminScheme, options =>    // 后台登录
                {
                    options.LoginPath = "/admin/home/login";
                    options.LogoutPath = "/admin/home/logout";
                    options.Cookie = new CookieBuilder { HttpOnly = true };
                }); ;

            services.AddDbContext<BBSDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySQL"), mysql =>
                {
                    mysql.CharSet(CharSet.Utf8Mb4);
                });
            }); // EF Core DbContext

            services.AddMiniProfiler().AddEntityFramework();    // MiniProfiler

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));// 解决中文乱码

            services.Configure<SiteOptions>(options =>　Configuration.GetSection("Sites").Bind(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseMiniProfiler();  // 监控EF Core SQL执行状况
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // cookie 登录
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "Admin",
                    pattern: "{Admin}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
