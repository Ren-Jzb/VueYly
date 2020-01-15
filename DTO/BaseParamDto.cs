using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BaseParamDto
    {
        public BaseParamDto()
        {
            welfareCentreId = Guid.Empty;
        }
        public Guid welfareCentreId { get; set; }
    }
}
