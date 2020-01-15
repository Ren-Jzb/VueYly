using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using CommonLib;
using System.Text;

namespace CareHome.Mvvm
{
    public class VmPaymentPlanController : Controller
    {
        private int ListPageNodeId { get { return (int)CommonLib.NodePagesSetting.EPaymentPlan.ListPage; } }
        private int AddPageNodeId { get { return (int)CommonLib.NodePagesSetting.ECustomer.AddPage; } }
        private int EditPageNodeId { get { return (int)CommonLib.NodePagesSetting.ECustomer.EditPage; } }
        private int DetailPageNodeId { get { return (int)CommonLib.NodePagesSetting.ECustomer.DetailPage; } }

        #region  string
        public string GetCustomerName()
        {
            var buf = new StringBuilder();
            var cBll = new CustomerBll();
            var list = cBll.SearchListByValid(new Guid("0295B2CA-4439-4A1D-90FD-A76FA089A3BA"));
            //var list = cBll.SearchListByValid(Utits.WelfareCentreID);
            buf.AppendFormat("<option value=\"{0}\">{1}</option>", "请选择", "==请选择==");
            if (list != null)
            {
                foreach (var item in list)
                {
                    buf.AppendFormat("<option value=\"{0}\">{1}</option>", item.ID, item.CustomerName);
                }
            }
            return buf.ToString();
        }


        public string GetCareName()
        {
            var buf = new StringBuilder();
            var cBll = new UsersBll();
            var list = cBll.SearchListByValid(new Guid("0295B2CA-4439-4A1D-90FD-A76FA089A3BA"));
            //var list = cBll.SearchListByValid(Utits.WelfareCentreID);
            buf.AppendFormat("<option value=\"{0}\">{1}</option>", "请选择", "==请选择==");
            if (list != null)
            {
                foreach (var item in list)
                {
                    buf.AppendFormat("<option value=\"{0}\">{1}</option>", item.UserID, item.RealName);
                }
            }
            return buf.ToString();
        }
        #endregion

        // GET: VmPaymentPlan
        public ActionResult List()
        {
            ViewBag.AddPageNodeId = AddPageNodeId;
            ViewBag.EditPageNodeId = EditPageNodeId;
            ViewBag.DetailPageNodeId = DetailPageNodeId;
            ViewBag.OperateButton = Utits.AuthOperateButton();
            ViewBag.DetailsOperateButton = Utits.AuthOperateButton(AddPageNodeId);

            ViewBag.GetCustomerName = GetCustomerName();
            ViewBag.GetCareName = GetCareName();

            return View();
        }
        public ActionResult ListOne()
        {
            ViewBag.AddPageNodeId = AddPageNodeId;
            ViewBag.EditPageNodeId = EditPageNodeId;
            ViewBag.DetailPageNodeId = DetailPageNodeId;
            ViewBag.OperateButton = Utits.AuthOperateButton();
            ViewBag.DetailsOperateButton = Utits.AuthOperateButton(AddPageNodeId);

            ViewBag.GetCustomerName = GetCustomerName();
            ViewBag.GetCareName = GetCareName();

            return View();
        }

        #region   JsonResult
        public JsonResult AddOrUpdate()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = AddPageNodeId;
            int iCurrentButtonId = (int)EButtonType.Save;


            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            #region AddOrUpdate
            Guid ID = RequestParameters.PGuid("ID");
            Guid ddlOrgID = RequestParameters.PGuid("ddlOrgID");
            Guid CustomerId = RequestParameters.PGuid("CustomerId");
            Guid HgId = RequestParameters.PGuid("HgId");
            DateTime? ServerStart = RequestParameters.PDateTime("ServerStart");
            DateTime? ServerEnd = RequestParameters.PDateTime("ServerEnd");
            string WeekOne = RequestParameters.Pstring("WeekOne");
            string WeekTwo = RequestParameters.Pstring("WeekTwo");
            string WeekThree = RequestParameters.Pstring("WeekThree");
            string WeekFour = RequestParameters.Pstring("WeekFour");
            string WeekFive = RequestParameters.Pstring("WeekFive");
            string WeekSix = RequestParameters.Pstring("WeekSix");
            string WeekServer = RequestParameters.Pstring("WeekServer");
            string ServerTime = RequestParameters.Pstring("ServerTime");
            string Remark = RequestParameters.Pstring("Remark");

