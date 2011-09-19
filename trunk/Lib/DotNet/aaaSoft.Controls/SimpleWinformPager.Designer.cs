namespace aaaSoft.Controls
{
    partial class SimpleWinformPager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleWinformPager));
            this.tsPager = new System.Windows.Forms.ToolStrip();
            this.btnLastPage = new System.Windows.Forms.ToolStripButton();
            this.btnNextPage = new System.Windows.Forms.ToolStripButton();
            this.lblTotalPageCount = new System.Windows.Forms.ToolStripLabel();
            this.txtPageIndex = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.btnPrePage = new System.Windows.Forms.ToolStripButton();
            this.btnFirstPage = new System.Windows.Forms.ToolStripButton();
            this.tsPager.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsPager
            // 
            this.tsPager.AutoSize = false;
            this.tsPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsPager.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsPager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLastPage,
            this.btnNextPage,
            this.lblTotalPageCount,
            this.txtPageIndex,
            this.toolStripLabel2,
            this.btnPrePage,
            this.btnFirstPage});
            this.tsPager.Location = new System.Drawing.Point(0, 0);
            this.tsPager.Name = "tsPager";
            this.tsPager.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tsPager.Size = new System.Drawing.Size(216, 25);
            this.tsPager.TabIndex = 11;
            this.tsPager.Text = "toolStrip1";
            // 
            // btnLastPage
            // 
            this.btnLastPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLastPage.Image = ((System.Drawing.Image)(resources.GetObject("btnLastPage.Image")));
            this.btnLastPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(23, 22);
            this.btnLastPage.Text = "最后一页";
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNextPage.Image = ((System.Drawing.Image)(resources.GetObject("btnNextPage.Image")));
            this.btnNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(23, 22);
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // lblTotalPageCount
            // 
            this.lblTotalPageCount.Name = "lblTotalPageCount";
            this.lblTotalPageCount.Size = new System.Drawing.Size(41, 22);
            this.lblTotalPageCount.Text = "页/0页";
            // 
            // txtPageIndex
            // 
            this.txtPageIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPageIndex.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPageIndex.Name = "txtPageIndex";
            this.txtPageIndex.Size = new System.Drawing.Size(40, 25);
            this.txtPageIndex.Text = "0";
            this.txtPageIndex.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPageIndex.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageIndex_KeyUp);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(17, 22);
            this.toolStripLabel2.Text = "第";
            // 
            // btnPrePage
            // 
            this.btnPrePage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrePage.Image = ((System.Drawing.Image)(resources.GetObject("btnPrePage.Image")));
            this.btnPrePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrePage.Name = "btnPrePage";
            this.btnPrePage.Size = new System.Drawing.Size(23, 22);
            this.btnPrePage.Text = "上一页";
            this.btnPrePage.Click += new System.EventHandler(this.btnPrePage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFirstPage.Image = ((System.Drawing.Image)(resources.GetObject("btnFirstPage.Image")));
            this.btnFirstPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(23, 22);
            this.btnFirstPage.Text = "第一页";
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // SimpleWinformPager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tsPager);
            this.Name = "SimpleWinformPager";
            this.Size = new System.Drawing.Size(216, 25);
            this.tsPager.ResumeLayout(false);
            this.tsPager.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsPager;
        private System.Windows.Forms.ToolStripButton btnLastPage;
        private System.Windows.Forms.ToolStripButton btnNextPage;
        private System.Windows.Forms.ToolStripLabel lblTotalPageCount;
        private System.Windows.Forms.ToolStripTextBox txtPageIndex;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton btnPrePage;
        private System.Windows.Forms.ToolStripButton btnFirstPage;
    }
}
