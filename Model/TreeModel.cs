using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class TreeModel
    {
        public TreeModel() { }
        public int? Id { get; set; }
        public int? Pid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string Icon { get; set; }
        public Boolean IconOpen { get; set; }
        public Boolean open { get; set; }
        /// <summary>
        /// 是否没有Checkbox
        /// </summary>
        public Boolean nocheck { get; set; }
        /// <summary>
        /// 是否有Checkbox
        /// </summary>
        public Boolean Checkbox { get; set; }
        /// <summary>
        /// Checkbox是否选中
        /// </summary>
        public Boolean Checked { get; set; }
        public string NodeClassName { get; set; }
        public string NodeStyle { get; set; }
        public string Remark { get; set; }
    }
}
