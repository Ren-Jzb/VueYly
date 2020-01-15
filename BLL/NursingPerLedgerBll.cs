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
    public class NursingPerLedgerBll
    {
        public IList<vNursingPerLedger>  GetById(Guid? welfareCentreId)
        {
            var szServices = new DbHelperEfSql<vNursingPerLedger>();
            return szServices.SearchListByCondition(c => c.WelfareCentreID == welfareCentreId && c.IsValid == 1);
        }
    }
}
