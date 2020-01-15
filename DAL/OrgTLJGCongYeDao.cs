using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using CommonLib;

namespace DAL
{
    public class OrgTLJGCongYeDao : BaseServiceEf<MISDBContainer>
    {
        public bool AddOrUpdae(tbOrgTLJGCongYe item)
        {
            try
            {
                var isAdd = true;

                #region
                var query = _context.Set<tbOrgTLJGCongYe>().FirstOrDefault(c => c.ID == item.ID);
                if (query == null)
                {
                    isAdd = true;
                    var model = new tbOrgTLJGCongYe();
                    model.ID = item.ID;
                    model.OrganizationId = item.OrganizationId;
                    model.WelfareId = item.WelfareId;
                    model.StaffName = item.StaffName;
                    model.StaffGender = item.StaffGender;
                    model.EmployType = item.EmployType;
                    model.SocialSecurity = item.SocialSecurity;
                    model.Profession = item.Profession;
                    model.WeiJi = item.WeiJi;
                    model.SheGong = item.SheGong;
                    model.QiTa = item.QiTa;
                    model.HgChiZheng = item.HgChiZheng;
                    model.HgXueLi = item.HgXueLi;
                    model.HgNianLing = item.HgNianLing;
                    model.HgRuZhiNian = item.HgRuZhiNian;
                    model.HgHuJi = item.HgHuJi;
                    model.HouQin = item.HouQin;
                    model.OperateUserID = item.OperateUserID;
                    model.CreateDate = item.CreateDate;
                    model.OperateDate = item.OperateDate;
                    model.IsValid = item.IsValid;
                    model.Remark = item.Remark;
                    model.Phone = item.Phone;
                    model.AddressPlace = item.AddressPlace;
                    _context.Set<tbOrgTLJGCongYe>().Add(model);
                }
                else
                {
                    isAdd = false;
                    query.ID = item.ID;
                    query.OrganizationId = item.OrganizationId;
                    query.WelfareId = item.WelfareId;
                    query.StaffName = item.StaffName;
                    query.StaffGender = item.StaffGender;
                    query.EmployType = item.EmployType;
                    query.SocialSecurity = item.SocialSecurity;
                    query.Profession = item.Profession;
                    query.WeiJi = item.WeiJi;
                    query.SheGong = item.SheGong;
                    query.QiTa = item.QiTa;
                    query.HgChiZheng = item.HgChiZheng;
                    query.HgXueLi = item.HgXueLi;
                    query.HgNianLing = item.HgNianLing;
                    query.HgRuZhiNian = item.HgRuZhiNian;
                    query.HgHuJi = item.HgHuJi;
                    query.HouQin = item.HouQin;
                    query.OperateUserID = item.OperateUserID;
                    query.CreateDate = item.CreateDate;
                    query.OperateDate = item.OperateDate;
                    query.IsValid = 1;
                    query.Remark = item.Remark;
                    query.Phone = item.Phone;
                    query.AddressPlace = item.AddressPlace;
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
                    MethodName = "AddOrUpdaeOrgTLJGCongYe",
                    MethodParameters = "添加失败",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }

    }
}
