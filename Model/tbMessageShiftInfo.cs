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
    
    public partial class tbMessageShiftInfo
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> MessageId { get; set; }
        public string shiftContent { get; set; }
        public Nullable<int> zrs { get; set; }
        public Nullable<int> syrs { get; set; }
        public Nullable<int> ry { get; set; }
        public Nullable<int> cy { get; set; }
        public Nullable<int> sw { get; set; }
        public Nullable<int> qj { get; set; }
        public Nullable<int> zr { get; set; }
        public Nullable<int> zc { get; set; }
        public Nullable<int> zwbr { get; set; }
        public Nullable<int> wcjz { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> OperatorUserID { get; set; }
        public Nullable<System.DateTime> OperateDate { get; set; }
        public Nullable<int> IsValid { get; set; }
        public string Remark { get; set; }
        public Nullable<System.Guid> WelfareCentreID { get; set; }
    }
}
