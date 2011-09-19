using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace aaaSoft.Net.Xiep.EventArgs
{
    public class ClientConnectionInfoArgs : System.EventArgs
    {

        private Socket socket;
        private IPAddress inetAddress;
        private int port;

        //获取Socket对象
        public Socket getSocket()
        {
            return socket;
        }

        //获取IP地址
        public IPAddress getInetAddress()
        {
            return inetAddress;
        }

        //获取端口
        public int getPort()
        {
            return port;
        }


        public override String ToString()
        {
            return String.Format("{0}:{1}", inetAddress.ToString(), port);
        }

        //构造函数
        public ClientConnectionInfoArgs(Socket socket, IPAddress inetAddress, int port)
        {
            this.socket = socket;
            this.inetAddress = inetAddress;
            this.port = port;
        }
    }
}
