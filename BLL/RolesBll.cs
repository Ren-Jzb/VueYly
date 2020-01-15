using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class RolesBll
    {
        public bool AddOrUpdate(Roles item, bool isAdd)
        {
            var szServices = new DbHelperEfSql<Roles>();
            return isAdd = true ? szServices.Add(item) : szServices.Update(item, c => c.RoleID == item.RoleID);
        }

        public Roles GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<Roles>();
            return szServices.SearchBySingle(c => c.RoleID == id);
        }

        public IList<Roles> SearchListByValid(Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<Roles>();
            return szServices.SearchListByCondition(c => c.IsValid == 1 && c.WelfareCentreID == welfareCentreId);
        }

        public IList<Roles> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord)
        {
            var szServices = new DbHelperEfSql<Roles>();
            return szServices.SearchByPageCondition(false, c => c.OperateDate, iPageIndex, iPageSize, ref iTotalRecord);
        }

        public IList<Roles> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<Roles>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public Boolean AddOrUpdate(Roles item)
        {
            var szServices = new DbHelperEfSql<Roles>();
            if (item.RoleID == Guid.Empty)
            {
                item.RoleID = Guid.NewGuid();
                return szServices.Add(item);
            }
            else
                return szServices.Update(item, c => c.RoleID == item.RoleID);
        }
        /// <summary>
        /// 角色表 添加修改方法
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddOrUpdateDao(Roles item)
        {
            var dao = new RolesDao();
            return dao.AddOrUpdae(item);
        }
        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<Roles>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.RoleID.ToString()));
        }
        public bool PhysicalDeleteByCondition(Guid id)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
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
            if (ids == null || ids.Length <= 0) return false;
            var buf = new System.Text.StringBuilder();
            foreach (var s in ids)
            {
                buf.AppendFormat("'{0}',", s);
            }
            string szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("UPDATE Roles SET IsValid={0},OperateDate=getdate() WHERE RoleID in ({1})", (int)eSystemStatus, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        /// <summary>
        /// 修改数据状态--逻辑删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="welfareCentreId"></param>
        /// <returns></returns>
        public bool PhysicalDelete(Guid Id, Guid welfareCentreId)
        {
            var dao = new RolesDao();
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
            var dao = new RolesDao();
            return dao.OperateDataStatus(Id, welfareCentreId, isValid);
        }
    }
}
