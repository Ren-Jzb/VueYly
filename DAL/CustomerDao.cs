using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Model;

namespace DAL
{
    public class CustomerDao : BaseServiceEf<MISDBContainer>
    {
        public bool AddOrUpdae(tbCustomer item, Guid oldBedId)
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
                var query = _context.Set<tbCustomer>().FirstOrDefault(c => c.ID == id);
                if (query == null)
                {
                    isAdd = true;
                    var model = new tbCustomer();
                    model.ID = id;
                    model.IsValid = 1;
                    model.CreateDate = DateTime.Now;
                    model.Stage = item.Stage;
                    model.OperateDate = item.OperateDate;
                    model.CustomerName = item.CustomerName;
                    model.BedNumber = item.BedNumber;
                    model.BedID = item.BedID;
                    model.CustomerGender = item.CustomerGender;
                    model.CustomerWedlock = item.CustomerWedlock;
                    model.CustomerAge = item.CustomerAge;
                    model.CustomerJG = item.CustomerJG;
                    model.CustomerAddress = item.CustomerAddress;
                    model.CustomerCompany = item.CustomerCompany;
                    model.CustomerTel = item.CustomerTel;
                    model.CustomerGSR = item.CustomerGSR;
                    model.AdmissionDate = item.AdmissionDate ?? DateTime.Now;
                    model.DiagnosticDate = item.DiagnosticDate ?? DateTime.Now;

                    model.CustomerCardID = item.CustomerCardID;
                    model.CustomerType_dic = item.CustomerType_dic;
                    model.CustomerSBKH = item.CustomerSBKH;
                    model.CustomerMZ = item.CustomerMZ;
                    model.CustomerWHCD_dic = item.CustomerWHCD_dic;
                    model.CustomerZW = item.CustomerZW;
                    model.CustomerYLJSR = item.CustomerYLJSR;
                    model.CustomerZZDH = item.CustomerZZDH;
                    model.CustomerYYZT = item.CustomerYYZT;
                    model.CustomerSFBS = item.CustomerSFBS;
                    model.CustomerSX = item.CustomerSX;
                    model.CustomerXMPY = item.CustomerXMPY;//姓名拼音
                    model.CustomerZYBH = item.CustomerZYBH;
                    model.CustomerYYSJ = item.CustomerYYSJ;
                    model.CustomerLYSJ = item.CustomerLYSJ;
                    model.CustomerLYYY = item.CustomerLYYY;
                    model.CustomerYLBZ = item.CustomerYLBZ;
                    model.CustomerJJYW = item.CustomerJJYW;
                    model.CustomerJJSW = item.CustomerJJSW;
                    model.CustomerYSLX_dic = item.CustomerYSLX_dic;
                    model.CustomerYYBH = item.CustomerYYBH;
                    model.CustomerSFWWYY = item.CustomerSFWWYY;
                    model.CustomerHLMC_dic = item.CustomerHLMC_dic;
                    model.CustomerBrith = item.CustomerBrith;
                    model.BedNumberName = item.BedNumberName;
                    model.CustomerHeadImg = item.CustomerHeadImg;

                    model.CustomerZZMM = item.CustomerZZMM;
                    model.CustomerJYCD = item.CustomerJYCD;
                    model.CustomerDlrXm = item.CustomerDlrXm;
                    model.CustomerDlrTel = item.CustomerDlrTel;
                    model.CustomerDlrDz = item.CustomerDlrDz;
                    model.CustomerDlrGx = item.CustomerDlrGx;
                    model.CustomerDlrYb = item.CustomerDlrYb;
                    model.CustomerHJDQ_dic = item.CustomerHJDQ_dic;
                    model.WelfareCentreID = item.WelfareCentreID;

                    _context.Set<tbCustomer>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.ID = id;
                    query.Stage = item.Stage;
                    query.OperateDate = item.OperateDate;
                    query.CustomerName = item.CustomerName;
                    query.BedNumber = item.BedNumber;
                    query.BedID = item.BedID;
                    query.CustomerGender = item.CustomerGender;
                    query.CustomerWedlock = item.CustomerWedlock;
                    query.CustomerAge = item.CustomerAge;
                    query.CustomerJG = item.CustomerJG;
                    query.CustomerAddress = item.CustomerAddress;
                    query.CustomerCompany = item.CustomerCompany;
                    query.CustomerTel = item.CustomerTel;
                    query.CustomerGSR = item.CustomerGSR;
                    query.AdmissionDate = item.AdmissionDate ?? DateTime.Now;
                    query.DiagnosticDate = item.DiagnosticDate ?? DateTime.Now;

                    query.CustomerCardID = item.CustomerCardID;
                    query.CustomerType_dic = item.CustomerType_dic;
                    query.CustomerSBKH = item.CustomerSBKH;
                    query.CustomerMZ = item.CustomerMZ;
                    query.CustomerWHCD_dic = item.CustomerWHCD_dic;
                    query.CustomerZW = item.CustomerZW;
                    query.CustomerYLJSR = item.CustomerYLJSR;
                    query.CustomerZZDH = item.CustomerZZDH;
                    query.CustomerYYZT = item.CustomerYYZT;
                    query.CustomerSFBS = item.CustomerSFBS;
                    query.CustomerSX = item.CustomerSX;
                    query.CustomerXMPY = item.CustomerXMPY;//姓名拼音
                    query.CustomerZYBH = item.CustomerZYBH;
                    query.CustomerYYSJ = item.CustomerYYSJ;
                    query.CustomerLYSJ = item.CustomerLYSJ;
                    query.CustomerLYYY = item.CustomerLYYY;
                    query.CustomerYLBZ = item.CustomerYLBZ;
                    query.CustomerJJYW = item.CustomerJJYW;
                    query.CustomerJJSW = item.CustomerJJSW;
                    query.CustomerYSLX_dic = item.CustomerYSLX_dic;
                    query.CustomerYYBH = item.CustomerYYBH;
                    query.CustomerSFWWYY = item.CustomerSFWWYY;
                    query.CustomerHLMC_dic = item.CustomerHLMC_dic;
                    query.CustomerBrith = item.CustomerBrith;
                    query.BedNumberName = item.BedNumberName;
                    query.CustomerHeadImg = item.CustomerHeadImg;

                    query.CustomerZZMM = item.CustomerZZMM;
                    query.CustomerJYCD = item.CustomerJYCD;
                    query.CustomerDlrXm = item.CustomerDlrXm;
                    query.CustomerDlrTel = item.CustomerDlrTel;
                    query.CustomerDlrDz = item.CustomerDlrDz;
                    query.CustomerDlrGx = item.CustomerDlrGx;
                    query.CustomerDlrYb = item.CustomerDlrYb;
                    query.CustomerHJDQ_dic = item.CustomerHJDQ_dic;
                    query.WelfareCentreID = item.WelfareCentreID;
                }
                #endregion

