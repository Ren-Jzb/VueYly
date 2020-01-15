using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CareHome
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("JS/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("Download/{*pathInfo}");
            routes.IgnoreRoute("Upload/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("ueditor/{*pathInfo}");
            routes.IgnoreRoute("static/{*pathInfo}");
            routes.IgnoreRoute("staticmobile/{*pathInfo}");

            //过滤后缀
            routes.IgnoreRoute("{*fileType}", new { fileType = @".*\.(js|css|html|htm|aspx|asmx)(/.*)?" });

            //日志文件只有管理员能查看。
            routes.MapRoute("myLogAuthRoute", "log/{*pathInfo}", new { controller = "FileAuth", action = "IsAdmin" }, new string[] { "WebMvc.Areas.Admin.Controllers" });
            //上传文件时，分是否带权限管理功能。
            routes.MapRoute("myFileUploadAuthRoute", "Upload/Auth/{*pathInfo}", new { controller = "FileAuth", action = "IsAuth" }, new string[] { "WebMvc.Areas.Admin.Controllers" });
            //下载文件夹，分是否带权限管理功能。
            routes.MapRoute("myFileDownloadAuthRoute", "Download/Auth/{*pathInfo}", new { controller = "FileAuth", action = "IsAuth" }, new string[] { "WebMvc.Areas.Admin.Controllers" });



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
