using System;
using System.Collections;

namespace Model
{
    public class UploadParameter
    {
        public UploadParameter()
        {
            PathPrefix = "/Upload/temp/";
            IsUploadImages = true;
            TableType = 0;
            ParentID = 0;
            ParentGUID = Guid.Empty;
            FileContentLengthLimit = 1024 * 1024 * 100;//100M
            FileExtensionLimit = new ArrayList();
            UploadLimit = 100;
        }

        /// <summary>
        /// 路径前缀
        /// </summary>
        public string PathPrefix { get; set; }
        /// <summary>
        /// 是否是上传图片
        /// </summary>
        public bool IsUploadImages { get; set; }
        /// <summary>
        /// 上传类型
        /// </summary>
        public int TableType { get; set; }

        /// <summary>
        /// 对应表ID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 对应表ID
        /// </summary>
        public Guid ParentGUID { get; set; }

        /// <summary>
        /// 文件大小限制单位（M）
        /// </summary>
        public int FileContentLengthLimit { get; set; }
        /// <summary>
        /// 文件扩展名限制
        /// </summary>
        public ArrayList FileExtensionLimit { get; set; }

        /// <summary>
        /// 上传文件个数限制
        /// </summary>
        public int UploadLimit { get; set; }
    }
}
