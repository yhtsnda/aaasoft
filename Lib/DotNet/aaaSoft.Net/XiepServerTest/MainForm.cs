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
using System.Net.Sockets;

namespace XiepServerTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        XiepServer xsServer;

        private void btnStartListen_Click(object sender, EventArgs e)
        {
            xsServer = new XiepServer(Int32.Parse(txtPort.Text.Trim()));
            xsServer.ClientConnected += new EventHandler<ClientConnectedArgs>(xsServer_ClientConnected);
            xsServer.ClientDisconnected += new EventHandler<ClientConnectionInfoArgs>(xsServer_ClientDisconnected);
            xsServer.ReceiveRequest += new EventHandler<ReceiveRequestArgs>(xsServer_ReceiveRequest);
            xsServer.Start();
            PushLog("已开启服务");
        }

        void xsServer_ReceiveRequest(object sender, ReceiveRequestArgs e)
        {
            var requestPackage = e.getRequestPackage();
            PushLog("收到请求:" + requestPackage.Request);
            e.setResponsePackage(new ResponsePackage(e.getRequestPackage(), "THISISRESPONSE"));
        }

        void xsServer_ClientDisconnected(object sender, ClientConnectionInfoArgs e)
        {
            PushLog(e.getInetAddress().ToString() + "已断开与服务器的连接");
        }

        void xsServer_ClientConnected(object sender, ClientConnectedArgs e)
        {
            var dict = new Dictionary<string, string>();
            EventPackage eventPackage = new EventPackage("Welcome");
            eventPackage.AddArgument("NowTime", DateTime.Now.ToString());
            e.setEventPackage(eventPackage);
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            xsServer.Stop();
        }

        private void btnBroadcastNotice_Click(object sender, EventArgs e)
        {
            var eventPackage = new EventPackage(txtNotice.Text.Trim());
            foreach (var socket in xsServer.ConnectedClientList)
            {
                xsServer.SendEvent(new NetworkStream(socket), eventPackage);
            }
            txtNotice.Clear();
        }
    }
}
