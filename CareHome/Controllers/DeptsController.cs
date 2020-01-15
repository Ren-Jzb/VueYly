using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using CommonLib;

namespace CareHome.Controllers
{

    public class DeptsController : Controller
    {
        private string ParamID = "";   //数据ID
        private string ParamName = new LogsBll().ReturnNodeName((int)CommonLib.NodePagesSetting.EDepts.ListPage);   //模块名称
        private string ParamState = "1";   //1：添加，2：修改，3：逻辑，4：物理，5：启用，6：禁用
        private int ListPageNodeId { get { return (int)CommonLib.NodePagesSetting.EDepts.ListPage; } }
        private int AddPageNodeId { get { return (int)CommonLib.NodePagesSetting.EDepts.AddPage; } }
        private int EditPageNodeId { get { return (int)CommonLib.NodePagesSetting.EDepts.EditPage; } }
        private int DetailPageNodeId { get { return (int)CommonLib.NodePagesSetting.EDepts.DetailPage; } }
        [OutputCache(Duration = 0)]
        public ActionResult List()
        {
            #region 页面权限判断
            if (!Utits.IsLogin)
            {
                return RedirectPermanent("../Login/Index");
            }
            #endregion
            int[] NodePages = { ListPageNodeId };
            int NodeId = CommonLib.RequestParameters.Pint("NodeId");
            if (!NodePages.Contains(NodeId))
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "参数(NodeId)错误，请联系系统管理员！", EErrorRank.Error));
            }
            if (Utits.IsNodePageAuth(NodeId).ErrorType != 1)
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "您没有该页面的访问权限！", EErrorRank.Error));
            }

            ViewBag.AddPageNodeId = AddPageNodeId;
            ViewBag.EditPageNodeId = EditPageNodeId;
            ViewBag.DetailPageNodeId = DetailPageNodeId;

            ViewBag.OperateButton = Utits.AuthOperateButton();
            ViewBag.DetailsOperateButton = Utits.AuthOperateButton(AddPageNodeId);

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
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "参数(NodeId)错误，请联系系统管理员！", EErrorRank.Error));
            }
            if (Utits.IsNodePageAuth(NodeId).ErrorType != 1)
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "您没有该页面的访问权限！", EErrorRank.Error));
            }
            ViewBag.ListPageNodeId = ListPageNodeId;

            ViewBag.OperateButton = Utits.AuthOperateButton();

            return View();
        }

        #region JsonResult
        public JsonResult SearchList()
        {
            #region 权限控制
            int[] iRangePage = { ListPageNodeId };
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
            Guid welfareCentreId = Utits.WelfareCentreID;
            if (welfareCentreId != Guid.Empty)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "WelfareCentreID";
                whereCondition.FieldValue = welfareCentreId;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            string Name = RequestParameters.Pstring("Name");
            if (Name.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "DeptName";
                whereCondition.FieldValue = Name;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }
            int? IsValid = RequestParameters.PintNull("IsValid");
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

            var cBll = new DeptsBll();
            var list = cBll.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, searchCondition);
            iPageSize = iPageSize == 0 ? iTotalRecord : iPageSize;
            int pageCount = iTotalRecord % iPageSize == 0 ? iTotalRecord / iPageSize : iTotalRecord / iPageSize + 1;
            var sReturnModel = new ResultList();
            sReturnModel.ErrorType = 1;
            sReturnModel.CurrentPage = iCurrentPage;
            sReturnModel.PageSize = iPageSize;
            sReturnModel.TotalRecord = iTotalRecord;
            sReturnModel.PageCount = pageCount;
            sReturnModel.Data = list;
            return Json(sReturnModel);
        }
        public JsonResult AddOrUpdate()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            bool isAdd = iCurrentPageNodeId == AddPageNodeId ? true : false;
            int iCurrentButtonId = (int)EButtonType.Save;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            #region AddOrUpdate
            string DeptCode = RequestParameters.Pstring("DeptCode");
            string DeptName = RequestParameters.Pstring("DeptName");
            string Remark = RequestParameters.Pstring("Remark");
            if (DeptName.Length <= 0)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "部门名称不能为空.";
                return Json(sRetrunModel);
            }

            var item = new Depts();
            Guid ID = RequestParameters.PGuid("ID");
            item.DeptID = ID;
            item.OperateDate = DateTime.Now;
            item.DeptCode = DeptCode;
            item.DeptName = DeptName;
            item.Remark = Remark;
            item.WelfareCentreID = Utits.WelfareCentreID;

            var cBll = new DeptsBll();
            bool isFlag = cBll.AddOrUpdateDao(item);
            if (isFlag)
            {
                ParamState = "1";
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
                var cBll = new DeptsBll();
                var item = cBll.GetObjectById(ID);
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

            //注释时间 20190306 林中枝  
            //后期多条删除数据时  酌情修改
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
                ParamState = "4";
                ParamID = ID.ToString();
                var cLog = new LogsBll();
                cLog.Log(ParamID, ParamName, ParamState, Utits.CurrentUserID.ToString(), Utits.CurrentRealName.ToString(), Utits.WelfareCentreID.ToString(), Utits.ClientIPAddress.ToString());

                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            var cBll = new DeptsBll();
            bool isFlag = cBll.PhysicalDelete(ID, Utits.WelfareCentreID);
            if (isFlag)
            {
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

            //注释时间 20190306 林中枝  
            //后期多条删除数据时  酌情修改
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
            var cBll = new DeptsBll();
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
        #endregion
    }
}