using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Model;
using BLL;
using CommonLib;

namespace CareHome.Controllers
{
    public class OrgTLJGCongYeController : Controller
    {
        private string ParamID = "";   //数据ID
        private string ParamName = new LogsBll().ReturnNodeName((int)CommonLib.NodePagesSetting.EOrgTLJGCongYe.ListPage);   //模块名称
        private string ParamState = "1";   //1：添加，2：修改，3：逻辑，4：物理，5：启用，6：禁用

        private int ListPageNodeId { get { return (int)CommonLib.NodePagesSetting.EOrgTLJGCongYe.ListPage; } }
        private int ListPageNodeId90 { get { return (int)CommonLib.NodePagesSetting.EOrgTLJGCongYe.ListPage90; } }
        private int AddPageNodeId { get { return (int)CommonLib.NodePagesSetting.EOrgTLJGCongYe.AddPage; } }
        private int EditPageNodeId { get { return (int)CommonLib.NodePagesSetting.EOrgTLJGCongYe.EditPage; } }
        private int DetailPageNodeId { get { return (int)CommonLib.NodePagesSetting.EOrgTLJGCongYe.DetailPage; } }


        private string GetOrgID()
        {
            var buf = new StringBuilder();
            var cBll = new OrganizationBll();
            var list = cBll.selectAll();
            if (list != null)
            {
                foreach (var item in list)
                {
                    buf.AppendFormat("<option value=\"{0}\">{1}</option>", item.WelfareId, item.WelfareName);
                }
            }
            return buf.ToString();
        }
        [OutputCache(Duration = 0)]
        public ActionResult List()
        {
            #region 页面权限判断
            if (!Utits.IsLogin)
            {
                return RedirectPermanent("../Login/Index");
            }
            #endregion
            int[] NodePages = { ListPageNodeId, ListPageNodeId90 };
            int NodeId = CommonLib.RequestParameters.Pint("NodeId");
            if (!NodePages.Contains(NodeId))
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "参数(NodeId)错误，请联系系统管理员！", EErrorRank.Error));
            }
            if (Utits.IsNodePageAuth(NodeId).ErrorType != 1)
            {
                return RedirectPermanent(string.Format("../Error/Index?url={0}&msg={1}&errorrank={2}", System.Web.HttpUtility.UrlEncode(Request.Url?.ToString() ?? ""), "您没有该页面的访问权限！", EErrorRank.Error));
            }
            ViewBag.OperateButton = Utits.AuthOperateButton();
            if (NodeId != ListPageNodeId90)
            {
                ViewBag.AddPageNodeId = AddPageNodeId;
                ViewBag.EditPageNodeId = EditPageNodeId;
                ViewBag.DetailPageNodeId = DetailPageNodeId;

                ViewBag.DetailsOperateButton = Utits.AuthOperateButton(AddPageNodeId);
            }
            ViewBag.GetOrgID = GetOrgID();
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
            if (NodeId == ListPageNodeId90)
            {
                ViewBag.ListPageNodeId = ListPageNodeId90;
            }
            else { ViewBag.ListPageNodeId = ListPageNodeId; }

            ViewBag.OperateButton = Utits.AuthOperateButton();
            return View();
        }


        #region
        public JsonResult SearchList()
        {
            #region 权限控制
            int[] iRangePage = { ListPageNodeId, ListPageNodeId90 };
            int iCurrentPageNodeId = RequestParameters.Pint("NodeId");
            var tempNoAuth = Utits.IsNodePageAuth(iRangePage, iCurrentPageNodeId);
            if (tempNoAuth.ErrorType != 1)
                return Json(tempNoAuth);
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
            var Name = RequestParameters.Pstring("Name");
            if (Name.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "StaffName";
                whereCondition.FieldValue = Name;
                whereCondition.FieldOperator = EnumOper.Contains;
                WhereList.Add(whereCondition);
            }
            Guid welfareCentreId = Utits.WelfareCentreID;
            if (welfareCentreId != Guid.Empty)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "WelfareId";
                whereCondition.FieldValue = welfareCentreId;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            if (true)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "IsValid";
                whereCondition.FieldValue = 1;
                whereCondition.FieldOperator = EnumOper.Equal;
                WhereList.Add(whereCondition);
            }
            searchCondition.WhereList = WhereList;

            var OrderList = new List<OrderCondition>();
            string sortfield = RequestParameters.Pstring("sortfield");
            string sorttype = RequestParameters.Pstring("sorttype");
            if (sortfield.Length <= 0)
            {
                sortfield = "OperateDate";
            }
            var orderCondition = new OrderCondition();
            orderCondition.FiledOrder = sortfield;
            orderCondition.Ascending = sorttype == "asc" ? true : false;
            OrderList.Add(orderCondition);

            searchCondition.OrderList = OrderList;
            #endregion

            var cBll = new OrgTLJGCongYeBll();
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
            return Json(sReturnModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListPhyDel()
        {
            #region 权限控制
            int[] iRangePage = { AddPageNodeId, EditPageNodeId };
            int iCurrentPageNodeId = AddPageNodeId;
            int[] iRangeButton = { (int)EButtonType.Delete, (int)EButtonType.Enable, (int)EButtonType.Disable };
            int iCurrentButtonId = RequestParameters.Pint("oButtonId");
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iRangeButton, iCurrentButtonId);
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
                    ParamID += strids[i] + ",";
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

            var cBll = new OrgTLJGCongYeBll();
            bool isFlag = cBll.PhysicalDeleteByCondition(ids);
            if (isFlag)
            {
                ParamState = "4";
                var cLog = new LogsBll();
                cLog.Log(ParamID.TrimEnd(','), ParamName, ParamState, Utits.CurrentUserID.ToString(), Utits.CurrentRealName.ToString(), Utits.WelfareCentreID.ToString(), Utits.ClientIPAddress.ToString());

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
            int[] iRangeButton = { (int)EButtonType.Delete, (int)EButtonType.Enable, (int)EButtonType.Disable };
            int iCurrentButtonId = RequestParameters.Pint("oButtonId");
            var tempNoAuth = Utits.IsOperateAuth(iRangePage, iCurrentPageNodeId, iRangeButton, iCurrentButtonId);
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
                    ParamID += strids[i] + ",";
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

            var cBll = new OrgTLJGCongYeBll();
            bool isFlag = false;
            switch (iCurrentButtonId)
            {
                case (int)EButtonType.Delete://删除
                    ParamState = "3";
                    isFlag = cBll.LogicDeleteByCondition(ids);
                    break;
                case (int)EButtonType.Enable://启用
                    ParamState = "5";
                    isFlag = cBll.EnableByCodeition(ids);
                    break;
                case (int)EButtonType.Disable://禁用
                    ParamState = "6";
                    isFlag = cBll.DisableByCodeition(ids);
                    break;
            }
            if (isFlag)
            {
                var cLog = new LogsBll();
                cLog.Log(ParamID.TrimEnd(','), ParamName, ParamState, Utits.CurrentUserID.ToString(), Utits.CurrentRealName.ToString(), Utits.WelfareCentreID.ToString(), Utits.ClientIPAddress.ToString());

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
                var usersBll = new OrgTLJGCongYeBll();
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

            Guid id = RequestParameters.PGuid("ID"); 
            Guid ddlOrgID = RequestParameters.PGuid("ddlOrgID");
            var StaffName = RequestParameters.Pstring("StaffName");
            var StaffGender = RequestParameters.Pint("StaffGender");
            var EmployType = RequestParameters.Pstring("EmployType");
            var SocialSecurity = RequestParameters.Pstring("SocialSecurity");
            var Profession = RequestParameters.Pstring("Profession");
            var WeiJi = RequestParameters.Pstring("WeiJi");
            var SheGong = RequestParameters.Pstring("SheGong");
            var QiTa = RequestParameters.Pstring("QiTa");
            var HgChiZheng = RequestParameters.Pstring("HgChiZheng");
            var HgXueLi = RequestParameters.Pstring("HgXueLi");
            var HgNianLing = RequestParameters.Pstring("HgNianLing");
            var HgRuZhiNian = RequestParameters.Pstring("HgRuZhiNian");
            var HgHuJi = RequestParameters.Pstring("HgHuJi");
            var HouQin = RequestParameters.Pstring("HouQin");
            var Phone = RequestParameters.Pstring("Phone");
            var AddressPlace = RequestParameters.Pstring("AddressPlace");

            var item = new tbOrgTLJGCongYe();
            item.ID = id;
            item.OperateDate = DateTime.Now;
            item.OperateUserID = Utits.CurrentUserID;
            item.OrganizationId = ddlOrgID;
            item.WelfareId = ddlOrgID;

            item.StaffName = StaffName;
            item.StaffGender = StaffGender;
            item.EmployType = EmployType;
            item.SocialSecurity = SocialSecurity;
            item.Profession = Profession;
            item.WeiJi = WeiJi;
            item.SheGong = SheGong;
            item.QiTa = QiTa;
            item.HgChiZheng = HgChiZheng;
            item.HgXueLi = HgXueLi;
            item.HgNianLing = HgNianLing;
            item.HgRuZhiNian = HgRuZhiNian;
            item.HgHuJi = HgHuJi;
            item.HouQin = HouQin;

            item.Phone = Phone;
            item.AddressPlace = AddressPlace;

            if (item.ID == Guid.Empty)
            {
                ParamState = "1";
                ParamID = id.ToString();
            }
            else
            {
                ParamState = "2";
                ParamID = id.ToString();
            }
            var cBll = new OrgTLJGCongYeBll();
            var isFlag = cBll.AddOrUpdate(item);

            if (isFlag)
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
        }
        #endregion

    }
}