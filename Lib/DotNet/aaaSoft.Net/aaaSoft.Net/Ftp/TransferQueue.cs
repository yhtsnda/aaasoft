using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using aaaSoft.Helpers;

namespace aaaSoft.Net.Ftp
{
    /// <summary>
    /// 传输队列
    /// </summary>
    public class TransferQueue
    {
        //传输线程
        private Thread trdTransfer;
        //报告进度线程
        private Thread trdUpdateProgress;
        //队列对象列表
        private List<TransferQueueItem> QueueItemList;

        /// <summary>
        /// 已传送文件数量
        /// </summary>
        public Int32 TransferedFileCount;
        /// <summary>
        /// 已传送目录数量
        /// </summary>
        public Int32 TransferedFolderCount;
        /// <summary>
        /// 已传送数据的长度
        /// </summary>
        [NonSerialized()]
        public Int64 TransferedDataLength;
        /// <summary>
        /// 传输开始时间
        /// </summary>
        [NonSerialized()]
        public DateTime TransferStartTime;
        /// <summary>
        /// 传输结束时间
        /// </summary>
        [NonSerialized()]
        public DateTime TransferEndTime;
        /// <summary>
        /// 传输所用的时间
        /// </summary>
        public TimeSpan TransferUsedTime
        {
            get
            {
                DateTime start, end;
                start = TransferStartTime;
                if (QueueState == TransferQueueState.Stoped)
                    end = TransferEndTime;
                else
                    end = DateTime.Now;
                return end - start;
            }
        }
        /// <summary>
        /// 平均传输速度(单位：字节每秒[byte/s])
        /// </summary>
        public Int64 AverageTransferSpeed
        {
            get
            {
                try
                {
                    Double rtnData = TransferedDataLength / TransferUsedTime.TotalSeconds;
                    if (Double.IsInfinity(rtnData) || Double.IsNaN(rtnData))
                        rtnData = 0;
                    return Convert.ToInt64(rtnData);
                }
                catch { return 0; }
            }
        }

        /// <summary>
        /// 报告进度的时间间隔(单位为毫秒，默认值为1000)
        /// </summary>
        public Int32 ProgressUpdateTimeInterval = 1000;

        //传输队列状态枚举
        public enum TransferQueueState
        {
            /// <summary>
            /// 运行中
            /// </summary>
            Running = 0,
            /// <summary>
            /// 已停止
            /// </summary>
            Stoped = 1
        }
        //传输队列状态
        public TransferQueueState QueueState = TransferQueueState.Stoped;

        /// <summary>
        /// 队列事件参数类
        /// </summary>
        public class QueueEventArgs : EventArgs
        {
            public TransferQueueItem QueueItem;
            public QueueEventArgs() { }
            public QueueEventArgs(TransferQueueItem QueueItem)
                : this()
            {
                this.QueueItem = QueueItem;
            }
        }
        /// <summary>
        /// 队列事件处理委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void QueueEventHandler(Object sender, QueueEventArgs e);
        /// <summary>
        /// 队列添加事件
        /// </summary>
        public event QueueEventHandler QueueAdded;
        /// <summary>
        /// 队列移除事件
        /// </summary>
        public event QueueEventHandler QueueRemoved;
        /// <summary>
        /// 队列启动事件
        /// </summary>
        public event QueueEventHandler QueueStarted;
        /// <summary>
        /// 队列停止事件
        /// </summary>
        public event QueueEventHandler QueueStoped;
        /// <summary>
        /// 队列完成事件
        /// </summary>
        public event QueueEventHandler QueueCompleted;
        /// <summary>
        /// 队列进度更新事件
        /// </summary>
        public event EventHandler QueueProgressUpdated;
        /// <summary>
        /// 当前正在传输的站点数据
        /// </summary>
        public FtpClient CurrentFtpClient;

        #region 传输队列构造函数
        /// <summary>
        /// 传输队列构造函数
        /// </summary>
        /// <param name="faForm">主窗体</param>
        public TransferQueue()
        {
            QueueItemList = new List<TransferQueueItem>();
        }
        #endregion

