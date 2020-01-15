using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHome.Controllers
{
    public class ValidateCodeController : Controller
    {
        // GET: ValidateCode
        public ActionResult Index()
        {
            var itemValidateCode = new CommonLib.ValidateCode();
            string code = itemValidateCode.CreateValidateCode(4);
            Session["CheckCode"] = code;
            byte[] bytes = itemValidateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}