using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHome.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            string url = Request.QueryString["url"];                //URL
            string message = Request.QueryString["msg"];            //信息
            string errorRank = Request.QueryString["errorrank"];    //错误等级

            ViewBag.URL = url;
            ViewBag.Msg = message;
            ViewBag.ErrorRank = errorRank;
            return View();
        }
    }
}