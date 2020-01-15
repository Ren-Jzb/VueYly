using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ConditionModel
    {
        /// <summary>
        /// 查询条件集合
        /// </summary>
        public IList<WhereCondition> WhereList { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public IList<OrderCondition> OrderList { get; set; }
        /// <summary>
        /// 分组字段名称
        /// </summary>
        public IList<string> GroupingList { get; set; }
        public override string ToString()
        {
            var buf = new StringBuilder();
            if (WhereList != null && WhereList.Count > 0)
            {
                int i = 1;
                foreach (var item in WhereList)
                {
                    if (item == null) continue;
                    buf.AppendFormat("条件{0}：{1};", i, item.ToString());
                    i++;
                }
            }
            if (OrderList != null && OrderList.Count > 0)
            {
                int i = 1;
                foreach (var item in OrderList)
                {
                    if (item == null) continue;
                    buf.AppendFormat("排序{0}：{1};", i, item.ToString());
                    i++;
                }
            }
            if (GroupingList != null && GroupingList.Count > 0)
            {
                int i = 1;
                foreach (var item in GroupingList)
                {
                    buf.AppendFormat("分组{0}：{1};", i, item.ToString());
                    i++;
                }
            }
            return buf.ToString();
        }
    }
    //=, ==, !=, <>, >, >=, <, <= operators
    public enum EnumOper
    {
        Equal,//=
        DoubleEqual,//==
        ExclamationEqual,//!=
        LessGreater,//<>
        GreaterThan,//>
        GreaterThanEqual,//>=
        LessThan,//<
        LessThanEqual,//<=
        Contains, //包括
        IndexOf,
        StartsWith,
        EndsWith
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    public class WhereCondition
    {
        public WhereCondition()
        {
            Relation = "and";
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段对应值
        /// </summary>
        public object FieldValue { get; set; }
        /// <summary>
        /// 字段和值的关系（如>、>=、== 等等关系运算符）
        /// </summary>
        public EnumOper FieldOperator { get; set; }

        /// <summary>
        /// 连接条件对应关系 and,or
        /// </summary>
        public string Relation { get; set; }
        public override string ToString()
        {
            return string.Format("FieldName:{0},FieldValue:{1},FieldOperator:{2},Relation:{3}", FieldName, FieldValue, FieldOperator, Relation);
        }
    }
    /// <summary>
    /// 排序
    /// </summary>
    public class OrderCondition
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string FiledOrder { get; set; }
        /// <summary>
        /// 排序值两种DESC或ASC
        /// </summary>        
        public bool Ascending { get; set; }
        public override string ToString()
        {
            return string.Format("FiledOrder:{0},Ascending:{1}", FiledOrder, Ascending);
        }
    }
}
