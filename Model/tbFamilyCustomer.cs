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
    
    public partial class tbFamilyCustomer
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<System.Guid> FamilyUsersID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> IsValid { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public Nullable<System.Guid> AuditUserID { get; set; }
    }
}
