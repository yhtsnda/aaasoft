using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Ftp;

namespace aaaSoft.FtpClient
{
    [Serializable()]
    /// <summary>
    /// FTP站点数据组
    /// </summary>
    public class FtpSiteDataGroup
    {
        /// <summary>
        /// FTP站点数据组GUID
        /// </summary>
        public String GUID { get; set; }
        /// <summary>
        /// 组名
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 子组列表
        /// </summary>
        public List<FtpSiteDataGroup> Groups { get; set; }
        /// <summary>
        /// 站点数据列表
        /// </summary>
        public List<FtpSiteData> Sites { get; set; }
        [NonSerialized()]
        /// <summary>
        /// 所在的组
        /// </summary>
        public FtpSiteDataGroup Group;

        public FtpSiteDataGroup()
        {
            Groups = new List<FtpSiteDataGroup>();
            Sites = new List<FtpSiteData>();
        }
    }
}
