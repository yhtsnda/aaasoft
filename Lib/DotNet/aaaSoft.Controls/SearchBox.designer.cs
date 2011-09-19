namespace aaaSoft.Controls
{
    partial class SearchBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchBox));
            this.btnSearch = new System.Windows.Forms.PictureBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lblBack = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(160, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(24, 25);
            this.btnSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnSearch.TabIndex = 4;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKey
            // 
            this.txtKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKey.ForeColor = System.Drawing.Color.DarkGray;
            this.txtKey.Location = new System.Drawing.Point(5, 3);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(154, 16);
            this.txtKey.TabIndex = 2;
            this.txtKey.Text = "搜索";
            this.txtKey.TextChanged += new System.EventHandler(this.txtKey_TextChanged);
            this.txtKey.Enter += new System.EventHandler(this.txtKey_Enter);
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
            this.txtKey.Leave += new System.EventHandler(this.txtKey_Leave);
            // 
            // lblBack
            // 
            this.lblBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBack.Location = new System.Drawing.Point(0, 0);
            this.lblBack.Name = "lblBack";
            this.lblBack.Size = new System.Drawing.Size(184, 24);
            this.lblBack.TabIndex = 5;
            // 
            // SearchBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblBack);
            this.Name = "SearchBox";
            this.Size = new System.Drawing.Size(184, 24);
            this.Load += new System.EventHandler(this.SearchBox_Load);
            this.FontChanged += new System.EventHandler(this.SearchBox_FontChanged);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox btnSearch;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblBack;
    }
}
