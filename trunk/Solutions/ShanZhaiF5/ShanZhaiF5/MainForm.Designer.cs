namespace ShanZhaiF5
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
        	this.tvFile = new System.Windows.Forms.TreeView();
        	this.cmsFile = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.在浏览器中访问ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.用记事本打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.ilFileIcon = new System.Windows.Forms.ImageList(this.components);
        	this.lblTip = new System.Windows.Forms.Label();
        	this.fswWebFolder = new System.IO.FileSystemWatcher();
        	this.txtWebRootUrl = new System.Windows.Forms.TextBox();
        	this.btnGo = new System.Windows.Forms.Button();
        	this.btnAbout = new System.Windows.Forms.Button();
        	this.lblCurrentFolderPath = new System.Windows.Forms.Label();
        	this.btnChangeFolder = new System.Windows.Forms.Button();
        	this.cmsFile.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.fswWebFolder)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// tvFile
        	// 
        	this.tvFile.AllowDrop = true;
        	this.tvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.tvFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        	this.tvFile.ContextMenuStrip = this.cmsFile;
        	this.tvFile.FullRowSelect = true;
        	this.tvFile.ImageIndex = 0;
        	this.tvFile.ImageList = this.ilFileIcon;
        	this.tvFile.Location = new System.Drawing.Point(0, 26);
        	this.tvFile.Name = "tvFile";
        	this.tvFile.PathSeparator = "/";
        	this.tvFile.SelectedImageIndex = 0;
        	this.tvFile.Size = new System.Drawing.Size(259, 332);
        	this.tvFile.TabIndex = 0;
        	this.tvFile.Visible = false;
        	this.tvFile.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvFile_ItemDrag);
        	this.tvFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvFile_DragDrop);
        	this.tvFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvFile_DragEnter);
        	this.tvFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvFile_MouseDoubleClick);
        	this.tvFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvFile_MouseDown);
        	// 
        	// cmsFile
        	// 
        	this.cmsFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.在浏览器中访问ToolStripMenuItem,
        	        	        	this.用记事本打开ToolStripMenuItem});
        	this.cmsFile.Name = "cmsFile";
        	this.cmsFile.Size = new System.Drawing.Size(167, 70);
        	this.cmsFile.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFile_Opening);
        	// 
        	// 在浏览器中访问ToolStripMenuItem
        	// 
        	this.在浏览器中访问ToolStripMenuItem.Name = "在浏览器中访问ToolStripMenuItem";
        	this.在浏览器中访问ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
        	this.在浏览器中访问ToolStripMenuItem.Text = "在浏览器中访问..";
        	this.在浏览器中访问ToolStripMenuItem.Click += new System.EventHandler(this.在浏览器中访问ToolStripMenuItem_Click);
        	// 
        	// 用记事本打开ToolStripMenuItem
        	// 
        	this.用记事本打开ToolStripMenuItem.Name = "用记事本打开ToolStripMenuItem";
        	this.用记事本打开ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
        	this.用记事本打开ToolStripMenuItem.Text = "用记事本打开..";
        	this.用记事本打开ToolStripMenuItem.Click += new System.EventHandler(this.用记事本打开ToolStripMenuItem_Click);
        	// 
        	// ilFileIcon
        	// 
        	this.ilFileIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilFileIcon.ImageStream")));
        	this.ilFileIcon.TransparentColor = System.Drawing.Color.Transparent;
        	this.ilFileIcon.Images.SetKeyName(0, "file");
        	this.ilFileIcon.Images.SetKeyName(1, "folder");
        	// 
        	// lblTip
        	// 
        	this.lblTip.Anchor = System.Windows.Forms.AnchorStyles.None;
        	this.lblTip.AutoSize = true;
        	this.lblTip.BackColor = System.Drawing.Color.Transparent;
        	this.lblTip.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.lblTip.Location = new System.Drawing.Point(32, 146);
        	this.lblTip.Name = "lblTip";
        	this.lblTip.Size = new System.Drawing.Size(199, 40);
        	this.lblTip.TabIndex = 1;
        	this.lblTip.Text = "拖放文件/文件夹到此\r\n或者点击打开文件夹";
        	this.lblTip.Click += new System.EventHandler(this.lblTip_Click);
        	// 
        	// fswWebFolder
        	// 
        	this.fswWebFolder.EnableRaisingEvents = true;
        	this.fswWebFolder.Filter = "*";
        	this.fswWebFolder.IncludeSubdirectories = true;
        	this.fswWebFolder.SynchronizingObject = this;
        	this.fswWebFolder.Changed += new System.IO.FileSystemEventHandler(this.fswWebFolder_Changed);
        	this.fswWebFolder.Created += new System.IO.FileSystemEventHandler(this.fswWebFolder_Changed);
        	this.fswWebFolder.Deleted += new System.IO.FileSystemEventHandler(this.fswWebFolder_Changed);
        	// 
        	// txtWebRootUrl
        	// 
        	this.txtWebRootUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.txtWebRootUrl.Location = new System.Drawing.Point(1, 363);
        	this.txtWebRootUrl.Name = "txtWebRootUrl";
        	this.txtWebRootUrl.ReadOnly = true;
        	this.txtWebRootUrl.Size = new System.Drawing.Size(190, 21);
        	this.txtWebRootUrl.TabIndex = 3;
        	// 
        	// btnGo
        	// 
        	this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnGo.Location = new System.Drawing.Point(196, 361);
        	this.btnGo.Name = "btnGo";
        	this.btnGo.Size = new System.Drawing.Size(35, 23);
        	this.btnGo.TabIndex = 2;
        	this.btnGo.Text = "GO";
        	this.btnGo.UseVisualStyleBackColor = true;
        	this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
        	// 
        	// btnAbout
        	// 
        	this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnAbout.Location = new System.Drawing.Point(237, 361);
        	this.btnAbout.Name = "btnAbout";
        	this.btnAbout.Size = new System.Drawing.Size(22, 23);
        	this.btnAbout.TabIndex = 4;
        	this.btnAbout.Text = "?";
        	this.btnAbout.UseVisualStyleBackColor = true;
        	this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
        	// 
        	// lblCurrentFolderPath
        	// 
        	this.lblCurrentFolderPath.AutoSize = true;
        	this.lblCurrentFolderPath.Location = new System.Drawing.Point(1, 4);
        	this.lblCurrentFolderPath.Name = "lblCurrentFolderPath";
        	this.lblCurrentFolderPath.Size = new System.Drawing.Size(65, 12);
        	this.lblCurrentFolderPath.TabIndex = 5;
        	this.lblCurrentFolderPath.Text = "当前路径：";
        	this.lblCurrentFolderPath.Visible = false;
        	// 
        	// btnChangeFolder
        	// 
        	this.btnChangeFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnChangeFolder.Location = new System.Drawing.Point(231, 1);
        	this.btnChangeFolder.Name = "btnChangeFolder";
        	this.btnChangeFolder.Size = new System.Drawing.Size(28, 19);
        	this.btnChangeFolder.TabIndex = 6;
        	this.btnChangeFolder.Text = "..";
        	this.btnChangeFolder.UseVisualStyleBackColor = true;
        	this.btnChangeFolder.Visible = false;
        	this.btnChangeFolder.Click += new System.EventHandler(this.BtnChangeFolderClick);
        	// 
        	// MainForm
        	// 
        	this.AllowDrop = true;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
        	this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        	this.ClientSize = new System.Drawing.Size(259, 385);
        	this.Controls.Add(this.btnChangeFolder);
        	this.Controls.Add(this.btnAbout);
        	this.Controls.Add(this.btnGo);
        	this.Controls.Add(this.txtWebRootUrl);
        	this.Controls.Add(this.lblTip);
        	this.Controls.Add(this.tvFile);
        	this.Controls.Add(this.lblCurrentFolderPath);
        	this.DoubleBuffered = true;
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "MainForm";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "山寨F5";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
        	this.Load += new System.EventHandler(this.MainForm_Load);
        	this.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvFile_DragDrop);
        	this.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvFile_DragEnter);
        	this.cmsFile.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.fswWebFolder)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Label lblCurrentFolderPath;
        private System.Windows.Forms.Button btnChangeFolder;

        #endregion

        private System.Windows.Forms.TreeView tvFile;
        private System.Windows.Forms.Label lblTip;
        private System.IO.FileSystemWatcher fswWebFolder;
        private System.Windows.Forms.ImageList ilFileIcon;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtWebRootUrl;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.ContextMenuStrip cmsFile;
        private System.Windows.Forms.ToolStripMenuItem 用记事本打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 在浏览器中访问ToolStripMenuItem;
    }
}

