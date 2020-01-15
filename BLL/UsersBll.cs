using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using DAL;
using Model;

namespace BLL
{
    public class UsersBll
    { 
        public bool ValidationUserName(string szUserName)
        {

            var szServices = new DbHelperEfSql<Users>();
            return szServices.Count(c => c.UserName == szUserName) <= 0;
        }
        public bool ValidationUserName(Guid id, string szUserName)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.Count(c => c.UserName == szUserName && c.UserID != id) <= 0;
        }

        public Users LoginUsers(Guid id, string password)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.SearchBySingle(c => (c.UserID == id && c.Password == password && c.IsValid == 1));
        }
        public Users LoginUsers(string szUserName, string password)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.SearchBySingle(c => (c.UserName == szUserName && c.Password == password && c.IsValid == 1));
        }

        public bool UpdatePassword(string logName, string passWord)
        {
            var szPassword = CommonLib.HashEncrypt.BgPassWord(HashEncrypt.md5(passWord));
            string sql = string.Format("UPDATE Users SET Password='{0}',OperateDate=getdate() where UserName in ({1})", szPassword, logName);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }
        public bool ResetPassword(string[] ids)
        {
            if (ids == null || ids.Length <= 0) return false;
            var szPassword = CommonLib.HashEncrypt.BgPassWord(HashEncrypt.md5(Config.SystemInitPassword));
            var buf = new System.Text.StringBuilder();
            foreach (var s in ids)
            {
                buf.AppendFormat("'{0}',", s);
            }
            string szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("UPDATE Users SET Password='{0}',OperateDate=getdate() where UserID in ({1})", szPassword, szIds);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        public Users GetObjectById(Guid id)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.SearchBySingle(c => c.UserID == id);
        } 

        public bool AddOrUpdate(Users item, bool isAdd)
        {
            var szServices = new DbHelperEfSql<Users>();
            if (isAdd)
            {
                var isFlag = szServices.Add(item);
                return isFlag;
            }
            else
            {
                var isFlag = szServices.Update(item, c => c.UserID == item.UserID);
                return isFlag;
            }
        }
        public bool AddOrUpdate(Users item)
        {
            var dao = new UsersDao();
            return dao.AddOrUpdae(item);
        }

        public bool OperateDataStatus(Guid Id, Guid welfareCentreId, int isValid)
        {
            var dao = new UsersDao();
            return dao.OperateDataStatus(Id, welfareCentreId, isValid);
        }

        public bool ChangePassword(Guid id, string iNewPassword)
        {
            string sql = string.Format("UPDATE Users SET Password='{0}',OperateDate=getdate() where UserID ='{1}'", iNewPassword, id);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        //姓名下拉
        public IList<tbOrgTLJGCongYe> SearchListByAuto(string keyword, int limit)
        {
            var szServices = new DbHelperEfSql<tbOrgTLJGCongYe>();
            var iTotalRecord = 0;
            if (!string.IsNullOrEmpty(keyword))
            {
                return szServices.SearchByPageCondition(c => c.StaffName.Contains(keyword) && c.IsValid == 1, true, c => c.StaffName, 0, limit, ref iTotalRecord);
            }
            else
            {
                return szServices.SearchByPageCondition(c => c.IsValid == 1, true, c => c.StaffName, 0, limit, ref iTotalRecord);
            }
        }
        public IList<vUsers> SearchVByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<vUsers>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public bool PhysicalDelete(Guid Id, Guid welfareCentreId)
        {
            var dao = new UsersDao();
            return dao.PhysicalDelete(Id, welfareCentreId);
        }

        public bool ValidationUserName(string szUserName, Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.Count(c => c.UserName == szUserName && c.WelfareCentreID == welfareCentreId && c.IsValid == 1) <= 0;
        }

        public bool ValidationUserName(Guid id, string szUserName, Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.Count(c => c.UserName == szUserName && c.UserID != id && c.WelfareCentreID == welfareCentreId && c.IsValid == 1) <= 0;
        }

        public Users LoginHgUsers(string szUserName, string password)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.SearchBySingle(c => (c.UserType == (int)EUserType.NursesAide || c.UserType == (int)EUserType.NursesAideBzz) && (c.UserName == szUserName && c.Password == password && c.IsValid == 1));
        }

        public bool ChangePassword(Guid welfareCentreId, Guid id, string iNewPassword)
        {
            string sql = string.Format("UPDATE Users SET Password='{0}',OperateDate=getdate() where UserID ='{1}'", iNewPassword, id);
            var isFlag = DbHelperSql.ExecuteNonQuery(sql) > 0;
            if (isFlag == true)
            {
                var isFw = false;
                if (isFw == false)
                {
                    var userService = new DbHelperEfSql<Users>();
                    var userItem = userService.SearchBySingle(c => c.UserID == id);
                }

            }
            return isFlag;
        }

        public IList<Users> SearchListByValid(Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<Users>();
            return szServices.SearchListByCondition(c => c.IsValid == 1 && c.WelfareCentreID == welfareCentreId);
        }


    }
}
