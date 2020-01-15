using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class AuthNodeBll
    {

        public IList<AuthNode> SearchByAll()
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.SearchByAll();
        }

        public Boolean AddOrUpdate(AuthNode item)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            if (item.AuthNodeID == Guid.Empty)
            {
                item.AuthNodeID = Guid.NewGuid();
                return szServices.Add(item);
            }
            else
                return szServices.Update(item, c => c.AuthNodeID == item.AuthNodeID);
        }

        public IList<AuthNode> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public AuthNode GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.SearchBySingle(c => c.AuthNodeID == id);
        }

        public bool IsAuthNodeByCondition(Guid iUserId, int iNodeId)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.SearchBySingle(c => c.UserID == iUserId && c.NodeId == iNodeId) != null;
        }

        public IList<AuthNode> GetListByUserId(Guid iUserId)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.SearchListByCondition(c => c.UserID == iUserId);
        }

        public bool AddAll(IList<AuthNode> list)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.AddAll(list);
        }

        public bool UpdateAuthNode(List<string> ids, Guid iUserId)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            IList<AuthNode> list = ids.Select(cc => new AuthNode
            {
                AuthNodeID = Guid.NewGuid(),
                UserID = iUserId,
                NodeId = Convert.ToInt32(cc),
                OperateDate = DateTime.Now
            }).ToList();
            return szServices.TransDeleteCAddL(list, c => c.UserID == iUserId);
        }

        public bool UpdateAuthNode(IList<AuthNode> list, Guid iUserId)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.TransDeleteCAddL(list, c => c.UserID == iUserId);
        }

        public bool PhysicalDeleteByUserIDs(string[] szUserIds)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.PhysicalDeleteByCondition(c => szUserIds.Contains(c.UserID.ToString()));
        }
        public bool PhysicalDeleteByUserIDs(Guid szUserId)
        {
            var szServices = new DbHelperEfSql<AuthNode>();
            return szServices.PhysicalDeleteByCondition(c => c.UserID == szUserId);
        }
    }
}
