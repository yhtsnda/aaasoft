using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Ftp.IO;

namespace MultiserverFtpClient
{
    /// <summary>
    /// 传输队列对象
    /// </summary>
    [Serializable()]
    public class TransferQueueItem
    {
        #region 事件部分
        /// <summary>
        /// 传输队列对象状态改变事件
        /// </summary>
        public event EventHandler StateChanged;
        #endregion

        /// <summary>
        /// 传输队列对象类型枚举
        /// </summary>
        public enum TransferQueueItemTypeEnum
        {
            /// <summary>
            /// 下载
            /// </summary>
            Download = 0,
            /// <summary>
            /// 上传
            /// </summary>
            Upload = 1
        }
        /// <summary>
        /// 传输队列对象类型
        /// </summary>
        public TransferQueueItemTypeEnum Type { get; set; }

        /// <summary>
        /// 传输队列对象状态枚举
        /// </summary>
        public enum TransferQueueItemStateEnum
        {
            /// <summary>
            /// 准备就绪
            /// </summary>
            Ready = 0,
            /// <summary>
            /// 传输中
            /// </summary>
            Transfering = 1,
            /// <summary>
            /// 传输完成
            /// </summary>
            TransferComplete = 2,
            /// <summary>
            /// 传输出错
            /// </summary>
            Error = 3
        }

        private TransferQueueItemStateEnum _State = TransferQueueItemStateEnum.Ready;
        /// <summary>
        /// 传输队列对象状态
        /// </summary>
        public TransferQueueItemStateEnum State
        {
            get { return _State; }
            set
            {
                bool IsStateChanged = _State != value;
                _State = value;
                if (IsStateChanged)
                    if (StateChanged != null)
                        StateChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// 本地路径
        /// </summary>
        public String LocalPath { get; set; }
        /// <summary>
        /// 远端路径
        /// </summary>
        public String RemotePath { get; set; }
        /// <summary>
        /// 远端基本文件对象(可能是文件或目录)
        /// </summary>
        public FtpBaseFileInfo RemoteBaseFile { get; set; }
        /// <summary>
        /// FTP站点数据的GUID
        /// </summary>
        public String FtpSiteDataGuid { get; set; }

        private FtpSiteData _SiteData;
        /// <summary>
        /// FTP站点数据
        /// </summary>
        public FtpSiteData SiteData { get { return _SiteData; } }
        /// <summary>
        /// 备注
        /// </summary>
        public String Tip { get; set; }
        /// <summary>
        /// 队列对象名称
        /// </summary>
        public String Name
        {
            get
            {
                switch (Type)
                {
                    case TransferQueueItemTypeEnum.Download:
                        return LocalPath;
                    case TransferQueueItemTypeEnum.Upload:
                        return RemotePath;
                }
                return "未定义";
            }
        }

        private TransferQueueItem() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Site"></param>
        public TransferQueueItem(FtpSiteData Site)
        {
            this._SiteData = Site;
            this.FtpSiteDataGuid = Site.GUID;
        }
    }
}
