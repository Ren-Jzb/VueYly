using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using BLL;
using CommonLib;
using Microsoft.Ajax.Utilities;
using Model;
using VO;
using DTO;
using System.Linq;

namespace CareHome.Ws
{
    /// <summary>
    /// FamilyOperate 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class FamilyOperate : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
