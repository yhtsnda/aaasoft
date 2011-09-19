using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using aaaSoft.Helpers;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using aaaSoft.Net.Xiep.Packages;
using System.Net;
using aaaSoft.Net.Xiep.EventArgs;
using aaaSoft.Net.Xiep.Helpers;

namespace aaaSoft.Net.Xiep
{
    /// <summary>
    /// XIEP(Xml-based Information Exchange Protocol)客户端
    /// </summary>
    public class XiepClient
    {
        //==========================
        //      属性部分开始
        //==========================

        private Socket _socket;
        private NetworkStream _networkStream;
        private String _serverHostName;
        private int _serverPort;
        private int _heartBeatInterval = 10;
        private int _heartBeatTimeout = 30;
        private Dictionary<String, ResponsePackage> _mapRequestResponse;

        //得到本地绑定的端口
        public int LocalPort
        {
            get
            {
                if (_socket == null)
                    return -1;
                else
                    return ((IPEndPoint)_socket.LocalEndPoint).Port;
            }
        }

        /// <summary>
        /// 获取或设置服务器主机名称
        /// </summary>
        public String ServerHostName
        {
            get { return _serverHostName; }
            set { _serverHostName = value; }
        }

        /// <summary>
        /// 获取或设置服务端端口
        /// </summary>
        public int ServerPort
        {
            get { return _serverPort; }
            set { _serverPort = value; }
        }

        /// <summary>
        /// 获取或设置心跳消息发送时间间隔(单位：秒)
        /// </summary>
        public int HeartBeatInterval
        {
            get { return _heartBeatInterval; }
            set { _heartBeatInterval = value; }
        }

        /// <summary>
        /// 获取或设置心跳消息超时时间（单位：秒）
        /// </summary>
        public int HeartBeatTimeout
        {
            get { return _heartBeatTimeout; }
            set { _heartBeatTimeout = value; }
        }

        //获取是否已连接到服务端
        public Boolean IsConnected
        {
            get { return _socket != null; }
        }
        //==========================
        //      事件部分开始
        //==========================
        //收到服务器事件的事件监听器
        public event EventHandler<XiepClientEventArgs> ServerEventCame;
        public event EventHandler<System.EventArgs> ServerDisconnected;

        //==========================
        //      函数部分开始
        //==========================
        //构造函数
        public XiepClient(Socket socket)
        {
            this._socket = socket;
            init();
        }

        //构造函数
        public XiepClient(String serverHostName, int serverPort)
        {
            this._serverHostName = serverHostName;
            this._serverPort = serverPort;

            init();
        }

        private void init()
        {
            _mapRequestResponse = new Dictionary<string, ResponsePackage>();
        }

        //开始
        public void Start()
        {
            if (!this.IsConnected)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.ReceiveTimeout = 0;
                _socket.Connect(_serverHostName, _serverPort);
            }

            _networkStream = new NetworkStream(_socket);
            //开启接收响应线程
            Thread trdRecv = new Thread(receiveResponseThreadFunction);
            trdRecv.Start();

            //开启心跳线程
            Thread trdHeartBeat = new Thread(heartBeatThreadFunction);
            trdHeartBeat.Start();
        }

        //停止
        public void Stop()
        {
            try
            {
                _socket.Close();
            }
            catch
            {
            }
            _networkStream = null;
            _socket = null;
        }

        /// <summary>
        /// 获取响应是否成功
        /// </summary>
        /// <param name="responsePackage">响应包</param>
        /// <returns></returns>
        public static Boolean IsResponseSuccess(ResponsePackage responsePackage)
        {
            if (responsePackage == null)
                return false;
            return responsePackage.Response == "Success";
        }

        // 发送请求数据包并得到响应包(默认超时时间为10秒)
        public ResponsePackage SendRequest(RequestPackage requestPackage)
        {
            return SendRequest(requestPackage, 10);
        }

        // 发送请求数据包并得到响应包
        //requestPackage:请求数据包
        //timeoutSeconds:超时时间(单位：秒)，如果超时时间小于等于0，则超时时间为无限大
        public ResponsePackage SendRequest(RequestPackage requestPackage, int timeoutSeconds)
        {
            String requestId = requestPackage.RequestId;
            //发送Request包
            if (!XiepIoHelper.SendPackage(_networkStream, requestPackage))
            {
                //发送失败
                return null;
            }

            lock (_mapRequestResponse)
            {
                _mapRequestResponse.Add(requestId, null);
            }

            DateTime startWaitResponseTime = DateTime.Now;
            ResponsePackage responsePackage = null;
            while (true)
            {
                lock (_mapRequestResponse)
                {
                    responsePackage = _mapRequestResponse[requestId];
                }
                long usedSeconds = Convert.ToInt64((DateTime.Now - startWaitResponseTime).TotalSeconds);
                if ( //如果已经得到响应包
                        responsePackage != null
                    //或者等待已经超时
                        || usedSeconds > timeoutSeconds)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            //从等待字典中移除
            lock (_mapRequestResponse)
            {
                if (_mapRequestResponse.ContainsKey(requestId))
                {
                    _mapRequestResponse.Remove(requestId);
                }
            }
            return responsePackage;
        }

        //定时发送心跳消息线程函数
        private void heartBeatThreadFunction()
        {
            Socket currentSocket = _socket;
            while (_socket != null && currentSocket == _socket)
            {
                //发送心跳线程
                SendRequest(new RequestPackage("XiepPing", null));
                //N秒发送一次心跳消息
                Thread.Sleep(_heartBeatInterval * 1000);
            }
        }

        //接收服务器响应线程函数
        private void receiveResponseThreadFunction()
        {
            try
            {
                while (true)
                {
                    //接收请求包
                    AbstractXiepPackage recvPackage = XiepIoHelper.ReceivePackage(_networkStream);
                    if (recvPackage == null)
                    {
                        throw new IOException("获取数据包失败。");
                    }

                    //如果是响应数据包
                    if (recvPackage is ResponsePackage)
                    {
                        ResponsePackage responsePackage = (ResponsePackage)recvPackage;
                        String requestId = responsePackage.RequestId;
                        lock (_mapRequestResponse)
                        {
                            if (_mapRequestResponse.ContainsKey(requestId))
                            {
                                _mapRequestResponse.Remove(requestId);
                                _mapRequestResponse.Add(requestId, responsePackage);
                            }
                        }
                    } //如果是事件数据包
                    else if (recvPackage is EventPackage)
                    {
                        EventPackage eventPackage = (EventPackage)recvPackage;
                        //触发收到服务器事件的事件
                        if (ServerEventCame != null)
                        {
                            ServerEventCame.BeginInvoke(this, new XiepClientEventArgs(eventPackage), new AsyncCallback(delegate(IAsyncResult iar) { }), null);
                        }
                    }
                }
            }
            catch
            {
                _networkStream = null;
                _socket = null;

                //触发与服务器连接断开事件
                if (ServerDisconnected != null)
                {
                    //ServerDisconnected.BeginInvoke(this, new System.EventArgs(), new AsyncCallback(delegate(IAsyncResult iar) { }), this);
                    ServerDisconnected.Invoke(this, new System.EventArgs());
                }
            }
        }
    }
}
