using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using aaaSoft.Net.Ftp;
using aaaSoft.Helpers;
using System.Diagnostics;
using System.Threading;

namespace aaaSoft.FtpClient.Forms
{
    public partial class MainForm : Form
    {
        #region 变量
        //站点配置文件名
        public const String FtpSitesFileName = "FtpSites.xml";

        //FTP站点集合
        public static FtpSiteDataGroup RootSiteGroup;
        //当前FTP站点数据
        public FtpSiteData CurrentFtpSiteData;

        //当前FTP站点的当前工作目录
        public String CurrentFolderPath;
        #endregion

        UnitStringConverting storageUnitStringConverting = UnitStringConverting.StorageUnitStringConverting;
        //状态的显示传输进度的控件列表
        private List<ToolStripItem> StatusBarToolStripItems = new List<ToolStripItem>();
        //当前传输队列对象
        public TransferQueue CurrentTransferQueue;
        //传输队列开始时的远端目录路径
        public String RemoteFolderPathWhenQueueStart;

        //日志窗体最大行数
        public Int32 LogWindowsMaxLinesCount = 100;

        #region 主窗体 - MainFrom构造函数
        public MainForm()
        {
            InitializeComponent();
            LogHelper.LogPushed += new LogHelper.LogEventHandler(LogHelper_LogPushed);
            CurrentTransferQueue = new TransferQueue();
            CurrentTransferQueue.QueueAdded += new TransferQueue.QueueEventHandler(CurrentTransferQueue_QueueAdded);
            CurrentTransferQueue.QueueRemoved += new TransferQueue.QueueEventHandler(CurrentTransferQueue_QueueRemoved);
            CurrentTransferQueue.QueueStarted += new TransferQueue.QueueEventHandler(CurrentTransferQueue_QueueStarted);
            CurrentTransferQueue.QueueCompleted += new TransferQueue.QueueEventHandler(CurrentTransferQueue_QueueCompleted);
            CurrentTransferQueue.QueueStoped += new TransferQueue.QueueEventHandler(CurrentTransferQueue_QueueStoped);
            CurrentTransferQueue.QueueProgressUpdated += new EventHandler(CurrentTransferQueue_QueueProgressUpdated);
            CurrentFolderPath = "/";
        }


