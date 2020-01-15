using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Web;
using System.ServiceModel.Channels;
using System.Net.NetworkInformation;
using System.IO;

namespace CommonLib
{
    public class UserLogIP
    {
        [DllImport("Iphlpapi.dll")]
        static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);

        [DllImport("Ws2_32.dll")]
        static extern Int32 inet_addr(string ipaddr);

        #region   获取客户端Mac地址
        /// <summary>
        /// 获取客户端Mac地址
        /// </summary>
        /// <returns></returns>
        public string GetClientMAC(string strClientIP)
        {
            string Mac = "";
            try
            {
                //将IP地址从 点数格式转换成无符号长整型
                Int32 ldest = inet_addr(strClientIP);
                Int64 macinfo = new Int64();
                Int32 len = 6;
                SendARP(ldest, 0, ref macinfo, ref len);
                //转换成16进制,注意有些没有十二位
                string TmpMac = Convert.ToString(macinfo, 16).PadLeft(12, '0');
                Mac = TmpMac.Substring(0, 2).ToUpper();
                for (int i = 2; i < TmpMac.Length; i = i + 2)
                {
                    Mac = TmpMac.Substring(i, 2).ToUpper() + "-" + Mac;
                }
            }
            catch (Exception Mye)
            {
                Mac = "MAC：" + Mye.Message;
            }
            return Mac;
        }

        public string ReturnClientMAC()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    var mac = mo["MacAddress"].ToString();
                    mo.Dispose();
                    return mac;
                }
            }
            return "00-00-00-00-00-00";
        }
        #endregion

    }

}
