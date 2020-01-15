using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VO
{
    public class CanLoginVo : ResultModel
    {
        public CanLoginVo()
        {
            userId = "";
            realName = "";
            welfareCentreID = "";
        }

        public string userId { get; set; }
        public string realName { get; set; }
        public string welfareCentreID { get; set; }
    }
}
