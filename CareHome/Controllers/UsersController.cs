using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Model;
using BLL;
using CommonLib;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace CareHome.Controllers
{
    public class UsersController : Controller
    {
        private string ParamID = "";   //数据ID
        private string ParamName = new LogsBll().ReturnNodeName((int)CommonLib.NodePagesSetting.EUsers.ListPage);   //模块名称
        private string ParamState = "1";   //1：添加，2：修改，3：逻辑，4：物理，5：启用，6：禁用
        private string GetRealName()
        {
            var buf = new StringBuilder();
            var cBll = new OrgTLJGCongYeBll();
            var list = cBll.SearchListByValid(Utits.WelfareCentreID);
            buf.AppendFormat("<option value=\"{0}\">{1}</option>", "请选择", "==请选择==");
            if (list != null)
            {
                foreach (var item in list)
                {
                    buf.AppendFormat("<option value=\"{0}\">{1}</option>", item.StaffName, item.StaffName);
                }
            }
            return buf.ToString();
        }
        private string GetRoleID()
        {
            var buf = new StringBuilder();
            var cBll = new RolesBll();
            var list = cBll.SearchListByValid(Utits.WelfareCentreID);
            if (list != null)
            {
                foreach (var item in list)
                {
                    buf.AppendFormat("<option value=\"{0}\">{1}</option>", item.RoleID, item.RoleName);
                }
            }
            return buf.ToString();
        }
        private string GetDeptID()
        {
            var buf = new StringBuilder();
            var cBll = new DeptsBll();
            var list = cBll.SearchListByValid(Utits.WelfareCentreID);
            if (list != null)
            {
                foreach (var item in list)
                {
                    buf.AppendFormat("<option value=\"{0}\">{1}</option>", item.DeptID, item.DeptName);
                }
            }
            return buf.ToString();
        }
        private int ListPageNodeId { get { return (int)CommonLib.NodePagesSetting.EUsers.ListPage; } }
        private int ListPageFNodeId { get { return (int)CommonLib.NodePagesSetting.EUsers.ListPageF; } }
        private int AddPageNodeId { get { return (int)CommonLib.NodePagesSetting.EUsers.AddPage; } }
        private int EditPageNodeId { get { return (int)CommonLib.NodePagesSetting.EUsers.EditPage; } }
        private int DetailPageNodeId { get { return (int)CommonLib.NodePagesSetting.EUsers.DetailPage; } }
        [OutputCache(Duration = 0)]
        public ActionResult List()
        {
            #region 页面权限判断
            if (!Utits.IsLogin)
            {
                return RedirectPermanent("../Login/Index");
            }
            #endregion
            //ViewBag.IndexName = "用户管理a";
            //ViewData["IndexName"] = "用户管理b";
            int[] NodePages = { ListPageNodeId, ListPageFNodeId };
            int NodeId = CommonLib.RequestParameters.Pint("NodeId");
            if (!NodePages.Contains(NodeId))
            {
                //return RedirectToAction("Index", "Error");//跳转
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "参数(NodeId)错误，请联系系统管理员！", EErrorRank.Error));
            }
            if (Utits.IsNodePageAuth(NodeId).ErrorType != 1)
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "您没有该页面的访问权限！", EErrorRank.Error));
            }

            ViewBag.AddPageNodeId = AddPageNodeId;
            ViewBag.EditPageNodeId = EditPageNodeId;
            ViewBag.DetailPageNodeId = DetailPageNodeId;
            ViewBag.GetRoleID = GetRoleID();
            ViewBag.GetDeptID = GetDeptID();
            ViewBag.GetRealName = GetRealName();

            ViewBag.OperateButton = Utits.AuthOperateButton();
            ViewBag.DetailsOperateButton = Utits.AuthOperateButton(AddPageNodeId);

            ViewBag.WelfareCentreID = Utits.WelfareCentreID;
            return View();
        }

        [OutputCache(Duration = 0)]
        public ActionResult Details()
        {
            #region 页面权限判断
            if (!Utits.IsLogin)
            {
                return RedirectPermanent("../Login/Index");
            }
            #endregion
            int[] NodePages = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int NodeId = CommonLib.RequestParameters.Pint("NodeId");
            if (!NodePages.Contains(NodeId))
            {
                //return RedirectToAction("Index", "Error");//跳转
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "参数(NodeId)错误，请联系系统管理员！", EErrorRank.Error));
            }
            if (Utits.IsNodePageAuth(NodeId).ErrorType != 1)
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "您没有该页面的访问权限！", EErrorRank.Error));
            }
            ViewBag.ListPageNodeId = ListPageNodeId;
            ViewBag.GetRoleID = GetRoleID();
            ViewBag.GetDeptID = GetDeptID();
            ViewBag.GetRealName = GetRealName();
            ViewBag.OperateButton = Utits.AuthOperateButton();
            return View();
        }

        #region JsonResult 
        //姓名下拉
        public JsonResult AutoUserName()
        {
            string keyword = RequestParameters.Pstring("q");
            int limit = RequestParameters.Pint("limit");
            if (limit <= 0)
                limit = 10;
            if (limit >= 10)
                limit = 10;
            if (limit > 100)
                limit = 10;

            var cBll = new UsersBll();
            var searchList = cBll.SearchListByAuto(keyword, limit);


            if (searchList == null)
                searchList = new List<tbOrgTLJGCongYe>();
            return Json(searchList);
        }

        public JsonResult SearchList()
        {
            #region 权限控制
            int[] iRangePage = { ListPageNodeId, ListPageFNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            var tempAuth = Utits.IsNodePageAuth(iRangePage, iCurrentPageNodeId);
            if (tempAuth.ErrorType != 1)
                return Json(tempAuth);
            #endregion

            //当前页                   
            int iCurrentPage = RequestParameters.Pint("currentPage");
            iCurrentPage = iCurrentPage <= 0 ? 1 : iCurrentPage;
            //索引页
            int iPageIndex = iCurrentPage - 1;
            //一页的数量
            int iPageSize = RequestParameters.Pint("pageSize");
            iPageSize = iPageSize <= 0 ? 5 : iPageSize;
            iPageSize = iPageSize > 100 ? 100 : iPageSize;
            //总记录数量
            int iTotalRecord = 0;

            #region 查询条件
            var searchCondition = new ConditionModel();

            var WhereList = new List<WhereCondition>();
            var UserName = RequestParameters.Pstring("UserName");
            if (UserName.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "UserName";
                whereCondition.FieldValue = UserName;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }

            var RealName = RequestParameters.Pstring("RealName");
            if (RealName.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "RealName";
                whereCondition.FieldValue = RealName;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }
            var RoleID = RequestParameters.PGuidNull("RoleID");
            if (RoleID != null)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "RoleID";
                whereCondition.FieldValue = RoleID.Value;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }

            var DeptID = RequestParameters.PGuidNull("DeptID");
            if (DeptID != null)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "DeptID";
                whereCondition.FieldValue = DeptID;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            var UserType = RequestParameters.PintNull("UserType");
            if (UserType != null)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "UserType";
                whereCondition.FieldValue = UserType;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            var super = Config.UserNameSuper;
            if (super.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "UserName";
                whereCondition.FieldValue = super;
                whereCondition.FieldOperator = EnumOper.ExclamationEqual;
                WhereList.Add(whereCondition);
            }
            var welfareCentreId = Utits.WelfareCentreID;
            if (welfareCentreId != null)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "WelfareCentreID";
                whereCondition.FieldValue = welfareCentreId;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            var IsValid = RequestParameters.PintNull("IsValid");
            if (IsValid != null)
            {
                if (IsValid == 1)
                {
                    var whereCondition = new WhereCondition();
                    whereCondition.FieldName = "IsValid";
                    whereCondition.FieldValue = IsValid;
                    whereCondition.FieldOperator = EnumOper.Equal;
                    WhereList.Add(whereCondition);
                }
                else
                {
                    var whereCondition = new WhereCondition();
                    whereCondition.FieldName = "IsValid";
                    whereCondition.FieldValue = 1;
                    whereCondition.FieldOperator = EnumOper.ExclamationEqual;
                    WhereList.Add(whereCondition);
                }
            }
            searchCondition.WhereList = WhereList;

            var OrderList = new List<OrderCondition>();
            var sortfield = RequestParameters.Pstring("sortfield");
            if (sortfield.Length <= 0)
            {
                sortfield = "OperateDate";
            }
            var orderCondition = new OrderCondition();
            orderCondition.FiledOrder = sortfield;
            orderCondition.Ascending = RequestParameters.Pstring("sorttype") == "asc" ? true : false;
            OrderList.Add(orderCondition);

            searchCondition.OrderList = OrderList;
            #endregion

            var cBll = new UsersBll();
            var list = cBll.SearchVByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, searchCondition);
            iPageSize = iPageSize == 0 ? iTotalRecord : iPageSize;
            int pageCount = iTotalRecord % iPageSize == 0 ? iTotalRecord / iPageSize : iTotalRecord / iPageSize + 1;
            var sReturnModel = new ResultList();
            sReturnModel.ErrorType = 1;
            sReturnModel.CurrentPage = iCurrentPage;
            sReturnModel.PageSize = iPageSize;
            sReturnModel.TotalRecord = iTotalRecord;
            sReturnModel.PageCount = pageCount;
            sReturnModel.Data = list;
            return Json(sReturnModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListPhyDel()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            int iCurrentButtonId = (int)EButtonType.PhyDelete;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            Guid ID = RequestParameters.PGuid("ids");
            if (ID == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }

            var cBll = new UsersBll();
            bool isFlag = cBll.PhysicalDelete(ID, Utits.WelfareCentreID);
            if (isFlag)
            {
                var authNodeBll = new AuthNodeBll();
                authNodeBll.PhysicalDeleteByUserIDs(ID);
                var authNodeButtonBll = new AuthNodeButtonBll();
                authNodeButtonBll.PhysicalDeleteByUserIDs(ID);

                ParamState = "4";
                ParamID = ID.ToString();
                var cLog = new LogsBll();
                cLog.Log(ParamID, ParamName, ParamState, Utits.CurrentUserID.ToString(), Utits.CurrentRealName.ToString(), Utits.WelfareCentreID.ToString(), Utits.ClientIPAddress.ToString());

                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 1;
                sRetrunModel.MessageContent = "操作成功.";
                return Json(sRetrunModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "操作失败.";
                return Json(sRetrunModel);
            }
        }

        public JsonResult ListOperateStatus()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            int[] iRangeButton = { (int)EButtonType.Delete, (int)EButtonType.Enable, (int)EButtonType.Disable };
            int iCurrentButtonId = RequestParameters.Pint("oButtonId");
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iRangeButton, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            //string _ids = RequestParameters.Pstring("ids");
            //if (string.IsNullOrEmpty(_ids))
            //{
            //    var sRetrunModel = new ResultMessage();
            //    sRetrunModel.ErrorType = 0;
            //    sRetrunModel.MessageContent = "参数错误.";
            //    return Json(sRetrunModel);
            //}
            //string[] strids = _ids.Split(',');
            //System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
            //for (int i = 0; i < strids.Length; i++)
            //{
            //    if (RegexValidate.IsGuid(strids[i]))
            //    {
            //        arrayList.Add(strids[i]);
            //    }
            //}
            //string[] ids = (string[])arrayList.ToArray(typeof(string));
            //if (!ids.Any())
            //{
            //    var sRetrunModel = new ResultMessage();
            //    sRetrunModel.ErrorType = 0;
            //    sRetrunModel.MessageContent = "参数错误.";
            //    return Json(sRetrunModel);
            //}
            Guid ID = RequestParameters.PGuid("ids");
            if (ID == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            var cBll = new UsersBll();
            bool isFlag = false;
            switch (iCurrentButtonId)
            {
                case (int)EButtonType.Delete://删除
                    ParamState = "3";
                    isFlag = cBll.OperateDataStatus(ID, Utits.WelfareCentreID, (int)ESystemStatus.Deleted);
                    break;
                case (int)EButtonType.Enable://启用
                    ParamState = "5";
                    isFlag = cBll.OperateDataStatus(ID, Utits.WelfareCentreID, (int)ESystemStatus.Valid);
                    break;
                case (int)EButtonType.Disable://禁用
                    ParamState = "6";
                    isFlag = cBll.OperateDataStatus(ID, Utits.WelfareCentreID, (int)ESystemStatus.Forbidden);
                    break;
            }
            if (isFlag)
            {
                ParamID = ID.ToString();
                var cLog = new LogsBll();
                cLog.Log(ParamID, ParamName, ParamState, Utits.CurrentUserID.ToString(), Utits.CurrentRealName.ToString(), Utits.WelfareCentreID.ToString(), Utits.ClientIPAddress.ToString());

                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 1;
                sRetrunModel.MessageContent = "操作成功.";
                return Json(sRetrunModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "操作失败.";
                return Json(sRetrunModel);
            }
        }

        public JsonResult AddOrUpdate()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            bool isAdd = iCurrentPageNodeId == AddPageNodeId ? true : false;
            int iCurrentButtonId = (int)EButtonType.Save;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            #region AddOrUpdate
            var welfareCentreId = Utits.WelfareCentreID;
            string UserCode = RequestParameters.Pstring("UserCode");
            string UserName = RequestParameters.Pstring("UserName");
            string RealName = RequestParameters.Pstring("RealName");
            string Password = RequestParameters.Pstring("Password");
            Guid DeptId = RequestParameters.PGuid("DeptId");
            Guid RoleId = RequestParameters.PGuid("RoleId");
            int UserType = RequestParameters.Pint("UserType");
            string Remark = RequestParameters.Pstring("Remark");
            string IcCardNO = RequestParameters.Pstring("IcCardNO");
            if (UserName.Length <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "用户名不能为空.";
                return Json(sRetrunModel);
            }
            var cBll = new UsersBll();
            Guid ID = RequestParameters.PGuid("ID");
            bool isFlagValidation = false;
            if (ID == Guid.Empty)
                isFlagValidation = cBll.ValidationUserName(UserName, welfareCentreId);
            else
                isFlagValidation = cBll.ValidationUserName(ID, UserName, welfareCentreId);


            if (!isFlagValidation)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "用户名已存在.";
                return Json(sRetrunModel);
            }

            if (RealName.Length <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "真实姓名不能为空.";
                return Json(sRetrunModel);
            }
            if (DeptId == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "部门参数错误，请返回列表页面重新操作.";
                return Json(sRetrunModel);
            }
            if (RoleId == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "角色参数错误，请返回列表页面重新操作.";
                return Json(sRetrunModel);
            }

            var item = new Users();
            if (ID == Guid.Empty)
            {
                item.UserID = Guid.NewGuid();
                item.CreateDate = DateTime.Now;
                item.IsValid = 1;
                if (Password.Length <= 0)
                    Password = HashEncrypt.md5(CommonLib.Config.SystemInitPassword);
                item.Password = CommonLib.HashEncrypt.BgPassWord(Password);
            }
            else
            {
                item.UserID = ID;
                if (Password.Length > 0)
                    item.Password = CommonLib.HashEncrypt.BgPassWord(Password);
            }
            item.WelfareCentreID = welfareCentreId;
            item.OperateDate = DateTime.Now;
            item.UserCode = UserCode;
            item.UserName = UserName;
            item.RealName = RealName;
            item.DeptID = DeptId;
            item.RoleID = RoleId;
            item.Remark = Remark;
            item.UserType = UserType;
            item.IcCardNo = IcCardNO;

            ParamState = "1";
            ParamID = item.UserID.ToString();
            bool IsFlag = cBll.AddOrUpdate(item);
            if (IsFlag)
            {
                var cLog = new LogsBll();
                cLog.Log(ParamID, ParamName, ParamState, Utits.CurrentUserID.ToString(), Utits.CurrentRealName.ToString(), Utits.WelfareCentreID.ToString(), Utits.ClientIPAddress.ToString());

                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 1;
                sRetrunModel.MessageContent = "操作成功.";
                return Json(sRetrunModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "操作失败.";
                return Json(sRetrunModel);
            }
            #endregion
        }

        public JsonResult InitSingle()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            var tempAuth = Utits.IsNodePageAuth(iRangePage, iCurrentPageNodeId);
            if (tempAuth.ErrorType != 1)
                return Json(tempAuth);
            #endregion

            #region InitSingle
            Guid ID = RequestParameters.PGuid("ID");
            if (ID != Guid.Empty)
            {
                var usersBll = new UsersBll();
                var item = usersBll.GetObjectById(ID);
                var szDetailModel = new ResultDetail();
                szDetailModel.Entity = item;
                szDetailModel.ErrorType = 1;
                return Json(szDetailModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            #endregion
        }

        public JsonResult BlurName()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            var tempAuth = Utits.IsNodePageAuth(iRangePage, iCurrentPageNodeId);
            if (tempAuth.ErrorType != 1)
                return Json(tempAuth);
            #endregion
            string UserName = RequestParameters.Pstring("UserName");
            if (UserName.Length <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "用户名不能为空.";
                return Json(sRetrunModel);
            }
            var cBll = new UsersBll();
            bool isFlagValidation = false;
            switch (iCurrentPageNodeId)
            {
                case (int)NodePagesSetting.EUsers.AddPage:
                    isFlagValidation = cBll.ValidationUserName(UserName);
                    break;
                case (int)NodePagesSetting.EUsers.EditPage:
                    Guid ID = RequestParameters.PGuid("ID");
                    isFlagValidation = cBll.ValidationUserName(ID, UserName);
                    break;
            }

            if (isFlagValidation)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 1;
                sRetrunModel.MessageContent = "用户名填写正确.";
                return Json(sRetrunModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "用户名已存在.";
                return Json(sRetrunModel);
            }
        }

        public JsonResult ResetPassword()
        {
            #region 权限控制
            //显示申明哪几个节点具有该功能->解决已拥有部分权限，而想跳过权限判断问题。
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            int iCurrentButtonId = (int)EButtonType.ResetPassword;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            string szIds = RequestParameters.Pstring("ids");
            if (string.IsNullOrEmpty(szIds))
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            string[] strids = szIds.Split(',');
            System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
            foreach (string t in strids)
            {
                if (RegexValidate.IsGuid(t))
                {
                    arrayList.Add(t);
                }
            }
            string[] ids = (string[])arrayList.ToArray(typeof(string));
            if (!ids.Any())
            {
                var sRetrunModel = new ResultMessage
                {
                    ErrorType = 0,
                    MessageContent = "参数错误."
                };
                return Json(sRetrunModel);
            }

            var cBll = new UsersBll();
            bool isFlag = cBll.ResetPassword(ids);
            if (isFlag)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 1;
                sRetrunModel.MessageContent = string.Format("操作成功，重置后的密码为：{0}.", CommonLib.Config.SystemInitPassword);
                return Json(sRetrunModel);
            }
            else
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "操作失败.";
                return Json(sRetrunModel);
            }
        }

        //导出报表
        public JsonResult ExportReport()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            int iCurrentButtonId = (int)EButtonType.Export;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion
            #region 查询条件
            var searchCondition = new ConditionModel();

            var WhereList = new List<WhereCondition>();
            string UserName = RequestParameters.Pstring("UserName");
            if (UserName.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "UserName";
                whereCondition.FieldValue = UserName;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }

            string RealName = RequestParameters.Pstring("RealName");
            if (RealName.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "RealName";
                whereCondition.FieldValue = RealName;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }
            Guid? RoleID = RequestParameters.PGuidNull("RoleID");
            if (RoleID != null)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "RoleID";
                whereCondition.FieldValue = RoleID.Value;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }

            Guid? DeptID = RequestParameters.PGuidNull("DeptID");
            if (DeptID != null)
            {
                //List<Guid?> ids_guid=new List<Guid?>(){DeptID,Guid.NewGuid()};
                //string[] ids_guid = {DeptID.ToString(),Guid.NewGuid().ToString()};
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "DeptID";
                whereCondition.FieldValue = DeptID;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }

            int? IsValid = RequestParameters.PintNull("IsValid");
            if (IsValid != null)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "IsValid";
                whereCondition.FieldValue = IsValid;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            searchCondition.WhereList = WhereList;

            var OrderList = new List<OrderCondition>();
            string sortfield = RequestParameters.Pstring("sortfield");
            if (sortfield.Length <= 0)
            {
                sortfield = "OperateDate";
            }
            var orderCondition = new OrderCondition();
            orderCondition.FiledOrder = sortfield;
            orderCondition.Ascending = RequestParameters.Pstring("sorttype") == "asc" ? true : false;
            OrderList.Add(orderCondition);

            searchCondition.OrderList = OrderList;
            #endregion
            var cBll = new UsersBll();
            int iTotalRecord = 0;
            var searchList = cBll.SearchVByPageCondition(0, 65536, ref iTotalRecord, searchCondition);

            if (searchList == null || searchList.Count == 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "操作失败：无导出数据.";
                return Json(sRetrunModel);
            }
            try
            {
                string fileName = "用户信息导出" + DateTime.Now.ToString("yyyyMMMMddHHmmss") + ".xls";
                string fileUrl = Server.MapPath("~/Upload") + "/temp/";
                if (!Directory.Exists(fileUrl))
                    Directory.CreateDirectory(fileUrl);
                var filePath = fileUrl + fileName;

                const int rowHeight = 20;
                const int colCount = 9; //导出数据的总列数
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("Sheet1");
                #region 对齐格式
                //左对齐格式
                ICellStyle styleLeft = workbook.CreateCellStyle();
                styleLeft.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                styleLeft.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                styleLeft.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                styleLeft.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                styleLeft.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                styleLeft.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //右对齐格式
                ICellStyle styleRight = workbook.CreateCellStyle();
                styleRight.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                styleRight.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                styleRight.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                styleRight.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                styleRight.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                styleRight.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //居中对齐格式
                ICellStyle styleCenter = workbook.CreateCellStyle();
                styleCenter.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                styleCenter.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                styleCenter.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                styleCenter.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                styleCenter.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                styleCenter.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                #endregion
                #region 标题和表头
                //标题
                ICellStyle styleTitle = workbook.CreateCellStyle();
                styleTitle.Alignment = HorizontalAlignment.Center;
                styleTitle.VerticalAlignment = VerticalAlignment.Center;
                styleTitle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                styleTitle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                styleTitle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                styleTitle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                var fontTitle = workbook.CreateFont();
                fontTitle.FontHeightInPoints = 16;
                fontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                fontTitle.FontName = "微软雅黑";
                styleTitle.SetFont(fontTitle);
                //表头
                ICellStyle styleTh = workbook.CreateCellStyle();
                styleTh.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                styleTh.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                styleTh.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                styleTh.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                styleTh.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                styleTh.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                var fontTh = workbook.CreateFont();
                fontTh.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                styleTh.SetFont(fontTh);
                #endregion

                IRow row1 = sheet1.CreateRow(0);
                row1.HeightInPoints = rowHeight;
                //CellRangeAddress（int， int， int， int）
                //参数：起始行号，终止行号， 起始列号，终止列号
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, colCount - 1));
                row1.CreateCell(0).SetCellValue("用户信息");
                row1.GetCell(0).CellStyle = styleCenter;

                IRow row2 = sheet1.CreateRow(1);
                row2.HeightInPoints = rowHeight;

                #region 创建表头
                row2.CreateCell(0).SetCellValue("序号");
                row2.CreateCell(1).SetCellValue("用户编号");
                row2.CreateCell(2).SetCellValue("用户名称");
                row2.CreateCell(3).SetCellValue("角色名称");
                row2.CreateCell(4).SetCellValue("部门名称");
                row2.CreateCell(5).SetCellValue("真实姓名");
                row2.CreateCell(6).SetCellValue("操作时间");
                row2.CreateCell(7).SetCellValue("状态");
                row2.CreateCell(8).SetCellValue("备注");
                for (int i = 0; i < colCount; i++)
                {
                    row2.GetCell(i).CellStyle = styleTh;
                }
                #endregion

                int rowNumber = 2;//行号索引
                IRow row;
                foreach (var item in searchList)
                {
                    row = sheet1.CreateRow(rowNumber);
                    row.HeightInPoints = rowHeight;
                    row.CreateCell(0).SetCellValue(rowNumber - 1);
                    row.GetCell(0).CellStyle = styleCenter;

                    row.CreateCell(1).SetCellValue(item.UserCode);
                    row.GetCell(1).CellStyle = styleLeft;

                    row.CreateCell(2).SetCellValue(item.UserName);
                    row.GetCell(2).CellStyle = styleLeft;

                    row.CreateCell(3).SetCellValue(item.RoleName);
                    row.GetCell(3).CellStyle = styleLeft;

                    row.CreateCell(4).SetCellValue(item.DeptName);
                    row.GetCell(4).CellStyle = styleLeft;

                    row.CreateCell(5).SetCellValue(item.RealName);
                    row.GetCell(5).CellStyle = styleLeft;

                    row.CreateCell(6).SetCellValue(item.OperateDate == null ? "" : item.OperateDate.Value.ToString("yyyy-MM-dd HH:mm"));
                    row.GetCell(6).CellStyle = styleLeft;

                    row.CreateCell(7).SetCellValue(item.IsValid == 1 ? "有效" : "无效");
                    row.GetCell(7).CellStyle = styleLeft;

                    row.CreateCell(8).SetCellValue(item.Remark);
                    row.GetCell(8).CellStyle = styleLeft;
                    rowNumber++;
                }
                for (int i = 0; i < colCount; i++)
                {
                    sheet1.AutoSizeColumn(i);
                }
                using (var file = new MemoryStream())
                {
                    workbook.Write(file);
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = file.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
                var sRetrunModel = new ResultMessage
                {
                    ErrorType = 1,
                    MessageContent = fileName
                };
                return Json(sRetrunModel);
            }
            catch (Exception ex)
            {
                MessageLog.WriteLog(new LogParameterModel
                {
                    LogLevel = ELogLevel.Error,
                    Title = "导出数据",
                    Message = ex.Message,
                    ClassName = this.GetType().FullName
                });
                var exRetrunModel = new ResultMessage
                {
                    ErrorType = 0,
                    MessageContent = "操作失败：系统性异常."
                };
                return Json(exRetrunModel);
            }
        }

        #endregion
    }
}