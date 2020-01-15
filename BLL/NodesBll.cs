using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class NodesBll
    {
        public Nodes GetObjectByName(string name)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchBySingle(c => c.NodeName == name);
        }
        public IList<Nodes> SearchByAll()
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchByAll();
        }
        public IList<Nodes> SearchListByValid()
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchListByCondition(c => c.IsValid == 1);
        }
        public IList<Nodes> SearchListByValid(bool isSuper)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            if (isSuper)
            {
                return szServices.SearchListByCondition(c => c.IsValid == 1);
            }
            else
            {
                return szServices.SearchListByCondition(c => c.IsValid == 1 && c.IsSuper == 0);

            }
        }

        public IList<Nodes> GetListByLeftNode()
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchListByCondition(c => c.IsLeftNode == 1);
        }
        public bool AddOrUpdate(Nodes item)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return item.NodeId <= 0 ? szServices.Add(item) : szServices.Update(item, c => c.NodeId == item.NodeId);
        }

        public bool AddOrUpdate(Nodes item, bool isAdd)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return isAdd ? szServices.Add(item) : szServices.Update(item, c => c.NodeId == item.NodeId);
        }

        public IList<Nodes> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public Nodes GetObjectById(int id)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchBySingle(c => c.NodeId == id);
        }
        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.NodeId.ToString()));
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
            string sql = string.Format("UPDATE Nodes SET IsValid={0},OperateDate=getdate() WHERE NodeId in ({1})", (int)eSystemStatus, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        public IList<Nodes> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchByPageCondition(true, c => c.NodeId, iPageIndex, iPageSize, ref iTotalRecord);
        }


        public IList<Nodes> GetListByLeftNode(int pId)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchListByCondition(c => c.IsLeftNode == 1 && c.ParentID == pId);
        }
        public IList<Nodes> SearchLeftListByValid(bool isSuper, int nodeType)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            if (isSuper)
            {
                return szServices.SearchListByCondition(c => c.IsValid == 1 && c.IsLeftNode == 1 && c.NodeBizType == nodeType);
            }
            else
            {
                return szServices.SearchListByCondition(c => c.IsValid == 1 && c.IsLeftNode == 1 && c.IsSuper == 0 && c.NodeBizType == nodeType);

            }
        }

        public IList<Nodes> SearchListByType(int pId, int nodeType)
        {
            var szServices = new DbHelperEfSql<Nodes>();
            return szServices.SearchListByCondition(c => c.IsLeftNode == 1 && c.ParentID == pId && c.NodeBizType == nodeType);
        }


    }
}
