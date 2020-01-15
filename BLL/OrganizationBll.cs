using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BLL
{

    public class OrganizationBll
    {
        public IList<tbOrganization> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szSrevices = new DbHelperEfSql<tbOrganization>();
            return szSrevices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }
        public tbOrganization SearchItemByNew(Guid? iWelfareId)
        {
            var szServices = new DbHelperEfSql<tbOrganization>();
            return szServices.SearchBySingle(c => c.IsValid == 1 && c.WelfareId == iWelfareId, true, c => c.OperateDate);
        }
        public bool AddOrUpdate(tbOrganization item, bool isAdd)
        {
            var szServices = new DbHelperEfSql<tbOrganization>();
            return isAdd ? szServices.Add(item) : szServices.Update(item, c => c.ID == item.ID);
        }
        public tbOrganization GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<tbOrganization>();
            return szServices.SearchBySingle(c => c.ID == id);
        }
        public tbOrganization GetObjectByWelfareId(Guid? welfareId)
        {
            var szServices = new DbHelperEfSql<tbOrganization>();
            return szServices.SearchBySingle(c => c.WelfareId == welfareId && c.IsValid == 1);
        }
        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<tbOrganization>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.ID.ToString()));
        }
        public bool PhysicalDeleteByCondition(Guid id)
        {
            var szServices = new DbHelperEfSql<tbOrganization>();
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
            string sql = string.Format("UPDATE tbOrganization SET IsValid={0},OperateDate=getdate() WHERE ID in ({1})", (int)eSystemStatus, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        public IList<tbOrganization> SearchListByCondition()
        {
            var szSrevices = new DbHelperEfSql<tbOrganization>();
            return szSrevices.SearchListByCondition(c => c.IsValid == 1);
        }

        public IList<tbOrganization> selectAll()
        {
            var szSrevices = new DbHelperEfSql<tbOrganization>();
            return szSrevices.SearchListByCondition(c => c.IsValid == 1);
        }

    }
}
