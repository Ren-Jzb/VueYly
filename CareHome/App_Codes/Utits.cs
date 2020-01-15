using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using Model;
using BLL;
using CommonLib;

public class Utits
{
    public static string CurrentIP
    {
        get
        {
            return CurrentUser == null ? "127.0.0.1" : CurrentUser.UserCode;
        }
    }

    //客户端登录IP
    public static string ClientIPAddress
    {
        get
        {
            var clientIPAddress = "127.0.0.1";
            return clientIPAddress = Utits.CurrentIP + "||" + new CommonLib.UserLogIP().ReturnClientMAC();
        }
    }

    /// <summary>
    /// 功能业务
    /// </summary>
    public static int CurrentBizType
    {
        get
        {
            int BizType = 0;
            try
            {
                if (HttpContext.Current.Session["BizType"] == null && HttpContext.Current.Request.Cookies["BizType"] != null)
                {
                    BizType = Convert.ToInt32(HttpContext.Current.Request.Cookies["BizType"].Value);
                    HttpContext.Current.Session["BizType"] = BizType;
                }
                BizType = Convert.ToInt32(HttpContext.Current.Session["BizType"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BizType = 0;
            }
            return BizType;
        }
    }

    #region 当前登录用户信息
    public static Users CurrentUser
    {
        get
        {
            Users item = null;
            var usersBll = new UsersBll();
            if (HttpContext.Current.Session["USERID"] == null && HttpContext.Current.Request.Cookies["USERINFO"] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["USERINFO"];
                string username = cookie.Values["USERNAME"];
                string password = cookie.Values["PASSWORD"];
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    item = usersBll.LoginUsers(CommonLib.HashEncrypt.DecryptQueryString(username), CommonLib.HashEncrypt.DecryptQueryString(password));
                    if (item != null)
                    {
                        HttpContext.Current.Session["USERID"] = HashEncrypt.EncryptQueryString(item.UserID.ToString());
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddDays(-30);
                        HttpContext.Current.Response.AppendCookie(cookie);
                    }
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-30);
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
            }
            if (HttpContext.Current.Session["USERID"] != null && item == null)
            {
                var temp = HashEncrypt.DecryptQueryString(HttpContext.Current.Session["USERID"].ToString());
                if (RegexValidate.IsGuid(temp))
                    item = usersBll.GetObjectById(new Guid(temp));
            }
            return item;
        }
    }

    public static Guid CurrentUserID
    {
        get
        {
            return CurrentUser == null ? Guid.Empty : CurrentUser.UserID;
        }
    }

    public static string CurrentUserName
    {
        get { return CurrentUser == null ? "" : CurrentUser.UserName; }
    }

    public static string CurrentRealName
    {
        get { return CurrentUser == null ? "" : CurrentUser.RealName; }
    }

    public static Guid CurrentDeptID
    {
        get { return CurrentUser == null ? Guid.Empty : (CurrentUser.DeptID ?? Guid.Empty); }
    }

    public static Guid CurrentRoleID
    {
        get { return CurrentUser == null ? Guid.Empty : (CurrentUser.RoleID ?? Guid.Empty); }
    }

    /// <summary>
    /// 判断是否登录
    /// 1防止session名称修改和sessionState形式存储判别
    /// 2一项目多种身份登录形式
    /// </summary>
    public static bool IsLogin
    {
        get
        {
            return CurrentUserID != Guid.Empty;
        }
    }
    /// <summary>
    /// 内置用户：系统管理员
    /// 账号：admin 
    /// 密码：admin
    /// </summary>
    public static bool IsAdmin
    {
        get
        {
            var AdminID = Config.AdminID;
            if (AdminID == Guid.Empty)
                return false;
            return CurrentUserID == AdminID;
        }
    }

    /// <summary>
    /// 内置用户：超级管理员
    /// 账号：super
    /// 密码：super0123!
    /// </summary>
    public static bool IsSuper
    {
        get
        {
            var SuperID = Config.SuperID;
            if (SuperID == Guid.Empty)
                return false;
            return CurrentUserID == SuperID;
        }
    }

    /// <summary>
    /// 门卫
    /// </summary>
    public static bool IsDoor
    {
        get
        {
            var DoorID = Config.DoorID;
            if (DoorID == Guid.Empty)
                return false;
            return CurrentUserID == DoorID;
        }
    }

    /// <summary>
    /// 内置角色：超级管理员
    /// 超级管理员拥有彻底删除按钮权限其它没有该权限
    /// </summary>
    public static bool IsSuperRole
    {
        get
        {
            var SuperRoleID = new Guid("DC5A583E-971A-4F07-ADEF-53C874D3219E");
            return CurrentRoleID == SuperRoleID;
        }
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public static int UserType
    {
        get
        {
            return CurrentUser == null ? 0 : CurrentUser.UserType ?? 0;
        }
    }

    public static Guid WelfareCentreID
    {
        get
        {
            var WelfareCentreId = Guid.Empty;
            if (CurrentUser == null)
                return Guid.Empty;
            if (CurrentUser.UserName == Config.UserNameSuper)
            {
                if (HttpContext.Current.Session["WelfareCentreId"] == null && HttpContext.Current.Request.Cookies["WelfareCentreId"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["USERINFO"];
                    string temp = cookie.Values["WelfareCentreId"];
                    if (RegexValidate.IsGuid(temp))
                        WelfareCentreId = new Guid(temp);
                }
                else if (HttpContext.Current.Session["WelfareCentreId"] != null && HttpContext.Current.Request.Cookies["WelfareCentreId"] == null)
                {
                    var temp = HashEncrypt.DecryptQueryString(HttpContext.Current.Session["WelfareCentreId"].ToString());
                    if (RegexValidate.IsGuid(temp))
                        WelfareCentreId = new Guid(temp);
                }
                else
                {
                    if (CurrentUser.WelfareCentreID != null)
                    {
                        WelfareCentreId = CurrentUser.WelfareCentreID.Value;
                    }
                }
            }
            else
            {
                if (CurrentUser.WelfareCentreID != null)
                    WelfareCentreId = CurrentUser.WelfareCentreID.Value;
                else
                    WelfareCentreId = Guid.Empty;
            }
            return WelfareCentreId;
        }
    }


    ////客户端登录IP
    //public static string ClientIPAddress
    //{
    //    get
    //    {
    //        var clientIPAddress = "127.0.0.1";
    //        return clientIPAddress = Utits.CurrentIP + "||" + new CommonLib.UserLogIP().ReturnClientMAC();
    //    }
    //}
    #endregion

    #region    Detail页面按钮
    /// <summary>
    /// 动态生成查看、修改、增加、删除、等等按钮
    /// </summary>
    public static string AuthOperateButtonDetail(int NodeId)
    {
        #region 页面权限及按钮权限
        //判断登录
        Guid UserId = Utits.CurrentUserID;

        if (!new AuthRoleNodeBll().IsNodePageAuth(UserId, NodeId, IsSuper))
        {
            return "";
        }
        #region 按钮权限
        //获得页面带权限按钮
        var listOperateAuthButton = new AuthRoleNodeButtonBll().GetListByUserIdNodeId(UserId, NodeId, IsSuper);
        if (listOperateAuthButton == null)
            listOperateAuthButton = new List<vAuthRoleNodeButton>();
        var builder = new StringBuilder();
        foreach (var item in listOperateAuthButton)
        {
            #region
            switch ((EButtonType)Enum.ToObject(typeof(EButtonType), item.ButtonId))
            {
                case EButtonType.Add:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-add\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Update:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-modify\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Delete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.PhyDelete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-deep-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Look:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Save:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-save bc\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Submit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-save\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Back:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-back\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Import:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-import\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Export:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.ExportWord:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Print:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-print\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Confirm:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Cancel:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton qx\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Edit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Detail:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Agree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoAgree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Pass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoPass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.ResetPassword:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-reset-pw\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Enable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-active\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Disable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-disable\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Return:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.SetHgBed:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.AuthRoleMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                default:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
            }
            #endregion
        }

        return builder.ToString();
        #endregion
        #endregion
    }
    #endregion



    /// <summary>
    /// 动态生成查看、修改、增加、删除、等等按钮
    /// </summary>
    public static string AuthOperateButton()
    {
        #region 页面权限及按钮权限
        int NodeId = RequestParameters.Pint("NodeId");
        //判断登录
        Guid UserId = Utits.CurrentUserID;

        if (!new AuthRoleNodeBll().IsNodePageAuth(UserId, NodeId, IsSuper))
        {
            return "";
        }
        #region 按钮权限
        //获得页面带权限按钮
        var listOperateAuthButton = new AuthRoleNodeButtonBll().GetListByUserIdNodeId(UserId, NodeId, IsSuper);
        if (listOperateAuthButton == null)
            listOperateAuthButton = new List<vAuthRoleNodeButton>();
        var builder = new StringBuilder();
        foreach (var item in listOperateAuthButton)
        {
            #region
            switch ((EButtonType)Enum.ToObject(typeof(EButtonType), item.ButtonId))
            {
                case EButtonType.Add:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-add\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Update:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-modify\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Delete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.PhyDelete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-deep-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Look:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Save:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-save\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Submit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-save\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Back:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-back\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Import:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-import\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Export:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.ExportWord:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Print:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-print\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Confirm:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Cancel:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Edit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Detail:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Agree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoAgree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Pass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoPass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.ResetPassword:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-reset-pw\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Enable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-active\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Disable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-disable\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Return:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.SetHgBed:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.AuthRoleMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                default:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
            }
            #endregion
        }

        return builder.ToString();
        #endregion
        #endregion
    }
    /// <summary>
    /// 动态生成查看、修改、增加、删除、等等按钮
    /// </summary>
    public static string AuthOperateButton(int NodeId)
    {
        #region 页面权限及按钮权限
        //判断登录
        Guid UserId = Utits.CurrentUserID;

        if (!new AuthRoleNodeBll().IsNodePageAuth(UserId, NodeId, IsSuper))
        {
            return "";
        }
        #region 按钮权限
        //获得页面带权限按钮
        var listOperateAuthButton = new AuthRoleNodeButtonBll().GetListByUserIdNodeId(UserId, NodeId, IsSuper);
        if (listOperateAuthButton == null)
            listOperateAuthButton = new List<vAuthRoleNodeButton>();
        var builder = new StringBuilder();
        foreach (var item in listOperateAuthButton)
        {
            #region
            switch ((EButtonType)Enum.ToObject(typeof(EButtonType), item.ButtonId))
            {
                case EButtonType.Add:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-add\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Update:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-modify\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Delete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.PhyDelete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-deep-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Look:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Save:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-save\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Submit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-save\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Back:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-back\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Import:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-import\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Export:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.ExportWord:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Print:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-print\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Confirm:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Cancel:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Edit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Detail:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Agree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoAgree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Pass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoPass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.ResetPassword:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-reset-pw\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Enable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-active\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Disable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-disable\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Return:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.SetHgBed:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.AuthRoleMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.CustomerLeave:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-del\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                default:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
            }
            #endregion
        }

        return builder.ToString();
        #endregion
        #endregion
    }
    public static string AuthOperateButton(EButtonType[] wantButtons)
    {
        var builder = new StringBuilder();
        foreach (var itemWb in wantButtons)
        {
            #region
            switch (itemWb)
            {
                case EButtonType.Add:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAdd", "新增");
                    break;
                case EButtonType.Update:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnUpdate", "修改");
                    break;
                case EButtonType.Delete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnDelete", "删除");
                    break;
                case EButtonType.PhyDelete:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnPhyDelete", "彻底删除");
                    break;
                case EButtonType.Look:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnLook", "查看");
                    break;

                case EButtonType.Save:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnSave", "保存");
                    break;
                case EButtonType.Submit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnSubmit", "提交");
                    break;
                case EButtonType.Back:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnBack", "返回");
                    break;

                case EButtonType.Import:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnImport", "导入");
                    break;
                case EButtonType.Export:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnExport", "导出Excel");
                    break;
                case EButtonType.ExportWord:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton btn-export\">{1}</a>", "btnExportWord", "导出Word");
                    break;

                case EButtonType.Print:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnPrint", "打印");
                    break;

                case EButtonType.Confirm:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnConfirm", "确定");
                    break;
                case EButtonType.Cancel:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnCancel", "取消");
                    break;

                case EButtonType.Edit:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnEdit", "编辑");
                    break;
                case EButtonType.Detail:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnDetail", "详细");
                    break;

                case EButtonType.Agree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAgree", "同意");
                    break;
                case EButtonType.NoAgree:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnNoAgree", "不同意");
                    break;

                case EButtonType.Pass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnPass", "通过");
                    break;
                case EButtonType.NoPass:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnNoPass", "不通过");
                    break;

                case EButtonType.ResetPassword:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnResetPassword", "重置密码");
                    break;
                case EButtonType.Enable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnEnable", "启用");
                    break;
                case EButtonType.Disable:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnDisable", "禁用");
                    break;

                case EButtonType.Return:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnReturn", "退回");
                    break;
                case EButtonType.SetHgBed:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnReturn", "设置管理床位");
                    break;

                case EButtonType.AuthRoleMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAuthRoleMenu", "角色菜单权限");
                    break;
                case EButtonType.AuthUserMenu:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAuthUserMenu", "用户菜单权限");
                    break;
                case EButtonType.AuthRoleNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAuthRoleNode", "角色节点权限");
                    break;
                case EButtonType.AuthUserNode:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAuthUserNode", "用户节点权限");
                    break;
                case EButtonType.AuthRoleNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAuthRoleNodeButton", "角色按钮权限");
                    break;
                case EButtonType.AuthUserNodeButton:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnAuthUserNodeButton", "用户按钮权限");
                    break;
                default:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}Operate\" class=\"OperateButton\">{1}</a>", "btnOther", "其它");
                    break;
            }
            #endregion
        }

        return builder.ToString();
    }

