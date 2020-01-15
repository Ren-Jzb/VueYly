using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 通用简单信息JSON数据
    /// </summary>
    public class ResultMessage
    {
        public ResultMessage()
        {
            ErrorType = 0;
            MessageContent = "";
        }

        /// <summary>
        /// <para>错误类型 错误0-50系统预定 其它为自定义错误</para>
        /// <para>0错误 1成功 2请求地址不正确 3未登录 4无页面权限 5无操作权限</para>
        /// </summary>
        public int ErrorType { get; set; }
        /// <summary>
        /// 提示信息内容
        /// </summary>
        public string MessageContent { get; set; }
    }


}
