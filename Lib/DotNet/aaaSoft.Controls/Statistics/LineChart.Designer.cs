namespace aaaSoft.Controls.Statistics
{
    partial class LineChart
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
            this.ttData = new System.Windows.Forms.ToolTip(this.components);
            this.flpDesc = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlDrawPaper = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flpDesc
            // 
            this.flpDesc.AutoSize = true;
            this.flpDesc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpDesc.Location = new System.Drawing.Point(0, 257);
            this.flpDesc.Name = "flpDesc";
            this.flpDesc.Size = new System.Drawing.Size(452, 0);
            this.flpDesc.TabIndex = 6;
            // 
            // pnlDrawPaper
            // 
            this.pnlDrawPaper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDrawPaper.Location = new System.Drawing.Point(16, 35);
            this.pnlDrawPaper.Name = "pnlDrawPaper";
            this.pnlDrawPaper.Size = new System.Drawing.Size(423, 135);
            this.pnlDrawPaper.TabIndex = 5;
            this.pnlDrawPaper.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDrawPaper_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(452, 32);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flpDesc);
            this.Controls.Add(this.pnlDrawPaper);
            this.Controls.Add(this.lblTitle);
            this.Name = "LineChart";
            this.Size = new System.Drawing.Size(452, 257);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip ttData;
        private System.Windows.Forms.FlowLayoutPanel flpDesc;
        private System.Windows.Forms.Panel pnlDrawPaper;
        private System.Windows.Forms.Label lblTitle;
    }
}
