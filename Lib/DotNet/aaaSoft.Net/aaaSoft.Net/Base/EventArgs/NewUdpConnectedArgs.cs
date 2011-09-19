using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace aaaSoft.Net.Base.EventArgs
{
    /**
     *新UDP连接事件参数
     * @author aaa
     */
    public class NewUdpConnectedArgs : System.EventArgs
    {

        private IPAddress remoteIP;
        private int remotePort;
        private byte[] data;

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

        // 数据
        public byte[] getData()
        {
            return data;
        }

        public NewUdpConnectedArgs(IPAddress remoteIP, int remotePort, byte[] data)
        {
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;
            this.data = data;
        }
    }
}
