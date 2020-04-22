#pragma checksum "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\Home\Admin.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7ae9717a7f197025e227ec770bf8f41a64c0e573"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Home_Admin), @"mvc.1.0.view", @"/Areas/Admin/Views/Home/Admin.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BBS.Web;

#line default
#line hidden
#line 2 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BBS.Models;

#line default
#line hidden
#line 3 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BBS.Framework.Configs;

#line default
#line hidden
#line 4 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BBS.Framework.Models;

#line default
#line hidden
#line 5 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BBS.Framework.Extensions;

#line default
#line hidden
#line 6 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using StackExchange.Profiling;

#line default
#line hidden
#line 7 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BBS.Requests.Admin;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7ae9717a7f197025e227ec770bf8f41a64c0e573", @"/Areas/Admin/Views/Home/Admin.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d40c177406cf1e2f7dafac323f5760e57cb1234d", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Home_Admin : BBS.Web.Areas.Admin.Infrastructure.BBSRazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#line 2 "E:\DEMO_SPACES\BBS\BBS.Web\Areas\Admin\Views\Home\Admin.cshtml"
  
    ViewData["Title"] = "后台账号";

#line default
#line hidden
            WriteLiteral(@"<style>
    .layui-table-cell {
        height: auto;
    }
</style>
<div class=""card card-outline card-lightblue"">
    <div class=""card-header"">
        <h3 class=""card-title"">后台账号</h3>
        <!-- /.card-tools -->
    </div>
    <!-- /.card-header -->
    <div class=""card-body"">
        <button type=""button"" class=""btn btn-success mb-2"" onclick=""bbs.add()""><i class=""fas fa-plus""></i>新增</button>
        <table id=""grid_0"" class=""layui-hide""></table>
    </div>
    <!-- /.card-body -->
</div>
<script>
    var bbs = {
        refresh: function () {
            layui.use('table', function () {
                this.table = layui.table;
                this.table.render({
                    elem: '#grid_0',
                    url: '/admin/home/admin/',
                    request: {
                        pageName: 'page', //页码的参数名称，默认：page
                        limitName: 'pageSize' //每页数据量的参数名，默认：limit
                    },
                    method: ""post"",
               ");
            WriteLiteral(@"     limit: 20,
                    limits: [10, 20, 30, 40, 50, 100],
                    cellMinWidth: 80, //全局定义常规单元格的最小宽度，layui 2.2.1 新增,
                    id: 'id',
                    cols: [[
                        { field: 'account', title: '账号' },
                        { field: 'nickName', title: '昵称' },
                        { field: 'createTime', title: '创建时间' },
                        {
                            field: 'id', title: '操作', width: 100, fixed: 'right', templet: function (data) {
                                return `<button type='button' class='btn btn-danger btn-sm' onclick='bbs.delete(${data.id})'><i class='fas fa-trash'></i>删除</button>`;
                            }
                        }
                    ]],
                    parseData: function (res) { //将原始数据解析成 table 组件所规定的数据
                        return {
                            ""code"": res.code === 1 ? 0 : -1, //解析接口状态
                            ""msg"": res.msg, //解析提示文本
          ");
            WriteLiteral(@"                  ""count"": res.data.total, //解析数据长度
                            ""data"": res.data.list //解析数据列表
                        };
                    },
                    page: true
                });
            });
        },
        search: function () {
            this.refresh();
        },
        add: function () {
            layer.open({
                type: 2,
                title: '新增账号',
                shadeClose: true,
                shade: 0.7,
                maxmin: true, //开启最大化最小化按钮
                area: ['70%', '60%'],
                content: '/admin/home/adminadd',
                end: function () {
                    bbs.search();
                }
            });
        },
        delete: function (id) {
            $.post(""/admin/home/admindelete?id="" + id, function (res) {
                if (res.code === 1) {
                    bbs.refresh();
                } else {
                    layer.msg(res.msg);
                };
         ");
            WriteLiteral("   })\r\n        }\r\n    };\r\n    $(document).ready(function () {\r\n        bbs.search();\r\n    });\r\n</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591