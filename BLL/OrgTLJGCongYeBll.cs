using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BLL
{
    public class OrgTLJGCongYeBll
    {
        public IList<tbOrgTLJGCongYe> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szSrevices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return szSrevices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }
        public bool AddOrUpdate(tbOrgTLJGCongYe item, bool isAdd)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return isAdd ? szServices.Add(item) : szServices.Update(item, c => c.ID == item.ID);
        }
        public bool AddOrUpdate(tbOrgTLJGCongYe item)
        {
            var dao = new OrgTLJGCongYeDao();
            return dao.AddOrUpdae(item);

            //var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            //if (item.ID == Guid.Empty)
            //{
            //    item.ID = Guid.NewGuid();
            //    item.CreateDate = DateTime.Now;
            //    item.IsValid = 1;
            //    return szServices.Add(item);
            //}
            //else
            //    return szServices.Update(item, c => c.ID == item.ID);
        }
        public tbOrgTLJGCongYe GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return szServices.SearchBySingle(c => c.ID == id);
        }
        public tbOrgTLJGCongYe GetObjectByWelfareId(Guid welfareId)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return szServices.SearchBySingle(c => c.WelfareId == welfareId);
        }
        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.ID.ToString()));
        }
        public bool PhysicalDeleteByConditions(Guid id)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return szServices.PhysicalDeleteByCondition(c => c.ID == id);
        }

        public bool LogicDeleteByCondition(string[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Deleted);
        }
        public bool EnableByCodeition(string[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Valid);
        }
        public bool DisableByCodeition(string[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Forbidden);
        }
        private bool OperateDataStatus(string[] ids, ESystemStatus eSystemStatus)
        {
            if (ids.Length <= 0) return false;
            var buf = new System.Text.StringBuilder();
            foreach (var s in ids)
            {
                buf.AppendFormat("'{0}',", s);
            }
            string szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("UPDATE tbOrgTLJGCongYe SET IsValid={0},OperateDate=getdate() WHERE ID in ({1})", (int)eSystemStatus, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }


        public IList<tbOrgTLJGCongYe> SearchListByValid(Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            return szServices.SearchListByCondition(c => c.IsValid == 1 && c.WelfareId == welfareCentreId);
        }

    }
}
