using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 通用详细JSON数据
    /// </summary>
    public class ResultDetail : ResultMessage
    {
        public ResultDetail()
        {

        }
        /// <summary>
        /// 实体对象
        /// </summary>
        public object Entity { get; set; }
    }
}