        #region 主窗体 - 当前队列增加对象时
        void CurrentTransferQueue_QueueAdded(object sender, TransferQueue.QueueEventArgs e)
        {
            var tq = sender as TransferQueue;

            this.Invoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                {
                    btnStartQueue.Enabled = tq.GetQueueItemCount() != 0 && tq.QueueState != TransferQueue.TransferQueueState.Running;

                    Int32 itemIndex = lvQueue.Items.Count - CurrentTransferQueue.GetTransferQueueItemBackIndex(e.QueueItem);
                    if (itemIndex > lvQueue.Items.Count || itemIndex < 0)
                        return;

                    var item = e.QueueItem;
                    var newLvi = lvQueue.Items.Insert(itemIndex, "");

                    if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Download)
                    {
                        newLvi.Text = item.RemoteBaseFile.FullName;
                        newLvi.SubItems.Add(item.LocalPath);
                        if (item.RemoteBaseFile is FtpBaseFileInfo)
                            newLvi.SubItems.Add(storageUnitStringConverting.GetString(item.RemoteBaseFile.Length, 2, false) + "B");
                        else
                            newLvi.SubItems.Add("<文件夹>");
                        newLvi.SubItems.Add(String.Format("从 {0} 下载", item.SiteData.HostName));
                        newLvi.Tag = item;
                        newLvi.ImageKey = "Download";
                    }
                    else if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Upload)
                    {
                        newLvi.Text = item.LocalPath;
                        newLvi.SubItems.Add(item.RemotePath);
                        if (File.Exists(item.LocalPath))
                        {
                            var fileInfo = new FileInfo(item.LocalPath);
                            newLvi.SubItems.Add(storageUnitStringConverting.GetString(fileInfo.Length, 2, false) + "B");                            
                        }
                        else if (Directory.Exists(item.LocalPath))
                        {
                            var folderInfo = new DirectoryInfo(item.LocalPath);
                            newLvi.SubItems.Add("<文件夹>");
                        }
                        newLvi.SubItems.Add(String.Format("上传到 {0}", item.SiteData.HostName));
                        newLvi.Tag = item;
                        newLvi.ImageKey = "Upload";
                    }
                }));
        }
        #endregion

        #region 主窗体 - 当前队列移除对象时
        void CurrentTransferQueue_QueueRemoved(object sender, TransferQueue.QueueEventArgs e)
        {
            var tq = sender as TransferQueue;

            this.BeginInvoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
            {
                btnStartQueue.Enabled = tq.GetQueueItemCount() != 0 && tq.QueueState != TransferQueue.TransferQueueState.Running;

                ListViewItem lvi = null;
                foreach (ListViewItem newLvi in lvQueue.Items)
                {
                    if (e.QueueItem.Equals(newLvi.Tag))
                    {
                        lvi = newLvi;
                        break;
                    }
                }

                if (lvi == null) return;

                if (e.QueueItem.State == TransferQueueItem.TransferQueueItemStateEnum.Error)
                {
                    lvi.ImageKey = "Error";
                    LogHelper.PushLog(e.QueueItem.Tip, Color.Red);
                    return;
                }
                else if (e.QueueItem.State == TransferQueueItem.TransferQueueItemStateEnum.TransferComplete)
                {
                    var item = e.QueueItem;
                    var siteData = item.SiteData;
                    var site = item.SiteData.GetFtpSite();

                    //显示传输成功日志
                    if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Download)
                    {
                        if (item.RemoteBaseFile is FtpBaseFileInfo)
                        {
                            var baseFile = item.RemoteBaseFile;
                            String logStr = String.Format("已传送: {0} {1} 于 {2} 秒 ({3}/秒)"
                                                , baseFile.Name
                                                , storageUnitStringConverting.GetString(baseFile.Length, 2, false) + "B"
                                                , site.TransferUsedTime.TotalSeconds.ToString("N2")
                                                , storageUnitStringConverting.GetString(site.AverageTransferSpeed, 1, false) + "B");
                            LogHelper.PushLog(logStr, Color.FromArgb(128, 0, 0));
                        }
                    }
                    else if (item.Type == TransferQueueItem.TransferQueueItemTypeEnum.Upload)
                    {
                        if (Directory.Exists(item.LocalPath))
                        {
                            var remoteFileName = aaaSoft.Helpers.IoHelper.GetFileOrFolderName(item.RemotePath, '/');
                            String logStr = String.Format("已传送: {0} {1} 于 {2} 秒 ({3}/秒)"
                                , remoteFileName
                                , storageUnitStringConverting.GetString(site.TotalDataLength, 2, false) + "B"
                                , site.TransferUsedTime.TotalSeconds.ToString("N2")
                                , storageUnitStringConverting.GetString(site.AverageTransferSpeed, 1, false) + "B");
                            LogHelper.PushLog(logStr, Color.FromArgb(128, 0, 0));
                        }
                    }
                }
                //将此对象从lvQueue中移除
                lvQueue.Items.Remove(lvi);
            }));
        }
        #endregion

        #region 主窗体 - 当前队列启动时
        void CurrentTransferQueue_QueueStarted(object sender, TransferQueue.QueueEventArgs e)
        {
            RemoteFolderPathWhenQueueStart = CurrentFolderPath;
            ShowStatusToolStripItems();
            this.BeginInvoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                {
                    btnStartQueue.Enabled = false;
                    btnStopQueue.Enabled = true;
                }));
        }
        #endregion

        #region 主窗体 - 当前队列完成时
        void CurrentTransferQueue_QueueCompleted(object sender, TransferQueue.QueueEventArgs e)
        {
            var tq = sender as TransferQueue;

            HidenStatusToolStripItems();
            this.BeginInvoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
            {
                btnStartQueue.Enabled = lvQueue.Items.Count != 0;
                btnStopQueue.Enabled = false;
            }));
            BeginListFolder(RemoteFolderPathWhenQueueStart);
            LogHelper.PushLog("传送队列已完成", Color.FromArgb(128, 0, 0));
            String LogStr = String.Format(
                "已传送 {0} 个文件, 总计 {1} 于 {2} 秒 ({3}/秒)"
                , tq.TransferedFileCount
                , storageUnitStringConverting.GetString(tq.TransferedDataLength, 2, false) + "B"
                , tq.TransferUsedTime.TotalSeconds.ToString("N2")
                , storageUnitStringConverting.GetString(tq.AverageTransferSpeed, 2, false) + "B"
                );
            LogHelper.PushLog(LogStr, Color.FromArgb(128, 0, 0));
        }
        #endregion

        #region 主窗体 - 当前队列停止时
        void CurrentTransferQueue_QueueStoped(object sender, TransferQueue.QueueEventArgs e)
        {
            var tq = sender as TransferQueue;

            HidenStatusToolStripItems();
            this.BeginInvoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                {
                    btnStartQueue.Enabled = lvQueue.Items.Count != 0;
                    btnStopQueue.Enabled = false;
                }));
            LogHelper.PushLog("用户中止", Color.Red);
        }
        #endregion

        #region 主窗体 - 当前队列进度更新时
        void CurrentTransferQueue_QueueProgressUpdated(object sender, EventArgs e)
        {
            TransferQueue tq = sender as TransferQueue;
            if (tq == null || tq.CurrentSiteData == null)
                return;
            aaaSoft.Net.Ftp.FtpClient site = tq.CurrentSiteData.GetFtpSite();
            Double progressDouble = site.TransferProgress;
            Int32 progressInt32 = Convert.ToInt32(progressDouble * (pbTransferProgress.Maximum - pbTransferProgress.Minimum) + pbTransferProgress.Minimum);
            if (progressInt32 < pbTransferProgress.Minimum) progressInt32 = pbTransferProgress.Minimum;
            if (progressInt32 > pbTransferProgress.Maximum) progressInt32 = pbTransferProgress.Maximum;

            this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate()
            {
                pbTransferProgress.Value = progressInt32;
                lblTransferUsedTime.Text = String.Format("耗时:{0}", site.TransferUsedTime.ToString().Substring(0, 8));
                lblTransferProgressText.Text = String.Format("{0}%", progressInt32);
                lblTransferLeftTime.Text = storageUnitStringConverting.GetString(site.ImmediateTransferSpeed, 2, false) + "B" + "/秒";                
            }));
        }
        #endregion

        #endregion

        #region 主窗体 - MainFrom窗体加载时
        private void MainForm_Load(object sender, EventArgs e)
        {
            StatusBarToolStripItems.AddRange(new ToolStripItem[]
            {
                spTransferProgress,
                pbTransferProgress,
                spTransferProgressText,
                lblTransferProgressText,
                spTransferUsedTime,
                lblTransferUsedTime,
                spTransferLeftTime,
                lblTransferLeftTime,
                spQueueUsedTime,
                lblQueueUsedTime,
            });

            HidenStatusToolStripItems();
            this.Text += String.Format(" (版本号:{0})", Application.ProductVersion);
            SetLvRemoteFileGray(true);
            ReadSetting();
        }
        #endregion

        #region 主窗体 - MainForm窗体准备关闭时
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
            Environment.Exit(0);
        }
        #endregion

        #region 主窗体 - 方法 - 加载设置
        /// <summary>
        /// 加载设置
        /// </summary>
        public void ReadSetting()
        {
            FtpSiteDataGroup tmpGroup = null;

            if (File.Exists(FtpSitesFileName))
            {
                //读取站点信息
                Byte[] buffer = File.ReadAllBytes(FtpSitesFileName);
                tmpGroup = SerializeHelper.XmlDeserializeObject(buffer, typeof(FtpSiteDataGroup)) as FtpSiteDataGroup;
            }

            if (tmpGroup == null)
            {
                RootSiteGroup = new FtpSiteDataGroup() { Name = "根" };
            }
            else
            {
                SetFtpSiteDataGroupRelationship(tmpGroup);
                RootSiteGroup = tmpGroup;
            }
        }
        private void SetFtpSiteDataGroupRelationship(FtpSiteDataGroup group)
        {
            foreach (var subsite in group.Sites)
            {
                if (!String.IsNullOrEmpty(subsite.Password))
                    subsite.Password = CryptographyHelper.DesDecrypt(subsite.Password, subsite.Name);
                subsite.Group = group;
            }
            foreach (var subgroup in group.Groups)
            {
                subgroup.Group = group;
                SetFtpSiteDataGroupRelationship(subgroup);
            }
        }
        #endregion

        #region 主窗体 - 方法 - 保存设置
        public void SaveSetting()
        {
            ClearFtpSiteDataGroupRelationship(RootSiteGroup);

            String xml = Encoding.UTF8.GetString(SerializeHelper.XmlSerializeObject(RootSiteGroup));
            File.WriteAllText(FtpSitesFileName, xml);
        }

        private void ClearFtpSiteDataGroupRelationship(FtpSiteDataGroup group)
        {
            group.Group = null;
            foreach (var subgroup in group.Groups)
            {
                ClearFtpSiteDataGroupRelationship(subgroup);
            }
            foreach (var subsite in group.Sites)
            {
                if (!String.IsNullOrEmpty(subsite.Password))
                    subsite.Password = CryptographyHelper.DesEncrypt(subsite.Password, subsite.Name);
                subsite.Group = null;
            }
        }
        #endregion

        #region 主窗体 - 方法 - 显示/隐藏状态栏的控件
        private void _ShowStatusToolStripItems()
        {
            foreach (var item in StatusBarToolStripItems)
                item.Visible = true;
        }
        /// <summary>
        /// 显示正文传输进度相关的控件
        /// </summary>
        public void ShowStatusToolStripItems()
        {
            if (aaaSoft.Helpers.ThreadHelper.IsCurrentThreadMainThread())
                _ShowStatusToolStripItems();
            else
                this.BeginInvoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                    {
                        _ShowStatusToolStripItems();
                    }));
        }

        private void _HidenStatusToolStripItems()
        {
            foreach (var item in StatusBarToolStripItems)
                item.Visible = false;

        }
        /// <summary>
        /// 隐藏正文传输进度相关的控件
        /// </summary>
        public void HidenStatusToolStripItems()
        {
            if (aaaSoft.Helpers.ThreadHelper.IsCurrentThreadMainThread())
                _HidenStatusToolStripItems();
            else
                this.BeginInvoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                {
                    _HidenStatusToolStripItems();
                }));
        }
        #endregion


        #region 日志框 - LogHelper当日志推送时
        private void PushLog(LogHelper.LogEventArgs e)
        {
            int StartIndex = rtbLog.TextLength;
            rtbLog.AppendText(e.LogText);
            rtbLog.AppendText(Environment.NewLine);
            int Length = rtbLog.TextLength - StartIndex;
            rtbLog.Select(StartIndex, Length);
            rtbLog.SelectionColor = e.LogColor;

            //如果日志行数超过最大值
            if (rtbLog.Lines.Length > LogWindowsMaxLinesCount)
            {
                for (int i = 0; i <= 50; i++)
                {
                    Int32 clIndex = rtbLog.Text.IndexOf("\n");
                    if (clIndex < 0) break;
                    rtbLog.Select(0, clIndex + 1);
                    rtbLog.SelectedText = "\0";
                }
            }
            rtbLog.Select(rtbLog.TextLength - 1, 1);
            rtbLog.ScrollToCaret();
        }
        void LogHelper_LogPushed(LogHelper.LogEventArgs e)
        {
            if (aaaSoft.Helpers.ThreadHelper.IsCurrentThreadMainThread())
            {
                PushLog(e);
            }
            else
            {
                this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate()
                {
                    PushLog(e);
                }));
            }
        }
        #endregion

        #region 日志框 - 菜单相关
        private void 复制到剪贴板CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbLog.SelectAll();
            rtbLog.Copy();
            rtbLog.Select(rtbLog.TextLength - 1, 1);
            rtbLog.ScrollToCaret();
        }

        private void 保存到文本文件TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sdf = new SaveFileDialog();
                sdf.Filter = "文本文件(*.txt)|*.txt";
                var dr = sdf.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.Cancel) return;
                String fileName = sdf.FileName;
                rtbLog.SaveFile(fileName, RichTextBoxStreamType.TextTextOleObjs);
            }
            catch (Exception ex)
            {
                FormHelper.ShowWarningDialog("保存日志文件时出错，原因：" + ex.Message);
            }
        }

        private void 保存到RTF文件RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sdf = new SaveFileDialog();
                sdf.Filter = "RTF文件(*.rtf)|*.rtf";
                var dr = sdf.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.Cancel) return;
                String fileName = sdf.FileName;
                rtbLog.SaveFile(fileName, RichTextBoxStreamType.RichNoOleObjs);
            }
            catch (Exception ex)
            {
                FormHelper.ShowWarningDialog("保存日志文件时出错，原因：" + ex.Message);
            }
        }

        private void 清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }
        #endregion



        #region 主菜单 - 退出
        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region 主菜单 - 站点管理
        private void 站点管理SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new SiteManageForm();
            frm.faForm = this;
            frm.ShowDialog();
        }
        #endregion

        #region 主菜单 - 关于
        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }
        #endregion



        #region 本地文件列表 - 刷新本地目录
        private void btnLocalRefresh_Click(object sender, EventArgs e)
        {
            fbLocal.RefreshCurrentFolder();
        }
        #endregion

        #region 远端文件列表 - 当开始登录时
        void site_LoginBegin(object sender, aaaSoft.Net.Ftp.FtpClient.FtpSiteEventArgs e)
        {
            var site = sender as aaaSoft.Net.Ftp.FtpClient;
            LogHelper.PushLog(String.Format("正在连接到 {0}", site.HostName), Color.Green);
        }
        #endregion

        #region 远端文件列表 - 当登录完成时
        void site_LoginComplete(object sender, aaaSoft.Net.Ftp.FtpClient.FtpSiteEventArgs e)
        {
            if (CurrentFtpSiteData == null)
                return;
            var site = sender as aaaSoft.Net.Ftp.FtpClient;
            this.Invoke(new ThreadHelper.UnnamedDelegate(delegate
            {
                if (site.IsLogin)
                {
                    btnDisconnect.Enabled = true;
                    //列出指定的远端路径
                    if (e.ShouldListFolder)
                        BeginListFolder(CurrentFtpSiteData.RemotePath);
                    //列出指定的本地路径
                    if (!String.IsNullOrEmpty(CurrentFtpSiteData.LocalPath))
                        fbLocal.ListFolder(CurrentFtpSiteData.LocalPath);
                }
                else
                {
                    LogHelper.PushLog("连接失败", Color.Red);
                    btnQuickConnect.Enabled = true;
                }
                SetLvRemoteFileGray(true);
            }));
        }
        #endregion

        #region 远端文件列表 - 当列目录完成时
        void site_ListFolderComplete(object sender, aaaSoft.Net.Ftp.FtpClient.FtpSiteEventArgs e)
        {
            var site = sender as aaaSoft.Net.Ftp.FtpClient;
            var BaseFileList = e.BaseFileList;

            this.Invoke(new ThreadHelper.UnnamedDelegate(delegate
            {
                CurrentFolderPath = site.CurrentDirectoryPath;
                txtAddress.Text = CurrentFolderPath;
                lvRemoteFile.Items.Clear();

                if (BaseFileList == null)
                    return;

                foreach (var BaseFile in BaseFileList)
                {
                    var newLvi = lvRemoteFile.Items.Add(BaseFile.Name);
                    if (BaseFile.IsFolder)
                    {
                        newLvi.SubItems.Add("");
                        newLvi.ImageIndex = 0;
                    }
                    else if (BaseFile is FtpBaseFileInfo)
                    {
                        newLvi.SubItems.Add(storageUnitStringConverting.GetString(BaseFile.Length, 0, false) + "B");
                        newLvi.ImageIndex = 1;
                    }
                    newLvi.SubItems.Add(BaseFile.LastModifyTime.ToString());
                    newLvi.SubItems.Add(BaseFile.Property);
                    newLvi.Tag = BaseFile;
                }
                SetLvRemoteFileGray(false);
            }));
        }
        #endregion

        #region 远端文件列表 - 当线程阻塞时
        void site_ThreadBlocked(object sender, aaaSoft.Net.Ftp.FtpClient.FtpSiteEventArgs e)
        {
            if (aaaSoft.Helpers.ThreadHelper.IsCurrentThreadMainThread())
                Application.DoEvents();
        }
        #endregion

        #region 远端文件列表 - 点击“快速连接”按钮时
        public void btnQuickConnect_Click(object sender, EventArgs e)
        {
            var rect = btnQuickConnect.Bounds;
            Point pt = new Point(rect.Left, rect.Bottom);
            pt = btnQuickConnect.GetCurrentParent().PointToScreen(pt);
            cmsQuickConnect.Show(pt);
        }

        void site_RequestEvent(object sender, aaaSoft.Net.Ftp.Core.FtpCommandData e)
        {
            LogHelper.PushLog(e.ToString(), Color.Black);
        }

        void site_ResponseEvent(object sender, aaaSoft.Net.Ftp.Core.FtpCommandData e)
        {
            LogHelper.PushLog(e.ToString(), Color.Green);
        }

        void site_ConnectionClosedEvent(object sender, EventArgs e)
        {
            this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate()
            {
                var site = sender as aaaSoft.Net.Ftp.FtpClient;
                if (site == null) return;

                btnQuickConnect.Enabled = true;
                SetLvRemoteFileGray(true);

                LogHelper.PushLog(String.Format("连接丢失: {0}", site.HostName), Color.Green);
            }));
        }
        #endregion

        #region 远端文件列表 - 双击列表
        private void lvServer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvRemoteFile.SelectedItems.Count != 1) return;
            var newLvi = lvRemoteFile.SelectedItems[0];
            var baseFile = newLvi.Tag as FtpBaseFileInfo;

            if (baseFile.IsFolder)
            {
                BeginListFolder(baseFile.FullName);
            }
        }
        #endregion

        #region 远端文件列表 - 点击“刷新”按钮
        private void btnRefrush_Click(object sender, EventArgs e)
        {
            RefreshRemote();
        }
        #endregion

        #region 远端文件列表 - 点击“向上”按钮
        private void btnUp_Click(object sender, EventArgs e)
        {
            var tmpPath = aaaSoft.Helpers.IoHelper.GetParentPath(CurrentFolderPath, '/');
            BeginListFolder(tmpPath);
        }
        #endregion

        #region 远端文件列表 - 拖放
        private void lvServer_DragEnter(object sender, DragEventArgs e)
        {
            String[] PathArray = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (PathArray == null) return;

            e.Effect = DragDropEffects.Copy;
        }

        /*
        private void TempUploadThread(Object obj)
        {
            var strs = obj as String[];
            var localFilePath = strs[0];
            var remoteFileName = strs[1];

            var ftpSite = CurrentFtpSiteData.GetFtpSite();
            ftpSite.UploadFile(remoteFileName, localFilePath);
            String logStr = String.Format("已传送: {0} {1} 于 {2} 秒 ({3}/秒)"
                                        , remoteFileName
                                        , IoHelper.GetFileLengthLevelString(ftpSite.TotalDataLength, 2)
                                        , ftpSite.TransferUsedTime.TotalSeconds.ToString("N2")
                                        , IoHelper.GetFileLengthLevelString(Convert.ToInt64(ftpSite.AverageTransferSpeed), 1));
            LogHelper.PushLog(logStr, Color.FromArgb(128, 0, 0));
            this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate
            {
                HidenStatusToolStripItems();
            }));
            RefreshRemote();
        }
         */

        private void lvServer_DragDrop(object sender, DragEventArgs e)
        {
            if (CurrentFtpSiteData == null)
                return;
            String[] PathArray = e.Data.GetData(DataFormats.FileDrop) as String[];

            foreach (var path in PathArray)
            {
                var subItem = new TransferQueueItem(CurrentFtpSiteData);
                subItem.Type = TransferQueueItem.TransferQueueItemTypeEnum.Upload;
                subItem.LocalPath = path;
                subItem.RemotePath = (CurrentFolderPath + "/" + IoHelper.GetFileOrFolderName(path, Path.DirectorySeparatorChar)).Replace("//", "/");
                CurrentTransferQueue.AddToQueue(subItem);
            }
            CurrentTransferQueue.StartQueue();
        }
        #endregion

        #region 远程文件列表 - 菜单打开时
        private void cmsRemote_Opening(object sender, CancelEventArgs e)
        {
            String FolderPath = fbLocal.CurrentFolderPath;
            bool CurrentLocalFolderExist = Directory.Exists(FolderPath);

            switch (lvRemoteFile.SelectedItems.Count)
            {
                case 0:
                    传送TToolStripMenuItem.Enabled = false;
                    添加到队列QToolStripMenuItem.Enabled = false;
                    删除DToolStripMenuItem.Enabled = false;
                    重命名RToolStripMenuItem.Enabled = false;
                    属性PToolStripMenuItem.Enabled = false;
                    break;
                case 1:
                    传送TToolStripMenuItem.Enabled = CurrentLocalFolderExist;
                    添加到队列QToolStripMenuItem.Enabled = CurrentLocalFolderExist;
                    删除DToolStripMenuItem.Enabled = true;
                    重命名RToolStripMenuItem.Enabled = true;
                    属性PToolStripMenuItem.Enabled = true;
                    break;
                default:
                    传送TToolStripMenuItem.Enabled = CurrentLocalFolderExist;
                    添加到队列QToolStripMenuItem.Enabled = CurrentLocalFolderExist;
                    删除DToolStripMenuItem.Enabled = true;
                    重命名RToolStripMenuItem.Enabled = false;
                    属性PToolStripMenuItem.Enabled = false;
                    break;
            }
            if (CurrentFtpSiteData == null)
            {
                新建文件夹NToolStripMenuItem.Enabled = false;
                刷新FToolStripMenuItem.Enabled = false;
            }
            else
            {
                新建文件夹NToolStripMenuItem.Enabled = true;
                刷新FToolStripMenuItem.Enabled = true;
            }
        }
        #endregion

        #region 远端文件列表 - 传送
        private void 传送TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            添加到队列QToolStripMenuItem_Click(sender, e);
            CurrentTransferQueue.StartQueue();
        }
        #endregion

        #region 远端文件列表 - 添加到队列
        private void 添加到队列QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String FolderPath = fbLocal.CurrentFolderPath;
            if (!Directory.Exists(FolderPath))
            {
                LogHelper.PushLog(String.Format("路径[{0}]不存在，无法下载！", FolderPath), Color.Red);
                return;
            }

            List<FtpBaseFileInfo> BaseFileList = new List<FtpBaseFileInfo>();
            foreach (ListViewItem tmpLvi in lvRemoteFile.SelectedItems)
            {
                BaseFileList.Add(tmpLvi.Tag as FtpBaseFileInfo);
            }

            foreach (var baseFile in BaseFileList)
            {
                var subItem = new TransferQueueItem(CurrentFtpSiteData);
                subItem.Type = TransferQueueItem.TransferQueueItemTypeEnum.Download;
                subItem.RemoteBaseFile = baseFile;
                subItem.RemotePath = baseFile.FullName;
                subItem.LocalPath = Path.Combine(FolderPath, baseFile.Name);
                CurrentTransferQueue.AddToQueue(subItem);
            }
        }
        #endregion

        #region 远端文件列表 - 删除
        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("删除所选对象，你确定吗?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;

            List<FtpBaseFileInfo> BaseFileList = new List<FtpBaseFileInfo>();
            foreach (ListViewItem newLvi in lvRemoteFile.SelectedItems)
            {
                var baseFile = newLvi.Tag as FtpBaseFileInfo;
                BaseFileList.Add(baseFile);
            }
            SetLvRemoteFileGray(true);
            //将删除远端路径线程加入线程池
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    String preWorkDirectory = CurrentFolderPath;
                    //相关统计
                    var DeletedFileCount = 0;
                    var DeletedFolderCount = 0;
                    var DeletedContentLength = 0L;
                    var DeleteStartTime = DateTime.Now;

                    DeleteRemotePathList(BaseFileList, ref  DeletedFileCount, ref  DeletedFolderCount, ref  DeletedContentLength);

                    var LogStr = String.Format("已删除 {0} 个文件夹, {1} 个文件, 总计 {2} 于 {3} 秒。"
                                    , DeletedFolderCount
                                    , DeletedFileCount
                                    , storageUnitStringConverting.GetString(DeletedContentLength, 2, false) + "B"
                                    , (DateTime.Now - DeleteStartTime).TotalSeconds.ToString("N2")
                                    );
                    LogHelper.PushLog(LogStr, Color.FromArgb(128, 0, 0));
                    BeginListFolder(preWorkDirectory);
                }));
        }

        #region 删除远端路径
        private bool DeleteRemotePathList(List<FtpBaseFileInfo> BaseFileList, ref Int32 DeletedFileCount, ref Int32 DeletedFolderCount, ref Int64 DeletedContentLength)
        {
            List<FtpBaseFileInfo> FileInfoList = new List<FtpBaseFileInfo>();
            List<FtpBaseFileInfo> FolderInfoList = new List<FtpBaseFileInfo>();

            foreach (var baseFile in BaseFileList)
            {
                if (baseFile.IsFolder)
                    FolderInfoList.Add(baseFile);
                else if (baseFile is FtpBaseFileInfo)
                    FileInfoList.Add(baseFile);
            }

            var client = CurrentFtpSiteData.GetFtpSite();


            //先删除文件
            foreach (var baseFile in FileInfoList)
            {
                if (client.DeleteFile(baseFile.FullName))
                {
                    DeletedFileCount++;
                    DeletedContentLength += baseFile.Length;
                }
            }

            //然后删除目录
            foreach (var baseFile in FolderInfoList)
            {
                //删除目录下的目录和文件
                var SubBaseFileList = client.ListDirectory(baseFile.FullName);
                if (SubBaseFileList == null)
                    continue;
                if (!DeleteRemotePathList(SubBaseFileList, ref  DeletedFileCount, ref  DeletedFolderCount, ref  DeletedContentLength))
                    return false;
                //返回到要删除目录的上层目录
                client.ListDirectory(IoHelper.GetParentPath(baseFile.FullName, '/'));
                //删除目录
                if (client.RemoveDirectory(baseFile.FullName))
                {
                    DeletedFolderCount++;
                }
            }
            return true;
        }
        #endregion
        #endregion

        #region 远端文件列表 - 重命名
        private void 重命名RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem newLvi = lvRemoteFile.SelectedItems[0];
            var baseFile = newLvi.Tag as FtpBaseFileInfo;

            InputForm frmInput = new InputForm("重命名", "请输入新的名称:", baseFile.Name);
            var dr = frmInput.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel) return;
            String newFolderName = frmInput.Value;

            var client = CurrentFtpSiteData.GetFtpSite();

            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    if (!client.Rename(baseFile.FullName, newFolderName))
                        return;

                    RefreshRemote();
                }));
        }
        #endregion

        #region 远端文件列表 - 属性
        private void 属性PToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 远端文件列表 - 新建文件夹
        private void 新建文件夹NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm frmInput = new InputForm("新建文件夹", "请输入新文件夹的名称:", "NewFolder");
            var dr = frmInput.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel) return;
            String newFolderName = frmInput.Value;
            if (!newFolderName.StartsWith("/"))
                newFolderName = CurrentFolderPath + "/" + newFolderName;

            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    var site = CurrentFtpSiteData.GetFtpSite();
                    if (!site.CreateDirectory(newFolderName))
                    {
                        return;
                    }
                    RefreshRemote();
                }));
        }
        #endregion

        #region 远端文件列表 - 点击“刷新”按钮时
        private void btnRemoteRefresh_Click(object sender, EventArgs e)
        {
            刷新FToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region 远端文件列表 - 点击“开始传送”按钮时
        private void btnStartQueue_Click(object sender, EventArgs e)
        {
            List<ListViewItem> ErrorLviList = new List<ListViewItem>();
            foreach (ListViewItem lvi in lvQueue.Items)
            {
                var item = lvi.Tag as TransferQueueItem;
                if (item.State == TransferQueueItem.TransferQueueItemStateEnum.Error)
                    ErrorLviList.Add(lvi);
            }
            foreach (var lvi in ErrorLviList)
            {
                RemoveListViewItem(lvi);
                CurrentTransferQueue.AddToQueue(lvi.Tag as TransferQueueItem);
            }
            CurrentTransferQueue.StartQueue();
        }
        #endregion

        #region 远端文件列表 - 点击“停止传送”按钮时
        private void btnStopQueue_Click(object sender, EventArgs e)
        {
            CurrentTransferQueue.StopQueue();
            btnStopQueue.Enabled = false;
        }
        #endregion

        #region 远端文件列表 - 点击“刷新”菜单时
        private void 刷新FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshRemote();
        }
        #endregion

        #region 远端文件列表 - 地址输入框_KeyUp
        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BeginListFolder(txtAddress.Text.Trim());
            }
        }
        #endregion

        #region 远端文件列表 - lvRemoeFile键盘按下时
        private void lvRemoteFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentFtpSiteData == null) return;
            switch (e.KeyCode)
            {
                case Keys.F2:
                    重命名RToolStripMenuItem_Click(sender, e);
                    break;
                case Keys.Delete:
                    删除DToolStripMenuItem_Click(sender, e);
                    break;
                case Keys.Back:
                    btnUp_Click(sender, e);
                    break;
            }
        }
        #endregion

        #region 远端文件列表 - 快速连接菜单打开时
        private void cmsQuickConnect_Opening(object sender, CancelEventArgs e)
        {
            站点ToolStripMenuItem.DropDownItems.Clear();
            AddItemToToolStripMenu(站点ToolStripMenuItem, RootSiteGroup);
        }

        #region 添加菜单对象到菜单中
        private void AddItemToToolStripMenu(ToolStripMenuItem item, FtpSiteDataGroup group)
        {
            foreach (var subgroup in group.Groups)
            {
                ToolStripMenuItem tsi = new ToolStripMenuItem(subgroup.Name);
                tsi.Image = ilSites.Images["Group"];
                tsi.Tag = subgroup;
                item.DropDownItems.Add(tsi);
                AddItemToToolStripMenu(tsi, subgroup);
            }
            foreach (var site in group.Sites)
            {
                ToolStripMenuItem tsi = new ToolStripMenuItem(site.Name);
                tsi.Image = ilSites.Images["Site"];
                tsi.Tag = site;
                item.DropDownItems.Add(tsi);
                tsi.Click += new EventHandler(tsi_Click);
            }
        }

        void tsi_Click(object sender, EventArgs e)
        {
            var tsi = sender as ToolStripMenuItem;
            var site = tsi.Tag as FtpSiteData;
            if (site == null) return;
            ConnectToSite(site);
        }
        #endregion

        #endregion

        #region 远端文件列表 - 断开连接
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (CurrentFtpSiteData == null)
                return;

            var site = CurrentFtpSiteData.GetFtpSite();
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    site.Quit();
                }));

            btnDisconnect.Enabled = false;
            CurrentFtpSiteData.ClearFtpSite();
            CurrentFtpSiteData = null;
            CurrentFolderPath = "";
            txtAddress.Text = "";
            lvRemoteFile.Items.Clear();
            UnbindCurrentFtpSiteEvent();
        }
        #endregion

        #region 远端文件列表 - 放弃
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CurrentFtpSiteData == null) return;

            btnStopQueue_Click(sender, e);
            var site = CurrentFtpSiteData.GetFtpSite();
            site.ForceCloseDataConnection();
        }
        #endregion

        #region 远端文件列表 - 方法 - 是否设置lvRemoteFile控件为灰色
        private void SetLvRemoteFileGray(bool IsGray)
        {
            if (IsGray)
                lvRemoteFile.BackColor = SystemColors.Control;
            else
                lvRemoteFile.BackColor = SystemColors.Window;
        }
        #endregion

        #region  远端文件列表 - 方法 - 刷新远端文件列表
        public void RefreshRemote()
        {
            BeginListFolder(CurrentFolderPath);
        }
        #endregion

        #region 远端文件列表 - 方法 - 转到远端目录
        /// <summary>
        /// 转到目录(异步)
        /// </summary>
        /// <param name="folderPath"></param>
        private void BeginListFolder(String folderPath)
        {
            if (CurrentFtpSiteData == null) return;
            var client = CurrentFtpSiteData.GetFtpSite();

            CurrentFolderPath = folderPath;
            if (String.IsNullOrEmpty(CurrentFolderPath))
            {
                CurrentFolderPath = client.BaseDirectoryPath;
            }

            this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate
            {
                SetLvRemoteFileGray(true);
            }));

            try
            {
                client.BeginListDirectory(CurrentFolderPath);
            }
            catch (Exception ex)
            {
                LogHelper.PushLog(ex.Message, Color.Red);
            }
        }
        #endregion

        #region 远端文件列表 - 方法 - 异步连接到远端服务器
        public void ConnectToSite(FtpSiteData siteData)
        {
            UnbindCurrentFtpSiteEvent();

            CurrentFtpSiteData = siteData;
            this.Invoke(new ThreadHelper.UnnamedDelegate(delegate
            {
                btnQuickConnect.Enabled = false;
                SetLvRemoteFileGray(false);

                BindCurrentFtpSiteEvent();
            }));

            var site = CurrentFtpSiteData.GetFtpSite();
            site.BeginLogin();
        }
        #endregion

        #region 远端文件列表 - 方法 - 绑定当前FtpSite的相关事件
        //绑定当前FTP连接的事件
        private void BindCurrentFtpSiteEvent()
        {
            var site = CurrentFtpSiteData.GetFtpSite();
            UnbindCurrentFtpSiteEvent();
            site.RequestEvent += new aaaSoft.Net.Ftp.FtpClient.FtpSiteCommandEventHandler(site_RequestEvent);
            site.ResponseEvent += new aaaSoft.Net.Ftp.FtpClient.FtpSiteCommandEventHandler(site_ResponseEvent);
            site.ConnectionClosedEvent += new EventHandler(site_ConnectionClosedEvent);

            site.LoginBegin += new aaaSoft.Net.Ftp.FtpClient.FtpSiteEventHandler(site_LoginBegin);
            site.LoginComplete += new aaaSoft.Net.Ftp.FtpClient.FtpSiteEventHandler(site_LoginComplete);
            site.ListFolderComplete += new aaaSoft.Net.Ftp.FtpClient.FtpSiteEventHandler(site_ListFolderComplete);
            site.ThreadBlocked += new aaaSoft.Net.Ftp.FtpClient.FtpSiteEventHandler(site_ThreadBlocked);
        }
        #endregion

        #region 远端文件列表 - 方法 - 解除绑定当前FTP连接的事件
        //解除绑定当前FTP连接的事件
        private void UnbindCurrentFtpSiteEvent()
        {
            if (CurrentFtpSiteData == null) return;
            var site = CurrentFtpSiteData.GetFtpSite();
            site.ClearEventBinding();
        }
        #endregion


        #region 传送队列窗口 - 传送队列窗口右键菜单
        private void cmsQueue_Opening(object sender, CancelEventArgs e)
        {
            foreach (ToolStripItem menuItem in cmsQueue.Items)
                menuItem.Enabled = false;

            if (CurrentFtpSiteData != null)
            {
                传送队列TToolStripMenuItem.Enabled = btnStartQueue.Enabled;
                删除ToolStripMenuItem.Enabled = lvQueue.SelectedItems.Count > 0;
#warning 载入队列、保存队列暂时不可用
                //载入队列LToolStripMenuItem.Enabled = CurrentTransferQueue.QueueState == TransferQueue.TransferQueueState.Stoped;
                //保存队列SToolStripMenuItem.Enabled = CurrentTransferQueue.QueueState == TransferQueue.TransferQueueState.Stoped;
                清除队列ToolStripMenuItem.Enabled = CurrentTransferQueue.GetQueueItemCount() > 0 && CurrentTransferQueue.QueueState != TransferQueue.TransferQueueState.Running;
            }
        }
        private void 传送队列TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStartQueue_Click(sender, e);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> deleteLviList = new List<ListViewItem>();
            foreach (ListViewItem lvi in lvQueue.SelectedItems)
                deleteLviList.Add(lvi);
            foreach (ListViewItem lvi in deleteLviList)
            {
                RemoveListViewItem(lvi);
            }
        }

        private void RemoveListViewItem(ListViewItem lvi)
        {
            var item = lvi.Tag as TransferQueueItem;
            if (item.State == TransferQueueItem.TransferQueueItemStateEnum.Ready)
                CurrentTransferQueue.RemoveFromQueue(item);
            else if (item.State == TransferQueueItem.TransferQueueItemStateEnum.Error)
                lvQueue.Items.Remove(lvi);
        }

        private void 载入队列LToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 保存队列SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "队列文件(*.xml)|*.xml";
            var dr = sfd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel) return;

            List<TransferQueueItem> QueueItemList = new List<TransferQueueItem>();
            foreach (ListViewItem lvi in lvQueue.Items)
                QueueItemList.Add(lvi.Tag as TransferQueueItem);
            String outStr = Encoding.UTF8.GetString(aaaSoft.Helpers.SerializeHelper.XmlSerializeObject(QueueItemList));
            var stream = sfd.OpenFile();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(outStr);
            writer.Flush();
            writer.Close();
        }

        private void 清除队列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> deleteLviList = new List<ListViewItem>();
            foreach (ListViewItem lvi in lvQueue.Items)
                deleteLviList.Add(lvi);
            foreach (ListViewItem lvi in deleteLviList)
            {
                RemoveListViewItem(lvi);
            }
            CurrentTransferQueue.ClearQueue();
        }
        #endregion
    }
}