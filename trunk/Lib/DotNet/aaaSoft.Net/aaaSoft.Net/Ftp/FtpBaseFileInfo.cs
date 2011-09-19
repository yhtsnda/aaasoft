using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Net.Ftp
{
    [Serializable()]
    public class FtpBaseFileInfo
    {
        /// <summary>
        /// 全路径
        /// </summary>
        public String FullName
        {
            get
            {
                String tmpPath = String.Format("{0}/{1}", ParentPath, Name);
                while (tmpPath.Contains("//"))
                {
                    tmpPath = tmpPath.Replace("//", "/");
                }
                return tmpPath;
            }
        }
        /// <summary>
        /// 父目录路径
        /// </summary>
        public String ParentPath { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public Int64 Length { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public String Property { get; set; }
        /// <summary>
        /// 是否是目录
        /// </summary>
        public bool IsFolder { get; set; }
    }
}