            var cBll = new PaymentPlanBll();
            var item = new tbPaymentPlan();

            item.ID = ID;
            item.CustomerId = CustomerId;
            item.HgId = HgId;
            item.ServerStart = ServerStart ?? DateTime.Now;
            item.ServerEnd = ServerEnd ?? DateTime.Now;
            item.WeekOne = WeekOne;
            item.WeekTwo = WeekTwo;
            item.WeekThree = WeekThree;
            item.WeekFour = WeekFour;
            item.WeekFive = WeekFive;
            item.WeekSix = WeekSix;
            item.WeekServer = WeekServer;
            item.ServerTime = ServerTime;
            item.Remark = Remark;
            item.OperateDate = DateTime.Now;
            item.CreateDate = DateTime.Now;
            item.OperatorUserID = Utits.CurrentUserID;
            item.WelfareCentreID = ddlOrgID;
            bool IsFlag = cBll.AddOrUpdate(item);
            if (IsFlag)
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
            #endregion
        }

        public JsonResult InitSingle()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId, DetailPageNodeId };
            int iCurrentPageNodeId = AddPageNodeId;
            var tempAuth = Utits.IsNodePageAuth(iRangePage, iCurrentPageNodeId);
            if (tempAuth.ErrorType != 1)
                return Json(tempAuth);
            #endregion

            #region InitSingle
            Guid ID = RequestParameters.PGuid("ID");
            if (ID != Guid.Empty)
            {
                var usersBll = new PaymentPlanBll();
                var item = usersBll.GetVObjectById(ID);
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

        public JsonResult ListPhyDelOld()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId };
            int iCurrentPageNodeId = AddPageNodeId;
            int iCurrentButtonId = (int)EButtonType.PhyDelete;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            string _ids = RequestParameters.Pstring("ids");
            if (string.IsNullOrEmpty(_ids))
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            string[] strids = _ids.Split(',');
            System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
            for (int i = 0; i < strids.Length; i++)
            {
                if (RegexValidate.IsGuid(strids[i]))
                {
                    arrayList.Add(strids[i]);
                }
            }
            string[] ids = (string[])arrayList.ToArray(typeof(string));
            if (!ids.Any())
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            string _bedIds = RequestParameters.Pstring("bedIds");
            string[] strbedIds = _bedIds.Split(',');
            System.Collections.ArrayList arrayBedIdList = new System.Collections.ArrayList();
            for (int i = 0; i < strbedIds.Length; i++)
            {
                if (RegexValidate.IsGuid(strbedIds[i]))
                {
                    arrayBedIdList.Add(strbedIds[i]);
                }
            }
            string[] bedIds = (string[])arrayBedIdList.ToArray(typeof(string));
            var welfareCentreId = Utits.WelfareCentreID;
            var cBll = new PaymentPlanBll();
            bool isFlag = cBll.PhysicalDeleteByCondition(ids);
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

        public JsonResult ListPhyDel()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId };
            int iCurrentPageNodeId = AddPageNodeId;
            int iCurrentButtonId = (int)EButtonType.PhyDelete;
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            var customerId = RequestParameters.PGuid("ids");
            if (customerId == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            var bedId = RequestParameters.PGuid("bedIds");
            //if (bedId == Guid.Empty)
            //{
            //    var sRetrunModel = new ResultMessage();
            //    sRetrunModel.ErrorType = 0;
            //    sRetrunModel.MessageContent = "参数错误.";
            //    return Json(sRetrunModel);
            //}
            var welfareCentreId = Utits.WelfareCentreID;
            var cBll = new PaymentPlanBll();
            bool isFlag = cBll.PhysicalDelete(customerId, bedId, welfareCentreId);
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
            int[] iRangePage = { AddPageNodeId, EditPageNodeId };
            int iCurrentPageNodeId = AddPageNodeId;
            int[] iRangeButton = { (int)EButtonType.CustomerLeave, (int)EButtonType.Enable, (int)EButtonType.Disable };
            int iCurrentButtonId = RequestParameters.Pint("oButtonId");
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iRangeButton, iCurrentButtonId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
            #endregion

            var customerId = RequestParameters.PGuid("ids");
            if (customerId == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            var bedId = RequestParameters.PGuid("bedIds");
            if (bedId == Guid.Empty)
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            var welfareCentreId = Utits.WelfareCentreID;

            var cBll = new PaymentPlanBll();
            bool isFlag = false;
            switch (iCurrentButtonId)
            {
                case (int)EButtonType.CustomerLeave://删除
                    isFlag = cBll.OperateDataStatus(customerId, bedId, welfareCentreId, (int)ESystemStatus.Deleted);
                    break;
                case (int)EButtonType.Enable://启用
                    isFlag = cBll.OperateDataStatus(customerId, bedId, welfareCentreId, (int)ESystemStatus.Valid);
                    break;
                case (int)EButtonType.Disable://禁用
                    isFlag = cBll.OperateDataStatus(customerId, bedId, welfareCentreId, (int)ESystemStatus.Forbidden);
                    break;
            }
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
        #endregion


        #region   NEW
        //查询
        public ActionResult SearchList(string Name, int curPage)
        {
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
            int size = 2;

            iCurrentPage = curPage <= 0 ? 1 : curPage;

            #region 查询条件
            var searchCondition = new ConditionModel();
            var WhereList = new List<WhereCondition>();
            if (!string.IsNullOrEmpty(Name))
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "RealName";
                whereCondition.FieldValue = Name;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }
            //var welfareCentreId = Utits.WelfareCentreID;
            //if (welfareCentreId != null)
            //{
            //    var whereCondition = new WhereCondition();
            //    whereCondition.FieldName = "WelfareCentreID";
            //    whereCondition.FieldValue = welfareCentreId;
            //    whereCondition.FieldOperator = EnumOper.Equal;
            //    WhereList.Add(whereCondition);
            //}
            searchCondition.WhereList = WhereList;

            var OrderList = new List<OrderCondition>();
            string sortfield = RequestParameters.Pstring("sortfield");
            if (sortfield.Length <= 0)
            {
                sortfield = "ID";
            }
            var orderCondition = new OrderCondition();
            orderCondition.FiledOrder = sortfield;
            orderCondition.Ascending = true;
            OrderList.Add(orderCondition);

            searchCondition.OrderList = OrderList;
            #endregion

            var cBll = new PaymentPlanBll();
            var list = cBll.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, searchCondition).ToList();
            iPageSize = iPageSize == 0 ? iTotalRecord : iPageSize;
            int pageCount = iTotalRecord % iPageSize == 0 ? iTotalRecord / iPageSize : iTotalRecord / iPageSize + 1;
            var sReturnModel = new ResultList();
            sReturnModel.ErrorType = 1;
            sReturnModel.CurrentPage = iCurrentPage;
            sReturnModel.PageSize = iPageSize;
            sReturnModel.TotalRecord = iTotalRecord;
            sReturnModel.PageCount = pageCount;
            sReturnModel.Data = list;
            var pages = new PageList(iCurrentPage, list.Count, size, 7);//初始化分页类
            list = list.OrderByDescending(x => x.OperateDate).Skip((iCurrentPage - 1) * size).Take(size).ToList();//取页面记录列表 同时取一对多数据
            var data = new { list = list, pages = pages };//构造对象
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加or修改
        public ActionResult AddOrEditPg(tbPaymentPlan pg)
        {
            var cBll = new PaymentPlanBll();
            bool val = cBll.EditPaymentPlan(pg);
            return Json(val);
        }

        //删除
        public JsonResult Delete(string id)
        {
            string _ids = RequestParameters.Pstring("id");
            if (string.IsNullOrEmpty(_ids))
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            string[] strids = _ids.Split(',');
            System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
            for (int i = 0; i < strids.Length; i++)
            {
                if (RegexValidate.IsGuid(strids[i]))
                {
                    arrayList.Add(strids[i]);
                }
            }
            string[] ids = (string[])arrayList.ToArray(typeof(string));
            if (!ids.Any())
            {
                var sRetrunModel = new ResultMessage();
                sRetrunModel.ErrorType = 0;
                sRetrunModel.MessageContent = "参数错误.";
                return Json(sRetrunModel);
            }
            //var welfareCentreId = Utits.WelfareCentreID;

            var cBll = new PaymentPlanBll();
            bool isFlag = false;
            isFlag = cBll.DeletePaymentPlan(ids, (int)ESystemStatus.Deleted);
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
        #endregion

    }
}