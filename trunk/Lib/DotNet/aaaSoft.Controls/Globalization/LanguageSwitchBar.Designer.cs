namespace aaaSoft.Controls.Globalization
{
    partial class LanguageSwitchBar
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
            this.cbLanguageSwitch = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbLanguageSwitch
            // 
            this.cbLanguageSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLanguageSwitch.DisplayMember = "NativeName";
            this.cbLanguageSwitch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguageSwitch.FormattingEnabled = true;
            this.cbLanguageSwitch.Location = new System.Drawing.Point(68, 3);
            this.cbLanguageSwitch.Name = "cbLanguageSwitch";
            this.cbLanguageSwitch.Size = new System.Drawing.Size(199, 20);
            this.cbLanguageSwitch.TabIndex = 3;
            this.cbLanguageSwitch.SelectedIndexChanged += new System.EventHandler(this.cbLanguageSwitch_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Tag = "";
            this.label1.Text = "Language:";
            // 
            // LanguageSwitchBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbLanguageSwitch);
            this.Controls.Add(this.label1);
            this.Name = "LanguageSwitchBar";
            this.Size = new System.Drawing.Size(270, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbLanguageSwitch;
        private System.Windows.Forms.Label label1;
    }
}
