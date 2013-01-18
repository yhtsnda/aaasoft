namespace DatabaseKeeper
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDatabaseType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.gbDatabaseConnection = new System.Windows.Forms.GroupBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnRefreshDatabaseName = new System.Windows.Forms.Button();
            this.gbChooseDatabaseName = new System.Windows.Forms.GroupBox();
            this.clbDatabaseName = new System.Windows.Forms.CheckedListBox();
            this.btnManualBackup = new System.Windows.Forms.Button();
            this.gbBackupFileSetting = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemoteFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAutoBackup = new System.Windows.Forms.Button();
            this.gbAutoBackupSettin = new System.Windows.Forms.GroupBox();
            this.lbEveryDayBackupTime = new System.Windows.Forms.ListBox();
            this.tsAutoBackupTime = new System.Windows.Forms.ToolStrip();
            this.btnAddTime = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveTime = new System.Windows.Forms.ToolStripButton();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.lvLog = new System.Windows.Forms.ListView();
            this.chTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEvent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.niMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcOperateSelect = new System.Windows.Forms.TabControl();
            this.tpBackup = new System.Windows.Forms.TabPage();
            this.tpRestore = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBackupFileName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSelectBackupFile = new System.Windows.Forms.Button();
            this.btnManualRestore = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.gbDatabaseConnection.SuspendLayout();
            this.gbChooseDatabaseName.SuspendLayout();
            this.gbBackupFileSetting.SuspendLayout();
            this.gbAutoBackupSettin.SuspendLayout();
            this.tsAutoBackupTime.SuspendLayout();
            this.gbLog.SuspendLayout();
            this.cmsNotifyIcon.SuspendLayout();
            this.tcOperateSelect.SuspendLayout();
            this.tpBackup.SuspendLayout();
            this.tpRestore.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库类型:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(85, 20);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(100, 21);
            this.txtHost.TabIndex = 1;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "主机:";
            // 
            // cbDatabaseType
            // 
            this.cbDatabaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabaseType.FormattingEnabled = true;
            this.cbDatabaseType.Location = new System.Drawing.Point(97, 12);
            this.cbDatabaseType.Name = "cbDatabaseType";
            this.cbDatabaseType.Size = new System.Drawing.Size(447, 20);
            this.cbDatabaseType.TabIndex = 0;
            this.cbDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cbDatabaseType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户名:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(85, 47);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(100, 21);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(211, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "端口:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(252, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 2;
            this.txtPort.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(211, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "密码:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(252, 47);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 21);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "连接字符串:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(85, 74);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(447, 21);
            this.txtConnectionString.TabIndex = 5;
            // 
            // gbDatabaseConnection
            // 
            this.gbDatabaseConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDatabaseConnection.Controls.Add(this.btnSaveConfig);
            this.gbDatabaseConnection.Controls.Add(this.btnLoadConfig);
            this.gbDatabaseConnection.Controls.Add(this.txtHost);
            this.gbDatabaseConnection.Controls.Add(this.label2);
            this.gbDatabaseConnection.Controls.Add(this.label3);
            this.gbDatabaseConnection.Controls.Add(this.txtPassword);
            this.gbDatabaseConnection.Controls.Add(this.txtUserName);
            this.gbDatabaseConnection.Controls.Add(this.label5);
            this.gbDatabaseConnection.Controls.Add(this.label6);
            this.gbDatabaseConnection.Controls.Add(this.txtPort);
            this.gbDatabaseConnection.Controls.Add(this.txtConnectionString);
            this.gbDatabaseConnection.Controls.Add(this.label4);
            this.gbDatabaseConnection.Location = new System.Drawing.Point(12, 38);
            this.gbDatabaseConnection.Name = "gbDatabaseConnection";
            this.gbDatabaseConnection.Size = new System.Drawing.Size(543, 106);
            this.gbDatabaseConnection.TabIndex = 3;
            this.gbDatabaseConnection.TabStop = false;
            this.gbDatabaseConnection.Text = "数据库连接";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(457, 45);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 6;
            this.btnSaveConfig.Text = "保存配置..";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(457, 18);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(75, 23);
            this.btnLoadConfig.TabIndex = 6;
            this.btnLoadConfig.Text = "加载配置..";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // btnRefreshDatabaseName
            // 
            this.btnRefreshDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDatabaseName.Location = new System.Drawing.Point(459, 18);
            this.btnRefreshDatabaseName.Name = "btnRefreshDatabaseName";
            this.btnRefreshDatabaseName.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshDatabaseName.TabIndex = 7;
            this.btnRefreshDatabaseName.Text = "刷新";
            this.btnRefreshDatabaseName.UseVisualStyleBackColor = true;
            this.btnRefreshDatabaseName.Click += new System.EventHandler(this.btnRefreshDatabaseName_Click);
            // 
            // gbChooseDatabaseName
            // 
            this.gbChooseDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbChooseDatabaseName.Controls.Add(this.clbDatabaseName);
            this.gbChooseDatabaseName.Controls.Add(this.btnRefreshDatabaseName);
            this.gbChooseDatabaseName.Location = new System.Drawing.Point(12, 150);
            this.gbChooseDatabaseName.Name = "gbChooseDatabaseName";
            this.gbChooseDatabaseName.Size = new System.Drawing.Size(543, 90);
            this.gbChooseDatabaseName.TabIndex = 4;
            this.gbChooseDatabaseName.TabStop = false;
            this.gbChooseDatabaseName.Text = "选择数据库";
            // 
            // clbDatabaseName
            // 
            this.clbDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbDatabaseName.FormattingEnabled = true;
            this.clbDatabaseName.Location = new System.Drawing.Point(6, 18);
            this.clbDatabaseName.Name = "clbDatabaseName";
            this.clbDatabaseName.Size = new System.Drawing.Size(447, 68);
            this.clbDatabaseName.TabIndex = 8;
            // 
            // btnManualBackup
            // 
            this.btnManualBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManualBackup.Enabled = false;
            this.btnManualBackup.Location = new System.Drawing.Point(443, 154);
            this.btnManualBackup.Name = "btnManualBackup";
            this.btnManualBackup.Size = new System.Drawing.Size(97, 23);
            this.btnManualBackup.TabIndex = 101;
            this.btnManualBackup.Text = "手动备份";
            this.btnManualBackup.UseVisualStyleBackColor = true;
            this.btnManualBackup.Click += new System.EventHandler(this.btnManualBackup_Click);
            // 
            // gbBackupFileSetting
            // 
            this.gbBackupFileSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBackupFileSetting.Controls.Add(this.label10);
            this.gbBackupFileSetting.Controls.Add(this.txtRemoteFolder);
            this.gbBackupFileSetting.Controls.Add(this.label7);
            this.gbBackupFileSetting.Location = new System.Drawing.Point(3, 7);
            this.gbBackupFileSetting.Name = "gbBackupFileSetting";
            this.gbBackupFileSetting.Size = new System.Drawing.Size(537, 65);
            this.gbBackupFileSetting.TabIndex = 9;
            this.gbBackupFileSetting.TabStop = false;
            this.gbBackupFileSetting.Text = "备份设置";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.DarkGray;
            this.label10.Location = new System.Drawing.Point(32, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(395, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "注意：自动备份时，会以\"{数据库名称}-{时间}.bak\"的格式自动命名文件";
            // 
            // txtRemoteFolder
            // 
            this.txtRemoteFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemoteFolder.Location = new System.Drawing.Point(85, 20);
            this.txtRemoteFolder.Name = "txtRemoteFolder";
            this.txtRemoteFolder.Size = new System.Drawing.Size(441, 21);
            this.txtRemoteFolder.TabIndex = 8;
            this.txtRemoteFolder.Text = "D:\\";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "远程目录:";
            // 
            // btnAutoBackup
            // 
            this.btnAutoBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoBackup.Enabled = false;
            this.btnAutoBackup.Location = new System.Drawing.Point(443, 127);
            this.btnAutoBackup.Name = "btnAutoBackup";
            this.btnAutoBackup.Size = new System.Drawing.Size(97, 23);
            this.btnAutoBackup.TabIndex = 100;
            this.btnAutoBackup.Text = "开始自动备份";
            this.btnAutoBackup.UseVisualStyleBackColor = true;
            this.btnAutoBackup.Click += new System.EventHandler(this.btnAutoBackup_Click);
            // 
            // gbAutoBackupSettin
            // 
            this.gbAutoBackupSettin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAutoBackupSettin.Controls.Add(this.lbEveryDayBackupTime);
            this.gbAutoBackupSettin.Controls.Add(this.tsAutoBackupTime);
            this.gbAutoBackupSettin.Location = new System.Drawing.Point(3, 78);
            this.gbAutoBackupSettin.Name = "gbAutoBackupSettin";
            this.gbAutoBackupSettin.Size = new System.Drawing.Size(434, 99);
            this.gbAutoBackupSettin.TabIndex = 10;
            this.gbAutoBackupSettin.TabStop = false;
            this.gbAutoBackupSettin.Text = "每日定时备份时间列表";
            // 
            // lbEveryDayBackupTime
            // 
            this.lbEveryDayBackupTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEveryDayBackupTime.FormattingEnabled = true;
            this.lbEveryDayBackupTime.ItemHeight = 12;
            this.lbEveryDayBackupTime.Location = new System.Drawing.Point(3, 17);
            this.lbEveryDayBackupTime.Name = "lbEveryDayBackupTime";
            this.lbEveryDayBackupTime.Size = new System.Drawing.Size(428, 79);
            this.lbEveryDayBackupTime.TabIndex = 10;
            this.lbEveryDayBackupTime.SelectedIndexChanged += new System.EventHandler(this.lbEveryDayBackupTime_SelectedIndexChanged);
            // 
            // tsAutoBackupTime
            // 
            this.tsAutoBackupTime.Dock = System.Windows.Forms.DockStyle.None;
            this.tsAutoBackupTime.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsAutoBackupTime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddTime,
            this.btnRemoveTime});
            this.tsAutoBackupTime.Location = new System.Drawing.Point(137, -4);
            this.tsAutoBackupTime.Name = "tsAutoBackupTime";
            this.tsAutoBackupTime.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsAutoBackupTime.Size = new System.Drawing.Size(49, 25);
            this.tsAutoBackupTime.TabIndex = 11;
            this.tsAutoBackupTime.Text = "tsAutoBackupTime";
            // 
            // btnAddTime
            // 
            this.btnAddTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddTime.Image = ((System.Drawing.Image)(resources.GetObject("btnAddTime.Image")));
            this.btnAddTime.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(23, 22);
            this.btnAddTime.Text = "toolStripButton1";
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // btnRemoveTime
            // 
            this.btnRemoveTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoveTime.Enabled = false;
            this.btnRemoveTime.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveTime.Image")));
            this.btnRemoveTime.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveTime.Name = "btnRemoveTime";
            this.btnRemoveTime.Size = new System.Drawing.Size(23, 22);
            this.btnRemoveTime.Text = "toolStripButton2";
            this.btnRemoveTime.Click += new System.EventHandler(this.btnRemoveTime_Click);
            // 
            // gbLog
            // 
            this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLog.Controls.Add(this.btnClearLogs);
            this.gbLog.Controls.Add(this.lvLog);
            this.gbLog.Location = new System.Drawing.Point(5, 458);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(550, 121);
            this.gbLog.TabIndex = 11;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "日志";
            // 
            // lvLog
            // 
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTime,
            this.chEvent});
            this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLog.FullRowSelect = true;
            this.lvLog.Location = new System.Drawing.Point(3, 17);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(544, 101);
            this.lvLog.TabIndex = 12;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            // 
            // chTime
            // 
            this.chTime.Text = "时间";
            this.chTime.Width = 130;
            // 
            // chEvent
            // 
            this.chEvent.Text = "事件";
            this.chEvent.Width = 300;
            // 
            // niMain
            // 
            this.niMain.ContextMenuStrip = this.cmsNotifyIcon;
            this.niMain.Icon = ((System.Drawing.Icon)(resources.GetObject("niMain.Icon")));
            this.niMain.Text = "DatabaseKeeper";
            this.niMain.Visible = true;
            this.niMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.niMain_MouseDoubleClick);
            // 
            // cmsNotifyIcon
            // 
            this.cmsNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.退出ToolStripMenuItem});
            this.cmsNotifyIcon.Name = "cmsNotifyIcon";
            this.cmsNotifyIcon.Size = new System.Drawing.Size(101, 54);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // tcOperateSelect
            // 
            this.tcOperateSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcOperateSelect.Controls.Add(this.tpBackup);
            this.tcOperateSelect.Controls.Add(this.tpRestore);
            this.tcOperateSelect.Location = new System.Drawing.Point(1, 246);
            this.tcOperateSelect.Name = "tcOperateSelect";
            this.tcOperateSelect.SelectedIndex = 0;
            this.tcOperateSelect.Size = new System.Drawing.Size(554, 210);
            this.tcOperateSelect.TabIndex = 102;
            // 
            // tpBackup
            // 
            this.tpBackup.Controls.Add(this.gbAutoBackupSettin);
            this.tpBackup.Controls.Add(this.btnAutoBackup);
            this.tpBackup.Controls.Add(this.gbBackupFileSetting);
            this.tpBackup.Controls.Add(this.btnManualBackup);
            this.tpBackup.Location = new System.Drawing.Point(4, 22);
            this.tpBackup.Name = "tpBackup";
            this.tpBackup.Padding = new System.Windows.Forms.Padding(3);
            this.tpBackup.Size = new System.Drawing.Size(546, 184);
            this.tpBackup.TabIndex = 0;
            this.tpBackup.Text = "备份";
            this.tpBackup.UseVisualStyleBackColor = true;
            // 
            // tpRestore
            // 
            this.tpRestore.Controls.Add(this.btnManualRestore);
            this.tpRestore.Controls.Add(this.label11);
            this.tpRestore.Controls.Add(this.groupBox1);
            this.tpRestore.Location = new System.Drawing.Point(4, 22);
            this.tpRestore.Name = "tpRestore";
            this.tpRestore.Padding = new System.Windows.Forms.Padding(3);
            this.tpRestore.Size = new System.Drawing.Size(546, 184);
            this.tpRestore.TabIndex = 1;
            this.tpRestore.Text = "还原";
            this.tpRestore.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSelectBackupFile);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtBackupFileName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 65);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "还原设置";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DarkGray;
            this.label8.Location = new System.Drawing.Point(32, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(437, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "注意：备份文件一定是要放在服务器的磁盘上，而不是远程连接的客户端机器上！";
            // 
            // txtBackupFileName
            // 
            this.txtBackupFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupFileName.Location = new System.Drawing.Point(109, 20);
            this.txtBackupFileName.Name = "txtBackupFileName";
            this.txtBackupFileName.Size = new System.Drawing.Size(389, 21);
            this.txtBackupFileName.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "远程备份文件:";
            // 
            // btnSelectBackupFile
            // 
            this.btnSelectBackupFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectBackupFile.Location = new System.Drawing.Point(504, 18);
            this.btnSelectBackupFile.Name = "btnSelectBackupFile";
            this.btnSelectBackupFile.Size = new System.Drawing.Size(29, 23);
            this.btnSelectBackupFile.TabIndex = 9;
            this.btnSelectBackupFile.Text = "..";
            this.btnSelectBackupFile.UseVisualStyleBackColor = true;
            this.btnSelectBackupFile.Click += new System.EventHandler(this.btnSelectBackupFile_Click);
            // 
            // btnManualRestore
            // 
            this.btnManualRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManualRestore.Location = new System.Drawing.Point(464, 77);
            this.btnManualRestore.Name = "btnManualRestore";
            this.btnManualRestore.Size = new System.Drawing.Size(75, 23);
            this.btnManualRestore.TabIndex = 11;
            this.btnManualRestore.Text = "开始还原";
            this.btnManualRestore.UseVisualStyleBackColor = true;
            this.btnManualRestore.Click += new System.EventHandler(this.btnManualRestore_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLogs.Location = new System.Drawing.Point(471, 0);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(75, 23);
            this.btnClearLogs.TabIndex = 13;
            this.btnClearLogs.Text = "清除日志";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.DarkGray;
            this.label11.Location = new System.Drawing.Point(237, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(221, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "请勾选上面数据库名称列表中的一个然后";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 591);
            this.Controls.Add(this.cbDatabaseType);
            this.Controls.Add(this.tcOperateSelect);
            this.Controls.Add(this.gbChooseDatabaseName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbDatabaseConnection);
            this.Controls.Add(this.gbLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DatabaseKeeper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.gbDatabaseConnection.ResumeLayout(false);
            this.gbDatabaseConnection.PerformLayout();
            this.gbChooseDatabaseName.ResumeLayout(false);
            this.gbBackupFileSetting.ResumeLayout(false);
            this.gbBackupFileSetting.PerformLayout();
            this.gbAutoBackupSettin.ResumeLayout(false);
            this.gbAutoBackupSettin.PerformLayout();
            this.tsAutoBackupTime.ResumeLayout(false);
            this.tsAutoBackupTime.PerformLayout();
            this.gbLog.ResumeLayout(false);
            this.cmsNotifyIcon.ResumeLayout(false);
            this.tcOperateSelect.ResumeLayout(false);
            this.tpBackup.ResumeLayout(false);
            this.tpRestore.ResumeLayout(false);
            this.tpRestore.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDatabaseType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.GroupBox gbDatabaseConnection;
        private System.Windows.Forms.Button btnRefreshDatabaseName;
        private System.Windows.Forms.GroupBox gbChooseDatabaseName;
        private System.Windows.Forms.Button btnManualBackup;
        private System.Windows.Forms.GroupBox gbBackupFileSetting;
        private System.Windows.Forms.TextBox txtRemoteFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAutoBackup;
        private System.Windows.Forms.GroupBox gbAutoBackupSettin;
        private System.Windows.Forms.ListBox lbEveryDayBackupTime;
        private System.Windows.Forms.ToolStrip tsAutoBackupTime;
        private System.Windows.Forms.ToolStripButton btnAddTime;
        private System.Windows.Forms.ToolStripButton btnRemoveTime;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chEvent;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NotifyIcon niMain;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox clbDatabaseName;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.TabControl tcOperateSelect;
        private System.Windows.Forms.TabPage tpBackup;
        private System.Windows.Forms.TabPage tpRestore;
        private System.Windows.Forms.Button btnManualRestore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectBackupFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBackupFileName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Label label11;
    }
}

