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
    
    public partial class vUsers
    {
        public System.Guid UserID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<System.Guid> RoleID { get; set; }
        public Nullable<System.Guid> DeptID { get; set; }
        public string RealName { get; set; }
        public string RealPassword { get; set; }
        public string Phone { get; set; }
        public string Telphone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> LoginLastDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> OperateDate { get; set; }
        public Nullable<int> IsValid { get; set; }
        public string Remark { get; set; }
        public string RoleName { get; set; }
        public string DeptName { get; set; }
        public Nullable<int> UserType { get; set; }
        public Nullable<System.Guid> WelfareCentreID { get; set; }
        public string IcCardNo { get; set; }
    }
}
