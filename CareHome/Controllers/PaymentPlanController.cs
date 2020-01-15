using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using CommonLib;
using System.Text;

namespace CareHome.Controllers
{
    public class PaymentPlanController : Controller
    {
        private string ParamID = "";   //数据ID
        private string ParamName = new LogsBll().ReturnNodeName((int)CommonLib.NodePagesSetting.ECustomer.ListPage);   //模块名称
        private string ParamState = "1";   //1：添加，2：修改，3：逻辑，4：物理，5：启用，6：禁用
        private int ListPageNodeId { get { return (int)CommonLib.NodePagesSetting.EPaymentPlan.ListPage; } }
        private int AddPageNodeId { get { return (int)CommonLib.NodePagesSetting.ECustomer.AddPage; } }
        private int EditPageNodeId { get { return (int)CommonLib.NodePagesSetting.ECustomer.EditPage; } }
        private int DetailPageNodeId { get { return (int)CommonLib.NodePagesSetting.ECustomer.DetailPage; } }

        #region
        public string GetCustomerName()
        {
            var buf = new StringBuilder();
            var cBll = new CustomerBll();
            var list = cBll.SearchListByValid(Utits.WelfareCentreID);
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
            var list = cBll.SearchListByValid(Utits.WelfareCentreID);
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

            ViewBag.GetCustomerName = GetCustomerName();
            ViewBag.GetCareName = GetCareName();
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
            string Name = RequestParameters.Pstring("Name");
            if (Name.Length > 0)
            {
                var whereCondition = new WhereCondition();
                whereCondition.FieldName = "RealName";
                whereCondition.FieldValue = Name;
                whereCondition.FieldOperator = EnumOper.Contains;
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
                sortfield = "ID";
            }
            var orderCondition = new OrderCondition();
            orderCondition.FiledOrder = sortfield;
            orderCondition.Ascending = true;
            OrderList.Add(orderCondition);

            searchCondition.OrderList = OrderList;
            #endregion

            var cBll = new PaymentPlanBll();
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
                    ParamID += strids[i] + ",";
                    arrayBedIdList.Add(strbedIds[i]);
                }
            }
            string[] bedIds = (string[])arrayBedIdList.ToArray(typeof(string));
            var welfareCentreId = Utits.WelfareCentreID;
            var cBll = new PaymentPlanBll();
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
                ParamState = "4";
                ParamID = customerId.ToString();
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
                    ParamState = "3";
                    isFlag = cBll.OperateDataStatus(customerId, bedId, welfareCentreId, (int)ESystemStatus.Deleted);
                    break;
                case (int)EButtonType.Enable://启用
                    ParamState = "5";
                    isFlag = cBll.OperateDataStatus(customerId, bedId, welfareCentreId, (int)ESystemStatus.Valid);
                    break;
                case (int)EButtonType.Disable://禁用
                    ParamState = "6";
                    isFlag = cBll.OperateDataStatus(customerId, bedId, welfareCentreId, (int)ESystemStatus.Forbidden);
                    break;
            }
            if (isFlag)
            {
                ParamID = customerId.ToString();
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

            ParamState = "1";
            ParamID = item.ID.ToString();

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

        #region 相对应的汉语拼音首字母串
        /// <summary> 
        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串 
        /// </summary> 
        /// <param name="CnStr">汉字字符串</param> 
        /// <returns>相对应的汉语拼音首字母串</returns> 
        public static string GetSpellCode(string CnStr)
        {
            string strTemp = "";
            int iLen = CnStr.Length;
            int i = 0;

            for (i = 0; i <= iLen - 1; i++)
            {
                strTemp += GetCharSpellCode(CnStr.Substring(i, 1));
            }

            return strTemp;
        }


        /// <summary> 
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 
        /// </summary> 
        /// <param name="CnChar">单个汉字</param> 
        /// <returns>单个大写字母</returns> 
        private static string GetCharSpellCode(string CnChar)
        {
            long iCnChar;

            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回 
            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }
            else
            {
                // get the array of byte from the single char 
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }

            //expresstion 
            //table of the constant list 
            // 'A'; //45217..45252 
            // 'B'; //45253..45760 
            // 'C'; //45761..46317 
            // 'D'; //46318..46825 
            // 'E'; //46826..47009 
            // 'F'; //47010..47296 
            // 'G'; //47297..47613 

            // 'H'; //47614..48118 
            // 'J'; //48119..49061 
            // 'K'; //49062..49323 
            // 'L'; //49324..49895 
            // 'M'; //49896..50370 
            // 'N'; //50371..50613 
            // 'O'; //50614..50621 
            // 'P'; //50622..50905 
            // 'Q'; //50906..51386 

            // 'R'; //51387..51445 
            // 'S'; //51446..52217 
            // 'T'; //52218..52697 
            //没有U,V 
            // 'W'; //52698..52979 
            // 'X'; //52980..53640 
            // 'Y'; //53689..54480 
            // 'Z'; //54481..55289 

            // iCnChar match the constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }

            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else return ("?");
        }

        #endregion 

        #endregion



    }
}