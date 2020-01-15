using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using CommonLib;

namespace DAL
{
    public class DeptsDao : BaseServiceEf<MISDBContainer>
    {
        public bool AddOrUpdae(Depts item)
        {
            try
            {
                var isAdd = true;
                var deptId = new Guid();
                if (item.DeptID == Guid.Empty)
                {
                    deptId = Guid.NewGuid();
                }
                else { deptId = item.DeptID; }

                #region
                var query = _context.Set<Depts>().FirstOrDefault(c => c.DeptID == item.DeptID && c.WelfareCentreID == item.WelfareCentreID && c.IsValid == 1);
                if (query == null)
                {
                    isAdd = true;
                    var model = new Depts();
                    model.DeptID = deptId;
                    model.DeptName = item.DeptName;
                    model.DeptCode = item.DeptCode;
                    model.OperateDate = item.OperateDate;
                    model.CreateDate = item.CreateDate;
                    model.IsValid = 1;
                    model.Remark = item.Remark;
                    model.WelfareCentreID = item.WelfareCentreID;
                    _context.Set<Depts>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.DeptID = deptId;
                    query.DeptName = item.DeptName;
                    query.DeptCode = item.DeptCode;
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
                    MethodName = "AddOrUpdaeDepts",
                    MethodParameters = "添加失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }

        public bool PhysicalDelete(Guid Id, Guid welfareCentreId)
        {
            var query = _context.Set<Depts>().FirstOrDefault(c => c.DeptID == Id && c.WelfareCentreID == welfareCentreId);
            if (query != null)
            {
                _context.Set<Depts>().Remove(query);
            }
            _context.SaveChanges();
            return true;
        }
        public bool OperateDataStatus(Guid Id, Guid welfareCentreId, int isValid)
        {
            var query = _context.Set<Depts>().FirstOrDefault(c => c.DeptID == Id && c.WelfareCentreID == welfareCentreId);
            if (query != null)
            {
                query.DeptID = Id;
                query.OperateDate = DateTime.Now;
                query.IsValid = isValid;
            }
            _context.SaveChanges();
            return true;
        }
    }
}
