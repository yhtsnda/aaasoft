namespace aaaSoft.Controls
{
    partial class FileBrowser
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileBrowser));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.cbAddress = new aaaSoft.Controls.ToolStripSpringComboBox();
            this.lvBrowser = new aaaSoft.Controls.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chModifyTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重命名RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.新建文件夹NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilAddress = new System.Windows.Forms.ImageList(this.components);
            this.tsMain.SuspendLayout();
            this.cmsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUp,
            this.cbAddress});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(335, 25);
            this.tsMain.Stretch = true;
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Text = "返回上一层";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // cbAddress
            // 
            this.cbAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbAddress.Name = "cbAddress";
            this.cbAddress.Size = new System.Drawing.Size(278, 25);
            this.cbAddress.DropDown += new System.EventHandler(this.cbAddress_DropDown);
            this.cbAddress.SelectedIndexChanged += new System.EventHandler(this.cbAddress_SelectedIndexChanged);
            this.cbAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbAddress_KeyDown);
            // 
            // lvBrowser
            // 
            this.lvBrowser.AllowDrop = true;
            this.lvBrowser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chSize,
            this.chModifyTime});
            this.lvBrowser.ContextMenuStrip = this.cmsMain;
            this.lvBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvBrowser.FloatingColumnIndex = 0;
            this.lvBrowser.FullRowSelect = true;
            this.lvBrowser.HideSelection = false;
            this.lvBrowser.IsUseFloating = true;
            this.lvBrowser.Location = new System.Drawing.Point(0, 25);
            this.lvBrowser.Name = "lvBrowser";
            this.lvBrowser.Size = new System.Drawing.Size(335, 303);
            this.lvBrowser.SmallImageList = this.ilAddress;
            this.lvBrowser.TabIndex = 1;
            this.lvBrowser.UseCompatibleStateImageBehavior = false;
            this.lvBrowser.View = System.Windows.Forms.View.Details;
            this.lvBrowser.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvBrowser_ItemDrag);
            this.lvBrowser.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvBrowser_DragDrop);
            this.lvBrowser.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvBrowser_DragEnter);
            this.lvBrowser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvBrowser_KeyDown);
            this.lvBrowser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvBrowser_MouseDoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "名称";
            this.chName.Width = 130;
            // 
            // chSize
            // 
            this.chSize.Text = "大小";
            this.chSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chModifyTime
            // 
            this.chModifyTime.Text = "修改";
            this.chModifyTime.Width = 140;
            // 
            // cmsMain
            // 
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.删除DToolStripMenuItem,
            this.重命名RToolStripMenuItem,
            this.toolStripMenuItem1,
            this.新建文件夹NToolStripMenuItem,
            this.toolStripMenuItem2,
            this.刷新XToolStripMenuItem});
            this.cmsMain.Name = "cmsMain";
            this.cmsMain.Size = new System.Drawing.Size(149, 126);
            this.cmsMain.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMain_Opening);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.打开ToolStripMenuItem.Text = "打开(&O)";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // 新建文件夹NToolStripMenuItem
            // 
            this.新建文件夹NToolStripMenuItem.Name = "新建文件夹NToolStripMenuItem";
            this.新建文件夹NToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.新建文件夹NToolStripMenuItem.Text = "新建文件夹(&N)";
            this.新建文件夹NToolStripMenuItem.Click += new System.EventHandler(this.新建文件夹NToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 6);
            // 
            // 刷新XToolStripMenuItem
            // 
            this.刷新XToolStripMenuItem.Name = "刷新XToolStripMenuItem";
            this.刷新XToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.刷新XToolStripMenuItem.Text = "刷新(&F)";
            this.刷新XToolStripMenuItem.Click += new System.EventHandler(this.刷新XToolStripMenuItem_Click);
            // 
            // ilAddress
            // 
            this.ilAddress.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilAddress.ImageStream")));
            this.ilAddress.TransparentColor = System.Drawing.Color.Transparent;
            this.ilAddress.Images.SetKeyName(0, "CD_Drive");
            this.ilAddress.Images.SetKeyName(1, "Hard_Drive");
            this.ilAddress.Images.SetKeyName(2, "Desktop");
            this.ilAddress.Images.SetKeyName(3, "File");
            this.ilAddress.Images.SetKeyName(4, "Folder");
            // 
            // FileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvBrowser);
            this.Controls.Add(this.tsMain);
            this.Name = "FileBrowser";
            this.Size = new System.Drawing.Size(335, 328);
            this.Load += new System.EventHandler(this.FileBrowser_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.cmsMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton btnUp;
        private ToolStripSpringComboBox cbAddress;
        private ListView lvBrowser;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chModifyTime;
        private System.Windows.Forms.ImageList ilAddress;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重命名RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 新建文件夹NToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 刷新XToolStripMenuItem;
    }
}
