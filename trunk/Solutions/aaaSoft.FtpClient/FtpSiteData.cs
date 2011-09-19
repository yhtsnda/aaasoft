using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Ftp;

namespace aaaSoft.FtpClient
{
    [Serializable()]
    /// <summary>
    /// FTP站点数据
    /// </summary>
    public class FtpSiteData
    {
        /// <summary>
        /// FTP站点数据GUID
        /// </summary>
        public String GUID { get; set; }
        /// <summary>
        /// 站点名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 主机名称
        /// </summary>
        public String HostName { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public Int32 Port { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public String UserName { get; set; }
        /// <summary>
        /// 是否匿名
        /// </summary>
        public Boolean IsAnonymous { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 是否显示隐藏文件 (LIST -al)
        /// </summary>
        public Boolean IsShowHidenFile = true;
        /// <summary>
        /// 使用 MLSD 列目录 *
        /// </summary>
        public Boolean IsUseMlsdToListFolder = true;
        /// <summary>
        /// 是否不支持FEAT命令
        /// </summary>
        public Boolean IsNotSupportFEAT = false;
        /// <summary>
        /// 缓存大小
        /// </summary>
        public Int32 BufferSize = 4 * 1024;
        /// <summary>
        /// 服务器字符编码
        /// </summary>
        public String StringEncoding = "gb2312";

        /// <summary>
        /// 远端路径
        /// </summary>
        public String RemotePath { get; set; }
        /// <summary>
        /// 本地路径
        /// </summary>
        public String LocalPath { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public String Tip { get; set; }
        [NonSerialized()]
        /// <summary>
        /// 该站点所在的组
        /// </summary>
        public FtpSiteDataGroup Group;

        private aaaSoft.Net.Ftp.FtpClient _FtpSite = null;
        /// <summary>
        /// 得到对应的FtpSite对象
        /// </summary>
        /// <returns></returns>
        public aaaSoft.Net.Ftp.FtpClient GetFtpSite()
        {
            if (_FtpSite == null)
            {
                CreateFtpSite();
            }
            return _FtpSite;
        }

        /// <summary>
        /// 清除FtpSite对象
        /// </summary>
        public void ClearFtpSite()
        {
            _FtpSite = null;
        }

        /// <summary>
        /// 创建新的FtpSite对象
        /// </summary>
        public void CreateFtpSite()
        {
            _FtpSite = new aaaSoft.Net.Ftp.FtpClient(HostName, Port, UserName, Password);
            _FtpSite.IsShowHidenFile = IsShowHidenFile;
            _FtpSite.IsUseMlsdToListFolder = IsUseMlsdToListFolder;
            _FtpSite.IsNotSupportFEAT = IsNotSupportFEAT;
            _FtpSite.StringEncoding = StringEncoding;
            _FtpSite.BufferSize = BufferSize;
            
        }
    }
}
