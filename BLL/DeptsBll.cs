using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class DeptsBll
    {
        public bool AddOrUpdate(Depts item, bool isAdd)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return isAdd = true ? szServices.Add(item) : szServices.Update(item, c => c.DeptID == item.DeptID);
        }
        public bool AddOrUpdate(Depts item)
        {
            var szServices = new DbHelperEfSql<Depts>();
            if (item.DeptID == Guid.Empty)
            {
                item.DeptID = Guid.NewGuid();
                return szServices.Add(item);
            }
            else
                return szServices.Update(item, c => c.DeptID == item.DeptID);
        }
        /// <summary>
        /// 部门添加修改方法
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddOrUpdateDao(Depts item)
        {
            var dao = new DeptsDao();
            return dao.AddOrUpdae(item);
        }
        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.DeptID.ToString()));
        }
        public bool PhysicalDeleteByCondition(Guid id)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return szServices.PhysicalDeleteByCondition(c => c.DeptID == id);
        }


        public bool EnableByCodeition(string[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Valid);
        }

        public bool DisableByCodeition(string[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Forbidden);
        }
        public bool LogicDeleteByCondition(string[] ids)
        {
            return OperateDataStatus(ids, ESystemStatus.Deleted);
        }
        private bool OperateDataStatus(string[] ids, ESystemStatus eSystemStatus)
        {
            if (ids == null || ids.Length <= 0) return false;
            var buf = new System.Text.StringBuilder();
            foreach (var s in ids)
            {
                buf.AppendFormat("'{0}',", s);
            }
            string szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("UPDATE Depts SET IsValid={0},OperateDate=getdate() WHERE DeptID in ({1})", (int)eSystemStatus, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        public Depts GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return szServices.SearchBySingle(c => c.DeptID == id);
        }

        public IList<Depts> SearchListByValid(Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return szServices.SearchListByCondition(c => c.IsValid == 1 && c.WelfareCentreID == welfareCentreId);
        }

        public IList<Depts> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public IList<Depts> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord)
        {
            var szServices = new DbHelperEfSql<Depts>();
            return szServices.SearchByPageCondition(false, c => c.OperateDate, iPageIndex, iPageSize, ref iTotalRecord);
        }
        /// <summary>
        /// 修改数据状态--逻辑删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="welfareCentreId"></param>
        /// <returns></returns>
        public bool PhysicalDelete(Guid Id, Guid welfareCentreId)
        {
            var dao = new DeptsDao();
            return dao.PhysicalDelete(Id, welfareCentreId);
        }
        /// <summary>
        /// 修改数据状态--物理删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="welfareCentreId"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public bool OperateDataStatus(Guid Id, Guid welfareCentreId, int isValid)
        {
            var dao = new DeptsDao();
            return dao.OperateDataStatus(Id, welfareCentreId, isValid);
        }
    }
}
