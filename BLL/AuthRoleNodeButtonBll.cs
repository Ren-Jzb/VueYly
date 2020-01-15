using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;


namespace BLL
{
    public class AuthRoleNodeButtonBll
    {

        public IList<AuthRoleNodeButton> GetListByRoleId(Guid? iRoleId)
        {
            var szServices = new DbHelperEfSql<AuthRoleNodeButton>();
            return szServices.SearchListByCondition(c => c.RoleID == iRoleId);
        }

        public bool UpdateAuthRoleNodeButton(List<string> ids, Guid iRoleId)
        {
            var szServices = new DbHelperEfSql<AuthRoleNodeButton>();
            var list = ids.Select(cc => new AuthRoleNodeButton
            {
                AuthRoleNodeButtonID = Guid.NewGuid(),
                RoleID = iRoleId,
                NodeButtonId = Convert.ToInt32(cc) - 100000,
                OperateDate = DateTime.Now
            }).ToList();
            return szServices.TransDeleteCAddL(list, c => c.RoleID == iRoleId);

        }

        public IList<vAuthRoleNodeButton> GetListByUserIdNodeId(Guid iUserId, int iNodeId, bool isSuper)
        {
            var szServices = new DbHelperEfSql<vAuthRoleNodeButton>();
            if (isSuper)
            {
                return szServices.SearchListByCondition(c => c.UserID == iUserId && c.NodeId == iNodeId, true, c => c.OrderIndex);
            }
            else
            {
                return szServices.SearchListByCondition(c => c.UserID == iUserId && c.NodeId == iNodeId && c.IsSuper == 0, true, c => c.OrderIndex);
            }
        }
        public bool IsAuthRoleNodeButton(Guid iUserid, int iNodeId, int iButtonId, bool isSuper)
        {
            var szServices = new DbHelperEfSql<vAuthRoleNodeButton>();
            if (isSuper)
            {
                return szServices.Count(c => c.UserID == iUserid && c.NodeId == iNodeId && c.ButtonId == iButtonId) > 0;
            }
            else
            {
                return szServices.Count(c => c.UserID == iUserid && c.NodeId == iNodeId && c.ButtonId == iButtonId && c.IsSuper == 0) > 0;
            }
        }
    }
}
