using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;
using Model;

namespace DAL
{
    /// <summary>
    /// 特殊数据层，针对不完善的事情
    /// </summary>
    public class SpecialService : BaseServiceEf<MISDBContainer>
    {
        public bool UpdateNodeButton(List<int> nodeIdList, List<int> buttonIdList)
        {
            try
            {
                foreach (var NodeId in nodeIdList)
                {
                    var removeList = _context.NodeButton.Where(c => c.NodeId == NodeId && !buttonIdList.Contains(c.ButtonId ?? 0)).ToList();
                    if (removeList != null && removeList.Count > 0)
                    {
                        _context.NodeButton.RemoveRange(removeList);
                    }
                    foreach (var ButtonId in buttonIdList)
                    {
                        var query = _context.NodeButton.AsQueryable().FirstOrDefault(c => c.NodeId == NodeId && c.ButtonId == ButtonId);
                        if (query == null)
                        {
                            var item = new NodeButton();
                            item.NodeId = NodeId;
                            item.ButtonId = ButtonId;
                            item.OperateDate = DateTime.Now;
                            _context.NodeButton.Add(item);
                        }
                        //else
                        //{
                        //    query.OperateDate = DateTime.Now;
                        //}
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageLog.WriteLog(new LogParameterModel()
                {
                    ClassName = this.GetType().ToString(),
                    MethodName = "UpdateNodeButton",
                    LogLevel = ELogLevel.Warn,
                    Message = ex.Message
                });
                return false;
            }
        }

    }
}
