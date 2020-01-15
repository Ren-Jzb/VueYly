using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VO
{
    public class GetNursingVo : ResultModel
    {
        public GetNursingVo()
        {
            callTime = "";
            person = new personModel();
            nursing = new List<nursingModel>();
        }

        public string callTime { get; set; }
        public personModel person { get; set; }
        public IList<nursingModel> nursing { get; set; }

    }

    public class personModel
    {
        public personModel()
        {
            userId = Guid.Empty;
            personName = "";
            gender = "";
            age = 0;
            remark = "";
            nursingType = "";
        }

        public Guid userId { get; set; }
        public string personName { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public string remark { get; set; }
        public string nursingType { get; set; }
    }

    public class nursingModel
    {
        public nursingModel()
        {
            nursingId = 0;
            fatherId = 0;
            nursingContent = "";
            nursingImgUrl = "";
            nursingExt = new List<nursingExtModel>() { new nursingExtModel() };
        }

        public int nursingId { get; set; }
        public int fatherId { get; set; }
        public string nursingContent { get; set; }
        public string nursingImgUrl { get; set; }
        public IList<nursingExtModel> nursingExt { get; set; }
    }

    public class nursingExtModel
    {
        public nursingExtModel()
        {
            nursingId = 0;
            extType = "";
            extTitle = "";
            extContent = "";
        }

        public int nursingId { get; set; }
        public string extType { get; set; }
        public string extTitle { get; set; }
        public string extContent { get; set; }
    }

}
