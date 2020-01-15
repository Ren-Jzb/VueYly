using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
   public class NursingRankBll
    {
        public IList<tbNursingRank> SearchListByParentId(Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<tbNursingRank>();
            return szServices.SearchListByCondition(c => c.ParentID == 0 && c.IsValid == 1 && c.WelfareCentreID == welfareCentreId, true, c => c.OperateDate);
        }

        /// <summary>
        /// 获取所有子级护理内容
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList<tbNursingRank> SearchListByWhole(int parentId, Guid welfareCentreId)
        {
            IList<tbNursingRank> resultList = new List<tbNursingRank>();
            SearchListByWhole(parentId, welfareCentreId, resultList, null);
            return resultList;
        }

        /// <summary>
        /// 递归获取子项内容
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="resultList"></param>
        /// <param name="list"></param>
        private void SearchListByWhole(int parentId, Guid? welfareCentreId, IList<tbNursingRank> resultList, IList<tbNursingRank> list)
        {
            var szServices = new DbHelperEfSql<tbNursingRank>();
            list = szServices.SearchListByCondition(c => c.ParentID == parentId && c.WelfareCentreID == welfareCentreId && c.IsValid == 1, true, c => c.OrderIndex);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    resultList.Add(item);
                    SearchListByWhole(item.ID, item.WelfareCentreID, resultList, list);
                }
            }
        }

    }
}
