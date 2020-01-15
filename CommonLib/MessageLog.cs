using System;
using System.Linq;
using System.Text;
using System.Web;
using log4net.DateFormatter;

namespace CommonLib
{
    public enum ELogLevel
    {
        None = 0,
        /// <summary>
        /// 指出细粒度信息事件对调试应用程序是非常有帮助的。
        /// </summary>
        Debug = 1,
        /// <summary>
        /// 消息在粗粒度级别上突出强调应用程序的运行过程。
        /// </summary>
        Info = 2,
        /// <summary>
        /// 表明会出现潜在错误的情形。
        /// </summary>
        Warn = 3,
        /// <summary>
        /// 指出虽然发生错误事件，但仍然不影响系统的继续运行。
        /// </summary>
        Error = 4,
        /// <summary>
        /// 指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        Fatal = 5
    }

    public class LogParameterModel
    {
        public LogParameterModel()
        {
            LogLevel = ELogLevel.Info;
            MethodName = "";
            ClassName = "";
            Title = "";
            Message = "";
            LogSize = 100;
            LogExt = "log";
        }

        /// <summary>
        /// 日志等级(默认Info)
        /// </summary>
        public ELogLevel LogLevel { get; set; }
        /// <summary>
        /// 执行-方法参数
        /// </summary>
        public string MethodParameters { get; set; }
        /// <summary>
        /// 执行-方法名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 执行-类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 日志标题（可为空）
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 日志内容（可为空）
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日志路径前缀（建议采用自动路径"/log/yyyy/MM/dd"）
        /// </summary>
        public string PathPrefix { get; set; }

        /// <summary>
        ///日志文件名不包括后缀（建议采用自动命名"1"）
        /// </summary>
        public string LogName { get; set; }
        /// <summary>
        /// 日志文件名后缀不包括.(默认log)
        /// </summary>
        public string LogExt { get; set; }

        /// <summary>
        /// 超过多少M自动命名加1（默认100M）
        /// </summary>
        public int LogSize { get; set; }
    }


    /// <summary>
    /// 错误日志类
    /// </summary>
    public class MessageLog
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger("Loggering");

        #region MyRegion
        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }

        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }
        #endregion


        public static void WriteLog(LogParameterModel item)
        {
            DateTime nowTime = DateTime.Now;
            //switch (item.LogLevel)
            //{
            //    case ELogLevel.Debug:

            //        break;
            //    case ELogLevel.Info:

            //        break;
            //    case ELogLevel.Error:

            //        break;
            //    case ELogLevel.Warn:

            //        break;
            //    case ELogLevel.Fatal:
            //        break;
            //}
            if (string.IsNullOrEmpty(item.ClassName))
            {
                item.ClassName = "";
            }
            if (string.IsNullOrEmpty(item.Title))
            {
                item.Title = "";
            }
            if (string.IsNullOrEmpty(item.Message))
            {
                item.Message = "";
            }
            if (string.IsNullOrEmpty(item.PathPrefix))
            {
                item.PathPrefix = "/log/" + nowTime.ToString("yyyy/MM/dd");
            }
            if (string.IsNullOrEmpty(item.LogName))
            {
                item.LogName = nowTime.ToString("yyyyMMdd");
            }
            if (string.IsNullOrEmpty(item.LogExt))
            {
                item.LogExt = "log";
            }


            string directory = GetMapPath(item.PathPrefix);
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }
            try
            {
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);
                string path = string.Format("{0}/{1}.{2}", directory, item.LogName, item.LogExt);

                var buf = new StringBuilder();
                buf.Append("日志时间:");
                buf.Append(DateTime.Now);
                buf.Append("\t");
                buf.Append("日志等级:");
                buf.Append(item.LogLevel);
                if (!string.IsNullOrEmpty(item.Title))
                {
                    buf.Append("\t");
                    buf.Append("日志标题:");
                    buf.Append(item.Title);

                }
                if (!string.IsNullOrEmpty(item.Message))
                {
                    buf.Append("\t");
                    buf.Append("日志信息:");
                    buf.Append(item.Message);
                }
                if (!string.IsNullOrEmpty(item.ClassName))
                {
                    buf.Append("\t");
                    buf.Append("ClassName:");
                    buf.Append(item.ClassName);
                }
                if (!string.IsNullOrEmpty(item.MethodName))
                {
                    buf.Append("\t");
                    buf.Append("MethodName:");
                    buf.Append(item.MethodName);
                }
                if (!string.IsNullOrEmpty(item.MethodParameters))
                {
                    buf.Append("\t");
                    buf.Append("MethodParameters:");
                    buf.Append(item.MethodParameters);
                }

                if (!System.IO.File.Exists(path))
                {
                    using (var sw = System.IO.File.CreateText(path))
                    {
                        sw.WriteLine(buf);
                        sw.WriteLine();
                        sw.Close();
                    }
                }
                else
                {
                    var fileinfo = new System.IO.FileInfo(path);
                    using (var sw = fileinfo.AppendText())
                    {
                        sw.WriteLine(buf);
                        sw.WriteLine();
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static string GetMapPath(string tempPath)
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Server.MapPath(tempPath);
                }
                tempPath = tempPath.Replace("/", "\\");
                if (tempPath.StartsWith("\\"))
                {
                    tempPath = tempPath.Substring(tempPath.IndexOf('\\', 0)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tempPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static void ClearLog()
        {
            string szPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log";
            if (!System.IO.Directory.Exists(szPath))
                return;
            string[] szLogFiles = System.IO.Directory.GetFiles(szPath);
            string szFileDate = string.Empty;
            TimeSpan span = new TimeSpan();
            var indexofDot = 0;
            foreach (string szFileName in szLogFiles)
            {
                if (szFileName == null) continue;
                indexofDot = szFileName.LastIndexOf(".");
                if (indexofDot > 8)
                {
                    szFileDate = szFileName.Substring(indexofDot - 8, 8);
                    szFileDate = szFileDate.Insert(4, "-");
                    szFileDate = szFileDate.Insert(7, "-");
                    try
                    {
                        span = DateTime.Today - DateTime.Parse(szFileDate);
                        if (span.Days > 30)
                        {
                            System.IO.File.Delete(szFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
            }
        }

    }
}
