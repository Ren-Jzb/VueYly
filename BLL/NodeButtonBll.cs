using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class NodeButtonBll
    {

        #region Add、Update、Delete Entity
        public bool AddOrUpdate(NodeButton item)
        {
            var szServices = new DbHelperEfSql<NodeButton>();
            return item.NodeButtonId <= 0 ? szServices.Add(item) : szServices.Update(item, c => c.NodeButtonId == item.NodeButtonId);
        }

        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<NodeButton>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.NodeButtonId.ToString()));
        }

        public bool LogicDeleteByCondition(int[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Deleted);
        }
        public bool EnableByCodeition(int[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Valid);
        }

        public bool DisableByCodeition(int[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Forbidden);
        }

        private bool OperateDataStatus(int[] ids, ESystemStatus eSystemStatus)
        {
            if (ids == null || ids.Length <= 0) return false;
            var buf = new System.Text.StringBuilder();
            foreach (var s in ids)
            {
                buf.AppendFormat("{0},", s);
            }
            string szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("UPDATE NodeButton SET IsValid={0},OperateDate=getdate() WHERE NodeButtonId in ({1})", (int)eSystemStatus, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        public NodeButton GetObjectByCondition(int nodeId, int buttonId)
        {
            var szServices = new DbHelperEfSql<NodeButton>();
            return szServices.SearchBySingle(c => c.NodeId == nodeId && c.ButtonId == buttonId);
        }
        public NodeButton GetObjectById(int id)
        {
            var szServices = new DbHelperEfSql<NodeButton>();
            return szServices.SearchBySingle(c => c.NodeButtonId == id);
        }

        public IList<NodeButton> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<NodeButton>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }
        #endregion

        public IList<vNodeButton> SearchByPagevCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<vNodeButton>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public IList<vNodeButton> SearchByVAll()
        {
            var szServices = new DbHelperEfSql<vNodeButton>();
            return szServices.SearchByAll();
        }

        public IList<NodeButton> SearchByValid(int nodeId)
        {
            var szServices = new DbHelperEfSql<NodeButton>();
            return szServices.SearchListByCondition(c => c.NodeId == nodeId);
        }

        public IList<vNodeButton> SearchByValid(bool isSuper)
        {
            var szServices = new DbHelperEfSql<vNodeButton>();
            if (isSuper)
            {
                return szServices.SearchByAll();
            }
            else
            {
                return szServices.SearchListByCondition(c => c.IsSuper == 0);
            }
        }
        public IList<vNodeButton> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord)
        {
            var szServices = new DbHelperEfSql<vNodeButton>();
            return szServices.SearchByPageCondition(true, c => c.NodeId, iPageIndex, iPageSize, ref iTotalRecord);
        }

        public bool UpdateNodeButton(List<int> nodeIdList, List<int> buttonIdList)
        {
            var szServices = new SpecialService();
            return szServices.UpdateNodeButton(nodeIdList, buttonIdList);
        }
    }
}
