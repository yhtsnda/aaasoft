using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using aaaSoft.Net.Xiep;
using aaaSoft.Net.Xiep.EventArgs;
using aaaSoft.Net.Xiep.Packages;

namespace XiepClientTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        XiepClient xcClient;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            xcClient = new XiepClient(txtHostName.Text.Trim(), Int32.Parse(txtPort.Text.Trim()));
            xcClient.ServerEventCame += new EventHandler<XiepClientEventArgs>(xcClient_ServerEventCame);
            xcClient.ServerDisconnected += new EventHandler<EventArgs>(xcClient_ServerDisconnected);
            try
            {
                xcClient.start();
            }
            catch (Exception ex)
            {
                PushLog("启动连接时失败，" + ex);
            }
        }

        void xcClient_ServerDisconnected(object sender, EventArgs e)
        {
            PushLog("已断开与服务器的连接！");
        }

        void xcClient_ServerEventCame(object sender, XiepClientEventArgs e)
        {
            var eventPackage = e.getEventPackage();
            PushLog("收到服务器事件：" + eventPackage.getEvent());
        }

        private void PushLog(String logText)
        {
            try
            {
                this.Invoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                    {
                        txtLog.AppendText(logText + Environment.NewLine);
                    }));
            }
            catch { }
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            ResponsePackage responsePackage = xcClient.SendRequest(new RequestPackage(txtRequest.Text));
            if (responsePackage != null)
            {
                PushLog("发送请求成功。");
                if (responsePackage == null)
                {
                    PushLog("接收响应超时！");
                }
                else
                {
                    PushLog("接收到响应:" + responsePackage.getResponse());
                    txtRequest.Clear();
                }
            }
            else
            {
                PushLog("发送请求失败。");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (xcClient != null)
                xcClient.stop();
        }
    }
}
