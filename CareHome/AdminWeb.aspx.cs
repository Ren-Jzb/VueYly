using System;
using System.Web;
using CommonLib;

namespace CareHome
{
    public partial class AdminWeb : System.Web.UI.Page
    {
        private bool IsDebug { get { return true; } }

        protected void Page_Load(object sender, EventArgs e)
        { }

        //超级管理员
        protected void btnsuper_Click(object sender, EventArgs e)
        {
            if (IsDebug)
            {
                HttpContext.Current.Session["USERID"] = HashEncrypt.EncryptQueryString(CommonLib.Config.SuperID.ToString());
                Response.Redirect("/Home/Index");
            }
        } 

    }
}