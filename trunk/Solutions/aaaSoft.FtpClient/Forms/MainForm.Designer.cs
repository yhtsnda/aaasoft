namespace aaaSoft.FtpClient.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scView = new System.Windows.Forms.SplitContainer();
            this.tsLocalOperate = new System.Windows.Forms.ToolStrip();
            this.btnLocalRefresh = new System.Windows.Forms.ToolStripButton();
            this.fbLocal = new aaaSoft.Controls.FileBrowser();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.tsAddress = new System.Windows.Forms.ToolStrip();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.tsRemoteOperate = new System.Windows.Forms.ToolStrip();
            this.btnQuickConnect = new System.Windows.Forms.ToolStripButton();
            this.btnDisconnect = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnRemoteRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnStartQueue = new System.Windows.Forms.ToolStripButton();
            this.btnStopQueue = new System.Windows.Forms.ToolStripButton();
            this.lvRemoteFile = new aaaSoft.Controls.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChangeTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProperty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsRemote = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.传送TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加到队列QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.删除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.属性PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.新建文件夹NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilExplorer = new System.Windows.Forms.ImageList(this.components);
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.站点TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.站点管理SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.选项PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinEngine1 = new aaaSoft.SkinEngine.SkinEngine(this.components);
            this.themeButton1 = new aaaSoft.SkinEngine.SkinControls.ThemeButton();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scDisplay = new System.Windows.Forms.SplitContainer();
            this.lvQueue = new aaaSoft.Controls.ListView();
            this.chQueueName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chQueueTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chQueueSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chQueueRemark = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsQueue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.执行队列TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.载入队列LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存队列SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.清除队列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilQueue = new System.Windows.Forms.ImageList(this.components);
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.cmsLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制到剪贴板CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存到文本文件TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存到RTF文件RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.清除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.lblTransferState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblQueueUsedTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.spQueueUsedTime = new System.Windows.Forms.ToolStripSeparator();
            this.lblTransferLeftTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.spTransferLeftTime = new System.Windows.Forms.ToolStripSeparator();
            this.lblTransferUsedTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.spTransferUsedTime = new System.Windows.Forms.ToolStripSeparator();
            this.lblTransferProgressText = new System.Windows.Forms.ToolStripStatusLabel();
            this.spTransferProgressText = new System.Windows.Forms.ToolStripSeparator();
            this.pbTransferProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.spTransferProgress = new System.Windows.Forms.ToolStripSeparator();
            this.cmsQuickConnect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.快速连接QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.历史ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.站点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilSites = new System.Windows.Forms.ImageList(this.components);
            this.scView.Panel1.SuspendLayout();
            this.scView.Panel2.SuspendLayout();
            this.scView.SuspendLayout();
            this.tsLocalOperate.SuspendLayout();
            this.tsAddress.SuspendLayout();
            this.tsRemoteOperate.SuspendLayout();
            this.cmsRemote.SuspendLayout();
            this.msMain.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.scDisplay.Panel1.SuspendLayout();
            this.scDisplay.Panel2.SuspendLayout();
            this.scDisplay.SuspendLayout();
            this.cmsQueue.SuspendLayout();
            this.cmsLog.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.cmsQuickConnect.SuspendLayout();
            this.SuspendLayout();
            // 
            // scView
            // 
            this.scView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scView.Location = new System.Drawing.Point(0, 0);
            this.scView.Name = "scView";
            // 
            // scView.Panel1
            // 
            this.scView.Panel1.Controls.Add(this.tsLocalOperate);
            this.scView.Panel1.Controls.Add(this.fbLocal);
            // 
            // scView.Panel2
            // 
            this.scView.Panel2.Controls.Add(this.txtAddress);
            this.scView.Panel2.Controls.Add(this.tsAddress);
            this.scView.Panel2.Controls.Add(this.tsRemoteOperate);
            this.scView.Panel2.Controls.Add(this.lvRemoteFile);
            this.scView.Size = new System.Drawing.Size(908, 395);
            this.scView.SplitterDistance = 454;
            this.scView.TabIndex = 1;
            // 
            // tsLocalOperate
            // 
            this.tsLocalOperate.AutoSize = false;
            this.tsLocalOperate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsLocalOperate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLocalRefresh});
            this.tsLocalOperate.Location = new System.Drawing.Point(0, 0);
            this.tsLocalOperate.Name = "tsLocalOperate";
            this.tsLocalOperate.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsLocalOperate.Size = new System.Drawing.Size(452, 36);
            this.tsLocalOperate.TabIndex = 3;
            this.tsLocalOperate.Text = "toolStrip1";
            // 
            // btnLocalRefresh
            // 
            this.btnLocalRefresh.AutoSize = false;
            this.btnLocalRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalRefresh.Image")));
            this.btnLocalRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLocalRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLocalRefresh.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnLocalRefresh.Name = "btnLocalRefresh";
            this.btnLocalRefresh.Size = new System.Drawing.Size(36, 32);
            this.btnLocalRefresh.ToolTipText = "刷新目录";
            this.btnLocalRefresh.Click += new System.EventHandler(this.btnLocalRefresh_Click);
            // 
            // fbLocal
            // 
            this.fbLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fbLocal.Location = new System.Drawing.Point(0, 36);
            this.fbLocal.Name = "fbLocal";
            this.fbLocal.Size = new System.Drawing.Size(452, 357);
            this.fbLocal.StartUpFolder = System.Environment.SpecialFolder.Personal;
            this.fbLocal.TabIndex = 0;
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(23, 37);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(422, 21);
            this.txtAddress.TabIndex = 0;
            this.txtAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAddress_KeyUp);
            // 
            // tsAddress
            // 
            this.tsAddress.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsAddress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUp});
            this.tsAddress.Location = new System.Drawing.Point(0, 36);
            this.tsAddress.Name = "tsAddress";
            this.tsAddress.Size = new System.Drawing.Size(448, 25);
            this.tsAddress.TabIndex = 3;
            this.tsAddress.Text = "tsAddress";
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // tsRemoteOperate
            // 
            this.tsRemoteOperate.AutoSize = false;
            this.tsRemoteOperate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsRemoteOperate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnQuickConnect,
            this.btnDisconnect,
            this.btnCancel,
            this.btnRemoteRefresh,
            this.btnStartQueue,
            this.btnStopQueue});
            this.tsRemoteOperate.Location = new System.Drawing.Point(0, 0);
            this.tsRemoteOperate.Name = "tsRemoteOperate";
            this.tsRemoteOperate.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsRemoteOperate.Size = new System.Drawing.Size(448, 36);
            this.tsRemoteOperate.TabIndex = 2;
            this.tsRemoteOperate.Text = "toolStrip1";
            // 
            // btnQuickConnect
            // 
            this.btnQuickConnect.AutoSize = false;
            this.btnQuickConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnQuickConnect.Image")));
            this.btnQuickConnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnQuickConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnQuickConnect.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnQuickConnect.Name = "btnQuickConnect";
            this.btnQuickConnect.Size = new System.Drawing.Size(36, 32);
            this.btnQuickConnect.ToolTipText = "快速连接";
            this.btnQuickConnect.Click += new System.EventHandler(this.btnQuickConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.AutoSize = false;
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("btnDisconnect.Image")));
            this.btnDisconnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(0);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(36, 32);
            this.btnDisconnect.ToolTipText = "断开连接";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = false;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(36, 32);
            this.btnCancel.ToolTipText = "放弃";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRemoteRefresh
            // 
            this.btnRemoteRefresh.AutoSize = false;
            this.btnRemoteRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoteRefresh.Image")));
            this.btnRemoteRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRemoteRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoteRefresh.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnRemoteRefresh.Name = "btnRemoteRefresh";
            this.btnRemoteRefresh.Size = new System.Drawing.Size(36, 32);
            this.btnRemoteRefresh.ToolTipText = "刷新目录";
            this.btnRemoteRefresh.Click += new System.EventHandler(this.btnRemoteRefresh_Click);
            // 
            // btnStartQueue
            // 
            this.btnStartQueue.AutoSize = false;
            this.btnStartQueue.Enabled = false;
            this.btnStartQueue.Image = ((System.Drawing.Image)(resources.GetObject("btnStartQueue.Image")));
            this.btnStartQueue.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStartQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartQueue.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnStartQueue.Name = "btnStartQueue";
            this.btnStartQueue.Size = new System.Drawing.Size(36, 32);
            this.btnStartQueue.ToolTipText = "执行队列";
            this.btnStartQueue.Click += new System.EventHandler(this.btnStartQueue_Click);
            // 
            // btnStopQueue
            // 
            this.btnStopQueue.AutoSize = false;
            this.btnStopQueue.Enabled = false;
            this.btnStopQueue.Image = ((System.Drawing.Image)(resources.GetObject("btnStopQueue.Image")));
            this.btnStopQueue.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStopQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStopQueue.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnStopQueue.Name = "btnStopQueue";
            this.btnStopQueue.Size = new System.Drawing.Size(36, 32);
            this.btnStopQueue.ToolTipText = "当前执行项结束后停止队列";
            this.btnStopQueue.Click += new System.EventHandler(this.btnStopQueue_Click);
            // 
            // lvRemoteFile
            // 
            this.lvRemoteFile.AllowDrop = true;
            this.lvRemoteFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRemoteFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chSize,
            this.chChangeTime,
            this.chProperty});
            this.lvRemoteFile.ContextMenuStrip = this.cmsRemote;
            this.lvRemoteFile.FloatingColumnIndex = 0;
            this.lvRemoteFile.FullRowSelect = true;
            this.lvRemoteFile.HideSelection = false;
            this.lvRemoteFile.IsUseFloating = true;
            this.lvRemoteFile.LargeImageList = this.ilExplorer;
            this.lvRemoteFile.Location = new System.Drawing.Point(0, 61);
            this.lvRemoteFile.Name = "lvRemoteFile";
            this.lvRemoteFile.Size = new System.Drawing.Size(449, 333);
            this.lvRemoteFile.SmallImageList = this.ilExplorer;
            this.lvRemoteFile.TabIndex = 1;
            this.lvRemoteFile.UseCompatibleStateImageBehavior = false;
            this.lvRemoteFile.View = System.Windows.Forms.View.Details;
            this.lvRemoteFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvServer_DragDrop);
            this.lvRemoteFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvServer_DragEnter);
            this.lvRemoteFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvRemoteFile_KeyDown);
            this.lvRemoteFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvServer_MouseDoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "名称";
            this.chName.Width = 120;
            // 
            // chSize
            // 
            this.chSize.Text = "大小";
            this.chSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chChangeTime
            // 
            this.chChangeTime.Text = "修改";
            this.chChangeTime.Width = 140;
            // 
            // chProperty
            // 
            this.chProperty.Text = "属性";
            // 
            // cmsRemote
            // 
            this.cmsRemote.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.传送TToolStripMenuItem,
            this.添加到队列QToolStripMenuItem,
            this.toolStripMenuItem4,
            this.删除DToolStripMenuItem,
            this.重命名RToolStripMenuItem,
            this.属性PToolStripMenuItem,
            this.toolStripMenuItem2,
            this.新建文件夹NToolStripMenuItem,
            this.toolStripMenuItem3,
            this.刷新FToolStripMenuItem});
            this.cmsRemote.Name = "cmsRemote";
            this.cmsRemote.Size = new System.Drawing.Size(153, 198);
            this.cmsRemote.Opening += new System.ComponentModel.CancelEventHandler(this.cmsRemote_Opening);
            // 
            // 传送TToolStripMenuItem
            // 
            this.传送TToolStripMenuItem.Name = "传送TToolStripMenuItem";
            this.传送TToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.传送TToolStripMenuItem.Text = "传送(&T)";
            this.传送TToolStripMenuItem.Click += new System.EventHandler(this.传送TToolStripMenuItem_Click);
            // 
            // 添加到队列QToolStripMenuItem
            // 
            this.添加到队列QToolStripMenuItem.Name = "添加到队列QToolStripMenuItem";
            this.添加到队列QToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加到队列QToolStripMenuItem.Text = "添加到队列(&Q)";
            this.添加到队列QToolStripMenuItem.Click += new System.EventHandler(this.添加到队列QToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(145, 6);
            // 
            // 删除DToolStripMenuItem
            // 
            this.删除DToolStripMenuItem.Name = "删除DToolStripMenuItem";
            this.删除DToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除DToolStripMenuItem.Text = "删除(&D)";
            this.删除DToolStripMenuItem.Click += new System.EventHandler(this.删除DToolStripMenuItem_Click);
            // 
            // 重命名RToolStripMenuItem
            // 
            this.重命名RToolStripMenuItem.Name = "重命名RToolStripMenuItem";
            this.重命名RToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.重命名RToolStripMenuItem.Text = "重命名(&R)";
            this.重命名RToolStripMenuItem.Click += new System.EventHandler(this.重命名RToolStripMenuItem_Click);
            // 
            // 属性PToolStripMenuItem
            // 
            this.属性PToolStripMenuItem.Name = "属性PToolStripMenuItem";
            this.属性PToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.属性PToolStripMenuItem.Text = "属性(&P)";
            this.属性PToolStripMenuItem.Click += new System.EventHandler(this.属性PToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 6);
            // 
            // 新建文件夹NToolStripMenuItem
            // 
            this.新建文件夹NToolStripMenuItem.Name = "新建文件夹NToolStripMenuItem";
            this.新建文件夹NToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.新建文件夹NToolStripMenuItem.Text = "新建文件夹(&N)";
            this.新建文件夹NToolStripMenuItem.Click += new System.EventHandler(this.新建文件夹NToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(145, 6);
            // 
            // 刷新FToolStripMenuItem
            // 
            this.刷新FToolStripMenuItem.Name = "刷新FToolStripMenuItem";
            this.刷新FToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.刷新FToolStripMenuItem.Text = "刷新(&F)";
            this.刷新FToolStripMenuItem.Click += new System.EventHandler(this.刷新FToolStripMenuItem_Click);
            // 
            // ilExplorer
            // 
            this.ilExplorer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilExplorer.ImageStream")));
            this.ilExplorer.TransparentColor = System.Drawing.Color.Transparent;
            this.ilExplorer.Images.SetKeyName(0, "Folder.png");
            this.ilExplorer.Images.SetKeyName(1, "File.png");
            this.ilExplorer.Images.SetKeyName(2, "SdCard.png");
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.站点TToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(908, 25);
            this.msMain.TabIndex = 3;
            this.msMain.Text = "menuStrip2";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出XToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            this.退出XToolStripMenuItem.Click += new System.EventHandler(this.退出XToolStripMenuItem_Click);
            // 
            // 站点TToolStripMenuItem
            // 
            this.站点TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.站点管理SToolStripMenuItem,
            this.toolStripMenuItem1,
            this.选项PToolStripMenuItem});
            this.站点TToolStripMenuItem.Name = "站点TToolStripMenuItem";
            this.站点TToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.站点TToolStripMenuItem.Text = "站点(&S)";
            // 
            // 站点管理SToolStripMenuItem
            // 
            this.站点管理SToolStripMenuItem.Name = "站点管理SToolStripMenuItem";
            this.站点管理SToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.站点管理SToolStripMenuItem.Text = "站点管理器(&S)...";
            this.站点管理SToolStripMenuItem.Click += new System.EventHandler(this.站点管理SToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // 选项PToolStripMenuItem
            // 
            this.选项PToolStripMenuItem.Enabled = false;
            this.选项PToolStripMenuItem.Name = "选项PToolStripMenuItem";
            this.选项PToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.选项PToolStripMenuItem.Text = "选项(&P)";
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于AToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于AToolStripMenuItem
            // 
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            this.关于AToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.关于AToolStripMenuItem.Text = "关于(&A)...";
            this.关于AToolStripMenuItem.Click += new System.EventHandler(this.关于AToolStripMenuItem_Click);
            // 
            // skinEngine1
            // 
            this.skinEngine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(147)))), ((int)(((byte)(193)))));
            this.skinEngine1.ContainerControl = this;
            this.skinEngine1.CurrentTheme = "默认主题";
            this.skinEngine1.IsUseSkin = false;
            // 
            // themeButton1
            // 
            this.themeButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.themeButton1.Image = ((System.Drawing.Image)(resources.GetObject("themeButton1.Image")));
            this.themeButton1.Location = new System.Drawing.Point(888, 3);
            this.themeButton1.Name = "themeButton1";
            this.themeButton1.Size = new System.Drawing.Size(16, 16);
            this.themeButton1.TabIndex = 4;
            this.themeButton1.UseVisualStyleBackColor = true;
            // 
            // scMain
            // 
            this.scMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.scView);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.scDisplay);
            this.scMain.Size = new System.Drawing.Size(908, 592);
            this.scMain.SplitterDistance = 395;
            this.scMain.TabIndex = 5;
            // 
            // scDisplay
            // 
            this.scDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scDisplay.Location = new System.Drawing.Point(0, 0);
            this.scDisplay.Name = "scDisplay";
            // 
            // scDisplay.Panel1
            // 
            this.scDisplay.Panel1.Controls.Add(this.lvQueue);
            // 
            // scDisplay.Panel2
            // 
            this.scDisplay.Panel2.Controls.Add(this.rtbLog);
            this.scDisplay.Size = new System.Drawing.Size(908, 193);
            this.scDisplay.SplitterDistance = 454;
            this.scDisplay.TabIndex = 0;
            // 
            // lvQueue
            // 
            this.lvQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chQueueName,
            this.chQueueTarget,
            this.chQueueSize,
            this.chQueueRemark});
            this.lvQueue.ContextMenuStrip = this.cmsQueue;
            this.lvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvQueue.FloatingColumnIndex = -1;
            this.lvQueue.FullRowSelect = true;
            this.lvQueue.HideSelection = false;
            this.lvQueue.IsUseFloating = true;
            this.lvQueue.Location = new System.Drawing.Point(0, 0);
            this.lvQueue.Name = "lvQueue";
            this.lvQueue.Size = new System.Drawing.Size(454, 193);
            this.lvQueue.SmallImageList = this.ilQueue;
            this.lvQueue.TabIndex = 1;
            this.lvQueue.UseCompatibleStateImageBehavior = false;
            this.lvQueue.View = System.Windows.Forms.View.Details;
            // 
            // chQueueName
            // 
            this.chQueueName.Text = "名称";
            this.chQueueName.Width = 160;
            // 
            // chQueueTarget
            // 
            this.chQueueTarget.Text = "目标";
            this.chQueueTarget.Width = 160;
            // 
            // chQueueSize
            // 
            this.chQueueSize.Text = "大小";
            this.chQueueSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chQueueSize.Width = 80;
            // 
            // chQueueRemark
            // 
            this.chQueueRemark.Text = "备注";
            this.chQueueRemark.Width = 160;
            // 
            // cmsQueue
            // 
            this.cmsQueue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.执行队列TToolStripMenuItem,
            this.toolStripMenuItem8,
            this.删除ToolStripMenuItem,
            this.toolStripMenuItem9,
            this.载入队列LToolStripMenuItem,
            this.保存队列SToolStripMenuItem,
            this.toolStripMenuItem10,
            this.清除队列ToolStripMenuItem});
            this.cmsQueue.Name = "cmsQueue";
            this.cmsQueue.Size = new System.Drawing.Size(153, 154);
            this.cmsQueue.Opening += new System.ComponentModel.CancelEventHandler(this.cmsQueue_Opening);
            // 
            // 执行队列TToolStripMenuItem
            // 
            this.执行队列TToolStripMenuItem.Name = "执行队列TToolStripMenuItem";
            this.执行队列TToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.执行队列TToolStripMenuItem.Text = "执行队列(&T)";
            this.执行队列TToolStripMenuItem.Click += new System.EventHandler(this.执行队列TToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(149, 6);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(149, 6);
            // 
            // 载入队列LToolStripMenuItem
            // 
            this.载入队列LToolStripMenuItem.Name = "载入队列LToolStripMenuItem";
            this.载入队列LToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.载入队列LToolStripMenuItem.Text = "载入队列(&L)...";
            this.载入队列LToolStripMenuItem.Click += new System.EventHandler(this.载入队列LToolStripMenuItem_Click);
            // 
            // 保存队列SToolStripMenuItem
            // 
            this.保存队列SToolStripMenuItem.Name = "保存队列SToolStripMenuItem";
            this.保存队列SToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.保存队列SToolStripMenuItem.Text = "保存队列(&S)...";
            this.保存队列SToolStripMenuItem.Click += new System.EventHandler(this.保存队列SToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(149, 6);
            // 
            // 清除队列ToolStripMenuItem
            // 
            this.清除队列ToolStripMenuItem.Name = "清除队列ToolStripMenuItem";
            this.清除队列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.清除队列ToolStripMenuItem.Text = "清除队列";
            this.清除队列ToolStripMenuItem.Click += new System.EventHandler(this.清除队列ToolStripMenuItem_Click);
            // 
            // ilQueue
            // 
            this.ilQueue.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilQueue.ImageStream")));
            this.ilQueue.TransparentColor = System.Drawing.Color.Transparent;
            this.ilQueue.Images.SetKeyName(0, "Error");
            this.ilQueue.Images.SetKeyName(1, "Download");
            this.ilQueue.Images.SetKeyName(2, "Upload");
            this.ilQueue.Images.SetKeyName(3, "Delete");
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.White;
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLog.ContextMenuStrip = this.cmsLog;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(450, 193);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // cmsLog
            // 
            this.cmsLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制到剪贴板CToolStripMenuItem,
            this.保存到文本文件TToolStripMenuItem,
            this.保存到RTF文件RToolStripMenuItem,
            this.toolStripMenuItem5,
            this.清除ToolStripMenuItem});
            this.cmsLog.Name = "cmsLog";
            this.cmsLog.Size = new System.Drawing.Size(173, 98);
            // 
            // 复制到剪贴板CToolStripMenuItem
            // 
            this.复制到剪贴板CToolStripMenuItem.Name = "复制到剪贴板CToolStripMenuItem";
            this.复制到剪贴板CToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.复制到剪贴板CToolStripMenuItem.Text = "复制到剪贴板(&C)";
            this.复制到剪贴板CToolStripMenuItem.Click += new System.EventHandler(this.复制到剪贴板CToolStripMenuItem_Click);
            // 
            // 保存到文本文件TToolStripMenuItem
            // 
            this.保存到文本文件TToolStripMenuItem.Name = "保存到文本文件TToolStripMenuItem";
            this.保存到文本文件TToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.保存到文本文件TToolStripMenuItem.Text = "保存到文本文件(&T)";
            this.保存到文本文件TToolStripMenuItem.Click += new System.EventHandler(this.保存到文本文件TToolStripMenuItem_Click);
            // 
            // 保存到RTF文件RToolStripMenuItem
            // 
            this.保存到RTF文件RToolStripMenuItem.Name = "保存到RTF文件RToolStripMenuItem";
            this.保存到RTF文件RToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.保存到RTF文件RToolStripMenuItem.Text = "保存到RTF文件(&R)";
            this.保存到RTF文件RToolStripMenuItem.Click += new System.EventHandler(this.保存到RTF文件RToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(169, 6);
            // 
            // 清除ToolStripMenuItem
            // 
            this.清除ToolStripMenuItem.Name = "清除ToolStripMenuItem";
            this.清除ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.清除ToolStripMenuItem.Text = "清除(&L)";
            this.清除ToolStripMenuItem.Click += new System.EventHandler(this.清除ToolStripMenuItem_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTransferState,
            this.toolStripStatusLabel1,
            this.lblQueueUsedTime,
            this.spQueueUsedTime,
            this.lblTransferLeftTime,
            this.spTransferLeftTime,
            this.lblTransferUsedTime,
            this.spTransferUsedTime,
            this.lblTransferProgressText,
            this.spTransferProgressText,
            this.pbTransferProgress,
            this.spTransferProgress});
            this.ssMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ssMain.Location = new System.Drawing.Point(0, 619);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(908, 23);
            this.ssMain.TabIndex = 6;
            this.ssMain.Text = "statusStrip1";
            // 
            // lblTransferState
            // 
            this.lblTransferState.Name = "lblTransferState";
            this.lblTransferState.Size = new System.Drawing.Size(32, 18);
            this.lblTransferState.Text = "就绪";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 18);
            // 
            // lblQueueUsedTime
            // 
            this.lblQueueUsedTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblQueueUsedTime.AutoSize = false;
            this.lblQueueUsedTime.Name = "lblQueueUsedTime";
            this.lblQueueUsedTime.Size = new System.Drawing.Size(131, 18);
            // 
            // spQueueUsedTime
            // 
            this.spQueueUsedTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.spQueueUsedTime.Name = "spQueueUsedTime";
            this.spQueueUsedTime.Size = new System.Drawing.Size(6, 23);
            // 
            // lblTransferLeftTime
            // 
            this.lblTransferLeftTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTransferLeftTime.AutoSize = false;
            this.lblTransferLeftTime.Name = "lblTransferLeftTime";
            this.lblTransferLeftTime.Size = new System.Drawing.Size(131, 18);
            // 
            // spTransferLeftTime
            // 
            this.spTransferLeftTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.spTransferLeftTime.Name = "spTransferLeftTime";
            this.spTransferLeftTime.Size = new System.Drawing.Size(6, 23);
            // 
            // lblTransferUsedTime
            // 
            this.lblTransferUsedTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTransferUsedTime.AutoSize = false;
            this.lblTransferUsedTime.Name = "lblTransferUsedTime";
            this.lblTransferUsedTime.Size = new System.Drawing.Size(131, 18);
            // 
            // spTransferUsedTime
            // 
            this.spTransferUsedTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.spTransferUsedTime.Name = "spTransferUsedTime";
            this.spTransferUsedTime.Size = new System.Drawing.Size(6, 23);
            // 
            // lblTransferProgressText
            // 
            this.lblTransferProgressText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTransferProgressText.AutoSize = false;
            this.lblTransferProgressText.Name = "lblTransferProgressText";
            this.lblTransferProgressText.Size = new System.Drawing.Size(32, 18);
            // 
            // spTransferProgressText
            // 
            this.spTransferProgressText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.spTransferProgressText.Name = "spTransferProgressText";
            this.spTransferProgressText.Size = new System.Drawing.Size(6, 23);
            // 
            // pbTransferProgress
            // 
            this.pbTransferProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pbTransferProgress.Name = "pbTransferProgress";
            this.pbTransferProgress.Size = new System.Drawing.Size(130, 17);
            // 
            // spTransferProgress
            // 
            this.spTransferProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.spTransferProgress.Name = "spTransferProgress";
            this.spTransferProgress.Size = new System.Drawing.Size(6, 23);
            // 
            // cmsQuickConnect
            // 
            this.cmsQuickConnect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.快速连接QToolStripMenuItem,
            this.toolStripMenuItem6,
            this.历史ToolStripMenuItem,
            this.toolStripMenuItem7,
            this.站点ToolStripMenuItem});
            this.cmsQuickConnect.Name = "cmsQuickConnect";
            this.cmsQuickConnect.Size = new System.Drawing.Size(137, 82);
            this.cmsQuickConnect.Opening += new System.ComponentModel.CancelEventHandler(this.cmsQuickConnect_Opening);
            // 
            // 快速连接QToolStripMenuItem
            // 
            this.快速连接QToolStripMenuItem.Enabled = false;
            this.快速连接QToolStripMenuItem.Name = "快速连接QToolStripMenuItem";
            this.快速连接QToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.快速连接QToolStripMenuItem.Text = "快速连接(&Q)";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(133, 6);
            // 
            // 历史ToolStripMenuItem
            // 
            this.历史ToolStripMenuItem.Enabled = false;
            this.历史ToolStripMenuItem.Name = "历史ToolStripMenuItem";
            this.历史ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.历史ToolStripMenuItem.Text = "历史";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(133, 6);
            // 
            // 站点ToolStripMenuItem
            // 
            this.站点ToolStripMenuItem.Name = "站点ToolStripMenuItem";
            this.站点ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.站点ToolStripMenuItem.Text = "站点";
            // 
            // ilSites
            // 
            this.ilSites.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSites.ImageStream")));
            this.ilSites.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSites.Images.SetKeyName(0, "Group");
            this.ilSites.Images.SetKeyName(1, "Site");
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 642);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.themeButton1);
            this.Controls.Add(this.msMain);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aaaSoft.FtpClient ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.scView.Panel1.ResumeLayout(false);
            this.scView.Panel2.ResumeLayout(false);
            this.scView.Panel2.PerformLayout();
            this.scView.ResumeLayout(false);
            this.tsLocalOperate.ResumeLayout(false);
            this.tsLocalOperate.PerformLayout();
            this.tsAddress.ResumeLayout(false);
            this.tsAddress.PerformLayout();
            this.tsRemoteOperate.ResumeLayout(false);
            this.tsRemoteOperate.PerformLayout();
            this.cmsRemote.ResumeLayout(false);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.scDisplay.Panel1.ResumeLayout(false);
            this.scDisplay.Panel2.ResumeLayout(false);
            this.scDisplay.ResumeLayout(false);
            this.cmsQueue.ResumeLayout(false);
            this.cmsLog.ResumeLayout(false);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.cmsQuickConnect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer scView;
        private System.Windows.Forms.TextBox txtAddress;
        private aaaSoft.Controls.ListView lvRemoteFile;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 站点TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 站点管理SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 选项PToolStripMenuItem;
        private System.Windows.Forms.ImageList ilExplorer;
        private System.Windows.Forms.ColumnHeader chChangeTime;
        private System.Windows.Forms.ToolStrip tsAddress;
        private System.Windows.Forms.ToolStrip tsRemoteOperate;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;
        private aaaSoft.SkinEngine.SkinEngine skinEngine1;
        private aaaSoft.SkinEngine.SkinControls.ThemeButton themeButton1;
        private System.Windows.Forms.ContextMenuStrip cmsRemote;
        private System.Windows.Forms.ToolStripMenuItem 删除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重命名RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 属性PToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 新建文件夹NToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 刷新FToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnUp;
        private System.Windows.Forms.ToolStripButton btnQuickConnect;
        private System.Windows.Forms.ToolStripButton btnDisconnect;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.SplitContainer scDisplay;
        private aaaSoft.Controls.ListView lvQueue;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ToolStripMenuItem 传送TToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ColumnHeader chQueueName;
        private System.Windows.Forms.ColumnHeader chQueueTarget;
        private System.Windows.Forms.ColumnHeader chQueueSize;
        private System.Windows.Forms.ColumnHeader chQueueRemark;
        private System.Windows.Forms.ColumnHeader chProperty;
        private System.Windows.Forms.ContextMenuStrip cmsLog;
        private System.Windows.Forms.ToolStripMenuItem 复制到剪贴板CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存到文本文件TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存到RTF文件RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem 清除ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel lblTransferState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblQueueUsedTime;
        private System.Windows.Forms.ToolStripSeparator spQueueUsedTime;
        private System.Windows.Forms.ToolStripStatusLabel lblTransferLeftTime;
        private System.Windows.Forms.ToolStripSeparator spTransferLeftTime;
        private System.Windows.Forms.ToolStripStatusLabel lblTransferUsedTime;
        private System.Windows.Forms.ToolStripSeparator spTransferUsedTime;
        private System.Windows.Forms.ToolStripStatusLabel lblTransferProgressText;
        private System.Windows.Forms.ToolStripSeparator spTransferProgressText;
        private System.Windows.Forms.ToolStripProgressBar pbTransferProgress;
        private System.Windows.Forms.ToolStripSeparator spTransferProgress;
        private aaaSoft.Controls.FileBrowser fbLocal;
        private System.Windows.Forms.ToolStrip tsLocalOperate;
        private System.Windows.Forms.ToolStripButton btnLocalRefresh;
        private System.Windows.Forms.ToolStripButton btnRemoteRefresh;
        private System.Windows.Forms.ContextMenuStrip cmsQuickConnect;
        private System.Windows.Forms.ToolStripMenuItem 快速连接QToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 历史ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem 站点ToolStripMenuItem;
        private System.Windows.Forms.ImageList ilSites;
        private System.Windows.Forms.ImageList ilQueue;
        private System.Windows.Forms.ToolStripButton btnStartQueue;
        private System.Windows.Forms.ToolStripButton btnStopQueue;
        private System.Windows.Forms.ContextMenuStrip cmsQueue;
        private System.Windows.Forms.ToolStripMenuItem 执行队列TToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem 载入队列LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存队列SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem 清除队列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加到队列QToolStripMenuItem;

    }
}

