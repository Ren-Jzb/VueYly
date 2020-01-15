using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Model;

namespace DAL
{
    public class PaymentPlanDao : BaseServiceEf<MISDBContainer>
    {
        public bool AddOrUpdae(tbPaymentPlan item)
        {
            try
            {
                var isAdd = true;

                var id = item.ID;
                if (id == Guid.Empty)
                {
                    id = Guid.NewGuid();
                }
                #region
                var query = _context.Set<tbPaymentPlan>().FirstOrDefault(c => c.ID == id);
                if (query == null)
                {
                    isAdd = true;
                    var model = new tbPaymentPlan();
                    model.ID = id;
                    model.IsValid = 1;
                    model.CreateDate = DateTime.Now;
                    model.CustomerId = item.CustomerId;
                    model.HgId = item.HgId;
                    model.ServerStart = item.ServerStart;
                    model.ServerEnd = item.ServerEnd;
                    model.WeekOne = item.WeekOne;
                    model.WeekTwo = item.WeekTwo;
                    model.WeekThree = item.WeekThree;
                    model.WeekFour = item.WeekFour;
                    model.WeekFive = item.WeekFive;
                    model.WeekSix = item.WeekSix;
                    model.WeekServer = item.WeekServer;
                    model.ServerTime = item.ServerTime;
                    model.Remark = item.Remark;
                    model.OperateDate = item.OperateDate;
                    model.OperatorUserID = item.OperatorUserID;
                    model.WelfareCentreID = item.WelfareCentreID;

                    _context.Set<tbPaymentPlan>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.ID = id;
                    query.IsValid = 1;
                    query.CustomerId = item.CustomerId;
                    query.HgId = item.HgId;
                    query.ServerStart = item.ServerStart;
                    query.ServerEnd = item.ServerEnd;
                    query.WeekOne = item.WeekOne;
                    query.WeekTwo = item.WeekTwo;
                    query.WeekThree = item.WeekThree;
                    query.WeekFour = item.WeekFour;
                    query.WeekFive = item.WeekFive;
                    query.WeekSix = item.WeekSix;
                    query.WeekServer = item.WeekServer;
                    query.ServerTime = item.ServerTime;
                    query.Remark = item.Remark;
                    query.OperateDate = item.OperateDate;
                    query.OperatorUserID = item.OperatorUserID;
                    query.WelfareCentreID = item.WelfareCentreID;
                }
                #endregion

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "AddOrUpdaeCustomer",
                    MethodParameters = "添加失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }

        public bool PhysicalDelete(Guid customerId, Guid bedId, Guid welfareCentreId)
        {
            try
            {
                var query = _context.Set<tbPaymentPlan>().FirstOrDefault(c => c.ID == customerId && c.WelfareCentreID == welfareCentreId);
                if (query != null)
                {
                    _context.Set<tbPaymentPlan>().Remove(query);
                }
                var queryBed = _context.Set<tbBed>().FirstOrDefault(c => c.ID == bedId && c.WelfareCentreID == welfareCentreId);
                if (queryBed != null)
                {
                    queryBed.ID = bedId;
                    queryBed.BedStatus = 0;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "PhysicalDeleteCustomer",
                    MethodParameters = "彻底删除失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }

        }

        public bool OperateDataStatus(Guid customerId, Guid bedId, Guid welfareCentreId, int isValid)
        {
            try
            {
                var query = _context.Set<tbPaymentPlan>().FirstOrDefault(c => c.ID == customerId && c.WelfareCentreID == welfareCentreId);
                if (query != null)
                {
                    query.ID = customerId;
                    query.OperateDate = DateTime.Now;
                    query.IsValid = isValid;
                }
                var queryBed = _context.Set<tbBed>().FirstOrDefault(c => c.ID == bedId && c.WelfareCentreID == welfareCentreId);
                if (queryBed != null)
                {
                    queryBed.ID = bedId;
                    queryBed.OperateDate = DateTime.Now;
                    queryBed.BedStatus = 0;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "OperateDataStatusCustomer",
                    MethodParameters = "逻辑删除失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }


        }

    }
}
