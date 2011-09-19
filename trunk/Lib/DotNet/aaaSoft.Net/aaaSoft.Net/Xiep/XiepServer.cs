using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Base;
using System.Net.Sockets;
using aaaSoft.Net.Xiep.Handlers;
using aaaSoft.Net.Xiep.EventArgs;
using aaaSoft.Net.Xiep.Packages;
using aaaSoft.Net.Xiep.Helpers;
using aaaSoft.Net.Base.EventArgs;
using System.Threading;
using System.Net;
using System.IO;

namespace aaaSoft.Net.Xiep
{
    /**
     *  XIEP(Xml-based Information Exchange Protocol)服务端
     *  @author aaaSoft
     *  @since 2011年3月3日 11:26:28
     *  说明：此协议被设计为应用在典型的C/S(客户端/服务端)模型的应用程序中。
     *  此协议基于TCP协议，通信的数据包为XML数据。
     */
    public class XiepServer
    {
        //==========================
        //      属性部分开始
        //==========================

        private int _tcpListenPort;
        private TcpIpServer _tcpIpServer;
        private int _heartBeatTimeout = 30;
        private List<Socket> _connectedClientList;
        private Dictionary<String, AbstractRequestHandler> _requestHandlerMap;

        
        /// <summary>
        /// 获取或设置监听的TCP端口号
        /// </summary>
        /// <returns></returns>
        public int getTcpListenPort
        {
            get { return _tcpListenPort; }
            set { _tcpListenPort = value; }
        }

        /// <summary>
        /// 获取基于TCP/IP协议网络平台类
        /// </summary>
        public TcpIpServer TcpIpServer
        {
            get { return _tcpIpServer; }
        }

        /// <summary>
        /// 获取或设置心跳超时时间（单位：秒）
        /// </summary>
        /// <returns></returns>
        public int HeartBeatTimeout
        {
            get { return _heartBeatTimeout; }
            set { _heartBeatTimeout = value; }
        }
        
        //获取已连接的客户端列表
        public List<Socket> ConnectedClientList
        {
            get { return _connectedClientList; }
        }

        /**
         * 添加请求处理器
         * @param requestName 请求名称
         * @param handler 处理器
         */
        public void AddRequestHandler(String requestName, AbstractRequestHandler handler)
        {
            this._requestHandlerMap.Add(requestName, handler);
        }

        /**
         * 设置请求名称-处理器映射图
         * @param value 请求名称-处理器映射图
         */
        public void SetRequestHandlerMap(Dictionary<String, AbstractRequestHandler> value)
        {
            this._requestHandlerMap = value;

            if (this._requestHandlerMap.ContainsKey("XiepPing"))
            {
                this._requestHandlerMap.Remove("XiepPing");
            }
            this.AddRequestHandler("XiepPing", new XiepPingRequestHandler());
        }
        //==========================
        //      事件部分开始
        //==========================

        //新调试信息事件
        public event EventHandler<DebugInfoArgs> NewDebugInfo;
        //接收请求包事件
        public event EventHandler<ReceiveRequestArgs> ReceiveRequest;
        //发送响应包后事件
        public event EventHandler<AfterSendResponseArgs> AfterSendResponse;
        //客户端连接时事件
        public event EventHandler<ClientConnectedArgs> ClientConnected;
        //客户端断开事件监听器
        public event EventHandler<ClientConnectionInfoArgs> ClientDisconnected;

        //==========================
        //      函数部分开始
        //==========================
        //构造函数
        public XiepServer(int tcpListenPort)
        {
            init(tcpListenPort);
        }

        //初始化
        private void init(int tcpListenPort)
        {
            _connectedClientList = new List<Socket>();
            this.SetRequestHandlerMap(new Dictionary<String, AbstractRequestHandler>());
            this._tcpListenPort = tcpListenPort;
            _tcpIpServer = new TcpIpServer();
            _tcpIpServer.TcpListenPort = tcpListenPort;
            _tcpIpServer.ReadTimeOut = _heartBeatTimeout * 1000;
            _tcpIpServer.WriteTimeOut = _heartBeatTimeout * 1000;

            //添加事件绑定
            _tcpIpServer.NewTcpConnected += new EventHandler<NewTcpConnectedArgs>(tcpIpServer_NewTcpConnected);
        }

        // 启动服务端
        public void Start()
        {
            _tcpIpServer.Start();
        }

        // 停止服务端
        public void Stop()
        {
            _tcpIpServer.Stop();
            lock (_connectedClientList)
            {
                foreach (Socket socket in _connectedClientList)
                {
                    try { socket.Close(); }
                    catch { }
                }
            }
        }

        // 发送事件包
        public Boolean SendEvent(NetworkStream os, EventPackage eventPackage)
        {
            return XiepIoHelper.SendPackage(os, eventPackage);
        }

