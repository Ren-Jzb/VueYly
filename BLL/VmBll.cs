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
    public class VmBll
    {
        public bool AddOrUpdate(Vm item, bool IsAdd)
        {
            var szServices = new DbHelperEfSql<Vm>();
            return IsAdd ? szServices.Add(item) : szServices.Update(item, c => c.Id == item.Id);
        }
    }
}
