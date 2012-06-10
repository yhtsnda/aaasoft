namespace XDAndroidExplorer
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
			this.txtShellInput = new System.Windows.Forms.TextBox();
			this.btnShellSendCmd = new System.Windows.Forms.Button();
			this.txtShellOutput = new System.Windows.Forms.TextBox();
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tpFileManager = new System.Windows.Forms.TabPage();
			this.tsAddress = new System.Windows.Forms.ToolStrip();
			this.txtAddress = new System.Windows.Forms.ToolStripTextBox();
			this.btnGotoFolder = new System.Windows.Forms.ToolStripButton();
			this.tsOperate = new System.Windows.Forms.ToolStrip();
			this.btnRefrush = new System.Windows.Forms.ToolStripButton();
			this.btnUp = new System.Windows.Forms.ToolStripButton();
			this.btnNewFolder = new System.Windows.Forms.ToolStripButton();
			this.lvExplorer = new System.Windows.Forms.ListView();
			this.chFileName = new System.Windows.Forms.ColumnHeader();
			this.chModifyTime = new System.Windows.Forms.ColumnHeader();
			this.chFileType = new System.Windows.Forms.ColumnHeader();
			this.chFileSize = new System.Windows.Forms.ColumnHeader();
			this.chFilePropery = new System.Windows.Forms.ColumnHeader();
			this.chFileExt = new System.Windows.Forms.ColumnHeader();
			this.chFileOwner = new System.Windows.Forms.ColumnHeader();
			this.chOwnerGroup = new System.Windows.Forms.ColumnHeader();
			this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.重命名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ilExplorer = new System.Windows.Forms.ImageList(this.components);
			this.tpCommand = new System.Windows.Forms.TabPage();
			this.btnShellHelp = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnAdbHelp = new System.Windows.Forms.Button();
			this.txtAdbOutput = new System.Windows.Forms.TextBox();
			this.btnAdbSendCmd = new System.Windows.Forms.Button();
			this.txtAdbInput = new System.Windows.Forms.TextBox();
			this.tpShowcase = new System.Windows.Forms.TabPage();
			this.btnPoweroff = new System.Windows.Forms.Button();
			this.btnRebootRecovery = new System.Windows.Forms.Button();
			this.btnReboot = new System.Windows.Forms.Button();
			this.tpAbout = new System.Windows.Forms.TabPage();
			this.lblVersion = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pbScbeta = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tcMain.SuspendLayout();
			this.tpFileManager.SuspendLayout();
			this.tsAddress.SuspendLayout();
			this.tsOperate.SuspendLayout();
			this.cmsMain.SuspendLayout();
			this.tpCommand.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tpShowcase.SuspendLayout();
			this.tpAbout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbScbeta)).BeginInit();
			this.SuspendLayout();
			// 
			// txtShellInput
			// 
			this.txtShellInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtShellInput.Location = new System.Drawing.Point(6, 385);
			this.txtShellInput.Name = "txtShellInput";
			this.txtShellInput.Size = new System.Drawing.Size(489, 21);
			this.txtShellInput.TabIndex = 0;
			this.txtShellInput.Leave += new System.EventHandler(this.Input_Leave);
			this.txtShellInput.Enter += new System.EventHandler(this.txtShellInput_Enter);
			// 
			// btnShellSendCmd
			// 
			this.btnShellSendCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnShellSendCmd.Location = new System.Drawing.Point(501, 383);
			this.btnShellSendCmd.Name = "btnShellSendCmd";
			this.btnShellSendCmd.Size = new System.Drawing.Size(75, 23);
			this.btnShellSendCmd.TabIndex = 1;
			this.btnShellSendCmd.Text = "发送";
			this.btnShellSendCmd.UseVisualStyleBackColor = true;
			this.btnShellSendCmd.Click += new System.EventHandler(this.btnShellSendCmd_Click);
			// 
			// txtShellOutput
			// 
			this.txtShellOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtShellOutput.Location = new System.Drawing.Point(6, 6);
			this.txtShellOutput.Multiline = true;
			this.txtShellOutput.Name = "txtShellOutput";
			this.txtShellOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtShellOutput.Size = new System.Drawing.Size(591, 373);
			this.txtShellOutput.TabIndex = 0;
			// 
			// tcMain
			// 
			this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tcMain.Controls.Add(this.tpFileManager);
			this.tcMain.Controls.Add(this.tpCommand);
			this.tcMain.Controls.Add(this.tabPage1);
			this.tcMain.Controls.Add(this.tpShowcase);
			this.tcMain.Controls.Add(this.tpAbout);
			this.tcMain.Location = new System.Drawing.Point(12, 12);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(734, 456);
			this.tcMain.TabIndex = 2;
			// 
			// tpFileManager
			// 
			this.tpFileManager.Controls.Add(this.tsAddress);
			this.tpFileManager.Controls.Add(this.tsOperate);
			this.tpFileManager.Controls.Add(this.lvExplorer);
			this.tpFileManager.Location = new System.Drawing.Point(4, 22);
			this.tpFileManager.Name = "tpFileManager";
			this.tpFileManager.Padding = new System.Windows.Forms.Padding(3);
			this.tpFileManager.Size = new System.Drawing.Size(726, 430);
			this.tpFileManager.TabIndex = 0;
			this.tpFileManager.Text = "文件管理";
			this.tpFileManager.UseVisualStyleBackColor = true;
			// 
			// tsAddress
			// 
			this.tsAddress.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsAddress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.txtAddress,
									this.btnGotoFolder});
			this.tsAddress.Location = new System.Drawing.Point(3, 28);
			this.tsAddress.Name = "tsAddress";
			this.tsAddress.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsAddress.Size = new System.Drawing.Size(720, 25);
			this.tsAddress.TabIndex = 2;
			this.tsAddress.Text = "tsAddress";
			// 
			// txtAddress
			// 
			this.txtAddress.AutoSize = false;
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(500, 25);
			// 
			// btnGotoFolder
			// 
			this.btnGotoFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnGotoFolder.Image")));
			this.btnGotoFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnGotoFolder.Name = "btnGotoFolder";
			this.btnGotoFolder.Size = new System.Drawing.Size(52, 22);
			this.btnGotoFolder.Text = "转到";
			this.btnGotoFolder.Click += new System.EventHandler(this.btnGotoFolder_Click);
			// 
			// tsOperate
			// 
			this.tsOperate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOperate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.btnRefrush,
									this.btnUp,
									this.btnNewFolder});
			this.tsOperate.Location = new System.Drawing.Point(3, 3);
			this.tsOperate.Name = "tsOperate";
			this.tsOperate.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsOperate.Size = new System.Drawing.Size(720, 25);
			this.tsOperate.TabIndex = 1;
			this.tsOperate.Text = "toolStrip1";
			// 
			// btnRefrush
			// 
			this.btnRefrush.Image = ((System.Drawing.Image)(resources.GetObject("btnRefrush.Image")));
			this.btnRefrush.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefrush.Name = "btnRefrush";
			this.btnRefrush.Size = new System.Drawing.Size(52, 22);
			this.btnRefrush.Text = "刷新";
			this.btnRefrush.Click += new System.EventHandler(this.btnRefrush_Click);
			// 
			// btnUp
			// 
			this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
			this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(52, 22);
			this.btnUp.Text = "向上";
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnNewFolder
			// 
			this.btnNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnNewFolder.Image")));
			this.btnNewFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNewFolder.Name = "btnNewFolder";
			this.btnNewFolder.Size = new System.Drawing.Size(88, 22);
			this.btnNewFolder.Text = "新建文件夹";
			this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click);
			// 
			// lvExplorer
			// 
			this.lvExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lvExplorer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.chFileName,
									this.chModifyTime,
									this.chFileType,
									this.chFileSize,
									this.chFilePropery,
									this.chFileExt,
									this.chFileOwner,
									this.chOwnerGroup});
			this.lvExplorer.ContextMenuStrip = this.cmsMain;
			this.lvExplorer.FullRowSelect = true;
			this.lvExplorer.Location = new System.Drawing.Point(0, 56);
			this.lvExplorer.Name = "lvExplorer";
			this.lvExplorer.Size = new System.Drawing.Size(726, 375);
			this.lvExplorer.SmallImageList = this.ilExplorer;
			this.lvExplorer.TabIndex = 0;
			this.lvExplorer.UseCompatibleStateImageBehavior = false;
			this.lvExplorer.View = System.Windows.Forms.View.Details;
			this.lvExplorer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvExplorer_MouseDoubleClick);
			// 
			// chFileName
			// 
			this.chFileName.Text = "名称";
			this.chFileName.Width = 140;
			// 
			// chModifyTime
			// 
			this.chModifyTime.Text = "修改时间";
			this.chModifyTime.Width = 120;
			// 
			// chFileType
			// 
			this.chFileType.Text = "类型";
			// 
			// chFileSize
			// 
			this.chFileSize.Text = "大小";
			this.chFileSize.Width = 80;
			// 
			// chFilePropery
			// 
			this.chFilePropery.Text = "属性";
			this.chFilePropery.Width = 80;
			// 
			// chFileExt
			// 
			this.chFileExt.Text = "其他";
			// 
			// chFileOwner
			// 
			this.chFileOwner.Text = "拥有者";
			// 
			// chOwnerGroup
			// 
			this.chOwnerGroup.Text = "所属组";
			// 
			// cmsMain
			// 
			this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.下载ToolStripMenuItem,
									this.删除ToolStripMenuItem,
									this.重命名ToolStripMenuItem});
			this.cmsMain.Name = "cmsMain";
			this.cmsMain.Size = new System.Drawing.Size(113, 70);
			this.cmsMain.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMain_Opening);
			// 
			// 下载ToolStripMenuItem
			// 
			this.下载ToolStripMenuItem.Name = "下载ToolStripMenuItem";
			this.下载ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.下载ToolStripMenuItem.Text = "下载";
			this.下载ToolStripMenuItem.Click += new System.EventHandler(this.下载ToolStripMenuItem_Click);
			// 
			// 删除ToolStripMenuItem
			// 
			this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
			this.删除ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.删除ToolStripMenuItem.Text = "删除";
			this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
			// 
			// 重命名ToolStripMenuItem
			// 
			this.重命名ToolStripMenuItem.Name = "重命名ToolStripMenuItem";
			this.重命名ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.重命名ToolStripMenuItem.Text = "重命名";
			this.重命名ToolStripMenuItem.Click += new System.EventHandler(this.重命名ToolStripMenuItem_Click);
			// 
			// ilExplorer
			// 
			this.ilExplorer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilExplorer.ImageStream")));
			this.ilExplorer.TransparentColor = System.Drawing.Color.Transparent;
			this.ilExplorer.Images.SetKeyName(0, "Folder.png");
			this.ilExplorer.Images.SetKeyName(1, "File.png");
			this.ilExplorer.Images.SetKeyName(2, "SdCard.png");
			// 
			// tpCommand
			// 
			this.tpCommand.Controls.Add(this.btnShellHelp);
			this.tpCommand.Controls.Add(this.txtShellOutput);
			this.tpCommand.Controls.Add(this.btnShellSendCmd);
			this.tpCommand.Controls.Add(this.txtShellInput);
			this.tpCommand.Location = new System.Drawing.Point(4, 22);
			this.tpCommand.Name = "tpCommand";
			this.tpCommand.Padding = new System.Windows.Forms.Padding(3);
			this.tpCommand.Size = new System.Drawing.Size(726, 430);
			this.tpCommand.TabIndex = 1;
			this.tpCommand.Text = "Shell控制台";
			this.tpCommand.UseVisualStyleBackColor = true;
			// 
			// btnShellHelp
			// 
			this.btnShellHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnShellHelp.Location = new System.Drawing.Point(582, 383);
			this.btnShellHelp.Name = "btnShellHelp";
			this.btnShellHelp.Size = new System.Drawing.Size(15, 23);
			this.btnShellHelp.TabIndex = 2;
			this.btnShellHelp.Text = "?";
			this.btnShellHelp.UseVisualStyleBackColor = true;
			this.btnShellHelp.Click += new System.EventHandler(this.btnShellHelp_Click);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnAdbHelp);
			this.tabPage1.Controls.Add(this.txtAdbOutput);
			this.tabPage1.Controls.Add(this.btnAdbSendCmd);
			this.tabPage1.Controls.Add(this.txtAdbInput);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(726, 430);
			this.tabPage1.TabIndex = 2;
			this.tabPage1.Text = "ADB控制台";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// btnAdbHelp
			// 
			this.btnAdbHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdbHelp.Location = new System.Drawing.Point(582, 384);
			this.btnAdbHelp.Name = "btnAdbHelp";
			this.btnAdbHelp.Size = new System.Drawing.Size(15, 23);
			this.btnAdbHelp.TabIndex = 6;
			this.btnAdbHelp.Text = "?";
			this.btnAdbHelp.UseVisualStyleBackColor = true;
			this.btnAdbHelp.Click += new System.EventHandler(this.btnAdbHelp_Click);
			// 
			// txtAdbOutput
			// 
			this.txtAdbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAdbOutput.Location = new System.Drawing.Point(6, 7);
			this.txtAdbOutput.Multiline = true;
			this.txtAdbOutput.Name = "txtAdbOutput";
			this.txtAdbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAdbOutput.Size = new System.Drawing.Size(591, 373);
			this.txtAdbOutput.TabIndex = 4;
			// 
			// btnAdbSendCmd
			// 
			this.btnAdbSendCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdbSendCmd.Location = new System.Drawing.Point(501, 384);
			this.btnAdbSendCmd.Name = "btnAdbSendCmd";
			this.btnAdbSendCmd.Size = new System.Drawing.Size(75, 23);
			this.btnAdbSendCmd.TabIndex = 5;
			this.btnAdbSendCmd.Text = "发送";
			this.btnAdbSendCmd.UseVisualStyleBackColor = true;
			this.btnAdbSendCmd.Click += new System.EventHandler(this.btnAdbSendCmd_Click);
			// 
			// txtAdbInput
			// 
			this.txtAdbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAdbInput.Location = new System.Drawing.Point(6, 386);
			this.txtAdbInput.Name = "txtAdbInput";
			this.txtAdbInput.Size = new System.Drawing.Size(489, 21);
			this.txtAdbInput.TabIndex = 3;
			this.txtAdbInput.Leave += new System.EventHandler(this.Input_Leave);
			this.txtAdbInput.Enter += new System.EventHandler(this.txtAdbInput_Enter);
			// 
			// tpShowcase
			// 
			this.tpShowcase.Controls.Add(this.btnPoweroff);
			this.tpShowcase.Controls.Add(this.btnRebootRecovery);
			this.tpShowcase.Controls.Add(this.btnReboot);
			this.tpShowcase.Location = new System.Drawing.Point(4, 22);
			this.tpShowcase.Name = "tpShowcase";
			this.tpShowcase.Padding = new System.Windows.Forms.Padding(3);
			this.tpShowcase.Size = new System.Drawing.Size(726, 430);
			this.tpShowcase.TabIndex = 3;
			this.tpShowcase.Text = "常用功能";
			this.tpShowcase.UseVisualStyleBackColor = true;
			// 
			// btnPoweroff
			// 
			this.btnPoweroff.Enabled = false;
			this.btnPoweroff.Location = new System.Drawing.Point(19, 64);
			this.btnPoweroff.Name = "btnPoweroff";
			this.btnPoweroff.Size = new System.Drawing.Size(101, 23);
			this.btnPoweroff.TabIndex = 0;
			this.btnPoweroff.Text = "关机";
			this.btnPoweroff.UseVisualStyleBackColor = true;
			this.btnPoweroff.Click += new System.EventHandler(this.btnPoweroff_Click);
			// 
			// btnRebootRecovery
			// 
			this.btnRebootRecovery.Location = new System.Drawing.Point(19, 35);
			this.btnRebootRecovery.Name = "btnRebootRecovery";
			this.btnRebootRecovery.Size = new System.Drawing.Size(101, 23);
			this.btnRebootRecovery.TabIndex = 0;
			this.btnRebootRecovery.Text = "进入Recovery";
			this.btnRebootRecovery.UseVisualStyleBackColor = true;
			this.btnRebootRecovery.Click += new System.EventHandler(this.btnRebootRecovery_Click);
			// 
			// btnReboot
			// 
			this.btnReboot.Location = new System.Drawing.Point(19, 6);
			this.btnReboot.Name = "btnReboot";
			this.btnReboot.Size = new System.Drawing.Size(101, 23);
			this.btnReboot.TabIndex = 0;
			this.btnReboot.Text = "重启";
			this.btnReboot.UseVisualStyleBackColor = true;
			this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
			// 
			// tpAbout
			// 
			this.tpAbout.Controls.Add(this.textBox1);
			this.tpAbout.Controls.Add(this.lblVersion);
			this.tpAbout.Controls.Add(this.label1);
			this.tpAbout.Controls.Add(this.pbScbeta);
			this.tpAbout.Location = new System.Drawing.Point(4, 22);
			this.tpAbout.Name = "tpAbout";
			this.tpAbout.Padding = new System.Windows.Forms.Padding(3);
			this.tpAbout.Size = new System.Drawing.Size(726, 430);
			this.tpAbout.TabIndex = 4;
			this.tpAbout.Text = "关于";
			this.tpAbout.UseVisualStyleBackColor = true;
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblVersion.AutoSize = true;
			this.lblVersion.Location = new System.Drawing.Point(311, 89);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(0, 12);
			this.lblVersion.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(264, 89);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "版本：";
			// 
			// pbScbeta
			// 
			this.pbScbeta.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.pbScbeta.BackColor = System.Drawing.Color.White;
			this.pbScbeta.Image = ((System.Drawing.Image)(resources.GetObject("pbScbeta.Image")));
			this.pbScbeta.Location = new System.Drawing.Point(191, 6);
			this.pbScbeta.Name = "pbScbeta";
			this.pbScbeta.Size = new System.Drawing.Size(214, 68);
			this.pbScbeta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbScbeta.TabIndex = 0;
			this.pbScbeta.TabStop = false;
			this.pbScbeta.Click += new System.EventHandler(this.pbScbeta_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(92, 130);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(566, 281);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "Made by aaaSoft\r\nEmail:scbeta@qq.com\r\n\r\n更新时间：2012-6-10";
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(758, 480);
			this.Controls.Add(this.tcMain);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "XDAndroid Explorer";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.tcMain.ResumeLayout(false);
			this.tpFileManager.ResumeLayout(false);
			this.tpFileManager.PerformLayout();
			this.tsAddress.ResumeLayout(false);
			this.tsAddress.PerformLayout();
			this.tsOperate.ResumeLayout(false);
			this.tsOperate.PerformLayout();
			this.cmsMain.ResumeLayout(false);
			this.tpCommand.ResumeLayout(false);
			this.tpCommand.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tpShowcase.ResumeLayout(false);
			this.tpAbout.ResumeLayout(false);
			this.tpAbout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbScbeta)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.TextBox textBox1;

		#endregion

		private System.Windows.Forms.TextBox txtShellInput;
		private System.Windows.Forms.Button btnShellSendCmd;
		private System.Windows.Forms.TextBox txtShellOutput;
		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.TabPage tpFileManager;
		private System.Windows.Forms.TabPage tpCommand;
		private System.Windows.Forms.ListView lvExplorer;
		private System.Windows.Forms.ColumnHeader chFileName;
		private System.Windows.Forms.ColumnHeader chFileSize;
		private System.Windows.Forms.ColumnHeader chFileType;
		private System.Windows.Forms.ColumnHeader chModifyTime;
		private System.Windows.Forms.ColumnHeader chFilePropery;
		private System.Windows.Forms.ColumnHeader chFileExt;
		private System.Windows.Forms.ColumnHeader chFileOwner;
		private System.Windows.Forms.ColumnHeader chOwnerGroup;
		private System.Windows.Forms.ToolStrip tsOperate;
		private System.Windows.Forms.ToolStripButton btnRefrush;
		private System.Windows.Forms.ToolStripButton btnUp;
		private System.Windows.Forms.ToolStrip tsAddress;
		private System.Windows.Forms.ToolStripTextBox txtAddress;
		private System.Windows.Forms.ToolStripButton btnGotoFolder;
		private System.Windows.Forms.ContextMenuStrip cmsMain;
		private System.Windows.Forms.ToolStripMenuItem 下载ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 重命名ToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton btnNewFolder;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnShellHelp;
		private System.Windows.Forms.Button btnAdbHelp;
		private System.Windows.Forms.TextBox txtAdbOutput;
		private System.Windows.Forms.Button btnAdbSendCmd;
		private System.Windows.Forms.TextBox txtAdbInput;
		private System.Windows.Forms.ImageList ilExplorer;
		private System.Windows.Forms.TabPage tpShowcase;
		private System.Windows.Forms.TabPage tpAbout;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label label2;
		
		private System.Windows.Forms.Button btnPoweroff;
		private System.Windows.Forms.Button btnReboot;
		private System.Windows.Forms.Button btnRebootRecovery;
		private System.Windows.Forms.PictureBox pbScbeta;
		
		private System.Windows.Forms.Label label1;
	}
}

