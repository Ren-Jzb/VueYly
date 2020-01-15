using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using CommonLib;

namespace BLL
{
    public class CustomerBll
    {
        public bool AddOrUpdate(bool IsAdd, tbCustomer item)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return IsAdd ? szServices.Add(item) : szServices.Update(item, c => c.ID == item.ID);
        }

        public IList<tbCustomer> SearchListByAuto1(string keyword, int limit, Guid welfareCentreIds)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            var iTotalRecord = 0;
            if (!string.IsNullOrEmpty(keyword))
            {
                return szServices.SearchByPageCondition(c => c.CustomerName.Contains(keyword) || c.CustomerCardID.Contains(keyword) && c.IsValid == 1 && c.WelfareCentreID == welfareCentreIds, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
            else
            {
                return szServices.SearchByPageCondition(c => c.IsValid == 1 && c.WelfareCentreID == welfareCentreIds && c.IsValid == 1, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
        }
        public bool AddOrUpdate(tbCustomer item, Guid oldBedId)
        {
            var dao = new CustomerDao();
            return dao.AddOrUpdae(item, oldBedId);
        }
        public bool AddOrUpdate(tbCustomer item)
        {
            var dao = new CustomerDao();
            return dao.AddOrUpdae(item);
        }
        public bool UpdateCustomerHeadImg(Guid sid, Guid welfareCentreIds, string headImg)
        {
            if (sid == null || sid == Guid.Empty) return false;
            string sql = string.Format("update tbCustomer set CustomerHeadImg='{0}',OperateDate=getdate() where id in('{1}')", headImg, sid);

            var isFlag = DbHelperSql.ExecuteNonQuery(sql) > 0;
            return isFlag;
        }
        public IList<tbCustomer> SearchAll()
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.SearchByAll();
        }
        public IList<tbCustomer> SearchAllByIsValid()
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.SearchListByCondition(c => c.IsValid == 1);
        }
        public IList<tbCustomer> SearchAllByIsValid(Guid welfareCentreId)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.SearchListByCondition(c => c.IsValid == 1 && c.WelfareCentreID == welfareCentreId);
        }
        public int LaorenCount(Guid welfareCentreId)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.WelfareCentreID == welfareCentreId);
        }
        public int SearchOldManByBed(int hlmc)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerHLMC_dic == hlmc);
        }
        public int SearchOldManByBed(int hlmc, Guid welfareCentreId)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerHLMC_dic == hlmc && c.WelfareCentreID == welfareCentreId);
        }
        public int SearchOldManByYcType(int yslx)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerYSLX_dic == yslx);
        }
        public int SearchByOldManType(int iType)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerType_dic == iType);
        }
        public int SearchOldManBySex(int hlmc)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerGender == hlmc);
        }

        public int SearchByCustomerHJDQ_dic(string customerHJDQ, Guid welfareCentreId)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerHJDQ_dic == customerHJDQ && c.WelfareCentreID == welfareCentreId);
        }
        public int SearchByCustomerHJDQ_dic(string customerHJDQ1, string customerHJDQ2)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerHJDQ_dic != customerHJDQ1 && c.CustomerHJDQ_dic != customerHJDQ2);
        }
        public int SearchByCustomerAge(int iAge1, int iAge2)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.Count(c => c.IsValid == 1 && c.BedID != null && c.CustomerAge >= iAge1 && c.CustomerAge <= iAge2);
        }
        public double SearchByCustomerAgeAverage()
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            var list = szSrevices.SearchListByCondition(c => c.IsValid == 1 && c.BedID != null);
            return list.Average(c => c.CustomerAge) ?? 0;
        }
        public tbCustomer SearchByMaxAge()
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.SearchBySingle(c => c.IsValid == 1, false, c => c.CustomerAge);
        }
        public IList<tbCustomer> SearchLaorenByZdzl(Guid welfareCentreId)
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.SearchListByCondition(c => c.IsValid == 1 && c.Describe == "重点照料" && c.WelfareCentreID == welfareCentreId);
        }
        public IList<vCustomer> SearchVByPageCondition(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szSrevices = new DbHelperEfSql<vCustomer>();
            return szSrevices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }
        public tbCustomer GetObjectById(Guid Id)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return szServices.SearchBySingle(c => c.ID == Id);
        }
        public vCustomer GetVObjectById(Guid Id)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            return szServices.SearchBySingle(c => c.ID == Id);
        }
        public bool PhysicalDeleteByCondition(string[] ids)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return szServices.PhysicalDeleteByCondition(c => ids.Contains(c.ID.ToString()));
        }
        public bool PhysicalDeleteByCondition(Guid id)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return szServices.PhysicalDeleteByCondition(c => c.ID == id);
        }
        public bool LogicDeleteByCondition(string[] ids, Guid welfareCentreId)
        {
            return OperateDataStatus(ids, ESystemStatus.Deleted, welfareCentreId);
        }
        public bool EnableByCodeition(string[] ids, Guid welfareCentreId)
        {
            return OperateDataStatus(ids, ESystemStatus.Valid, welfareCentreId);
        }
        public bool DisableByCodeition(string[] ids, Guid welfareCentreId)
        {
            return OperateDataStatus(ids, ESystemStatus.Forbidden, welfareCentreId);
        }
        public bool OperateDataStatus(string[] ids, ESystemStatus eSystemStatus, Guid welfareCentreId)
        {
            if (ids == null || ids.Length <= 0) return false;
            var buf = new System.Text.StringBuilder();
            foreach (var item in ids)
            {
                buf.AppendFormat("'{0}',", item);
            }
            var szIds = buf.ToString().TrimEnd(',');
            string sql = string.Format("update tbCustomer set IsValid={0},OperateDate=getdate() where id in({1})", (int)eSystemStatus, szIds);

            var isFlag = DbHelperSql.ExecuteNonQuery(sql) > 0;
            return isFlag;
        }

        public IList<vCustomer> SearchListByAuto(string keyword, int limit, Guid wId)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            var iTotalRecord = 0;
            if (!string.IsNullOrEmpty(keyword))
            {
                return szServices.SearchByPageCondition(c => c.CustomerName.Contains(keyword) && c.IsValid == 1 && c.WelfareCentreID == wId, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
            else
            {
                return szServices.SearchByPageCondition(c => c.IsValid == 1 && c.WelfareCentreID == wId, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
        }
        public IList<vCustomer> SearchListByAuto(string keyword, int limit, int isStage)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            var iTotalRecord = 0;
            if (!string.IsNullOrEmpty(keyword))
            {
                return szServices.SearchByPageCondition(c => c.CustomerName.Contains(keyword) && c.Stage == isStage && c.IsValid == 1, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
            else
            {
                return szServices.SearchByPageCondition(c => c.Stage == isStage && c.IsValid == 1, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
        }
        public bool ValidationBedByCustomer(Guid bedId, Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return szServices.Count(c => c.BedID == bedId && c.IsValid == 1 && c.WelfareCentreID == welfareCentreId) <= 0;
        }
        public bool ValidationBedByCustomer(Guid customerId, Guid bedId, Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return szServices.Count(c => c.BedID == bedId && c.ID != customerId && c.IsValid == 1 && c.WelfareCentreID == welfareCentreId) <= 0;
        }

        public IList<tbDictionary> SearchListByType(int iType)
        {
            var szService = new DbHelperEfSql<tbDictionary>();
            return szService.SearchListByCondition(c => c.DictionaryID == iType && c.IsValid == 1);
        }

        public bool UpdateCustomerNursType(Guid id, int nursType)
        {
            return OperateNursType(id, nursType);
        }
        public bool OperateNursType(Guid id, int nursType)
        {
            string sql = string.Format("update tbCustomer set CustomerHLMC_dic={0},OperateDate=getdate() where id in('{1}')", nursType, id);
            return DbHelperSql.ExecuteNonQuery(sql) > 0;
        }

        /// <summary>
        /// 入院老人
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<vCustomer> GetCustomerByCreateDate(Guid welfareCentreId, DateTime startDate, DateTime endDate)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            return szServices.SearchListByCondition(c => c.WelfareCentreID == welfareCentreId && c.CreateDate >= startDate && c.CreateDate < endDate && c.IsValid == 1 && c.BedID != null, false, c => c.CreateDate);
        }
        /// <summary>
        /// 离院老人
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<vCustomer> GetCustomerByLiyuan(Guid welfareCentreId, DateTime startDate, DateTime endDate)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            return szServices.SearchListByCondition(c => c.WelfareCentreID == welfareCentreId && c.OperateDate >= startDate && c.OperateDate < endDate && c.IsValid != 1, false, c => c.OperateDate);
        }

        public IList<vCustomer> SearchListByAutoName(string keyword, int limit, Guid wId)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            var iTotalRecord = 0;
            if (!string.IsNullOrEmpty(keyword))
            {
                return szServices.SearchByPageCondition(c => c.CustomerName.Contains(keyword) && c.IsValid == 1 && c.WelfareCentreID == wId, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
            else
            {
                return szServices.SearchByPageCondition(c => c.IsValid == 1 && c.WelfareCentreID == wId, true, c => c.CustomerName, 0, limit, ref iTotalRecord);
            }
        }

        public IList<tbCustomer> SearchAllOrderByName()
        {
            var szSrevices = new DbHelperEfSql<tbCustomer>();
            return szSrevices.SearchListByCondition(c => c.IsValid == 1, true, c => c.CustomerXMPY);
        }
        public bool PhysicalDelete(Guid customerId, Guid bedId, Guid welfareCentreId)
        {
            var dao = new CustomerDao();
            return dao.PhysicalDelete(customerId, bedId, welfareCentreId);
        }
        public bool OperateDataStatus(Guid customerId, Guid bedId, Guid welfareCentreId, int isValid)
        {
            var dao = new CustomerDao();
            return dao.OperateDataStatus(customerId, bedId, welfareCentreId, isValid);
        }

        public IList<vCustomer> GetCustomerByInfo(int iPageIndex, int iPageSize, ref int iTotalRecord, ConditionModel conditionModel)
        {
            var szSrevices = new DbHelperEfSql<vCustomer>();
            return szSrevices.SearchByPageCondition(iPageIndex, iPageSize, ref iTotalRecord, conditionModel);
        }

        public IList<vCustomer> SearchByCondition(ConditionModel conditionModel)
        {
            var szSrevices = new DbHelperEfSql<vCustomer>();
            return szSrevices.SearchByCondition(conditionModel);
        }

        public vCustomer GetVijObjectById(Guid welfareCentreId, string BEDNO)
        {
            var szServices = new DbHelperEfSql<vCustomer>();
            return szServices.SearchBySingle(c => c.WelfareCentreID == welfareCentreId && c.BedNumber == BEDNO && c.IsValid == 1);
        }

        public IList<tbCustomer> SearchListByValid(Guid welfareCentreId)
        {
            var szServices = new DbHelperEfSql<tbCustomer>();
            return szServices.SearchListByCondition(c => c.IsValid == 1 && c.WelfareCentreID == welfareCentreId);
        }

    }
}
