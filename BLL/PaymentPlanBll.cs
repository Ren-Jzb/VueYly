using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using CommonLib;

namespace BLL
{
    public class PaymentPlanBll
    {
        public tbPaymentPlan GetById(Guid? welfareCentreId, Guid CustomerId)
        {
            var szServices = new DbHelperEfSql<tbPaymentPlan>();
            return szServices.SearchBySingle(c => c.WelfareCentreID == welfareCentreId && c.CustomerId == CustomerId && c.IsValid == 1);
        }

        public IList<vPaymentPlan> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szSrevices = new DbHelperEfSql<vPaymentPlan>();
            return szSrevices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<tbPaymentPlan>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.ID.ToString()));
        }
        public bool PhysicalDelete(Guid customerId, Guid bedId, Guid welfareCentreId)
        {
            var dao = new PaymentPlanDao();
            return dao.PhysicalDelete(customerId, bedId, welfareCentreId);
        }
        public bool OperateDataStatus(Guid customerId, Guid bedId, Guid welfareCentreId, int isValid)
        {
            var dao = new PaymentPlanDao();
            return dao.OperateDataStatus(customerId, bedId, welfareCentreId, isValid);
        }
        public bool AddOrUpdate(tbPaymentPlan item)
        {
            var dao = new PaymentPlanDao();
            return dao.AddOrUpdae(item);
        }
        public vPaymentPlan GetVObjectById(Guid Id)
        {
            var szServices = new DbHelperEfSql<vPaymentPlan>();
            return szServices.SearchBySingle(c => c.ID == Id);
        }

        public bool EditPaymentPlan(tbPaymentPlan item)
        {
            var dao = new PaymentPlanDao();
            return dao.AddOrUpdae(item);
        }

        public bool DeletePaymentPlan(string[] ids, int isValid)
        {
            if (ids == null || ids.Length <= 0) return false;
            var buf = new System.Text.StringBuilder();
            foreach (var s in ids)
            {
                buf.AppendFormat("'{0}',", s);
            }
            string szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("UPDATE tbPaymentPlan SET IsValid={0},OperateDate=getdate() WHERE ID in ({1})", isValid, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

    }
}
