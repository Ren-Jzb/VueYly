using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using BLL;
using CommonLib;
using Microsoft.Ajax.Utilities;
using Model;
using VO;
using DTO;
using System.Linq;

namespace CareHome.Ws
{
    /// <summary>
    /// CareOperate 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class CareOperate : System.Web.Services.WebService
    {
        #region   登录系统
        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="bizContent"></param>
        /// <param name="timeStamp"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [WebMethod(Description = "登录系统")]
        public string canLogin(string bizContent, long timeStamp, string signature)
        {
            MessageLog.WriteLog(new LogParameterModel
            {
                ClassName = this.GetType().ToString(),
                MethodName = "canLogin",
                MethodParameters = $"bizContent:{bizContent},timeStamp:{timeStamp},signature:{signature}",
                LogLevel = ELogLevel.Info,
                Message = "接收参数",
                PathPrefix = "/log/ws",
                LogExt = "txt"
            });
            var result = new CanLoginVo();
            var paramItem = CommonLib.JsonHelper.Deserialize<LoginDto>(bizContent);
            if (paramItem == null)
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败:bizContent不合法.";
                return result.ToJSON();
            }
            var loginName = paramItem.loginName;
            if (loginName.Length < 0)
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败:账号不能为空.";
                return result.ToJSON();
            }
            var cBll = new UsersBll();
            var item = cBll.LoginHgUsers(loginName, paramItem.password);
            if (item != null)
            {
                result.resultCode = 1;
                result.resultMessage = "登录成功.";
                result.userId = item.UserID.ToString();
                result.realName = item.RealName;
                result.welfareCentreID = item.WelfareCentreID == null ? Guid.Empty.ToString() : item.WelfareCentreID.ToString();
            }
            else
            {
                result.resultCode = 0;
                result.resultMessage = "用户名或密码错误.";
            }
            return result.ToJSON();
        }
        #endregion


        #region   获取服务计划
        /// <summary>
        /// 获取服务计划
        /// </summary>
        /// <param name="bizContent"></param>
        /// <param name="timeStamp"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取服务计划")]
        public string getCarePlan(string bizContent, long timeStamp, string signature)
        {
            //MessageLog.WriteLog(new LogParameterModel
            //{
            //    ClassName = this.GetType().ToString(),
            //    MethodName = "getCarePlan",
            //    MethodParameters = $"bizContent:{bizContent},timeStamp:{timeStamp},signature:{signature}",
            //    LogLevel = ELogLevel.Info,
            //    Message = "接收参数",
            //    PathPrefix = "/log/ws",
            //    LogExt = "txt"
            //});
            //var result = new GetNursingVo();
            //var paramItem = CommonLib.JsonHelper.Deserialize<GetNursingDto>(bizContent);
            //if (paramItem == null)
            //{
            //    result.resultCode = 0;
            //    result.resultMessage = "操作失败:bizContent不合法.";
            //    return result.ToJSON();
            //}
            //var customerBll = new CustomerBll();
            //var itemCustomer = customerBll.GetVijObjectById(paramItem.welfareCentreId, paramItem.bedNumber);
            //if (itemCustomer == null)
            //{
            //    result.resultCode = 0;
            //    result.resultMessage = "未获得信息.";
            //    return result.ToJSON();
            //}
            //var prModel = new personModel();
            //prModel.userId = itemCustomer.ID;
            //prModel.personName = itemCustomer.CustomerName;
            //prModel.age = itemCustomer.CustomerAge ?? 0;
            //prModel.gender = itemCustomer.CustomerGender == 1 ? "男" : "女";

            //var bufRemark = "";
            //var iteRemark = new PaymentPlanBll().GetById(itemCustomer.WelfareCentreID, itemCustomer.ID);
            //if (iteRemark != null)
            //{
            //    bufRemark = iteRemark.Remark;
            //}
            //prModel.remark = bufRemark.ToString();
            //result.person = prModel;


            //var listNursing = new List<nursingModel>();
            //var fatherId = 1;
            //var nursingRankBll = new NursingRankBll();
            //var nursingRankExtBll = new NursingRankExtBll();
            //nursingModel itemNModel;
            //var listNurs = nursingRankBll.SearchListByWhole(fatherId, paramItem.welfareCentreId);
            //foreach (var itemInfo in listNurs)
            //{
            //    itemNModel = new nursingModel();
            //    itemNModel.nursingContent = itemInfo.RankContent;
            //    itemNModel.nursingId = itemInfo.ID;
            //    itemNModel.fatherId = itemInfo.ParentID ?? 0;
            //    itemNModel.nursingImgUrl = itemInfo.RankImgUrl;

            //    var listNursingExt = new List<nursingExtModel>();
            //    nursingExtModel itemEXTModel;
            //    var listNursExt = nursingRankExtBll.SearchListByValid(itemNModel.nursingId);
            //    if (listNursExt.Count > 0)
            //    {
            //        itemEXTModel = new nursingExtModel();
            //        itemEXTModel.nursingId = itemNModel.nursingId;
            //        itemEXTModel.extType = listNursExt[0].ExtType;
            //        itemEXTModel.extTitle = listNursExt[0].RankTitle;
            //        string extContent = "";
            //        foreach (var itemInfoExt in listNursExt)
            //        {
            //            extContent += itemInfoExt.RankContent + "||";
            //        }
            //        if (!string.IsNullOrEmpty(extContent))
            //        {
            //            extContent = extContent.TrimEnd('|').TrimEnd('|');
            //        }
            //        itemEXTModel.extContent = extContent;
            //        listNursingExt.Add(itemEXTModel);
            //    }
            //    itemNModel.nursingExt = listNursingExt;

            //    listNursing.Add(itemNModel);
            //}
            //result.nursing = listNursing;
            //var nursingMessageBll = new NursingMessageBll();
            //var itemNursingMessage = nursingMessageBll.GetObjectByCustomerID(paramItem.welfareCentreId, itemCustomer.ID);
            //if (itemNursingMessage != null)
            //{
            //    result.callTime = itemNursingMessage.CallTime == null ? "" : itemNursingMessage.CallTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //else
            //{
            //    result.callTime = "";
            //}

            //result.resultCode = 1;
            //result.resultMessage = "操作成功.";
            //return result.ToJSON();

            return "";
        }
        #endregion


        #region   获取服务内容(九宫格)
        /// <summary>
        /// 获取服务内容(九宫格)
        /// </summary>
        /// <param name="bizContent"></param>
        /// <param name="timeStamp"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取服务内容(九宫格)")]
        public string getCareContent(string bizContent, long timeStamp, string signature)
        {
            MessageLog.WriteLog(new LogParameterModel
            {
                ClassName = this.GetType().ToString(),
                MethodName = "getCareContent",
                MethodParameters = $"bizContent:{bizContent},timeStamp:{timeStamp},signature:{signature}",
                LogLevel = ELogLevel.Info,
                Message = "接收参数",
                PathPrefix = "/log/ws",
                LogExt = "txt"
            });
            var result = new GetNursingVo();
            var paramItem = CommonLib.JsonHelper.Deserialize<GetNursingDto>(bizContent);
            if (paramItem == null)
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败:bizContent不合法.";
                return result.ToJSON();
            }
            var customerBll = new CustomerBll();
            var itemCustomer = customerBll.GetVijObjectById(paramItem.welfareCentreId, paramItem.bedNumber);
            if (itemCustomer == null)
            {
                result.resultCode = 0;
                result.resultMessage = "未获得信息.";
                return result.ToJSON();
            }
            var prModel = new personModel();
            prModel.userId = itemCustomer.ID;
            prModel.personName = itemCustomer.CustomerName;
            prModel.age = itemCustomer.CustomerAge ?? 0;
            prModel.gender = itemCustomer.CustomerGender == 1 ? "男" : "女";

            var bufRemark = "";
            var iteRemark = new PaymentPlanBll().GetById(itemCustomer.WelfareCentreID, itemCustomer.ID);
            if (iteRemark != null)
            {
                bufRemark = iteRemark.Remark;
            }
            prModel.remark = bufRemark.ToString();
            result.person = prModel;

            var listNursing = new List<nursingModel>();
            var fatherId = 1;
            var nursingRankBll = new NursingRankBll();
            nursingModel itemNModel;
            var listNurs = nursingRankBll.SearchListByWhole(fatherId, paramItem.welfareCentreId);
            foreach (var itemInfo in listNurs)
            {
                itemNModel = new nursingModel();
                itemNModel.nursingContent = itemInfo.RankContent;
                itemNModel.nursingId = itemInfo.ID;
                itemNModel.fatherId = itemInfo.ParentID ?? 0;
                itemNModel.nursingImgUrl = itemInfo.RankImgUrl;
                listNursing.Add(itemNModel);
            }
            result.nursing = listNursing;
            result.resultCode = 1;
            result.resultMessage = "操作成功.";
            return result.ToJSON();
        }
        #endregion


        #region   获取服务记录
        /// <summary>
        /// 获取服务记录
        /// </summary>
        /// <param name="bizContent"></param>
        /// <param name="timeStamp"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取服务记录")]
        public string getCareRecord(string bizContent, long timeStamp, string signature)
        {
            MessageLog.WriteLog(new LogParameterModel
            {
                ClassName = this.GetType().ToString(),
                MethodName = "getCareRecord",
                MethodParameters = $"bizContent:{bizContent},timeStamp:{timeStamp},signature:{signature}",
                LogLevel = ELogLevel.Info,
                Message = "接收参数",
                PathPrefix = "/log/ws",
                LogExt = "txt"
            });
            var result = new GetNursingPerLedgerVo();
            var paramItem = CommonLib.JsonHelper.Deserialize<GetNursingDto>(bizContent);
            if (paramItem == null)
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败:bizContent不合法.";
                return result.ToJSON();
            }
            var nursingPerLedgerBll = new NursingPerLedgerBll();
            var itemNursingPerLedger = nursingPerLedgerBll.GetById(paramItem.welfareCentreId);
            if (itemNursingPerLedger == null)
            {
                result.resultCode = 0;
                result.resultMessage = "未获得信息.";
                return result.ToJSON();
            }

            var listNursingPerLedger = new List<nursingPerLedgerModel>();
            nursingPerLedgerModel itemNModel;
            foreach (var itemInfo in itemNursingPerLedger)
            {
                itemNModel = new nursingPerLedgerModel();
                itemNModel.nursingContent = itemInfo.NursingContent;
                itemNModel.customerName = itemInfo.CustomerName;
                itemNModel.nursingName = itemInfo.RealName;
                itemNModel.nursingTime = itemInfo.ServerTime;
                itemNModel.nursingState = itemInfo.LedgerType.ToString() ?? "0";
                listNursingPerLedger.Add(itemNModel);
            }
            result.nursing = listNursingPerLedger;
            result.resultCode = 1;
            result.resultMessage = "操作成功.";
            return result.ToJSON();
        }
        #endregion


        #region   添加服务内容(九宫格)
        /// <summary>
        /// 添加服务内容
        /// </summary>
        /// <param name="bizContent"></param>
        /// <param name="timeStamp"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [WebMethod(Description = "添加服务内容")]
        public string setCareContent(string bizContent, long timeStamp, string signature)
        {
            MessageLog.WriteLog(new LogParameterModel
            {
                ClassName = this.GetType().ToString(),
                MethodName = "setCareContent",
                MethodParameters = $"bizContent:{bizContent},timeStamp:{timeStamp},signature:{signature}",
                LogLevel = ELogLevel.Info,
                Message = "接收参数",
                PathPrefix = "/log/ws",
                LogExt = "txt"
            });
            var result = new ResultModel();
            var paramItem = CommonLib.JsonHelper.Deserialize<SetNursingDto>(bizContent);
            if (paramItem == null)
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败:bizContent不合法.";
                return result.ToJSON();
            }

            var HgID = paramItem.userId;
            var customerBll = new CustomerBll();
            var itemCustomer = customerBll.GetVijObjectById(paramItem.welfareCentreId, paramItem.bedNumber);
            if (itemCustomer == null)
            {
                result.resultCode = 0;
                result.resultMessage = "未获得信息.";
                return result.ToJSON();
            }
            var arrayNursingIds = paramItem.nursingIds.Split(',');
            var list = new List<tbNursingPer>();
            tbNursingPer item;
            var nowTime = DateTime.Now;
            Guid NursingMessageID = Guid.NewGuid(); 
            foreach (var nursingId in arrayNursingIds)
            {
                if (!RegexValidate.IsInt(nursingId))
                {
                    continue;
                }
                item = new tbNursingPer();
                item.ID = Guid.NewGuid();
                item.CustomerID = itemCustomer.ID;
                item.NursingRankTopID = Convert.ToInt32(nursingId);
                item.NursingRankID = Convert.ToInt32(nursingId);
                item.HgID = HgID;
                item.OperatorUserID = HgID;
                item.NursingMessageID = NursingMessageID;
                item.NursingTime = nowTime;
                item.OperateDate = nowTime;
                item.CreateDate = nowTime;
                item.IsValid = 1;
                item.Remark = "cr";
                item.WelfareCentreID = paramItem.welfareCentreId;
                list.Add(item);
            }

            var listExt = new List<tbNursingPerExt>();
            tbNursingPerExt itemExt;
            var listNursingExt = JsonHelper.DeserializeToIList<nursingExtModel>(paramItem.nursingExt);
            if (listNursingExt != null)
            {
                foreach (var itemInfo in listNursingExt)
                {
                    itemExt = new tbNursingPerExt();
                    itemExt.ID = Guid.NewGuid();
                    itemExt.CustomerID = itemCustomer.ID;
                    itemExt.NursingRankTopID = itemInfo.nursingId;
                    itemExt.NursingRankID = itemInfo.nursingId;
                    itemExt.extTitle = itemInfo.extTitle;
                    itemExt.extContent = itemInfo.extContent;
                    itemExt.HgID = HgID;
                    itemExt.OperatorUserID = HgID;
                    itemExt.NursingTime = nowTime;
                    itemExt.NursingMessageID = NursingMessageID;
                    itemExt.OperateDate = nowTime;
                    itemExt.CreateDate = nowTime;
                    itemExt.IsValid = 1;
                    itemExt.Remark = "cr";
                    itemExt.WelfareCentreID = paramItem.welfareCentreId;
                    listExt.Add(itemExt);
                }
            }
            var cBll = new NursingPerBll();
            var isFlag = cBll.AddAll(paramItem.welfareCentreId, HgID, itemCustomer.ID, list, listExt);
             
            if (isFlag)
            {
                result.resultCode = 1;
                result.resultMessage = "操作成功.";
            }
            else
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败.";
            }
            return result.ToJSON();
        }
        #endregion


        #region   修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="bizContent"></param>
        /// <param name="timeStamp"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [WebMethod(Description = "修改密码")]
        public string setPassword(string bizContent, long timeStamp, string signature)
        {
            MessageLog.WriteLog(new LogParameterModel
            {
                ClassName = this.GetType().ToString(),
                MethodName = "setPassword",
                MethodParameters = $"bizContent:{bizContent},timeStamp:{timeStamp},signature:{signature}",
                LogLevel = ELogLevel.Info,
                Message = "接收参数",
                PathPrefix = "/log/ws",
                LogExt = "txt"
            });

            var result = new ResultModel();
            var paramItem = CommonLib.JsonHelper.Deserialize<ChangePasswordDto>(bizContent);
            if (paramItem == null)
            {
                result.resultCode = 0;
                result.resultMessage = "操作失败:bizContent不合法.";
                return result.ToJSON();
            }

            var cBll = new UsersBll();
            var item = cBll.LoginUsers(paramItem.userId, paramItem.oldPassword);
            if (item != null)
            {
                var isFlag = cBll.ChangePassword(paramItem.welfareCentreId, paramItem.userId, paramItem.newPassword);
                if (isFlag)
                {
                    result.resultCode = 1;
                    result.resultMessage = "修改密码成功.";
                }
                else
                {
                    result.resultCode = 0;
                    result.resultMessage = "操作失败.";
                }
            }
            else
            {
                result.resultCode = 0;
                result.resultMessage = "旧密码错误.";
            }
            return result.ToJSON();
        }
        #endregion


    }
}
