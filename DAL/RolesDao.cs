using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using CommonLib;

namespace DAL
{
    public class RolesDao : BaseServiceEf<MISDBContainer>
    {
        public bool AddOrUpdae(Roles item)
        {
            try
            {
                var isAdd = true;
                var roleId = new Guid();
                if (item.RoleID == Guid.Empty)
                {
                    roleId = Guid.NewGuid();
                }
                else { roleId = item.RoleID; }
                #region
                var query = _context.Set<Roles>().FirstOrDefault(c => c.RoleID == item.RoleID);
                if (query == null)
                {
                    isAdd = true;
                    var model = new Roles();
                    model.RoleID = roleId;
                    model.RoleName = item.RoleName;
                    model.RoleCode = item.RoleCode;
                    model.OperateDate = item.OperateDate;
                    model.CreateDate = item.CreateDate;
                    model.IsValid = 1;
                    model.Remark = item.Remark;
                    model.WelfareCentreID = item.WelfareCentreID;
                    _context.Set<Roles>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.RoleID = roleId;
                    query.RoleName = item.RoleName;
                    query.RoleCode = item.RoleCode;
                    query.OperateDate = item.OperateDate;
                    query.Remark = item.Remark;
                    query.WelfareCentreID = item.WelfareCentreID;
                }
                #endregion

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "AddOrUpdaeRoles",
                    MethodParameters = "添加失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }

        public bool PhysicalDelete(Guid Id, Guid welfareCentreId)
        {
            var query = _context.Set<Roles>().FirstOrDefault(c => c.RoleID == Id && c.WelfareCentreID == welfareCentreId);
            if (query != null)
            {
                _context.Set<Roles>().Remove(query);
            }
            _context.SaveChanges();
            return true;
        }
        public bool OperateDataStatus(Guid Id, Guid welfareCentreId, int isValid)
        {
            var query = _context.Set<Roles>().FirstOrDefault(c => c.RoleID == Id && c.WelfareCentreID == welfareCentreId);
            if (query != null)
            {
                query.RoleID = Id;
                query.OperateDate = DateTime.Now;
                query.IsValid = isValid;
            }
            _context.SaveChanges();
            return true;
        }
    }
}
