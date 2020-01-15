using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using CommonLib;

namespace DAL
{
    public class UsersDao : BaseServiceEf<MISDBContainer>
    {
        public bool AddOrUpdae(Users item)
        {
            try
            {
                var isAdd = true;

                #region
                var query = _context.Set<Users>().FirstOrDefault(c => c.UserID == item.UserID);
                if (query == null)
                {
                    isAdd = true;
                    var model = new Users();
                    model.UserID = item.UserID;
                    model.WelfareCentreID = item.WelfareCentreID;
                    model.Password = item.Password;
                    model.OperateDate = item.OperateDate;
                    model.UserCode = item.UserCode;
                    model.UserName = item.UserName;
                    model.RealName = item.RealName;
                    model.DeptID = item.DeptID;
                    model.RoleID = item.RoleID;
                    model.Remark = item.Remark;
                    model.UserType = item.UserType;
                    model.IcCardNo = item.IcCardNo;
                    model.CreateDate = item.CreateDate;
                    model.IsValid = item.IsValid;
                    _context.Set<Users>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.UserID = item.UserID;
                    query.WelfareCentreID = item.WelfareCentreID;
                    if (item.Password != null && item.Password.Length > 0)
                    {
                        query.Password = item.Password;
                    }

                    query.OperateDate = item.OperateDate;
                    query.UserCode = item.UserCode;
                    query.UserName = item.UserName;
                    query.RealName = item.RealName;
                    query.DeptID = item.DeptID;
                    query.RoleID = item.RoleID;
                    query.Remark = item.Remark;
                    query.UserType = item.UserType;
                    query.IcCardNo = item.IcCardNo;
                }
                #endregion

                #region tbUserWelfareCentre
                if (item.UserType == (int)EUserType.YuanZhang)
                {
                    var welfareCentreDao = new DbHelperEfSql<tbWelfareCentre>();
                    var welfareCentreItem = welfareCentreDao.SearchBySingle(c => c.ID == item.WelfareCentreID);

                    var query2 = _context.Set<tbUserWelfareCentre>().FirstOrDefault(c => c.WelfareCentreId == item.WelfareCentreID && c.UserName == item.UserName);
                    if (query2 == null)
                    {
                        var itemUserWelfareCentre = new tbUserWelfareCentre();
                        itemUserWelfareCentre.ID = Guid.NewGuid();
                        itemUserWelfareCentre.UserName = item.UserName;
                        if (item.Password != null && item.Password.Length > 0)
                        {
                            itemUserWelfareCentre.UserPassword = item.Password;
                        }
                        itemUserWelfareCentre.WebServiesUrl = Config.YuanZhan_URL;
                        itemUserWelfareCentre.IsUpdate = 1;
                        itemUserWelfareCentre.WelfareCentreId = item.WelfareCentreID;
                        itemUserWelfareCentre.WelfareCentreName = welfareCentreItem.WelfareCentreName;
                        itemUserWelfareCentre.IsValid = 1;
                        itemUserWelfareCentre.CreateDate = DateTime.Now;
                        itemUserWelfareCentre.Remark = "";
                        _context.Set<tbUserWelfareCentre>().Add(itemUserWelfareCentre);
                    }
                    else
                    {
                        query2.UserName = item.UserName;
                        if (item.Password != null && item.Password.Length > 0)
                        {
                            query2.UserPassword = item.Password;
                        }
                        query2.WebServiesUrl = Config.YuanZhan_URL;
                        query2.IsUpdate = 1;
                        query2.WelfareCentreId = item.WelfareCentreID;
                        query2.WelfareCentreName = welfareCentreItem.WelfareCentreName;
                        query2.IsValid = 1;
                        query2.CreateDate = DateTime.Now;
                        query2.Remark = "";
                    }
                }

                #endregion

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "AddOrUpdaeUsers",
                    MethodParameters = "添加失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }

        public bool PhysicalDelete(Guid Id, Guid welfareCentreId)
        {
            var query = _context.Set<Users>().FirstOrDefault(c => c.UserID == Id && c.WelfareCentreID == welfareCentreId);
            if (query != null)
            {
                _context.Set<Users>().Remove(query);
            }
            _context.SaveChanges();
            return true;
        }
        public bool OperateDataStatus(Guid Id, Guid welfareCentreId, int isValid)
        {
            var query = _context.Set<Users>().FirstOrDefault(c => c.UserID == Id && c.WelfareCentreID == welfareCentreId);
            if (query != null)
            {
                query.UserID = Id;
                query.OperateDate = DateTime.Now;
                query.IsValid = isValid;
            }
            _context.SaveChanges();
            return true;
        }
        public bool ChangePassword(IList<Users> userList, string password)
        {
            try
            {
                foreach (var item in userList)
                {
                    var isAdd = false;
                    var query = _context.Set<Users>().FirstOrDefault(c => c.UserID == item.UserID);
                    if (query != null)
                    {
                        query.Password = password;
                    }

                    #region tbUserWelfareCentre
                    if (item.UserType == (int)EUserType.YuanZhang)
                    {
                        var queryUserWelfareCentre = _context.Set<tbUserWelfareCentre>().FirstOrDefault(c => c.WelfareCentreId == item.WelfareCentreID && c.UserName == item.UserName);
                        if (queryUserWelfareCentre != null)
                        {
                            queryUserWelfareCentre.UserName = item.UserName;
                            queryUserWelfareCentre.UserPassword = password;
                            queryUserWelfareCentre.WebServiesUrl = Config.YuanZhan_URL;
                            queryUserWelfareCentre.IsUpdate = 1;
                            queryUserWelfareCentre.WelfareCentreId = item.WelfareCentreID;
                            queryUserWelfareCentre.IsValid = 1;
                            queryUserWelfareCentre.CreateDate = DateTime.Now;
                            queryUserWelfareCentre.Remark = "";
                        }
                    }
                    #endregion
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "ChangePassword",
                    MethodParameters = "操作失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }
    }
}