        // 输出调试信息
        private void pushLog(String logText)
        {
            DebugInfoArgs dia = new DebugInfoArgs();
            dia.setDebugText(logText);
            if (NewDebugInfo != null)
                NewDebugInfo.Invoke(this, dia);
        }

        private void tcpIpServer_NewTcpConnected(Object sender, NewTcpConnectedArgs e)
        {
            lock (_connectedClientList)
            {
                _connectedClientList.Add(e.getSocket());
            }

            Thread trdNewTcp = new Thread(receiveRequestThreadFunction);
            trdNewTcp.Start(e.getSocket());
        }

        private void receiveRequestThreadFunction(Object obj)
        {
            Socket socket = (Socket)obj;

            IPEndPoint ipep = (IPEndPoint)socket.RemoteEndPoint;
            //连接的IP地址
            IPAddress remoteInetAddress = ipep.Address;
            int remotePort = ipep.Port;

            //客户端连接信息参数对象
            ClientConnectionInfoArgs clientConnectionInfoArgs = new ClientConnectionInfoArgs(socket, remoteInetAddress, remotePort);

            //是否在发送完响应后断开连接
            Boolean isDisconnectWhenSendResponseFinish = false;

            pushLog(String.Format("{0}:{1} 连接到服务器。", remoteInetAddress, remotePort));
            try
            {
                //设置接受超时时间
                socket.ReceiveTimeout = _heartBeatTimeout * 1000;
                NetworkStream networkStream = new NetworkStream(socket);

                ClientConnectedArgs clientConnectedArgs = new ClientConnectedArgs(clientConnectionInfoArgs);
                //触发客户端连接事件
                if (this.ClientConnected != null)
                    ClientConnected.Invoke(this, clientConnectedArgs);

                if (!clientConnectedArgs.getIsAccept())
                {
                    throw new Exception("此连接不被接受，已断开。");
                }
                if (clientConnectedArgs.getEventPackage() != null)
                {
                    XiepIoHelper.SendPackage(networkStream, clientConnectedArgs.getEventPackage());
                }
                while (true)
                {
                    //接收请求包的XML字符串
                    String xml = XiepIoHelper.ReceiveXml(networkStream);
                    if (xml == null)
                    {
                        throw new IOException("获取数据包字符串失败。");
                    }
                    //请求包
                    RequestPackage requestPackage = RequestPackage.fromXml(xml);
                    if (requestPackage == null)
                    {
                        throw new IOException("字符串转换为请求数据包时失败。");
                    }
                    //响应包
                    ResponsePackage responsePackage = null;

                    String requestName = requestPackage.Request;
                    //如果找到了对应的处理器
                    if (this._requestHandlerMap.ContainsKey(requestName))
                    {
                        responsePackage = this._requestHandlerMap[requestName].execute(clientConnectionInfoArgs, requestPackage);
                    }
                    //如果是客户端发来的心跳消息包
                    if (requestPackage.Request.Equals("XiepPing"))
                    {
                    }
                    else
                    {
                        ReceiveRequestArgs receiveRequestArgs = new ReceiveRequestArgs(clientConnectionInfoArgs, requestPackage);
                        receiveRequestArgs.setResponsePackage(responsePackage);
                        //触发接收到客户端请求事件
                        if (ReceiveRequest != null)
                            ReceiveRequest.Invoke(this, receiveRequestArgs);

                        responsePackage = receiveRequestArgs.getResponsePackage();
                        isDisconnectWhenSendResponseFinish = receiveRequestArgs.getIsDisconnectWhenSendResponseFinish();

                        //发送响应包
                        if (responsePackage != null)
                        {
                            //为响应数据包添加RequestId
                            responsePackage.RequestId = requestPackage.RequestId;

                            //发送响应包
                            if (!XiepIoHelper.SendPackage(networkStream, responsePackage))
                            {
                                pushLog("错误：发送响应包失败：" + remoteInetAddress + "   |   " + responsePackage);
                            }
                            //触发发送响应后事件
                            AfterSendResponseArgs afterSendResponseArgs = new AfterSendResponseArgs(clientConnectionInfoArgs, requestPackage, responsePackage);
                            if (AfterSendResponse != null)
                                AfterSendResponse.Invoke(this, afterSendResponseArgs);

                            if (isDisconnectWhenSendResponseFinish)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch { }
            finally
            {
                //触发与客户端连接断开事件
                if (ClientDisconnected != null)
                    ClientDisconnected.Invoke(this, clientConnectionInfoArgs);

                lock (_connectedClientList)
                {
                    _connectedClientList.Remove(socket);
                }
                try
                {
                    socket.Close();
                }
                catch (Exception ex)
                {
                    pushLog("关闭Socket时异常：" + ex.ToString());
                }
            }
        }
    }
}
