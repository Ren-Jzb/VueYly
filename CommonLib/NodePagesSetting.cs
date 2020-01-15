using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public static class NodePagesSetting
    {
        /// <summary>
        /// 定义关于Users对应的所有相关页面
        /// 列表页面，新增页面，修改页面，详细页面
        /// </summary>
        public enum EUsers { ListPage = 100, AddPage = 101, EditPage = 102, DetailPage = 103, ListPageF = 4 }

        public enum ECustomer { ListPage = 1100, AddPage = 1101, EditPage = 1102, DetailPage = 1103 }

        public enum EOrgTLJGCongYe { ListPage = 9910, ListPage90 = 9012, AddPage = 9911, EditPage = 9912, DetailPage = 9913 }

        public enum ERoles { ListPage = 200, AddPage = 201, EditPage = 202, DetailPage = 203 }

        public enum EDepts { ListPage = 300, AddPage = 301, EditPage = 302, DetailPage = 303 }

        public enum EPaymentPlan { ListPage = 120, AddPage = 121, EditPage = 122, DetailPage = 123 }
        
    }
}
