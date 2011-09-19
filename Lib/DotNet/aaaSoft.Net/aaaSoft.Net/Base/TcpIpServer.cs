using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using aaaSoft.Helpers;
using aaaSoft.Net.Base.EventArgs;

namespace aaaSoft.Net.Base
{
    /// <summary>
    /// 基于TCP/IP协议网络平台类
    /// </summary>
    public partial class TcpIpServer
    {
        private TcpListener _tcpListener;
        private UdpClient _udpListener;
        private int _writeTimeOut = 0;
        private int _readTimeOut = 0;
        private int _tcpListenPort = -1;
        private int _udpListenPort = -1;
        private IPAddress ipAddress = IPAddress.Any;

        private const int bufferSize = 1024;

        /// <summary>
        /// IP地址
        /// </summary>
        public IPAddress IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        /// <summary>
        /// 获取或设置发送超时设置
        /// </summary>
        public int WriteTimeOut
        {
            get { return _writeTimeOut; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("只能将超时设置为“System.Threading.Timeout.Infinite”或大于 0 的值。");
                }
                _writeTimeOut = value;
            }
        }

        /// <summary>
        /// 获取或设置接收超时设置
        /// </summary>
        public int ReadTimeOut
        {
            get { return _readTimeOut; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("只能将超时设置为“System.Threading.Timeout.Infinite”或大于 0 的值。");
                }
                _readTimeOut = value;
            }
        }

        /// <summary>
        /// 获取或设置TCP侦听端口(-1为不侦听，0为随机端口)
        /// </summary>
        public int TcpListenPort
        {
            get { return _tcpListenPort; }
            set
            {
                if (value > 65535 || value < -1)
                {
                    throw new Exception("端口号范围为0至65535");
                }
                _tcpListenPort = value;
            }
        }

        /// <summary>
        /// 获取或设置UDP侦听端口(-1为不侦听，0为随机端口)
        /// </summary>
        public int UdpListenPort
        {
            get { return _udpListenPort; }
            set
            {
                if (value > 65535 || value < -1)
                {
                    throw new Exception("端口号范围为0至65535");
                }
                _udpListenPort = value;
            }
        }
                
        /// <summary>
        /// 有新的Tcp连接时
        /// </summary>
        public event EventHandler<NewTcpConnectedArgs> NewTcpConnected;
        /// <summary>
        /// 有新的Udp连接时
        /// </summary>
        public event EventHandler<NewUdpConnectedArgs> NewUdpConnected;

        
        #region 获取当前TCP侦听端口
        /// <summary>
        /// 获取当前TCP侦听端口
        /// </summary>
        /// <returns></returns>
        public int GetTcpListenPort()
        {
            if (_tcpListener == null) return -1;
            if (_tcpListenPort != 0) return _tcpListenPort;
            try
            {
                IPEndPoint ipep = _tcpListener.LocalEndpoint as IPEndPoint;
                return ipep.Port;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 获取当前UDP侦听端口
        /// <summary>
        /// 获取当前UDP侦听端口
        /// </summary>
        /// <returns></returns>
        public int GetUdpListenPort()
        {            
            if (_udpListener == null) return -1;
            if (_udpListenPort != 0) return _udpListenPort;
            try
            {
                IPEndPoint ipep = _udpListener.Client.LocalEndPoint as IPEndPoint;
                return ipep.Port;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 停止运行平台
        /// <summary>
        /// 停止运行平台
        /// </summary>
        public void Stop()
        {
            if (_udpListener != null && _udpListenPort != -1)
            {
                _udpListener.Close();
                _udpListener = null;
            }

            if (_tcpListener != null && _tcpListenPort != -1)
            {
                _tcpListener.Stop();
                _tcpListener = null;
            }
        }
        #endregion

        #region 开始运行平台
        /// <summary>
        /// 开始运行平台
        /// </summary>
        public void Start()
        {
            if (_udpListenPort != -1)
            {
                if (_udpListenPort == 0)
                {
                    while (true)
                    {
                        int randomPort = NetHelper.GetRandomPort();
                        try
                        {                            
                            _udpListener = new UdpClient(randomPort);
                            break;
                        }
                        catch
                        {
                            Thread.Sleep(10);
                        }
                    }
                }
                else
                {
                    _udpListener = new UdpClient(_udpListenPort);                    
                }
                _udpListener.EnableBroadcast = true;
                Thread trdListenUdp = new Thread(ListenUdpThread);
                trdListenUdp.Start();
            }

            if (_tcpListenPort != -1)
            {
                if (_tcpListenPort == 0)
                {
                    //如果是随机端口
                    while (true)
                    {
                        int randomPort = NetHelper.GetRandomPort();
                        try
                        {
                            _tcpListener = new TcpListener(ipAddress, randomPort);
                            break;
                        }
                        catch
                        {
                            Thread.Sleep(10);
                        }
                    }
                }
                else
                {
                    _tcpListener = new TcpListener(ipAddress, _tcpListenPort);                    
                }
                _tcpListener.Start();
                Thread trdListenTcp = new Thread(ListenTcpThread);
                trdListenTcp.Start();
            }
        }
        #endregion

        //==================
        //侦听部分
        //==================
        #region 侦听线程部分
        //侦听UDP线程
        private void ListenUdpThread()
        {
            IPEndPoint remoteIPEP = new IPEndPoint(ipAddress, 0);
            while (GetUdpListenPort() > 0)
            {
                NewUdpConnectedArgs e = null;
                try
                {
                    byte[] buffer = _udpListener.Receive(ref remoteIPEP);
                    e = new NewUdpConnectedArgs(remoteIPEP.Address, remoteIPEP.Port, buffer);
                }
                catch 
                {
                    continue;
                }
                if (NewUdpConnected != null)
                {
                    NewUdpConnected(this, e);
                }
            }
        }

        //侦听TCP线程
        private void ListenTcpThread()
        {
            
            while (GetTcpListenPort() > 0)
            {
                NewTcpConnectedArgs e = null;
                try
                {
                    var socket = _tcpListener.AcceptSocket();
                    socket.ReceiveTimeout = _readTimeOut;
                    socket.SendTimeout = _writeTimeOut;

                    IPEndPoint ipep = socket.RemoteEndPoint as IPEndPoint;
                    e = new NewTcpConnectedArgs(ipep.Address,ipep.Port,socket);
                }
                catch
                {
                    continue;
                }
                if (NewTcpConnected != null)
                {
                    NewTcpConnected(this, e);
                }
            }
        }
        #endregion
    }
}
