using System;

namespace CommonLib
{
    public class Config
    {
        public static Guid WelfareCentreID
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["WelfareCentreId"];
                if (RegexValidate.IsGuid(temp))
                    return new Guid(temp);
                return Guid.Empty;
            }
        }
        public static int FwqType
        {
            get
            {
                var fwqType = System.Configuration.ConfigurationManager.AppSettings["FwqType"];
                return string.IsNullOrEmpty(fwqType) ? 0 : int.Parse(fwqType);
            }
        }
        public static string QueueURL
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["QueueURL"];
                return temp;
            }
        }
        /// <summary>
        /// SuperID
        /// </summary>
        public static Guid SuperID
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["SuperID"];
                if (RegexValidate.IsGuid(temp))
                    return new Guid(temp);
                return Guid.Empty;
            }
        }
        /// <summary>
        /// AdminID
        /// </summary>
        public static Guid AdminID
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["AdminID"];
                if (RegexValidate.IsGuid(temp))
                    return new Guid(temp);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// DoorID
        /// </summary>
        public static Guid DoorID
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["DoorID"];
                if (RegexValidate.IsGuid(temp))
                    return new Guid(temp);
                return Guid.Empty;
            }
        }
        public static string MinYing_URL
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["MinYing_URL"];
                if (temp.Length > 0)
                    return temp;
                return "";
            }
        }
        /// <summary>
        /// 暂未定义--用于养老院院长端webService接口网址
        /// </summary>
        public static string YuanZhan_URL
        {
            get
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["YuanZhan_URL"];
                return string.IsNullOrEmpty(url) ? "" : url;
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public static readonly string Version = System.Configuration.ConfigurationManager.AppSettings["Version"];
        private static readonly string EntityName = System.Configuration.ConfigurationManager.AppSettings["EntityName"];
        private static readonly string DatabaseServer = System.Configuration.ConfigurationManager.AppSettings["DatabaseServer"];
        private static readonly string DatabaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
        private static readonly string DatabaseUid = System.Configuration.ConfigurationManager.AppSettings["DatabaseUid"];
        private static readonly string DatabasePwd = System.Configuration.ConfigurationManager.AppSettings["DatabasePwd"];
        /// <summary>
        /// 默认Ef
        /// </summary>
        /// <param name="isEf"></param>
        /// <returns></returns>
        public static string PlatformConnectionString(bool isEf = true)
        {
            if (isEf)
            {
                return string.Concat("metadata=res://*/",
                    EntityName, ".csdl|res://*/",
                    EntityName, ".ssdl|res://*/",
                    EntityName, ".msl;provider=System.Data.SqlClient;provider connection string='Data Source=",
                    DatabaseServer, ";Initial Catalog=",
                    DatabaseName, ";Persist Security Info=True;User ID=",
                    DatabaseUid, ";Password=",
                    DatabasePwd, ";MultipleActiveResultSets=True'");
                //return System.Configuration.ConfigurationManager.ConnectionStrings["PlatformConnectionEFMSSQL"].ConnectionString;
            }
            else
            {
                return string.Concat("server=", DatabaseServer, ";database=", DatabaseName, ";uid=", DatabaseUid, ";pwd=", DatabasePwd, ";");
                //return System.Configuration.ConfigurationManager.ConnectionStrings["PlatformConnectionMSSQL"].ConnectionString;
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DataBaseType
        {
            get
            {
                string dbType = System.Configuration.ConfigurationManager.AppSettings["DatabaseType"];
                return string.IsNullOrEmpty(dbType) ? "MSSQL" : dbType.ToUpper();
            }
        }

        /// <summary>
        /// 系统初始密码
        /// </summary>
        public static string SystemInitPassword
        {
            get
            {
                string pass = System.Configuration.ConfigurationManager.AppSettings["InitPassword"];
                return string.IsNullOrEmpty(pass) ? "111111" : pass.Trim();
            }
        }

        /// <summary>
        /// 得到当前主题
        /// </summary>
        public static string Theme
        {
            get
            {
                var cookie = System.Web.HttpContext.Current.Request.Cookies["theme_platform"];
                return cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : "Blue";
            }
        }
        public static string UserNameSuper
        {
            get { return "super"; }
        }

        #region   护理量同步时间设置
        /// <summary>
        /// 护理量同步时间设置
        /// </summary>
        public static string IsHour
        {
            get
            {
                string SynHour = System.Configuration.ConfigurationManager.AppSettings["IsHour"];
                return string.IsNullOrEmpty(SynHour) ? "" : SynHour.Trim();
            }
        }
        #endregion

        #region   数据是否同步
        /// <summary>
        /// 上传
        /// </summary>
        public static string IsSynchroUpLoaddata
        {
            get
            {
                string SynchroUpLoaddata = System.Configuration.ConfigurationManager.AppSettings["IsSynchroUpLoaddata"];
                return string.IsNullOrEmpty(SynchroUpLoaddata) ? "" : SynchroUpLoaddata.Trim();
            }
        }
        /// <summary>
        /// 下载
        /// </summary>
        public static string IsSynchroDowndata
        {
            get
            {
                string SynchroDowndata = System.Configuration.ConfigurationManager.AppSettings["IsSynchroDowndata"];
                return string.IsNullOrEmpty(SynchroDowndata) ? "" : SynchroDowndata.Trim();
            }
        }
        /// <summary>
        /// Demo数据库数据添加
        /// </summary>
        public static string IsSynchroDemodata
        {
            get
            {
                string SynchroDemodata = System.Configuration.ConfigurationManager.AppSettings["IsSynchroDemodata"];
                return string.IsNullOrEmpty(SynchroDemodata) ? "" : SynchroDemodata.Trim();
            }
        }

        /// <summary>
        /// 更新APP
        /// </summary>
        public static string IsSynchroUpAppData
        {
            get
            {
                string SynchroUpLoaddata = System.Configuration.ConfigurationManager.AppSettings["IsSynchroUpAppData"];
                return string.IsNullOrEmpty(SynchroUpLoaddata) ? "" : SynchroUpLoaddata.Trim();
            }
        }
        public static string Fw_URL
        {
            get
            {
                string Fw_URL = System.Configuration.ConfigurationManager.AppSettings["Fw_URL"];
                return string.IsNullOrEmpty(Fw_URL) ? "" : Fw_URL.Trim();
            }
        }

        public static string IsSynchrodata
        {
            get
            {
                string Synchrodata = System.Configuration.ConfigurationManager.AppSettings["IsSynchrodata"];
                return string.IsNullOrEmpty(Synchrodata) ? "" : Synchrodata.Trim();
            }
        }

        public static string AppEndURL
        {
            get
            {
                string temp = System.Configuration.ConfigurationManager.AppSettings["AppEndURL"];
                return temp ?? "";
            }
        }
        #endregion

    }
}
