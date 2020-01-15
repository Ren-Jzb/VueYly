using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VO
{
    public class ResultModel
    {
        public ResultModel()
        {
            resultCode = 0;
            resultMessage = "操作失败.";
        }
        public ResultModel(int resultCode, string resultMessage)
        {
            this.resultCode = resultCode;
            this.resultMessage = resultMessage;
        }

        /// <summary>
        /// 是否成功标识（1：成功；0：失败）
        /// </summary>
        public int resultCode { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string resultMessage { get; set; }
    }
}