    /// <summary>
    /// 动态生成查看、修改、增加、删除、等等按钮
    /// </summary>
    /// <param name="operateButton"></param>
    public static void AuthContent(System.Web.UI.HtmlControls.HtmlGenericControl operateButton)
    {
        #region 页面权限及按钮权限
        int NodeId = RequestParameters.Pint("NodeId");
        //判断登录
        Guid UserId = Utits.CurrentUserID;
        //if (UserId == Guid.Empty)
        //{
        //    HttpContext.Current.Response.Write("<script>if(window.top != window.self){top.location=\"/Chinese/Login.aspx\";}else{location=\"/Chinese/Login.aspx\";}</script>");
        //    HttpContext.Current.Response.End();
        //}

        if (!new AuthRoleNodeBll().IsNodePageAuth(UserId, NodeId, IsSuper))
        {
            HttpContext.Current.Response.Redirect(string.Format("/Chinese/Error.aspx?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(HttpContext.Current.Request.Url?.ToString() ?? ""), "您没有该页面的访问权限！", EErrorRank.Error));
            HttpContext.Current.Response.End();
        }
        #region 按钮权限
        //获得页面带权限按钮
        var listOperateAuthButton = new AuthRoleNodeButtonBll().GetListByUserIdNodeId(UserId, NodeId, IsSuper);
        if (listOperateAuthButton == null)
            listOperateAuthButton = new List<vAuthRoleNodeButton>();
        var builder = new StringBuilder();
        foreach (var item in listOperateAuthButton)
        {
            #region
            switch ((EButtonType)Enum.ToObject(typeof(EButtonType), item.ButtonId))
            {
                case EButtonType.Add:
                    builder.AppendFormat("<a href=\"javascript:ClickAdd();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Update:
                    builder.AppendFormat("<a href=\"javascript:ClickUpdate();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Delete:
                    builder.AppendFormat("<a href=\"javascript:ClickDelete();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.PhyDelete:
                    builder.AppendFormat("<a href=\"javascript:ClickPhyDelete();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Look:
                    builder.AppendFormat("<a href=\"javascript:ClickLook();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Save:
                    builder.AppendFormat("<a href=\"javascript:ClickSave();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Submit:
                    builder.AppendFormat("<a href=\"javascript:ClickSubmit();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Back:
                    builder.AppendFormat("<a href=\"javascript:ClickBack();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Import:
                    builder.AppendFormat("<a href=\"javascript:ClickImport();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Export:
                    builder.AppendFormat("<a href=\"javascript:ClickExport();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Print:
                    builder.AppendFormat("<a href=\"javascript:ClickPrint();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Confirm:
                    builder.AppendFormat("<a href=\"javascript:ClickConfirm();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Cancel:
                    builder.AppendFormat("<a href=\"javascript:ClickCancel();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Edit:
                    builder.AppendFormat("<a href=\"javascript:ClickEdit();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Detail:
                    builder.AppendFormat("<a href=\"javascript:ClickDetail();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Agree:
                    builder.AppendFormat("<a href=\"javascript:ClickAgree();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoAgree:
                    builder.AppendFormat("<a href=\"javascript:ClickNoAgree();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Pass:
                    builder.AppendFormat("<a href=\"javascript:ClickPass();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.NoPass:
                    builder.AppendFormat("<a href=\"javascript:ClickNoPass();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.ResetPassword:
                    builder.AppendFormat("<a href=\"javascript:ClickResetPassword();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Enable:
                    builder.AppendFormat("<a href=\"javascript:ClickEnable()\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.Disable:
                    builder.AppendFormat("<a href=\"javascript:ClickDisable();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.Return:
                    builder.AppendFormat("<a href=\"javascript:ClickReturn();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;

                case EButtonType.AuthRoleMenu:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthRoleMenu();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserMenu:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthUserMenu();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNode:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthRoleNode();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNode:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthUserNode();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthRoleNodeButton:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthRoleNodeButton();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                case EButtonType.AuthUserNodeButton:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthUserNodeButton();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
                default:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", item.BtnID, item.BtnName);
                    break;
            }
            #endregion
        }

        operateButton.InnerHtml = builder.ToString();
        #endregion
        #endregion
    }

    public static void AuthContent(EButtonType[] wantButtons, System.Web.UI.HtmlControls.HtmlGenericControl operateButton)
    {
        var builder = new StringBuilder();
        foreach (var itemWb in wantButtons)
        {
            #region
            switch (itemWb)
            {
                case EButtonType.Add:
                    builder.AppendFormat("<a href=\"javascript:ClickAdd();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAdd", "新增");
                    break;
                case EButtonType.Update:
                    builder.AppendFormat("<a href=\"javascript:ClickUpdate();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnUpdate", "修改");
                    break;
                case EButtonType.Delete:
                    builder.AppendFormat("<a href=\"javascript:ClickDelete();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnDelete", "删除");
                    break;
                case EButtonType.PhyDelete:
                    builder.AppendFormat("<a href=\"javascript:ClickPhyDelete();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnPhyDelete", "彻底删除");
                    break;
                case EButtonType.Look:
                    builder.AppendFormat("<a href=\"javascript:ClickLook();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnLook", "查看");
                    break;

                case EButtonType.Save:
                    builder.AppendFormat("<a href=\"javascript:ClickSave();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnSave", "保存");
                    break;
                case EButtonType.Submit:
                    builder.AppendFormat("<a href=\"javascript:ClickSubmit();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnSubmit", "提交");
                    break;
                case EButtonType.Back:
                    builder.AppendFormat("<a href=\"javascript:ClickBack();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnBack", "返回");
                    break;

                case EButtonType.Import:
                    builder.AppendFormat("<a href=\"javascript:ClickImport();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnImport", "导入");
                    break;
                case EButtonType.Export:
                    builder.AppendFormat("<a href=\"javascript:ClickExport();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnExport", "导出");
                    break;

                case EButtonType.Print:
                    builder.AppendFormat("<a href=\"javascript:ClickPrint();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnPrint", "打印");
                    break;

                case EButtonType.Confirm:
                    builder.AppendFormat("<a href=\"javascript:ClickConfirm();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnConfirm", "确定");
                    break;
                case EButtonType.Cancel:
                    builder.AppendFormat("<a href=\"javascript:ClickCancel();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnCancel", "取消");
                    break;

                case EButtonType.Edit:
                    builder.AppendFormat("<a href=\"javascript:ClickEdit();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnEdit", "编辑");
                    break;
                case EButtonType.Detail:
                    builder.AppendFormat("<a href=\"javascript:ClickDetail();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnDetail", "详细");
                    break;

                case EButtonType.Agree:
                    builder.AppendFormat("<a href=\"javascript:ClickAgree();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAgree", "同意");
                    break;
                case EButtonType.NoAgree:
                    builder.AppendFormat("<a href=\"javascript:ClickNoAgree();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnNoAgree", "不同意");
                    break;

                case EButtonType.Pass:
                    builder.AppendFormat("<a href=\"javascript:ClickPass();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnPass", "通过");
                    break;
                case EButtonType.NoPass:
                    builder.AppendFormat("<a href=\"javascript:ClickNoPass();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnNoPass", "不通过");
                    break;

                case EButtonType.ResetPassword:
                    builder.AppendFormat("<a href=\"javascript:ClickResetPassword();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnResetPassword", "重置密码");
                    break;
                case EButtonType.Enable:
                    builder.AppendFormat("<a href=\"javascript:ClickEnable()\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnEnable", "启用");
                    break;
                case EButtonType.Disable:
                    builder.AppendFormat("<a href=\"javascript:ClickDisable();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnDisable", "禁用");
                    break;

                case EButtonType.Return:
                    builder.AppendFormat("<a href=\"javascript:ClickReturn();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnReturn", "返回");
                    break;

                case EButtonType.AuthRoleMenu:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthRoleMenu();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAuthRoleMenu", "角色菜单权限");
                    break;
                case EButtonType.AuthUserMenu:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthUserMenu();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAuthUserMenu", "用户菜单权限");
                    break;
                case EButtonType.AuthRoleNode:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthRoleNode();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAuthRoleNode", "角色节点权限");
                    break;
                case EButtonType.AuthUserNode:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthUserNode();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAuthUserNode", "用户节点权限");
                    break;
                case EButtonType.AuthRoleNodeButton:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthRoleNodeButton();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAuthRoleNodeButton", "角色按钮权限");
                    break;
                case EButtonType.AuthUserNodeButton:
                    builder.AppendFormat("<a href=\"javascript:ClickAuthUserNodeButton();\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnAuthUserNodeButton", "用户按钮权限");
                    break;
                default:
                    builder.AppendFormat("<a href=\"javascript:void(0);\" hidefocus id=\"{0}\" class=\"OperateButton\">{1}</a>", "btnOther", "其它");
                    break;
            }
            #endregion
        }

        operateButton.InnerHtml = builder.ToString();
    }

    #region 判断页面权限
    public static ResultMessage IsNodePageAuth(int iCurrentPageNodeId)
    {
        var isFlag = new AuthRoleNodeBll().IsNodePageAuth(CurrentUserID, iCurrentPageNodeId, IsSuper);
        return isFlag ? new ResultMessage { ErrorType = 1, MessageContent = "有操作权限." } : new ResultMessage { ErrorType = 5, MessageContent = "无操作权限." };
    }

    public static ResultMessage IsNodePageAuth(int[] iRangePage, int iCurrentPageNodeId)
    {
        Guid iUSERID = Utits.CurrentUserID;
        if (iUSERID == Guid.Empty)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 3;
            sRetrunModel.MessageContent = "未登录.";
            return sRetrunModel;
        }
        if (!iRangePage.Contains(iCurrentPageNodeId))
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "NodeId参数错误：该页面不具有操作该功能.";
            return sRetrunModel;
        }
        bool isFlag = new AuthRoleNodeBll().IsNodePageAuth(iUSERID, iCurrentPageNodeId, IsSuper);
        if (!isFlag)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 5;
            sRetrunModel.MessageContent = "无操作权限.";
            return sRetrunModel;
        }
        else
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 1;
            sRetrunModel.MessageContent = "有操作权限.";
            return sRetrunModel;
        }
    }
    #endregion


    #region 判断操作权限

    /// <summary>
    /// 判断页面上的按钮是否拥有操作权限
    /// </summary>
    /// <param name="iRangePage">能操作该按钮的页面数组</param>
    /// <param name="iCurrentPageNodeId">当前操作页面的ID</param>
    /// <param name="iCurrentButtonId">当前操作按钮的ID</param>
    /// <returns></returns>
    public static ResultMessage IsOperateAuth(int[] iRangePage, int iCurrentPageNodeId, int iCurrentButtonId)
    {
        Guid iUSERID = Utits.CurrentUserID;
        if (iUSERID == Guid.Empty)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 3;
            sRetrunModel.MessageContent = "未登录.";
            return sRetrunModel;
        }
        if (!iRangePage.Contains(iCurrentPageNodeId))
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "NodeId参数错误：该页面不能操作该功能.";
            return sRetrunModel;
        }
        var cBll = new AuthRoleNodeButtonBll();
        bool isFlag = cBll.IsAuthRoleNodeButton(iUSERID, iCurrentPageNodeId, iCurrentButtonId, IsSuper);
        if (!isFlag)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 5;
            sRetrunModel.MessageContent = "无操作权限.";
            return sRetrunModel;
        }
        else
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 1;
            sRetrunModel.MessageContent = "有操作权限.";
            return sRetrunModel;
        }
    }

    /// <summary>
    /// 判断页面上的按钮是否拥有操作权限
    /// </summary>
    /// <param name="iRangePage">能操作该按钮的页面数组</param>    
    /// <param name="iCurrentPageNodeId">当前操作页面的ID</param>
    /// <param name="iRangeButton">能操作按钮数组</param>
    /// <param name="iCurrentButtonId">当前操作按钮的ID</param>
    /// <returns></returns>
    public static ResultMessage IsOperateAuth(int[] iRangePage, int iCurrentPageNodeId, int[] iRangeButton, int iCurrentButtonId)
    {
        Guid iUSERID = Utits.CurrentUserID;
        if (iUSERID == Guid.Empty)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 3;
            sRetrunModel.MessageContent = "未登录.";
            return sRetrunModel;
        }
        if (!iRangePage.Contains(iCurrentPageNodeId))
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "NodeId参数错误：该页面不能操作该功能.";
            return sRetrunModel;
        }
        if (!iRangeButton.Contains(iCurrentButtonId))
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "ButtonId参数错误：该按钮不能操作该功能.";
            return sRetrunModel;
        }
        var cBll = new AuthRoleNodeButtonBll();
        bool isFlag = cBll.IsAuthRoleNodeButton(iUSERID, iCurrentPageNodeId, iCurrentButtonId, IsSuper);
        if (!isFlag)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 5;
            sRetrunModel.MessageContent = "无操作权限.";
            return sRetrunModel;
        }
        else
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 1;
            sRetrunModel.MessageContent = "有操作权限.";
            return sRetrunModel;
        }
    }

    #endregion


    #region 信息提示框
    public static void Alert(System.Web.UI.Page p, string messageContent)
    {
        string builder = string.Format("<script>alert('{0}');</script>", messageContent);
        p.ClientScript.RegisterStartupScript(p.GetType(), "js", builder);
    }
    public static void LayerAlert(System.Web.UI.Page p, string messageContent, int type)
    {
        string builder = string.Format("<script>layer.alert('{0}',{1});</script>", messageContent, type);
        p.ClientScript.RegisterStartupScript(p.GetType(), "js", builder);
    }
    public static void LayerMsg(System.Web.UI.Page p, string messageContent, int type)
    {
        string builder = string.Format("<script>layer.msg('{0}',2,{1});</script>", messageContent, type);
        p.ClientScript.RegisterStartupScript(p.GetType(), "js", builder);
    }
    #endregion

    /// <summary>
    /// 得到CheckBoxList中选中了的值,去掉最后一个分割符号
    /// </summary>
    /// <param name="checkList">CheckBoxList</param>
    /// <param name="separator">分割符号</param>
    /// <returns></returns>
    public static string GetChecked(CheckBoxList checkList, char separator)
    {
        string selval = "";
        for (var i = 0; i < checkList.Items.Count; i++)
        {
            if (checkList.Items[i].Selected)
            {
                selval += checkList.Items[i].Value + separator;
            }
        }
        if (!string.IsNullOrEmpty(selval))
        {
            selval = selval.TrimEnd(separator);
        }
        return selval;
    }

    /// <summary>
    /// 用字符串分割字符串
    /// 得到的就是通过模式串分割后得到的数组
    /// </summary>
    /// <param name="szString">将要分割的字符串</param>
    /// <param name="separator">用来分割的模式串</param>
    /// <returns></returns>
    public static string[] StringSplit(string szString, string separator)
    {
        // 在由正则表达式模式定义的位置(这里是逗号的位置)拆分输入字符串。
        return Regex.Split(szString, separator);
    }

    /// <summary>
    /// 返回与 Web 服务器上的指定虚拟路径相对应的物理文件路径。
    /// </summary>
    /// <param name="strPath"></param>
    /// <returns></returns>
    public static string GetMapPath(string strPath)
    {
        if (HttpContext.Current != null)
        {
            return HttpContext.Current.Server.MapPath(strPath);
        }

        //非web程序引用
        strPath = strPath.Replace("/", "\\");
        if (strPath.StartsWith("\\"))
        {
            strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
        }
        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
    }

    #region DownloadAttachment
    /// <summary>
    /// 下载附件
    /// </summary>
    /// <param name="ms">MemoryStream是内存流,为系统内存提供读写操作</param>
    /// <param name="fileName">附件名称</param>
    public static void DownloadAttachment(System.IO.MemoryStream ms, string fileName)
    {
        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Charset = "utf-8";
        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;fileName=" + HttpUtility.UrlEncode(fileName));
        System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        System.Web.HttpContext.Current.Response.End();
    }
    public static void DownloadAttachment(System.IO.FileInfo file, EExportFormat exportFormat)
    {
        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Charset = "GB2312";
        System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //添加头信息，为"文件下载/另存为"对话框指定默认文件名
        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(file.Name));
        //添加头信息，指定文件大小，让浏览器能够显示下载进度
        System.Web.HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

        //指定返回的是一个不能被客户端读取的流，必须被下载
        System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
        switch (exportFormat)
        {
            case EExportFormat.Doc:
                //指定返回的是一个不能被客户端读取的流，必须被下载
                System.Web.HttpContext.Current.Response.ContentType = "application/ms-word";
                break;
            case EExportFormat.Xls:
                //指定返回的是一个不能被客户端读取的流，必须被下载
                System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
                break;
            default:
                //指定返回的是一个不能被客户端读取的流，必须被下载
                //System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
                System.Web.HttpContext.Current.Response.ContentType = "application nd.ms-excel";
                break;
        }
        //把文件流发送到客户端
        System.Web.HttpContext.Current.Response.WriteFile(file.FullName);
        //停止页面的执行
        System.Web.HttpContext.Current.Response.End();
    }
    /// <summary>
    /// 下载附件
    /// cvs 逗号“，”换列
    /// excel “\t”换列
    /// cvs、excel “\n”换行
    /// </summary>
    /// <param name="fileName">导出名称</param>
    /// <param name="exportFormat">导出类型CSV，xls</param>
    /// <param name="buf">导出的内容</param>
    public static void DownloadAttachment(string fileName, EExportFormat exportFormat, System.Text.StringBuilder buf)
    {

        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Buffer = true;
        System.Web.HttpContext.Current.Response.Charset = "GB2312";
        System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        switch (exportFormat)
        {
            case EExportFormat.Doc:
                //添加头信息，为"文件下载/另存为"对话框指定默认文件名
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToString("yyyy-MM-dd") + ".doc");
                //指定返回的是一个不能被客户端读取的流，必须被下载
                System.Web.HttpContext.Current.Response.ContentType = "application/ms-word";
                break;
            case EExportFormat.Xls:
                //添加头信息，为"文件下载/另存为"对话框指定默认文件名
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
                //指定返回的是一个不能被客户端读取的流，必须被下载
                System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
                break;
            default:
                //添加头信息，为"文件下载/另存为"对话框指定默认文件名
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToString("yyyy-MM-dd") + ".csv");
                //指定返回的是一个不能被客户端读取的流，必须被下载
                //System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
                System.Web.HttpContext.Current.Response.ContentType = "application nd.ms-excel";
                break;
        }


        var rw = new System.IO.StringWriter();
        var hw = new System.Web.UI.HtmlTextWriter(rw);
        hw.Write(buf.ToString());
        System.Web.HttpContext.Current.Response.Write(rw.ToString());
        System.Web.HttpContext.Current.Response.End();
    }
    #endregion

    /// <summary>
    /// 产生不重复随机数
    /// </summary>
    /// <param name="count">共产生多少随机数</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>int[]数组</returns>
    public static int[] GetRandomNum(int count, int minValue, int maxValue)
    {
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        int length = maxValue - minValue + 1;
        byte[] keys = new byte[length];
        rnd.NextBytes(keys);
        int[] items = new int[length];
        for (int i = 0; i < length; i++)
        {
            items[i] = i + minValue;
        }
        Array.Sort(keys, items);
        int[] result = new int[count];
        Array.Copy(items, result, count);
        return result;
    }

    /// <summary>
    /// 产生随机字符串
    /// </summary>
    /// <returns>字符串位数</returns> 
    public static string GetRandomString(int length = 5)
    {
        int number;
        char code;
        string checkCode = String.Empty;
        var random = new Random(Guid.NewGuid().GetHashCode());

        for (int i = 0; i < length + 1; i++)
        {
            number = random.Next();

            if (number % 2 == 0)
                code = (char)('0' + (char)(number % 10));
            else
                code = (char)('A' + (char)(number % 26));
            checkCode += code.ToString();
        }
        return checkCode;
    }

    /// <summary>
    /// 产生随机字母
    /// </summary>
    /// <returns>字符串位数</returns>
    public static string GetRandomLetter(int length = 2)
    {
        int number;
        char code;
        string checkCode = String.Empty;
        System.Random random = new Random(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < length; i++)
        {
            number = random.Next();
            code = (char)('A' + (char)(number % 26));
            checkCode += code.ToString();
        }
        return checkCode;
    }

    /// <summary>
    /// 得到一个文件的大小(单位KB)
    /// </summary>
    /// <returns></returns>
    public static string GetFileSize(string file)
    {
        if (!System.IO.File.Exists(file))
        {
            return "";
        }
        System.IO.FileInfo fi = new System.IO.FileInfo(file);

        return (fi.Length / 1000).ToString("###,###");
    }

    /// <summary>
    /// Json特符字符过滤
    /// </summary>
    /// <param name="s">要过滤的源字符串</param>
    /// <returns>返回过滤的字符串</returns>
    public static string JsonCharFilter(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;
        s = s.Replace("\\", "\\\\");
        s = s.Replace("\b", "\\\b");
        s = s.Replace("\t", "\\\t");
        s = s.Replace("\n", "\\\n");
        s = s.Replace("\n", "\\\n");
        s = s.Replace("\f", "\\\f");
        s = s.Replace("\r", "\\\r");
        return s.Replace("\"", "\\\"");
    }

    #region bese64转为图片
    public static string base64TOImg(String stream, string fileUrl)
    {
        Byte[] streamByte = Convert.FromBase64String(stream);
        System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        string _fileSavePath = System.Web.HttpContext.Current.Server.MapPath(fileUrl);
        //当前待上传的服务端路径
        string _fileName = Guid.NewGuid() + ".jpg";
        //开始上传
        try
        {
            if (!System.IO.Directory.Exists(_fileSavePath))
                System.IO.Directory.CreateDirectory(_fileSavePath);

            img.Save(_fileSavePath + _fileName);
        }
        catch
        {

        }
        return _fileName;
    }

    #endregion

    #region Attachment通用
    public ResultMessage DeleteAttachment(int tableType, Guid id, Guid parentGuid, int parentId = 0)
    {
        id = id == Guid.Empty ? CommonLib.RequestParameters.PGuid("ID") : Guid.Empty;
        if (id == Guid.Empty)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "ID参数错误.";
            return sRetrunModel;
        }
        parentId = parentId == 0 ? CommonLib.RequestParameters.Pint("PID") : parentId;
        parentGuid = parentGuid == Guid.Empty ? CommonLib.RequestParameters.PGuid("PID") : parentGuid;
        if (parentGuid == Guid.Empty && parentId == 0)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "PID参数错误.";
            return sRetrunModel;
        }
        var cBll = new AttachmentsBll();
        bool isFlag = false;
        if (parentGuid != Guid.Empty)
        {
            isFlag = cBll.PhyDelete(id, parentGuid, tableType);
        }
        else
        {
            isFlag = cBll.PhyDelete(id, parentId, tableType);
        }
        if (isFlag)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 1;
            sRetrunModel.MessageContent = "删除成功.";
            return sRetrunModel;
        }
        else
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "删除失败.";
            return sRetrunModel;
        }
    }
    public ResultList SearchAttachment(int tableType, Guid parentGuid, int parentId = 0)
    {
        parentId = parentId == 0 ? CommonLib.RequestParameters.Pint("PID") : parentId;
        parentGuid = parentGuid == Guid.Empty ? CommonLib.RequestParameters.PGuid("PID") : parentGuid;
        if (parentGuid == Guid.Empty && parentId == 0)
        {
            var sRetrunModel = new ResultList();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "PID参数错误.";
            return sRetrunModel;
        }
        var cBll = new AttachmentsBll();
        if (parentGuid != Guid.Empty)
        {
            var list = cBll.SearchListByPid(parentGuid, tableType);
            var sReturnModel = new ResultList();
            sReturnModel.ErrorType = 1;
            sReturnModel.Data = list;
            return sReturnModel;
        }
        else
        {
            var list = cBll.SearchListByPid(parentId, tableType);
            var sReturnModel = new ResultList();
            sReturnModel.ErrorType = 1;
            sReturnModel.Data = list;
            return sReturnModel;
        }
    }
    public ResultMessage UploadAttachment(UploadParameter modelUp)
    {
        if (modelUp.TableType == 0)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败：TableType参数不建议设置为0.";
            return sRetrunModel;
        }

        if (modelUp.UploadLimit <= 0 || modelUp.UploadLimit > 300)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败：文件上传数量限制在1-300个.";
            return sRetrunModel;
        }

        if (modelUp.FileExtensionLimit == null)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败：FileExtensionLimit参数不能为null.";
            return sRetrunModel;
        }

        var ParentID = modelUp.ParentID == 0 ? CommonLib.RequestParameters.Pint("PID") : modelUp.ParentID;
        var ParentGUID = modelUp.ParentGUID == Guid.Empty ? CommonLib.RequestParameters.PGuid("PID") : modelUp.ParentGUID;
        if (ParentGUID == Guid.Empty && ParentID == 0)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败：PID参数错误.";
            return sRetrunModel;
        }

        var fileCollection = System.Web.HttpContext.Current.Request.Files;//文件列表
        if (fileCollection.Count <= 0)
        {
            //上传失败
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败：未接受到文件.";
            return sRetrunModel;
        }


        HttpPostedFile file = fileCollection[0];
        string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

        if (modelUp.IsUploadImages)
        {
            if (modelUp.FileExtensionLimit.Count <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "上传失败：图片应设置支持格式.";
                return sRetrunModel;
            }
            if (modelUp.FileContentLengthLimit <= 0 || modelUp.FileContentLengthLimit > 50 * 1024 * 1024)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "上传失败：图片大小范围大于0小于50M.";
                return sRetrunModel;
            }
        }

        if (modelUp.FileExtensionLimit.Count > 0)
        {
            if (!modelUp.FileExtensionLimit.Contains(fileExtension))
            {
                //不支持该后缀
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "上传失败：不支持该后缀.";
                return sRetrunModel;
            }
        }

        if (file.ContentLength > modelUp.FileContentLengthLimit)
        {
            //文件大小超出
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = string.Format("上传失败：文件不能超过{0}M.", modelUp.FileContentLengthLimit);
            return sRetrunModel;
        }

        var cBll = new AttachmentsBll();
        if (modelUp.UploadLimit > 1)//文件数量验证
        {
            bool isUl = false;
            if (ParentGUID != Guid.Empty)
            {
                isUl = cBll.Count(ParentGUID, modelUp.TableType) > modelUp.UploadLimit;
            }
            else
            {
                isUl = cBll.Count(ParentID, modelUp.TableType) > modelUp.UploadLimit;
            }

            if (isUl)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "上传失败：上传数量已超出，请先删除再上传.";
                return sRetrunModel;
            }
        }


