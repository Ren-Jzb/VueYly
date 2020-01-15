using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SetNursingDto : BaseParamDto
    {
        public Guid userId { get; set; }
        public string bedNumber { get; set; }
        public string nursingIds { get; set; }
        public string nursingExt { get; set; }
    }
}
