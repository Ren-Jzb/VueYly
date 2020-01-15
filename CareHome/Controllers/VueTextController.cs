using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using CommonLib;
using System.Text;

namespace CareHome.Controllers
{
    public class VueTextController : Controller
    {
        // GET: VueText
        public ActionResult List()
        {
            return View();
        }

        public JsonResult AddOrUpdate()
        {
            Guid ID = RequestParameters.PGuid("id");
            string name = RequestParameters.Pstring("name");
            string ck = RequestParameters.Pstring("ck");
            int age = RequestParameters.Pint("age");
            int rd = RequestParameters.Pint("rd");
            string st = RequestParameters.Pstring("st");
            string msg = RequestParameters.Pstring("msg");


            Vm item = new Vm();
            item.Id = ID;
            item.Name = name;
            item.Sex = rd;
            item.Age = age;
            item.Hobby = ck;
            item.Address = st;
            item.Remark = msg;
            item.IsValid = 1;


            bool IsAdd = false;
            if (ID == Guid.Empty)
            {
                IsAdd = true;
            }
            var cBll = new VmBll();
            bool IsFlag = cBll.AddOrUpdate(item, IsAdd);
            if (IsFlag)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 1;
                sRetrunModel.MessageContent = "操作成功.";
                return Json(sRetrunModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "操作失败.";
                return Json(sRetrunModel);
            }
        }


    }
}