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
    
    public partial class tbLaoRenFanShenJiLu
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CustomerId { get; set; }
        public Nullable<System.Guid> HgId { get; set; }
        public Nullable<int> FanShen { get; set; }
        public Nullable<int> PiFuQingKuang { get; set; }
        public Nullable<System.DateTime> NursingDate { get; set; }
        public Nullable<System.Guid> OperateUserID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> OperateDate { get; set; }
        public Nullable<int> IsValid { get; set; }
        public string Remark { get; set; }
        public Nullable<System.Guid> WelfareCentreID { get; set; }
    }
}
