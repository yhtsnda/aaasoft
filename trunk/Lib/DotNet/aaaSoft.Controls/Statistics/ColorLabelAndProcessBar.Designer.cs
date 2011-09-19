namespace aaaSoft.Controls.Statistics
{
    partial class ColorLabelAndProcessBar
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
            this.lblProcessText = new System.Windows.Forms.Label();
            this.pnlProcessBar = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblProcessText
            // 
            this.lblProcessText.AutoSize = true;
            this.lblProcessText.BackColor = System.Drawing.Color.Transparent;
            this.lblProcessText.Location = new System.Drawing.Point(3, 4);
            this.lblProcessText.Name = "lblProcessText";
            this.lblProcessText.Size = new System.Drawing.Size(23, 12);
            this.lblProcessText.TabIndex = 4;
            this.lblProcessText.Text = "13%";
            // 
            // pnlProcessBar
            // 
            this.pnlProcessBar.Location = new System.Drawing.Point(32, 3);
            this.pnlProcessBar.Name = "pnlProcessBar";
            this.pnlProcessBar.Size = new System.Drawing.Size(61, 14);
            this.pnlProcessBar.TabIndex = 5;
            this.pnlProcessBar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlProcessBar_Paint);
            // 
            // ColorLabelAndProcessBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lblProcessText);
            this.Controls.Add(this.pnlProcessBar);
            this.Name = "ColorLabelAndProcessBar";
            this.Size = new System.Drawing.Size(96, 20);
            this.Load += new System.EventHandler(this.ColorLabelAndProcessBar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessText;
        private System.Windows.Forms.Panel pnlProcessBar;


    }
}
