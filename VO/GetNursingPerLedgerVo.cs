using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VO
{
    public class GetNursingPerLedgerVo : ResultModel
    {
        public GetNursingPerLedgerVo()
        {
            nursing = new List<nursingPerLedgerModel>();
        }
        public IList<nursingPerLedgerModel> nursing { get; set; }

    } 

    public class nursingPerLedgerModel
    {
        public nursingPerLedgerModel()
        {
            customerName = "";
            nursingContent = "";
            nursingName = "";
            nursingTime = "";
            nursingState = "";
            nursingPerLedger = new List<nursingPerLedgerModel>() { new nursingPerLedgerModel() };
        }

        public string customerName { get; set; }
        public string nursingContent { get; set; }
        public string nursingName { get; set; }
        public string nursingTime { get; set; }
        public string nursingState { get; set; }
        public IList<nursingPerLedgerModel> nursingPerLedger { get; set; }
    }

}
