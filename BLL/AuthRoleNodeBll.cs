using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Model;

namespace BLL
{
    public class AuthRoleNodeBll
    {

        public IList<AuthRoleNode> GetListByRoleId(Guid? iRoleId)
        {
            var szServices = new DbHelperEfSql<AuthRoleNode>();
            return szServices.SearchListByCondition(c => c.RoleID == iRoleId);
        }

        public bool UpdateAuthRoleNode(List<string> ids, Guid iRoleId)
        {
            var szServices = new DbHelperEfSql<AuthRoleNode>();
            IList<AuthRoleNode> list = ids.Select(cc => new AuthRoleNode
            {
                AuthRoleNodeID = Guid.NewGuid(),
                RoleID = iRoleId,
                NodeId = Convert.ToInt32(cc),
                OperateDate = DateTime.Now
            }).ToList();
            return szServices.TransDeleteCAddL(list, c => c.RoleID == iRoleId);
        }

        public bool IsNodePageAuth(Guid iUserId, int iNodeId, bool isSuper)
        {
            var szServices = new DbHelperEfSql<vAuthRoleNode>();
            if (isSuper)
            {
                return szServices.Count(c => c.UserID == iUserId && c.NodeId == iNodeId) > 0;
            }
            else
            {
                return szServices.Count(c => c.UserID == iUserId && c.NodeId == iNodeId && c.IsSuper == 0) > 0;
            }
        }

        public IList<vLeftAuthRoleNode> SearchListByLeftUserId(Guid iUserId, bool isSuper)
        {
            var szServices = new DbHelperEfSql<vLeftAuthRoleNode>();
            if (isSuper)
            {
                return szServices.SearchListByCondition(c => c.UserID == iUserId, true, c => c.OrderIndex);
            }
            else
            {
                return szServices.SearchListByCondition(c => c.UserID == iUserId && c.IsSuper == 0, true, c => c.OrderIndex);
            }
        }


    }
}
