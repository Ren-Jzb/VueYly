using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BLL
{
    public class DictionaryBll
    {
        public IList<tbDictionary> SearchListByType(Guid welfareCentreId, int iType)
        {
            var szService = new DbHelperEfSql<tbDictionary>();
            return szService.SearchListByCondition(c => c.DictionaryType == iType && c.WelfareCentreID == welfareCentreId && c.IsValid == 1, true, c => c.DictionaryID);
        }
    }
}
