namespace aaaSoft.SkinEngine.SkinControls
{
    partial class ThemeForm
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
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpTheme = new System.Windows.Forms.TabPage();
            this.lbSkin = new System.Windows.Forms.ListBox();
            this.tpColor = new System.Windows.Forms.TabPage();
            this.tcMain.SuspendLayout();
            this.tpTheme.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpTheme);
            this.tcMain.Controls.Add(this.tpColor);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(168, 216);
            this.tcMain.TabIndex = 0;
            // 
            // tpTheme
            // 
            this.tpTheme.Controls.Add(this.lbSkin);
            this.tpTheme.Location = new System.Drawing.Point(4, 22);
            this.tpTheme.Name = "tpTheme";
            this.tpTheme.Padding = new System.Windows.Forms.Padding(3);
            this.tpTheme.Size = new System.Drawing.Size(160, 190);
            this.tpTheme.TabIndex = 0;
            this.tpTheme.Text = "主题";
            this.tpTheme.UseVisualStyleBackColor = true;
            // 
            // lbSkin
            // 
            this.lbSkin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSkin.FormattingEnabled = true;
            this.lbSkin.ItemHeight = 12;
            this.lbSkin.Location = new System.Drawing.Point(3, 3);
            this.lbSkin.Name = "lbSkin";
            this.lbSkin.Size = new System.Drawing.Size(154, 184);
            this.lbSkin.TabIndex = 0;
            this.lbSkin.SelectedIndexChanged += new System.EventHandler(this.lbSkin_SelectedIndexChanged);
            // 
            // tpColor
            // 
            this.tpColor.Location = new System.Drawing.Point(4, 22);
            this.tpColor.Name = "tpColor";
            this.tpColor.Padding = new System.Windows.Forms.Padding(3);
            this.tpColor.Size = new System.Drawing.Size(160, 190);
            this.tpColor.TabIndex = 1;
            this.tpColor.Text = "颜色";
            this.tpColor.UseVisualStyleBackColor = true;
            // 
            // ThemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(168, 216);
            this.Controls.Add(this.tcMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ThemeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ThemeForm";
            this.Deactivate += new System.EventHandler(this.ThemeForm_Deactivate);
            this.Load += new System.EventHandler(this.ThemeForm_Load);
            this.tcMain.ResumeLayout(false);
            this.tpTheme.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpTheme;
        private System.Windows.Forms.TabPage tpColor;
        private System.Windows.Forms.ListBox lbSkin;
    }
}