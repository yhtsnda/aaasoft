namespace SnmpTest
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCommunity = new System.Windows.Forms.TextBox();
            this.txtOid = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnWalk = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpGetResult = new System.Windows.Forms.TabPage();
            this.tpWalkResult = new System.Windows.Forms.TabPage();
            this.btnWalkStoargeInfo = new System.Windows.Forms.Button();
            this.btnGetInterfaceInfo = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.tcMain.SuspendLayout();
            this.tpGetResult.SuspendLayout();
            this.tpWalkResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "主机:";
            // 
            // txtHostName
            // 
            this.txtHostName.Location = new System.Drawing.Point(62, 12);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(100, 21);
            this.txtHostName.TabIndex = 1;
            this.txtHostName.Text = "localhost";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "端口:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(209, 12);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(37, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "161";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "社区名:";
            // 
            // txtCommunity
            // 
            this.txtCommunity.Location = new System.Drawing.Point(308, 12);
            this.txtCommunity.Name = "txtCommunity";
            this.txtCommunity.Size = new System.Drawing.Size(100, 21);
            this.txtCommunity.TabIndex = 1;
            this.txtCommunity.Text = "public";
            // 
            // txtOid
            // 
            this.txtOid.Location = new System.Drawing.Point(23, 39);
            this.txtOid.Name = "txtOid";
            this.txtOid.Size = new System.Drawing.Size(370, 21);
            this.txtOid.TabIndex = 3;
            // 
            // btnGet
            // 
            this.btnGet.Enabled = false;
            this.btnGet.Location = new System.Drawing.Point(399, 37);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(75, 23);
            this.btnGet.TabIndex = 4;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnWalk
            // 
            this.btnWalk.Enabled = false;
            this.btnWalk.Location = new System.Drawing.Point(479, 37);
            this.btnWalk.Name = "btnWalk";
            this.btnWalk.Size = new System.Drawing.Size(75, 23);
            this.btnWalk.TabIndex = 4;
            this.btnWalk.Text = "Walk";
            this.btnWalk.UseVisualStyleBackColor = true;
            this.btnWalk.Click += new System.EventHandler(this.btnWalk_Click);
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(3, 3);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(536, 273);
            this.txtResult.TabIndex = 3;
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(3, 3);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowTemplate.Height = 23;
            this.dgvResult.Size = new System.Drawing.Size(536, 273);
            this.dgvResult.TabIndex = 5;
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpGetResult);
            this.tcMain.Controls.Add(this.tpWalkResult);
            this.tcMain.Location = new System.Drawing.Point(12, 95);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(550, 305);
            this.tcMain.TabIndex = 6;
            // 
            // tpGetResult
            // 
            this.tpGetResult.Controls.Add(this.txtResult);
            this.tpGetResult.Location = new System.Drawing.Point(4, 22);
            this.tpGetResult.Name = "tpGetResult";
            this.tpGetResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpGetResult.Size = new System.Drawing.Size(542, 279);
            this.tpGetResult.TabIndex = 0;
            this.tpGetResult.Text = "Get结果";
            this.tpGetResult.UseVisualStyleBackColor = true;
            // 
            // tpWalkResult
            // 
            this.tpWalkResult.Controls.Add(this.dgvResult);
            this.tpWalkResult.Location = new System.Drawing.Point(4, 22);
            this.tpWalkResult.Name = "tpWalkResult";
            this.tpWalkResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpWalkResult.Size = new System.Drawing.Size(542, 279);
            this.tpWalkResult.TabIndex = 1;
            this.tpWalkResult.Text = "Walk结果";
            this.tpWalkResult.UseVisualStyleBackColor = true;
            // 
            // btnWalkStoargeInfo
            // 
            this.btnWalkStoargeInfo.Enabled = false;
            this.btnWalkStoargeInfo.Location = new System.Drawing.Point(12, 66);
            this.btnWalkStoargeInfo.Name = "btnWalkStoargeInfo";
            this.btnWalkStoargeInfo.Size = new System.Drawing.Size(75, 23);
            this.btnWalkStoargeInfo.TabIndex = 7;
            this.btnWalkStoargeInfo.Text = "存储信息";
            this.btnWalkStoargeInfo.UseVisualStyleBackColor = true;
            this.btnWalkStoargeInfo.Click += new System.EventHandler(this.btnWalkStoargeInfo_Click);
            // 
            // btnGetInterfaceInfo
            // 
            this.btnGetInterfaceInfo.Enabled = false;
            this.btnGetInterfaceInfo.Location = new System.Drawing.Point(93, 66);
            this.btnGetInterfaceInfo.Name = "btnGetInterfaceInfo";
            this.btnGetInterfaceInfo.Size = new System.Drawing.Size(75, 23);
            this.btnGetInterfaceInfo.TabIndex = 8;
            this.btnGetInterfaceInfo.Text = "网卡信息";
            this.btnGetInterfaceInfo.UseVisualStyleBackColor = true;
            this.btnGetInterfaceInfo.Click += new System.EventHandler(this.btnGetInterfaceInfo_Click);
            // 
            // btnLock
            // 
            this.btnLock.Location = new System.Drawing.Point(414, 10);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(75, 23);
            this.btnLock.TabIndex = 9;
            this.btnLock.Text = "确定";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 412);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.btnGetInterfaceInfo);
            this.Controls.Add(this.btnWalkStoargeInfo);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.btnWalk);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.txtOid);
            this.Controls.Add(this.txtCommunity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHostName);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.tcMain.ResumeLayout(false);
            this.tpGetResult.ResumeLayout(false);
            this.tpGetResult.PerformLayout();
            this.tpWalkResult.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHostName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCommunity;
        private System.Windows.Forms.TextBox txtOid;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnWalk;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpGetResult;
        private System.Windows.Forms.TabPage tpWalkResult;
        private System.Windows.Forms.Button btnWalkStoargeInfo;
        private System.Windows.Forms.Button btnGetInterfaceInfo;
        private System.Windows.Forms.Button btnLock;
    }
}

