//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log4net
    {
        public System.Guid LogID { get; set; }
        public Nullable<System.DateTime> log_date { get; set; }
        public string log_thread { get; set; }
        public string log_level { get; set; }
        public string log_logger { get; set; }
        public string log_message { get; set; }
        public string log_exception { get; set; }
        public string log_TableID { get; set; }
        public Nullable<System.Guid> log_WelfareCentreID { get; set; }
        public Nullable<System.Guid> log_UserID { get; set; }
        public string log_UserName { get; set; }
        public string log_State { get; set; }
        public string log_StateName { get; set; }
        public string log_ClassName { get; set; }
    }
}
