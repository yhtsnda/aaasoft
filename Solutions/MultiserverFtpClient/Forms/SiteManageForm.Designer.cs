namespace MultiserverFtpClient.Forms
{
    partial class SiteManageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteManageForm));
            this.btnAddSite = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.tvSites = new System.Windows.Forms.TreeView();
            this.ilSites = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.tcSiteEdit = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.btnSelectLocalPath = new System.Windows.Forms.Button();
            this.cbIsAnonymous = new System.Windows.Forms.CheckBox();
            this.txtTip = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRemotePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSiteName = new System.Windows.Forms.TextBox();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpConfig = new System.Windows.Forms.TabPage();
            this.cbIsShowHidenFile = new System.Windows.Forms.CheckBox();
            this.cbIsUseMlsdToListFolder = new System.Windows.Forms.CheckBox();
            this.cbIsNotSupportFEAT = new System.Windows.Forms.CheckBox();
            this.tpTransfer = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nudBufferSize = new System.Windows.Forms.NumericUpDown();
            this.tpAdvance = new System.Windows.Forms.TabPage();
            this.cbStringEncoding = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tcSiteEdit.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpConfig.SuspendLayout();
            this.tpTransfer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBufferSize)).BeginInit();
            this.tpAdvance.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddSite
            // 
            this.btnAddSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSite.Location = new System.Drawing.Point(12, 334);
            this.btnAddSite.Name = "btnAddSite";
            this.btnAddSite.Size = new System.Drawing.Size(79, 23);
            this.btnAddSite.TabIndex = 1;
            this.btnAddSite.Text = "新建站点(&S)";
            this.btnAddSite.UseVisualStyleBackColor = true;
            this.btnAddSite.Click += new System.EventHandler(this.btnAddSite_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(315, 334);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "应用(&A)";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(182, 334);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(79, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Enabled = false;
            this.btnConnect.Location = new System.Drawing.Point(396, 334);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "连接(&C)";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddGroup.Location = new System.Drawing.Point(97, 334);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(79, 23);
            this.btnAddGroup.TabIndex = 2;
            this.btnAddGroup.Text = "新建组(&G)";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // tvSites
            // 
            this.tvSites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSites.HideSelection = false;
            this.tvSites.ImageIndex = 0;
            this.tvSites.ImageList = this.ilSites;
            this.tvSites.Location = new System.Drawing.Point(0, 0);
            this.tvSites.Name = "tvSites";
            this.tvSites.SelectedImageIndex = 0;
            this.tvSites.Size = new System.Drawing.Size(209, 329);
            this.tvSites.TabIndex = 0;
            this.tvSites.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvSites_BeforeSelect);
            this.tvSites.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSites_AfterSelect);
            this.tvSites.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvSites_MouseDoubleClick);
            // 
            // ilSites
            // 
            this.ilSites.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSites.ImageStream")));
            this.ilSites.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSites.Images.SetKeyName(0, "Group");
            this.ilSites.Images.SetKeyName(1, "Site");
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(477, 334);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tcSiteEdit
            // 
            this.tcSiteEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcSiteEdit.Controls.Add(this.tpGeneral);
            this.tcSiteEdit.Controls.Add(this.tpConfig);
            this.tcSiteEdit.Controls.Add(this.tpTransfer);
            this.tcSiteEdit.Controls.Add(this.tpAdvance);
            this.tcSiteEdit.Location = new System.Drawing.Point(210, 0);
            this.tcSiteEdit.Name = "tcSiteEdit";
            this.tcSiteEdit.SelectedIndex = 0;
            this.tcSiteEdit.Size = new System.Drawing.Size(355, 329);
            this.tcSiteEdit.TabIndex = 100;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.btnSelectLocalPath);
            this.tpGeneral.Controls.Add(this.cbIsAnonymous);
            this.tpGeneral.Controls.Add(this.txtTip);
            this.tpGeneral.Controls.Add(this.label8);
            this.tpGeneral.Controls.Add(this.txtLocalPath);
            this.tpGeneral.Controls.Add(this.label7);
            this.tpGeneral.Controls.Add(this.txtRemotePath);
            this.tpGeneral.Controls.Add(this.label6);
            this.tpGeneral.Controls.Add(this.txtPassword);
            this.tpGeneral.Controls.Add(this.label4);
            this.tpGeneral.Controls.Add(this.txtUserName);
            this.tpGeneral.Controls.Add(this.label3);
            this.tpGeneral.Controls.Add(this.txtPort);
            this.tpGeneral.Controls.Add(this.label2);
            this.tpGeneral.Controls.Add(this.txtSiteName);
            this.tpGeneral.Controls.Add(this.txtHostName);
            this.tpGeneral.Controls.Add(this.label5);
            this.tpGeneral.Controls.Add(this.label1);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(347, 303);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "常规";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // btnSelectLocalPath
            // 
            this.btnSelectLocalPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectLocalPath.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectLocalPath.Image")));
            this.btnSelectLocalPath.Location = new System.Drawing.Point(308, 148);
            this.btnSelectLocalPath.Name = "btnSelectLocalPath";
            this.btnSelectLocalPath.Size = new System.Drawing.Size(30, 23);
            this.btnSelectLocalPath.TabIndex = 16;
            this.btnSelectLocalPath.UseVisualStyleBackColor = true;
            this.btnSelectLocalPath.Click += new System.EventHandler(this.btnSelectLocalPath_Click);
            // 
            // cbIsAnonymous
            // 
            this.cbIsAnonymous.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIsAnonymous.AutoSize = true;
            this.cbIsAnonymous.Location = new System.Drawing.Point(254, 70);
            this.cbIsAnonymous.Name = "cbIsAnonymous";
            this.cbIsAnonymous.Size = new System.Drawing.Size(66, 16);
            this.cbIsAnonymous.TabIndex = 11;
            this.cbIsAnonymous.Text = "匿名(&Y)";
            this.cbIsAnonymous.UseVisualStyleBackColor = true;
            // 
            // txtTip
            // 
            this.txtTip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTip.Location = new System.Drawing.Point(116, 176);
            this.txtTip.Multiline = true;
            this.txtTip.Name = "txtTip";
            this.txtTip.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTip.Size = new System.Drawing.Size(222, 121);
            this.txtTip.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "注释(&T)";
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalPath.Location = new System.Drawing.Point(116, 149);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.Size = new System.Drawing.Size(186, 21);
            this.txtLocalPath.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "本地路径(&L)";
            // 
            // txtRemotePath
            // 
            this.txtRemotePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemotePath.Location = new System.Drawing.Point(116, 122);
            this.txtRemotePath.Name = "txtRemotePath";
            this.txtRemotePath.Size = new System.Drawing.Size(222, 21);
            this.txtRemotePath.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "远端路径(&O)";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(116, 95);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(132, 21);
            this.txtPassword.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "密码(&P)";
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(116, 68);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(132, 21);
            this.txtUserName.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "用户名称(&U)";
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(289, 41);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(49, 21);
            this.txtPort.TabIndex = 9;
            this.txtPort.Text = "21";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "端口";
            // 
            // txtSiteName
            // 
            this.txtSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSiteName.Location = new System.Drawing.Point(116, 14);
            this.txtSiteName.Name = "txtSiteName";
            this.txtSiteName.Size = new System.Drawing.Size(222, 21);
            this.txtSiteName.TabIndex = 7;
            // 
            // txtHostName
            // 
            this.txtHostName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostName.Location = new System.Drawing.Point(116, 41);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(132, 21);
            this.txtHostName.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "站点名称(&N)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "主机地址(&H)";
            // 
            // tpConfig
            // 
            this.tpConfig.Controls.Add(this.cbIsShowHidenFile);
            this.tpConfig.Controls.Add(this.cbIsUseMlsdToListFolder);
            this.tpConfig.Controls.Add(this.cbIsNotSupportFEAT);
            this.tpConfig.Location = new System.Drawing.Point(4, 22);
            this.tpConfig.Name = "tpConfig";
            this.tpConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfig.Size = new System.Drawing.Size(347, 303);
            this.tpConfig.TabIndex = 1;
            this.tpConfig.Text = "选项";
            this.tpConfig.UseVisualStyleBackColor = true;
            // 
            // cbIsShowHidenFile
            // 
            this.cbIsShowHidenFile.AutoSize = true;
            this.cbIsShowHidenFile.Location = new System.Drawing.Point(14, 6);
            this.cbIsShowHidenFile.Name = "cbIsShowHidenFile";
            this.cbIsShowHidenFile.Size = new System.Drawing.Size(162, 16);
            this.cbIsShowHidenFile.TabIndex = 0;
            this.cbIsShowHidenFile.Text = "显示隐藏文件 (LIST -al)";
            this.cbIsShowHidenFile.UseVisualStyleBackColor = true;
            // 
            // cbIsUseMlsdToListFolder
            // 
            this.cbIsUseMlsdToListFolder.AutoSize = true;
            this.cbIsUseMlsdToListFolder.Location = new System.Drawing.Point(14, 28);
            this.cbIsUseMlsdToListFolder.Name = "cbIsUseMlsdToListFolder";
            this.cbIsUseMlsdToListFolder.Size = new System.Drawing.Size(132, 16);
            this.cbIsUseMlsdToListFolder.TabIndex = 0;
            this.cbIsUseMlsdToListFolder.Text = "使用 MLSD 列目录 *";
            this.cbIsUseMlsdToListFolder.UseVisualStyleBackColor = true;
            // 
            // cbIsNotSupportFEAT
            // 
            this.cbIsNotSupportFEAT.AutoSize = true;
            this.cbIsNotSupportFEAT.Location = new System.Drawing.Point(14, 50);
            this.cbIsNotSupportFEAT.Name = "cbIsNotSupportFEAT";
            this.cbIsNotSupportFEAT.Size = new System.Drawing.Size(144, 16);
            this.cbIsNotSupportFEAT.TabIndex = 0;
            this.cbIsNotSupportFEAT.Text = "站点不支持 FEAT 命令";
            this.cbIsNotSupportFEAT.UseVisualStyleBackColor = true;
            // 
            // tpTransfer
            // 
            this.tpTransfer.Controls.Add(this.groupBox1);
            this.tpTransfer.Location = new System.Drawing.Point(4, 22);
            this.tpTransfer.Name = "tpTransfer";
            this.tpTransfer.Padding = new System.Windows.Forms.Padding(3);
            this.tpTransfer.Size = new System.Drawing.Size(347, 303);
            this.tpTransfer.TabIndex = 2;
            this.tpTransfer.Text = "传送";
            this.tpTransfer.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.nudBufferSize);
            this.groupBox1.Location = new System.Drawing.Point(180, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 55);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP/IP 缓冲大小";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(123, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "字节";
            // 
            // nudBufferSize
            // 
            this.nudBufferSize.Increment = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudBufferSize.Location = new System.Drawing.Point(6, 20);
            this.nudBufferSize.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.nudBufferSize.Minimum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.nudBufferSize.Name = "nudBufferSize";
            this.nudBufferSize.Size = new System.Drawing.Size(111, 21);
            this.nudBufferSize.TabIndex = 2;
            this.nudBufferSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBufferSize.ThousandsSeparator = true;
            this.nudBufferSize.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            // 
            // tpAdvance
            // 
            this.tpAdvance.Controls.Add(this.cbStringEncoding);
            this.tpAdvance.Controls.Add(this.label10);
            this.tpAdvance.Location = new System.Drawing.Point(4, 22);
            this.tpAdvance.Name = "tpAdvance";
            this.tpAdvance.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdvance.Size = new System.Drawing.Size(347, 303);
            this.tpAdvance.TabIndex = 3;
            this.tpAdvance.Text = "高级";
            this.tpAdvance.UseVisualStyleBackColor = true;
            // 
            // cbStringEncoding
            // 
            this.cbStringEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStringEncoding.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbStringEncoding.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbStringEncoding.FormattingEnabled = true;
            this.cbStringEncoding.Location = new System.Drawing.Point(164, 103);
            this.cbStringEncoding.Name = "cbStringEncoding";
            this.cbStringEncoding.Size = new System.Drawing.Size(174, 20);
            this.cbStringEncoding.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(162, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "字符编码";
            // 
            // SiteManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 369);
            this.Controls.Add(this.tcSiteEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tvSites);
            this.Controls.Add(this.btnAddGroup);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnAddSite);
            this.MinimumSize = new System.Drawing.Size(572, 396);
            this.Name = "SiteManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "站点管理器";
            this.Load += new System.EventHandler(this.SiteManageForm_Load);
            this.tcSiteEdit.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpConfig.ResumeLayout(false);
            this.tpConfig.PerformLayout();
            this.tpTransfer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBufferSize)).EndInit();
            this.tpAdvance.ResumeLayout(false);
            this.tpAdvance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddSite;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.TreeView tvSites;
        private System.Windows.Forms.ImageList ilSites;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tcSiteEdit;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHostName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSiteName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbIsAnonymous;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRemotePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTip;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tpConfig;
        private System.Windows.Forms.Button btnSelectLocalPath;
        private System.Windows.Forms.TabPage tpTransfer;
        private System.Windows.Forms.TabPage tpAdvance;
        private System.Windows.Forms.CheckBox cbIsNotSupportFEAT;
        private System.Windows.Forms.CheckBox cbIsUseMlsdToListFolder;
        private System.Windows.Forms.CheckBox cbIsShowHidenFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudBufferSize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbStringEncoding;
        private System.Windows.Forms.Label label10;
    }
}