        if (string.IsNullOrEmpty(modelUp.PathPrefix))
        {
            modelUp.PathPrefix = "/Upload/temp/";
        }
        string fileSavePath = HttpContext.Current.Server.MapPath(modelUp.PathPrefix);
        string newFileName = Guid.NewGuid() + fileExtension;
        try
        {
            if (!System.IO.Directory.Exists(fileSavePath))
                System.IO.Directory.CreateDirectory(fileSavePath);
            file.SaveAs(fileSavePath + newFileName);
        }
        catch
        {
            //上传异常
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败：上传异常.";
            return sRetrunModel;
        }

        //文件上传到服务器后，保存相应记录到数据库
        var item = new Attachments();
        item.AttachmentID = Guid.NewGuid();
        item.ParentGUID = ParentGUID;
        item.ParentID = ParentID;
        item.NewFileName = newFileName;
        item.OldFileName = System.IO.Path.GetFileName(file.FileName);
        item.FileExtension = fileExtension;
        item.FilePathPrefix = modelUp.PathPrefix;
        item.FileContentLength = file.ContentLength;
        item.FileContentType = file.ContentType;
        item.TableType = modelUp.TableType;
        item.IsValid = 1;
        item.OperateDate = DateTime.Now;
        item.WelfareCentreID = WelfareCentreID == Guid.Empty ? Config.WelfareCentreID : WelfareCentreID;

        bool isDel = true;
        if (modelUp.UploadLimit == 1)
        {
            if (ParentGUID != Guid.Empty)
            {
                isDel = cBll.PhyDelete(ParentGUID, modelUp.TableType);
            }
            else
                isDel = cBll.PhyDelete(ParentID, modelUp.TableType);
        }

        bool isFlag = false;
        if (isDel)
        {
            isFlag = cBll.Add(item);
        }
        if (isFlag)
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 1;
            sRetrunModel.MessageContent = "上传成功.";
            return sRetrunModel;
        }
        else
        {
            var sRetrunModel = new ResultMessage();
            sRetrunModel.ErrorType = 0;
            sRetrunModel.MessageContent = "上传失败.";
            return sRetrunModel;
        }
    }
    #endregion
}
