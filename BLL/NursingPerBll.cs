using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using DAL;
using Model;
using VO;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    public class NursingPerBll
    {

        #region   护工护理内容
        /// <summary>
        /// 护工护理内容
        /// </summary>
        /// <param name="welfareCentreId">养老院ID</param>
        /// <param name="HgID">护工ID</param>
        /// <param name="CustomerID">老人ID</param>
        /// <param name="list">护理内容</param>
        /// <param name="listExt">护理内容</param>
        /// <returns></returns>
        public bool AddAll(Guid welfareCentreId, Guid HgID, Guid CustomerID, IList<tbNursingPer> list, IList<tbNursingPerExt> listExt)
        {
            if (list == null || list.Count <= 0)
                return false;
            var szNmDao = new DbHelperEfSql<tbNursingMessage>();
            var nowTime = DateTime.Now;
            Guid NursingMessageId = Guid.Empty;//操作记录
            NursingMessageId = Guid.NewGuid();

            var szServices = new DbHelperEfSql<tbNursingPer>();
            var resultList = new List<tbNursingPer>();

            //tbNursingPer
            foreach (var item in list)
            {
                int resultId = 0;
                DoExtByWhole(welfareCentreId, item.NursingRankID, ref resultId);//处理list中NursingRankTopID值
                item.NursingRankTopID = resultId;
                item.NursingMessageID = NursingMessageId;
                resultList.Add(item);
            }
            bool isFlagA = szServices.AddAll(resultList);

            //tbNursingPerExt
            var szServiceExt = new DbHelperEfSql<tbNursingPerExt>();
            var resultExtList = new List<tbNursingPerExt>();
            foreach (var itemExt in listExt)
            {
                int resultId = 0;
                DoExtByWhole(welfareCentreId, itemExt.NursingRankID, ref resultId);//处理listExt中NursingRankTopID值
                itemExt.NursingRankTopID = resultId;
                itemExt.NursingMessageID = NursingMessageId;
                resultExtList.Add(itemExt);
            }
            bool isFlagB = szServiceExt.AddAll(resultExtList);

            //tbNursingPerLedger
            var szServiceLedger = new DbHelperEfSql<tbNursingPerLedger>();
            var itemLedger = new tbNursingPerLedger();
            itemLedger.ID = Guid.NewGuid();
            itemLedger.CustomerID = list[0].CustomerID;
            itemLedger.HgID = list[0].HgID;
            itemLedger.OperatorUserID = itemLedger.HgID;
            itemLedger.PaymentPlanId = list[0].NursingMessageID;
            itemLedger.NursingTimeStart = list[0].NursingTime;
            itemLedger.NursingTimeEnd = list[0].NursingTime;
            string content = "";
            DoNurseByWhole(welfareCentreId, list[0].CustomerID ?? Guid.Empty, list[0].HgID ?? Guid.Empty, list[0].NursingMessageID ?? Guid.Empty, ref content);   //护理NursingMessageID不变,导致数据重复问题 (2019-6-11)
            itemLedger.NursingContent = content;
            itemLedger.IsValid = 1;
            itemLedger.Remark = "";
            itemLedger.LedgerType = 1;
            itemLedger.CreateDate = list[0].CreateDate;
            itemLedger.OperateDate = list[0].OperateDate;
            itemLedger.WelfareCentreID = welfareCentreId;
            bool isFlagC = szServiceLedger.Add(itemLedger);

            return isFlagA && isFlagB && isFlagC;
        }
        #endregion


        /// <summary>
        ///  递归获取护理内容项顶级id
        ///  修改方法：添加参数养老院id--welfareCentreId，防止多家养老院是取值重复 2019-02-28 林中枝修改
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="resultId"></param>
        private void DoExtByWhole(Guid? welfareCentreId, int? parentId, ref int resultId)
        {
            var szServices = new DbHelperEfSql<tbNursingRank>();
            var item = szServices.SearchBySingle(c => c.ID == parentId && c.WelfareCentreID == welfareCentreId);
            if (item == null)
                return;
            var listK = szServices.SearchListByCondition(c => c.ParentID == 0 && c.WelfareCentreID == welfareCentreId && c.IsValid == 1);
            if (listK == null || listK.Count <= 0)
                return;
            if (listK.Count(c => c.ID == item.ParentID && c.WelfareCentreID == welfareCentreId) > 0)
            {
                resultId = item.ID;
            }
            else
            {
                DoExtByWhole(item.WelfareCentreID, item.ParentID, ref resultId);
            }
        }
        private void DoNurseByWhole(Guid welfareCentreId, Guid CustomerID, Guid HgID, Guid NursingMessageID, ref string content)
        {
            var szServices = new DbHelperEfSql<tbNursingPer>();
            var list = szServices.SearchListByCondition(c => c.CustomerID == CustomerID && c.HgID == HgID && c.NursingMessageID == NursingMessageID && c.WelfareCentreID == welfareCentreId && c.IsValid == 1);
            if (list == null || list.Count <= 0)
                return;
            var arrayRank = new List<int?>();
            foreach (var item in list)
            {
                arrayRank.Add(item.NursingRankID);
            }
            var szRank = new DbHelperEfSql<tbNursingRank>();
            var source = szRank.SearchListByCondition(c => arrayRank.Contains(c.ID) && c.WelfareCentreID == welfareCentreId);
            if (source == null)
                source = new List<tbNursingRank>();

            var szServiceExt = new DbHelperEfSql<tbNursingPerExt>();
            var sourceExt = szServiceExt.SearchListByCondition(c => c.CustomerID == CustomerID && c.HgID == HgID && c.NursingMessageID == NursingMessageID && c.WelfareCentreID == welfareCentreId && c.IsValid == 1);
            if (sourceExt == null)
                sourceExt = new List<tbNursingPerExt>();


            DoFactNurseByWhole(welfareCentreId, source, sourceExt, ref content);
        }

        private void DoFactNurseByWhole(Guid? welfareCentreId, IList<tbNursingRank> listRank, IList<tbNursingPerExt> listNursingPerExt, ref string content)
        {
            //顶级护理项
            //var source = listRank.Where(c => c.WelfareCentreID == welfareCentreId && c.ParentID >= 1 && c.ParentID <= 10).ToList();
            var source = listRank.Where(c => c.WelfareCentreID == welfareCentreId && c.ParentID == 1).ToList();
            foreach (var item in source)
            {
                var sourceRank = listRank.Where(c => c.WelfareCentreID == welfareCentreId && c.ParentID == item.ID).ToList();
                if (sourceRank.Count > 0) //表示子项
                {
                    string s0 = "";
                    foreach (var itemRank in sourceRank)
                    {
                        var sourceRank2 = listRank.Where(c => c.WelfareCentreID == welfareCentreId && c.ParentID == itemRank.ID).ToList();
                        if (sourceRank2.Count > 0)
                        {
                            string s1 = "";
                            foreach (var itemRank2 in sourceRank2)
                            {
                                s1 += itemRank2.RankContent + ",";
                            }
                            if (!string.IsNullOrEmpty(s1))
                            {
                                s1 = s1.TrimEnd(',');
                            }
                            s0 += itemRank.RankContent + "[" + s1 + "],";
                        }
                        else
                        {
                            s0 += itemRank.RankContent;
                            var sourceRankExt = listNursingPerExt.Where(c => c.WelfareCentreID == welfareCentreId && c.NursingRankID == itemRank.ID).ToList();
                            if (sourceRankExt.Count > 0) //表示有扩展项
                            {
                                string contentExt = "";
                                DoFactExtNurseByWhole(welfareCentreId, itemRank.ID, listNursingPerExt, ref contentExt);
                                s0 += contentExt;
                            }
                            s0 += ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(s0))
                    {
                        s0 = s0.TrimEnd(',');
                    }
                    content += item.RankContent + "[" + s0 + "],";
                }
                else
                {
                    content += item.RankContent;
                    var sourceRankExt = listNursingPerExt.Where(c => c.WelfareCentreID == welfareCentreId && c.NursingRankID == item.ID).ToList();
                    if (sourceRankExt.Count > 0) //表示有扩展项
                    {
                        string contentExt = "";
                        DoFactExtNurseByWhole(welfareCentreId, item.ID, listNursingPerExt, ref contentExt);
                        content += contentExt;
                    }
                    content += ",";
                }
            }
            if (!string.IsNullOrEmpty(content))
            {
                content = content.TrimEnd(',');
            }
        }

        /// <summary>
        /// 拼接扩展项内容
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="listNursingPerExt"></param>
        /// <param name="contentExt"></param>
        private void DoFactExtNurseByWhole(Guid? welfareCentreId, int? pid, IList<tbNursingPerExt> listNursingPerExt, ref string contentExt)
        {
            var sourceExt = listNursingPerExt.Where(c => c.WelfareCentreID == welfareCentreId && c.NursingRankID == pid).ToList();
            foreach (var itemExt in sourceExt)
            {
                contentExt += itemExt.extContent + ",";
            }
            if (!string.IsNullOrEmpty(contentExt))
            {
                contentExt = contentExt.TrimEnd(',');
            }
            contentExt = "(" + contentExt + ")";
        }

    }
}
