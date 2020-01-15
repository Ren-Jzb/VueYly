using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using CommonLib;

namespace CareHome.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            #region 页面权限判断
            if (!Utits.IsLogin)
            {
                return RedirectPermanent("../Login/Index");
            }
            #endregion
            var welfareCentreId = Utits.WelfareCentreID;
            ViewBag.CurrentRealName = Utits.CurrentRealName;


            //var cBll = new LmcjxdBll();
            //var id = new Guid("0bf3ef8b-4b90-49c4-849a-e2cbc82ae782");
            //bool isFlag = cBll.LogicDeleteByCondition(id);
            //var wBll = new FloorOrRoomBll();
            //var welfareCentreItem = wBll.GetObjectByID(welfareCentreId);
            //if (welfareCentreItem != null)
            //{
            //    ViewBag.WelfareCentreName = welfareCentreItem.WelfareCentreName ?? "智慧养老运管平台"; //养老院名称
            //}
            ViewBag.WelfareCentreName = "居家养老运管平台"; //养老院名称


            //var strUserWelfareCentre = "";
            //var isShowUserWelfareCentre = "none";
            //var userName = Utits.CurrentUserName;
            //if (userName.Length > 0)
            //{
            //    var buf = new StringBuilder();
            //    var userWelfareCentreBll = new UserWelfareCentreBll();
            //    if (userName == Config.UserNameSuper)
            //    {

            //        var list = userWelfareCentreBll.SearchList();
            //        if (list.Count > 0)
            //        {
            //            isShowUserWelfareCentre = "inline-block";
            //            foreach (var item in list)
            //            {
            //                buf.AppendFormat("<div class=\"hosp-item\">{0} <a class=\"hosp-link\" href=\"javascript:; \" data-val=\"{1}\">进入</a></div>", item.WelfareCentreName, item.WelfareCentreId);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var list = userWelfareCentreBll.SearchListByUesrName(userName);
            //        if (list.Count > 0)
            //        {
            //            isShowUserWelfareCentre = "inline-block";
            //            foreach (var item in list)
            //            {
            //                buf.AppendFormat("<div class=\"hosp-item\">{0} <a class=\"hosp-link\" href=\"javascript:; \" data-val=\"{1}\">进入</a></div>", item.WelfareCentreName, item.WelfareCentreId);
            //            }
            //        }
            //    }

            //    strUserWelfareCentre = buf.ToString();
            //}
            //ViewBag.UserWelfareCentre = strUserWelfareCentre;
            //ViewBag.IsShowUserWelfareCentre = isShowUserWelfareCentre;


            return View();
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        public JsonResult QuitLogin()
        {
            #region 安全退出
            try
            {
                Session.Abandon();
                HttpCookie userCookie = Request.Cookies["USERINFO"];
                if (userCookie != null)
                {
                    userCookie.Expires = DateTime.Now.AddDays(-30);
                    Response.AppendCookie(userCookie);
                }
            }
            catch
            {
                // ignored
            }
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 1;
            sRetrunModel.MessageContent = "退出系统成功.";
            return Json(sRetrunModel);
            #endregion
        }

        [OutputCache(Duration = 0)]
        public ActionResult Main()
        {
            #region 页面权限判断
            if (!Utits.IsLogin)
            {
                return RedirectPermanent("../Login/Index");
            }
            #endregion

            //登陆用户养老院id
            var welfareCentreId = Utits.WelfareCentreID;

            return View();
        }

        public ActionResult Hello()
        {
            return View();
        }
        public JsonResult AchieveLeftAuthNode()
        {
            if (Utits.IsLogin)
            {
                #region   设置IP
                string GetIP = RequestParameters.Pstring("YlyClientIP");   //登录IP
                var itemUsers = new Users();
                itemUsers.UserID = Utits.CurrentUserID;
                itemUsers.UserCode = GetIP;
                var cBllUsers = new UsersBll();
                bool IsFlagUsers = cBllUsers.AddOrUpdate(itemUsers, false);
                #endregion

                var listLeftAuthNode = new AuthRoleNodeBll().SearchListByLeftUserId(Utits.CurrentUserID, Utits.IsSuper);
                if (listLeftAuthNode != null)
                {
                    var listAchieveAuthNode = listLeftAuthNode.Select(itemNode => new TreeModel
                    {
                        Id = itemNode.NodeId,
                        Pid = itemNode.ParentID,
                        Name = itemNode.NodeName,
                        Target = itemNode.NodeTarget,
                        Url = itemNode.NodePath,
                        NodeClassName = itemNode.NodeClassName,
                        Remark = itemNode.Remark
                    }).OrderBy(c => c.Remark).ToList();
                    if (listAchieveAuthNode.Count > 0)
                    {
                        return Json(listAchieveAuthNode);
                    }
                }
            }
            return Json("[]");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public JsonResult ModifyPassword()
        {
            Guid iUSERID = Utits.CurrentUserID;
            if (iUSERID == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 3;
                sRetrunModel.MessageContent = "未登录.";
                return Json(sRetrunModel);
            }
            string szOldPassword = RequestParameters.Pstring("OldPassword");
            if (szOldPassword.Length <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "旧密码不能为空.";
                return Json(sRetrunModel);
            }
            string szNewPassword = RequestParameters.Pstring("NewPassword");
            if (szNewPassword.Length <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "新密码不能为空.";
                return Json(sRetrunModel);
            }
            var cBll = new UsersBll();
            var item = cBll.GetObjectById(iUSERID);
            if (item == null)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "用户名不存在.";
                return Json(sRetrunModel);
            }
            if (CommonLib.HashEncrypt.BgPassWord(szOldPassword) != item.Password)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "旧密码有误.";
                return Json(sRetrunModel);
            }
            string iNewPassword = HashEncrypt.BgPassWord(szNewPassword);
            bool isFlag = cBll.ChangePassword(item.UserID, iNewPassword);
            if (isFlag)
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