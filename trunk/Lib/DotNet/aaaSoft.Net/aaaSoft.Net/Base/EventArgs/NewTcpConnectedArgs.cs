using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace aaaSoft.Net.Base.EventArgs
{
    /**
     *新TCP连接事件参数
     * @author aaa
     */
    public class NewTcpConnectedArgs : System.EventArgs
    {

        private IPAddress remoteIP;
        private int remotePort;
        private Socket socket;

        // 对方IP地址
        public IPAddress getRemoteIP()
        {
            return remoteIP;
        }

        // 对方端口
        public int getRemotePort()
        {
            return remotePort;
        }

        // Socket对象
        public Socket getSocket()
        {
            return socket;
        }

        // 构造函数
        public NewTcpConnectedArgs(IPAddress remoteIP, int remotePort, Socket socket)
        {
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;
            this.socket = socket;
        }
    }
}