        #region 开始队列
        /// <summary>
        /// 开始队列
        /// </summary>
        public void StartQueue()
        {
            QueueState = TransferQueueState.Running;
            if (trdTransfer == null)
            {
                trdUpdateProgress = new Thread(QueueProgressUpdateThread);
                trdUpdateProgress.Start();
                trdTransfer = new Thread(QueueThread);
                trdTransfer.Start();
            }
        }
        #endregion

        #region 停止队列
        /// <summary>
        /// 停止队列
        /// </summary>
        public void StopQueue()
        {
            QueueState = TransferQueueState.Stoped;
        }
        #endregion

        #region 队列进度更新线程方法
        private void QueueProgressUpdateThread()
        {
            while (QueueItemList.Count > 0)
            {
                if (QueueState == TransferQueueState.Stoped)
                    break;
                //触发进度更新事件
                if (QueueProgressUpdated != null)
                {
                    QueueProgressUpdated(this, new EventArgs());
                }
                Thread.Sleep(ProgressUpdateTimeInterval);
            }
            trdUpdateProgress = null;
        }
        #endregion

        #region 队列线程方法
        private void QueueThread()
        {
            //触发队列启动事件
            if (QueueStarted != null)
                QueueStarted(this, new QueueEventArgs());

            //初始化相关变量
            TransferedFileCount = 0;
            TransferedFolderCount = 0;
            TransferedDataLength = 0;
            TransferStartTime = DateTime.Now;

            //开始传输队列
            while (QueueItemList.Count > 0)
            {
                if (QueueState == TransferQueueState.Stoped)
                    break;
                var item = QueueItemList[0];

                //处理队列对象
                item.State = TransferQueueItem.TransferQueueItemStateEnum.Transfering;
                var ftpClient = item.FtpClient;
                CurrentFtpClient = ftpClient;

                if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Download)
                {
                    try
                    {
                        var baseFile = item.RemoteBaseFile;

                        //如果是目录
                        if (baseFile.IsFolder)
                        {
                            var subLocalPath = System.IO.Path.Combine(item.LocalPath, item.Name);
                            if (!aaaSoft.Helpers.IoHelper.CreateMultiFolder(subLocalPath))
                            {
                                item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                item.Tip = String.Format("创建目录 {0} 时失败。", subLocalPath);
                                RemoveFromQueue(item);
                                continue;
                            }
                            //列出目录
                            var subBaseFiles = ftpClient.ListDirectory(baseFile.FullName);
                            if (subBaseFiles == null)
                            {
                                item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                item.Tip = String.Format("列目录 {0} 时失败，原因:{1}。", baseFile.FullName, ftpClient.ErrMsg);
                                RemoveFromQueue(item);
                                continue;
                            }
                            foreach (var subBaseFile in subBaseFiles)
                            {
                                var subItem = new TransferQueueItem(ftpClient);
                                subItem.Type = TransferQueueItem.TransferQueueItemTypeEnum.Download;
                                subItem.RemoteBaseFile = subBaseFile;
                                subItem.RemotePath = subBaseFile.FullName;
                                subItem.LocalPath = System.IO.Path.Combine(subLocalPath, subBaseFile.Name);
                                InsertIntoQueue(subItem);
                            }
                            item.State = TransferQueueItem.TransferQueueItemStateEnum.TransferComplete;
                            TransferedFolderCount++;
                        }
                        //如果是文件
                        else
                        {
                            //如果当前目录不是要下载文件的目录，则改变工作目录
                            if (ftpClient.CurrentDirectoryPath.ToUpper() != baseFile.ParentPath.ToUpper())
                            {
                                ftpClient.ListDirectory(baseFile.ParentPath);
                            }

                            if (!ftpClient.DownloadFile(baseFile as FtpBaseFileInfo, item.LocalPath))
                            {
                                item.Tip = String.Format("下载文件 {0} 时失败，原因:{1}。", baseFile.FullName, ftpClient.ErrMsg);
                                item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                RemoveFromQueue(item);
                                continue;
                            }
                            item.State = TransferQueueItem.TransferQueueItemStateEnum.TransferComplete;
                            TransferedFileCount++;
                            TransferedDataLength += ftpClient.TransferedDataLength;
                        }
                    }
                    catch (Exception ex)
                    {
                        item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                        item.Tip = String.Format("下载路径 {0} 时出现异常，原因:{1}。", item.RemotePath, ex.Message);
                        RemoveFromQueue(item);
                        continue;
                    }
                }
                else if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Upload)
                {
                    try
                    {
                        //如果是文件
                        if (System.IO.File.Exists(item.LocalPath))
                        {
                            //远端目录路径
                            var remoteFolderPath = aaaSoft.Helpers.IoHelper.GetParentPath(item.RemotePath, '/');
                            if (ftpClient.CurrentDirectoryPath != remoteFolderPath)
                            {
                                var baseFiles = ftpClient.ListDirectory(remoteFolderPath);
                                //如果列目录失败
                                if (baseFiles == null)
                                {
                                    item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                    item.Tip = String.Format("列目录 {0} 时失败，原因:{1}。", remoteFolderPath, ftpClient.ErrMsg);
                                    RemoveFromQueue(item);
                                    continue;
                                }
                            }

                            //远端文件名
                            var remoteFileName = aaaSoft.Helpers.IoHelper.GetFileOrFolderName(item.RemotePath, '/');

                            if (!ftpClient.UploadFile(remoteFileName, item.LocalPath))
                            {
                                item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                item.Tip = String.Format("上传文件 {0} 时失败，原因:{1}。", item.LocalPath, ftpClient.ErrMsg);
                                RemoveFromQueue(item);
                                continue;
                            }
                            item.State = TransferQueueItem.TransferQueueItemStateEnum.TransferComplete;
                            TransferedFileCount++;
                            TransferedDataLength += ftpClient.TransferedDataLength;
                        }
                        //如果是目录
                        else if (System.IO.Directory.Exists(item.LocalPath))
                        {
                            //=====================
                            //检查远端目录是否存在
                            //=====================
                            var baseFiles = ftpClient.ListDirectory(item.RemotePath);
                            //如果远端目录不存在
                            if (baseFiles == null)
                            {
                                if (!ftpClient.CreateDirectory(item.RemotePath))
                                {
                                    item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                    item.Tip = String.Format("创建远端目录 {0} 时失败，原因:{0}。", item.LocalPath, ftpClient.ErrMsg);
                                    RemoveFromQueue(item);
                                    continue;
                                }
                            }

                            var localFolderInfo = new System.IO.DirectoryInfo(item.LocalPath);

                            List<String> PathList = new List<string>();
                            foreach (var subFileInfo in localFolderInfo.GetFiles())
                                PathList.Add(subFileInfo.FullName);
                            foreach (var subFolderInfo in localFolderInfo.GetDirectories())
                                PathList.Add(subFolderInfo.FullName);

                            foreach (var subPath in PathList)
                            {
                                var subItem = new TransferQueueItem(ftpClient);
                                subItem.Type = TransferQueueItem.TransferQueueItemTypeEnum.Upload;
                                subItem.LocalPath = subPath;
                                subItem.RemotePath = (item.RemotePath + "/" + IoHelper.GetFileOrFolderName(subPath, System.IO.Path.DirectorySeparatorChar)).Replace("//", "/");
                                InsertIntoQueue(subItem);
                            }
                            item.State = TransferQueueItem.TransferQueueItemStateEnum.TransferComplete;
                            TransferedFolderCount++;

                        }
                    }
                    catch (Exception ex)
                    {
                        item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                        item.Tip = String.Format("上传路径 {0} 时出现异常，原因:{1}。", item.LocalPath, ex.Message);
                        RemoveFromQueue(item);
                        continue;
                    }
                }
                //如果是删除
                else if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Delete)
                {
                    try
                    {
                        var baseFile = item.RemoteBaseFile;

                        //如果是目录
                        if (baseFile.IsFolder)
                        {
                            List<FtpBaseFileInfo> ftpBaseFileInfoList = ftpClient.ListDirectory(baseFile.FullName);
                            if (ftpBaseFileInfoList.Count == 0)
                            {
                                if (!ftpClient.RemoveDirectory(baseFile.FullName))
                                {
                                    item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                    item.Tip = String.Format("删除目录 {0} 时失败，原因:{1}。", item.LocalPath, ftpClient.ErrMsg);
                                    RemoveFromQueue(item);
                                    continue;
                                }
                                TransferedFolderCount++;
                            }
                            else
                            {
                                this.InsertIntoQueue(new TransferQueueItem(ftpClient)
                                {
                                    Type = TransferQueueItem.TransferQueueItemTypeEnum.Delete,
                                    RemoteBaseFile = baseFile,
                                    RemotePath = baseFile.FullName
                                });
                                foreach (FtpBaseFileInfo tmpInfo in ftpBaseFileInfoList)
                                {
                                    this.InsertIntoQueue(new TransferQueueItem(ftpClient)
                                    {
                                        Type = TransferQueueItem.TransferQueueItemTypeEnum.Delete,
                                        RemoteBaseFile = tmpInfo,
                                        RemotePath = tmpInfo.FullName
                                    });
                                }
                            }
                        }
                        //如果是文件
                        else
                        {
                            if (!ftpClient.DeleteFile(baseFile.FullName))
                            {
                                item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                                item.Tip = String.Format("删除文件 {0} 时失败，原因:{1}。", item.LocalPath, ftpClient.ErrMsg);
                                RemoveFromQueue(item);
                                continue;
                            }
                            TransferedFileCount++;
                            TransferedDataLength += baseFile.Length;
                        }
                        item.State = TransferQueueItem.TransferQueueItemStateEnum.TransferComplete;
                    }
                    catch (Exception ex)
                    {
                        item.State = TransferQueueItem.TransferQueueItemStateEnum.Error;
                        item.Tip = String.Format("删除路径 {0} 时出现异常，原因:{1}。", item.RemotePath, ex.Message);
                        RemoveFromQueue(item);
                        continue;
                    }
                }
                //如果处理成功则从队列中移除
                RemoveFromQueue(item);                    
            }
            TransferEndTime = DateTime.Now;
            trdTransfer = null;
            CurrentFtpClient = null;
            if (QueueItemList.Count == 0)
            {
                //触发队列完成事件
                if (QueueCompleted != null)
                    QueueCompleted(this, new QueueEventArgs());
            }
            else
            {
                //触发队列停止事件
                if (QueueStoped != null)
                    QueueStoped(this, new QueueEventArgs());
            }
        }
        #endregion

        #region 增加到队列
        /// <summary>
        /// 增加到队列
        /// </summary>
        /// <param name="item">传输队列对象</param>
        public void AddToQueue(TransferQueueItem item)
        {
            QueueItemList.Add(item);
            if (QueueAdded != null)
                QueueAdded(this, new QueueEventArgs(item));
        }
        #endregion

        #region 插入到队列开始
        /// <summary>
        /// 插入到队列开始
        /// </summary>
        /// <param name="item"></param>
        public void InsertIntoQueue(TransferQueueItem item)
        {
            QueueItemList.Insert(0, item);
            if (QueueAdded != null)
                QueueAdded(this, new QueueEventArgs(item));
        }
        #endregion

        #region 得到对象在队列中倒数的序号(从1开始)
        public Int32 GetTransferQueueItemBackIndex(TransferQueueItem item)
        {
            return QueueItemList.Count - 1 - QueueItemList.IndexOf(item);
        }
        #endregion

        #region 从队列中移除
        /// <summary>
        /// 从队列中移除
        /// </summary>
        /// <param name="item">传输队列对象</param>
        public void RemoveFromQueue(TransferQueueItem item)
        {
            //如果正在传输这个队列对象，则不能移除
            if (item.State == TransferQueueItem.TransferQueueItemStateEnum.Transfering)
                return;

            QueueItemList.Remove(item);
            if (QueueRemoved != null)
                QueueRemoved(this, new QueueEventArgs(item));
        }
        #endregion

        #region 得到队列对象的数量
        /// <summary>
        /// 得到队列对象的数量
        /// </summary>
        /// <returns></returns>
        public Int32 GetQueueItemCount()
        {
            return QueueItemList.Count;
        }
        #endregion

        #region 清除队列
        /// <summary>
        /// 清除队列
        /// </summary>
        public void ClearQueue()
        {
            QueueState = TransferQueueState.Stoped;
            foreach (var item in QueueItemList.ToArray())
            {
                RemoveFromQueue(item);
            }
        }
        #endregion
    }
}
