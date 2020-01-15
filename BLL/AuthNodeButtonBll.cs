using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class AuthNodeButtonBll
    {
        public IList<AuthNodeButton> SearchByAll()
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.SearchByAll();
        }

        public bool AddOrUpdate(AuthNodeButton item)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            if (item.AuthNodeButtonID == Guid.Empty)
            {
                item.AuthNodeButtonID = Guid.NewGuid();
                return szServices.Add(item);
            }
            else
                return szServices.Update(item, c => c.AuthNodeButtonID == item.AuthNodeButtonID);
        }

        public IList<AuthNodeButton> SearchByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public AuthNodeButton GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.SearchBySingle(c => c.AuthNodeButtonID == id);
        }


        public IList<AuthNodeButton> GetListByUserId(Guid iUserId)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.SearchListByCondition(c => c.UserID == iUserId);
        }

        public bool UpdateAuthNodeButton(List<string> ids, Guid iUserId)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            IList<AuthNodeButton> list = ids.Select(cc => new AuthNodeButton
            {
                AuthNodeButtonID = Guid.NewGuid(),
                UserID = iUserId,
                NodeButtonId = Convert.ToInt32(cc) - 100000,
                OperateDate = DateTime.Now
            }).ToList();

            return szServices.TransDeleteCAddL(list, c => c.UserID == iUserId);
        }

        public bool AddAll(IList<AuthNodeButton> list)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.AddAll(list);
        }

        public bool UpdateAuthNodeButton(IList<AuthNodeButton> list, Guid iUserId)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.TransDeleteCAddL(list, c => c.UserID == iUserId);
        }
        public bool PhysicalDeleteByUserIDs(string[] ids)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.UserID.ToString()));
        }
        public bool PhysicalDeleteByUserIDs(Guid id)
        {
            var szServices = new DbHelperEfSql<AuthNodeButton>();
            return szServices.PhysicalDeleteByCondition(c => c.UserID == id);
        }
    }
}
