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
    public class LoginController : Controller
    {
        #region   登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["USERINFO"];
            if (cookie == null) return View();
            string USERNAME = cookie.Values["USERNAME"];
            string PASSWORD = cookie.Values["PASSWORD"];
            if (string.IsNullOrEmpty(USERNAME) || string.IsNullOrEmpty(PASSWORD))
            {
                cookie.Expires = DateTime.Now.AddDays(-30);
                Response.AppendCookie(cookie);
            }
            else
            {
                var usersBll = new UsersBll();
                var item = usersBll.LoginUsers(HashEncrypt.DecryptQueryString(USERNAME), HashEncrypt.DecryptQueryString(PASSWORD));
                if (item != null)
                {
                    Session["USERID"] = HashEncrypt.EncryptQueryString(item.UserID.ToString());
                    return RedirectPermanent("../Home/Index");
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-30);
                    Response.AppendCookie(cookie);
                }
            }

            return View();
        }
        #endregion

        #region   登录验证
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <returns></returns>
        public JsonResult LoginSystem()
        {
            //System.Threading.Thread.Sleep(5000);
            int isCookieUp = 1; //1：cookie用户名和密码；2：cookie用户名；3：不要cookie
            bool isCode = true;//是否有验证码，默认有(true)
            string UserName = RequestParameters.Pstring("UserName");
            string Password = RequestParameters.Pstring("Password");
            bool Remember = RequestParameters.Pstring("Remember") == "1";//记住密码
            string code = RequestParameters.Pstring("code");

            if (UserName.Length <= 0)
            {
                var sReturnModel = new ResultMessage();
                sReturnModel.ErrorType = 0;
                sReturnModel.MessageContent = "用户名不能为空.";
                return Json(sReturnModel);
            }
            if (Password.Length <= 0)
            {
                var sReturnModel = new ResultMessage();
                sReturnModel.ErrorType = 0;
                sReturnModel.MessageContent = "密码不能为空.";
                return Json(sReturnModel);
            }
            if (code.Length <= 0)
            {
                var sReturnModel = new ResultMessage();
                sReturnModel.ErrorType = 0;
                sReturnModel.MessageContent = "验证码不能为空.";
                return Json(sReturnModel);
            }

            if (!IsOkValidateCode(isCode, code))
            {
                ClearValidateCode(isCode);
                var sReturnModel = new ResultMessage
                {
                    ErrorType = 0,
                    MessageContent = "验证码错误."
                };
                return Json(sReturnModel);
            }

            ClearValidateCode(isCode);

            var usersBll = new UsersBll();
            if (usersBll.ValidationUserName(UserName))
            {
                var sReturnModel = new ResultMessage
                {
                    ErrorType = 0,
                    MessageContent = "用户名不存在."
                };
                return Json(sReturnModel);
            }
            var item = usersBll.LoginUsers(UserName, HashEncrypt.BgPassWord(Password));
            if (item != null)
            {
                #region   设置IP
                string GetIP = RequestParameters.Pstring("YlyClientIP");   //登录IP
                var itemUsers = new Users();
                itemUsers.UserID = Utits.CurrentUserID;
                itemUsers.UserCode = GetIP;
                var cBllUsers = new UsersBll();
                bool IsFlagUsers = cBllUsers.AddOrUpdate(itemUsers, false);
                #endregion
                try
                {
                    Session["USERID"] = HashEncrypt.EncryptQueryString(item.UserID.ToString());
                    Session["WelfareCentreId"] = HashEncrypt.EncryptQueryString(item.WelfareCentreID.ToString());
                    if (Remember)
                    {
                        #region 记住内容详细

                        if (isCookieUp == 1) //记住用户名和密码
                        {
                            #region Cookie

                            HttpCookie cookies = Request.Cookies["USERINFO"];
                            if (cookies != null)
                            {
                                cookies.Expires = DateTime.Now.AddDays(-30);
                                Response.AppendCookie(cookies);
                            }
                            HttpCookie cookie = new HttpCookie("USERINFO");
                            cookie.Values.Add("USERNAME", HashEncrypt.EncryptQueryString(UserName));
                            cookie.Values.Add("PASSWORD", HashEncrypt.EncryptQueryString(HashEncrypt.BgPassWord(Password)));
                            cookie.Values.Add("WelfareCentreId", HashEncrypt.EncryptQueryString(item.WelfareCentreID.ToString()));
                            cookie.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(cookie);

                            #endregion
                        }
                        else if (isCookieUp == 2) //记住用户名不记住密码
                        {
                            #region Cookie

                            HttpCookie cookies = Request.Cookies["USERINFO"];
                            if (cookies != null)
                            {
                                cookies.Expires = DateTime.Now.AddDays(-30);
                                Response.AppendCookie(cookies);
                            }
                            HttpCookie cookie = new HttpCookie("USERINFO");
                            cookie.Values.Add("USERNAME", HashEncrypt.EncryptQueryString(UserName));
                            cookie.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(cookie);

                            #endregion
                        }
                        else //都不用记
                        {
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var sReturnModel = new ResultMessage
                {
                    ErrorType = 1,
                    MessageContent = "登录成功."
                };
                return Json(sReturnModel);
            }
            else
            {
                var sReturnModel = new ResultMessage
                {
                    ErrorType = 0,
                    MessageContent = "密码错误."
                };
                return Json(sReturnModel);
            }
        }
        #endregion

        #region   辅助方法
        private bool IsOkValidateCode(bool isCode, string code)
        {
            if (!isCode) { return true; }
            if (Session["CheckCode"] == null || string.IsNullOrEmpty(code))
            {
                return false;
            }
            if (code.Trim().ToLower() != Session["CheckCode"].ToString().ToLower())
            {
                return false;
            }
            return true;
        }

        private void ClearValidateCode(bool isCode)
        {
            if (isCode)
            {
                Session["CheckCode"] = null;
            }
        }
        #endregion


    }
}