                #region tbBed
                //BedStatus 1 有人住，0 没人住
                if (item.BedID != oldBedId)
                {
                    if (item.BedID != Guid.Empty)
                    {
                        var queryBed = _context.Set<tbBed>().FirstOrDefault(c => c.ID == item.BedID);
                        if (queryBed != null)
                        {
                            queryBed.ID = item.BedID ?? Guid.Empty;
                            queryBed.BedStatus = 1;
                        }
                    }
                    if (oldBedId != Guid.Empty)
                    {
                        var queryBed2 = _context.Set<tbBed>().FirstOrDefault(c => c.ID == oldBedId);
                        if (queryBed2 != null)
                        {
                            queryBed2.ID = oldBedId;
                            queryBed2.BedStatus = 0;
                        }
                    }
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

        public bool AddOrUpdae(tbCustomer item)
        {
            try
            {
                var isAdd = true;

                #region
                var query = _context.Set<tbCustomer>().FirstOrDefault(c => c.ID == item.ID);
                if (query == null)
                {
                    isAdd = true;
                    var model = new tbCustomer();
                    model.ID = Guid.NewGuid();
                    model.IsValid = 1;
                    model.CreateDate = DateTime.Now;
                    model.Stage = item.Stage;
                    model.OperateDate = item.OperateDate;
                    model.CustomerName = item.CustomerName;
                    model.BedNumber = item.BedNumber;
                    model.BedID = item.BedID;
                    model.CustomerGender = item.CustomerGender;
                    model.CustomerWedlock = item.CustomerWedlock;
                    model.CustomerAge = item.CustomerAge;
                    model.CustomerJG = item.CustomerJG;
                    model.CustomerAddress = item.CustomerAddress;
                    model.CustomerCompany = item.CustomerCompany;
                    model.CustomerTel = item.CustomerTel;
                    model.CustomerGSR = item.CustomerGSR;
                    model.AdmissionDate = item.AdmissionDate ?? DateTime.Now;
                    model.DiagnosticDate = item.DiagnosticDate ?? DateTime.Now;

                    model.CustomerCardID = item.CustomerCardID;
                    model.CustomerType_dic = item.CustomerType_dic;
                    model.CustomerSBKH = item.CustomerSBKH;
                    model.CustomerMZ = item.CustomerMZ;
                    model.CustomerWHCD_dic = item.CustomerWHCD_dic;
                    model.CustomerZW = item.CustomerZW;
                    model.CustomerYLJSR = item.CustomerYLJSR;
                    model.CustomerZZDH = item.CustomerZZDH;
                    model.CustomerYYZT = item.CustomerYYZT;
                    model.CustomerSFBS = item.CustomerSFBS;
                    model.CustomerSX = item.CustomerSX;
                    model.CustomerXMPY = item.CustomerXMPY;//姓名拼音
                    model.CustomerZYBH = item.CustomerZYBH;
                    model.CustomerYYSJ = item.CustomerYYSJ;
                    model.CustomerLYSJ = item.CustomerLYSJ;
                    model.CustomerLYYY = item.CustomerLYYY;
                    model.CustomerYLBZ = item.CustomerYLBZ;
                    model.CustomerJJYW = item.CustomerJJYW;
                    model.CustomerJJSW = item.CustomerJJSW;
                    model.CustomerYSLX_dic = item.CustomerYSLX_dic;
                    model.CustomerYYBH = item.CustomerYYBH;
                    model.CustomerSFWWYY = item.CustomerSFWWYY;
                    model.CustomerHLMC_dic = item.CustomerHLMC_dic;
                    model.CustomerBrith = item.CustomerBrith;
                    model.BedNumberName = item.BedNumberName;
                    model.CustomerHeadImg = item.CustomerHeadImg;

                    model.CustomerZZMM = item.CustomerZZMM;
                    model.CustomerJYCD = item.CustomerJYCD;
                    model.CustomerDlrXm = item.CustomerDlrXm;
                    model.CustomerDlrTel = item.CustomerDlrTel;
                    model.CustomerDlrDz = item.CustomerDlrDz;
                    model.CustomerDlrGx = item.CustomerDlrGx;
                    model.CustomerDlrYb = item.CustomerDlrYb;
                    model.CustomerHJDQ_dic = item.CustomerHJDQ_dic;
                    model.WelfareCentreID = item.WelfareCentreID;

                    _context.Set<tbCustomer>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.ID = item.ID;
                    query.Stage = item.Stage;
                    query.OperateDate = item.OperateDate;
                    query.CustomerName = item.CustomerName;
                    query.BedNumber = item.BedNumber;
                    query.BedID = item.BedID;
                    query.CustomerGender = item.CustomerGender;
                    query.CustomerWedlock = item.CustomerWedlock;
                    query.CustomerAge = item.CustomerAge;
                    query.CustomerJG = item.CustomerJG;
                    query.CustomerAddress = item.CustomerAddress;
                    query.CustomerCompany = item.CustomerCompany;
                    query.CustomerTel = item.CustomerTel;
                    query.CustomerGSR = item.CustomerGSR;
                    query.AdmissionDate = item.AdmissionDate ?? DateTime.Now;
                    query.DiagnosticDate = item.DiagnosticDate ?? DateTime.Now;

                    query.CustomerCardID = item.CustomerCardID;
                    query.CustomerType_dic = item.CustomerType_dic;
                    query.CustomerSBKH = item.CustomerSBKH;
                    query.CustomerMZ = item.CustomerMZ;
                    query.CustomerWHCD_dic = item.CustomerWHCD_dic;
                    query.CustomerZW = item.CustomerZW;
                    query.CustomerYLJSR = item.CustomerYLJSR;
                    query.CustomerZZDH = item.CustomerZZDH;
                    query.CustomerYYZT = item.CustomerYYZT;
                    query.CustomerSFBS = item.CustomerSFBS;
                    query.CustomerSX = item.CustomerSX;
                    query.CustomerXMPY = item.CustomerXMPY;//姓名拼音
                    query.CustomerZYBH = item.CustomerZYBH;
                    query.CustomerYYSJ = item.CustomerYYSJ;
                    query.CustomerLYSJ = item.CustomerLYSJ;
                    query.CustomerLYYY = item.CustomerLYYY;
                    query.CustomerYLBZ = item.CustomerYLBZ;
                    query.CustomerJJYW = item.CustomerJJYW;
                    query.CustomerJJSW = item.CustomerJJSW;
                    query.CustomerYSLX_dic = item.CustomerYSLX_dic;
                    query.CustomerYYBH = item.CustomerYYBH;
                    query.CustomerSFWWYY = item.CustomerSFWWYY;
                    query.CustomerHLMC_dic = item.CustomerHLMC_dic;
                    query.CustomerBrith = item.CustomerBrith;
                    query.BedNumberName = item.BedNumberName;
                    query.CustomerHeadImg = item.CustomerHeadImg;

                    query.CustomerZZMM = item.CustomerZZMM;
                    query.CustomerJYCD = item.CustomerJYCD;
                    query.CustomerDlrXm = item.CustomerDlrXm;
                    query.CustomerDlrTel = item.CustomerDlrTel;
                    query.CustomerDlrDz = item.CustomerDlrDz;
                    query.CustomerDlrGx = item.CustomerDlrGx;
                    query.CustomerDlrYb = item.CustomerDlrYb;
                    query.CustomerHJDQ_dic = item.CustomerHJDQ_dic;
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
                var query = _context.Set<tbCustomer>().FirstOrDefault(c => c.ID == customerId && c.WelfareCentreID == welfareCentreId);
                if (query != null)
                {
                    _context.Set<tbCustomer>().Remove(query);
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
                var query = _context.Set<tbCustomer>().FirstOrDefault(c => c.ID == customerId && c.WelfareCentreID == welfareCentreId);
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
