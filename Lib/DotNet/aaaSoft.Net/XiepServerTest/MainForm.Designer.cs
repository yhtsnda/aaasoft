namespace XiepServerTest
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
            this.btnStartListen = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtNotice = new System.Windows.Forms.TextBox();
            this.btnBroadcastNotice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartListen
            // 
            this.btnStartListen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartListen.Location = new System.Drawing.Point(196, 12);
            this.btnStartListen.Name = "btnStartListen";
            this.btnStartListen.Size = new System.Drawing.Size(84, 23);
            this.btnStartListen.TabIndex = 0;
            this.btnStartListen.Text = "StartListen";
            this.btnStartListen.UseVisualStyleBackColor = true;
            this.btnStartListen.Click += new System.EventHandler(this.btnStartListen_Click);
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(12, 14);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(178, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "21345";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 39);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(268, 193);
            this.txtLog.TabIndex = 1;
            // 
            // txtNotice
            // 
            this.txtNotice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotice.Location = new System.Drawing.Point(12, 240);
            this.txtNotice.Name = "txtNotice";
            this.txtNotice.Size = new System.Drawing.Size(178, 21);
            this.txtNotice.TabIndex = 1;
            // 
            // btnBroadcastNotice
            // 
            this.btnBroadcastNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBroadcastNotice.Location = new System.Drawing.Point(196, 238);
            this.btnBroadcastNotice.Name = "btnBroadcastNotice";
            this.btnBroadcastNotice.Size = new System.Drawing.Size(84, 23);
            this.btnBroadcastNotice.TabIndex = 0;
            this.btnBroadcastNotice.Text = "Broadcast";
            this.btnBroadcastNotice.UseVisualStyleBackColor = true;
            this.btnBroadcastNotice.Click += new System.EventHandler(this.btnBroadcastNotice_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtNotice);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnBroadcastNotice);
            this.Controls.Add(this.btnStartListen);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XiepServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartListen;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtNotice;
        private System.Windows.Forms.Button btnBroadcastNotice;
    }
}

