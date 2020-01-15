using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLib
{
    public static class JsonHelper
    {
        /// <summary>
        /// Entity转化为JSON格式
        /// </summary>
        /// <typeparam name="TSource">数据类型</typeparam>
        /// <param name="source">实体对象</param>
        /// <param name="isRef">实体中是否有REF或SET</param>
        /// <returns>JSON格式字符串</returns>
        public static string ToJSON<TSource>(this TSource source, Boolean isRef) where TSource : new()
        {
            return source == null ? null : Serialize(source, isRef);
        }

        /// <summary>
        /// IList转化为JSON格式
        /// </summary>
        /// <typeparam name="TSource">数据类型</typeparam>
        /// <param name="source">IList对象</param>
        /// <param name="isRef">实体中是否有REF或SET</param>
        /// <returns>JSON格式字符串</returns>
        public static string ToJSON<TSource>(this IList<TSource> source, Boolean isRef) where TSource : new()
        {
            if (source == null)
                return "[]";
            var szRet = Serialize(source, isRef);
            return string.IsNullOrEmpty(szRet) ? "[]" : szRet;
        }
        public static string ToJSON<TSource>(this TSource source) where TSource : new()
        {
            return source == null ? null : Serialize(source);
        }

        public static string ToJSON<TSource>(this IList<TSource> source) where TSource : new()
        {

            if (source == null)
                return "[]";
            var szRet = Serialize(source);
            return string.IsNullOrEmpty(szRet) ? "[]" : szRet;
        }

        public static string ToJSON(this DataTable source)
        {
            var szRet = Serialize(source);
            return string.IsNullOrEmpty(szRet) ? "[]" : szRet;
        }

        #region Serialize
        public static string Serialize(object obj)
        {
            try
            {
                var iso = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                //var jSet= new JsonSerializerSettings {NullValueHandling = NullValueHandling.Include};
                return JsonConvert.SerializeObject(obj, iso);
            }
            catch
            {
                try
                {
                    return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DateFormatString = "yyyy-MM-dd HH:mm:ss"
                    });
                }
                catch (Exception ex)
                {
                    MessageLog.WriteLog(new LogParameterModel { LogLevel = ELogLevel.Error, ClassName = "CommonLib.JsonHelper", Title = "Serialize(object obj)异常信息", Message = ex.Message });
                    return null;
                }
            }
        }
        private static string Serialize(object obj, bool isReference)
        {
            if (isReference)
            {

                try
                {
                    return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DateFormatString = "yyyy-MM-dd HH:mm:ss"
                    });
                }
                catch
                {
                    try
                    {
                        var iso = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                        return JsonConvert.SerializeObject(obj, iso);
                    }
                    catch (Exception ex)
                    {
                        MessageLog.WriteLog(new LogParameterModel { LogLevel = ELogLevel.Error, ClassName = "CommonLib.JsonHelper", Title = "Serialize(object obj, bool isReference)异常信息", Message = ex.Message });
                        return null;
                    }
                }
            }
            else
                return Serialize(obj);
        }
        public static T Deserialize<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
                return null;
            try
            {
                return new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(json)), typeof(T)) as T;
            }
            catch
            {
                return null;
            }
        }
        public static IList<T> DeserializeToIList<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
                return null;
            try
            {
                return new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(json)), typeof(IList<T>)) as IList<T>;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            try
            {
                return JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            }
            catch
            {
                return default(T);
            }
        }
        #endregion
    }
}
