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
    
    public partial class tbNursingPerExt
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<int> NursingRankTopID { get; set; }
        public Nullable<int> NursingRankID { get; set; }
        public string extTitle { get; set; }
        public string extContent { get; set; }
        public Nullable<System.Guid> HgID { get; set; }
        public Nullable<System.Guid> NursingMessageID { get; set; }
        public Nullable<System.DateTime> NursingTime { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> OperatorUserID { get; set; }
        public Nullable<System.DateTime> OperateDate { get; set; }
        public Nullable<int> IsValid { get; set; }
        public string Remark { get; set; }
        public Nullable<System.Guid> WelfareCentreID { get; set; }
    }
}
