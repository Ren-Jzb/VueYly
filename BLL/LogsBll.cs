using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Model;
using DAL;

namespace BLL
{
    public class LogsBll
    {
        #region   功能节点
        public string ReturnNodeName(int NodeId)
        {
            var NodeName = "";
            var szServices = new DbHelperEfSql<Nodes>();
            var item = szServices.SearchBySingle(c => c.NodeId == NodeId);
            if (item != null)
            {
                NodeName = item.NodeName;
            }
            return NodeName;
        }
        #endregion


        #region   用户操作日志
        public void Log(string tableID, string className, string paramState, string userID, string roleName, string welfareCentreID, string clientIP)
        {
            int pState = 0;
            if (paramState != "")
            {
                pState = Convert.ToInt32(paramState);
            }
            #region   Log4net对象
            var item = new Log4net();
            item.LogID = Guid.NewGuid();
            item.log_date = DateTime.Now;
            item.log_WelfareCentreID = new Guid(welfareCentreID);
            item.log_TableID = tableID;
            item.log_UserID = new Guid(userID);
            item.log_UserName = roleName;
            item.log_State = paramState;
            switch (pState)
            {
                case 1:
                    item.log_StateName = "添加操作.";
                    break;
                case 2:
                    item.log_StateName = "修改操作.";
                    break;
                case 3:
                    item.log_StateName = "逻辑操作.";
                    break;
                case 4:
                    item.log_StateName = "物理操作.";
                    break;
                case 5:
                    item.log_StateName = "启用操作.";
                    break;
                case 6:
                    item.log_StateName = "禁用操作.";
                    break;
                default:
                    item.log_StateName = "操作.";
                    break;
            }
            item.log_ClassName = className;
            item.log_logger = clientIP;
            item.log_message = "参数-表ID：" + tableID + "用户ID:" + userID + "用户名称：" + roleName + "操作日期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "养老院ID：" + welfareCentreID;


            bool IsFlag = AddLog4net(true, item);
            if (!IsFlag)
            {
                MessageLog.WriteLog(new LogParameterModel
                {
                    ClassName = className,
                    MethodName = paramState,
                    MethodParameters = "添加日志操作.",
                    LogLevel = ELogLevel.Info,
                    Message = "参数-表ID：" + tableID + "用户ID:" + userID + "用户名称：" + roleName + "操作日期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "养老院ID：" + welfareCentreID,
                    PathPrefix = "/log/ws/logs",
                    LogExt = "txt"
                });
            }
            #endregion
        }
        #endregion


        #region   插入数据
        public bool AddLog4net(bool IsAdd, Log4net item)
        {
            var szServices = new DbHelperEfSql<Log4net>();
            return IsAdd ? szServices.Add(item) : szServices.Update(item, c => c.LogID == item.LogID);
        }
        #endregion


        #region   查询日志记录
        public IList<Log4net> SearchChargeSituationInfoByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szServices = new DbHelperEfSql<Log4net>();
            return szServices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }
        #endregion

    }
}
