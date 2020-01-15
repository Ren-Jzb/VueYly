using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 通用列表JSON数据
    /// </summary>
    public class ResultList : ResultMessage
    {
        public ResultList()
        {
            CurrentPage = 1;
            PageSize = 20;
        }

        /// <summary>
        /// 当前页码默认1
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页条数，默认20条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// 页的总数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 民政专用JSON数据
    /// </summary>
    public class ResultBasicList : ResultMessage
    {
        /// <summary>
        /// 数据内容
        /// </summary>
        public object Data { get; set; }
    }
}
