/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep;

import com.scbeta.helpers.EventHelper;
import com.scbeta.helpers.SingleEventListener;
import com.scbeta.net.base.TcpIpServer;
import com.scbeta.net.base.TcpIpServer.NewTcpConnectedListener;
import com.scbeta.net.base.eventArgs.NewTcpConnectedArgs;
import com.scbeta.net.xiep.eventArgs.AfterSendResponseArgs;
import com.scbeta.net.xiep.eventArgs.ClientConnectedArgs;
import com.scbeta.net.xiep.eventArgs.ClientConnectionInfoArgs;
import com.scbeta.net.xiep.eventArgs.DebugInfoArgs;
import com.scbeta.net.xiep.eventArgs.ReceiveRequestArgs;
import com.scbeta.net.xiep.handlers.AbstractRequestHandler;
import com.scbeta.net.xiep.handlers.XiepPingRequestHandler;
import com.scbeta.net.xiep.helpers.XiepIoHelper;
import com.scbeta.net.xiep.packages.EventPackage;
import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.net.SocketException;
import java.util.LinkedHashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

/**
 *  XIEP(Xml-based Information Exchange Protocol)服务端
 *  @author aaaSoft
 *  @since 2011年3月3日 11:26:28
 *  说明：此协议被设计为应用在典型的C/S(客户端/服务端)模型的应用程序中。
 *  此协议基于TCP协议，通信的数据包为XML数据。
 */
public class XiepServer {
    //==========================
    //      属性部分开始
    //==========================

    private int tcpListenPort;
    private TcpIpServer tcpIpServer;
    private int heartBeatTimeout = 30;
    private List<Socket> connectedClientList;
    private Map<String, AbstractRequestHandler> requestHandlerMap;

    //获取监听的TCP端口号
    public int getTcpListenPort() {
        return tcpListenPort;
    }

    //设置监听的TCP端口号
    public void setTcpListenPort(int value) {
        tcpListenPort = value;
    }

    //获取基于TCP/IP协议网络平台类
    public TcpIpServer getTcpIpServer() {
        return tcpIpServer;
    }

    //获取心跳超时时间（单位：秒）
    public int getHeartBeatTimeout() {
        return heartBeatTimeout;
    }

    //设置心跳超时时间（单位：秒）
    public void setHeartBeatTimeout(int value) {
        heartBeatTimeout = value;
    }

    //获取已连接的客户端列表
    public List<Socket> getConnectedClientList() {
        return connectedClientList;
    }

    /**
     * 添加请求处理器
     * @param requestName 请求名称
     * @param handler 处理器
     */
    public void addRequestHandler(String requestName, AbstractRequestHandler handler) {
        this.requestHandlerMap.put(requestName, handler);
    }

    /**
     * 设置请求名称-处理器映射图
     * @param value 请求名称-处理器映射图
     */
    public void setRequestHandlerMap(Map<String, AbstractRequestHandler> value) {
        this.requestHandlerMap = value;

        if (this.requestHandlerMap.containsKey("XiepPing")) {
            this.requestHandlerMap.remove("XiepPing");
        }
        this.addRequestHandler("XiepPing", new XiepPingRequestHandler());
    }
    //==========================
    //      事件部分开始
    //==========================
    //用来存放已注册的监听对象的集合
    private EventHelper eventHelper = new EventHelper();

    //获取事件辅助对象
    public EventHelper getEventHelper() {
        return eventHelper;
    }

    //新调试信息事件监听器
    public interface NewDebugInfoListener extends SingleEventListener<DebugInfoArgs> {
    }

    //添加新调试信息事件监听器
    public void addNewDebugInfoListener(NewDebugInfoListener listener) {
        eventHelper.addListener(listener);
    }

    //接收请求包事件监听器
    public interface ReceiveRequestListener extends SingleEventListener<ReceiveRequestArgs> {
    }

    //添加接收请求包事件监听器
    public void addReceiveRequestListener(ReceiveRequestListener listener) {
        eventHelper.addListener(listener);
    }

    //发送响应包后事件监听器
    public interface AfterSendResponseListener extends SingleEventListener<AfterSendResponseArgs> {
    }

    //添加发送响应包后事件监听器
    public void addAfterSendResponseListener(AfterSendResponseListener listener) {
        eventHelper.addListener(listener);
    }

    //客户端连接时事件监听器
    public interface ClientConnectedListener extends SingleEventListener<ClientConnectedArgs> {
    }

    //添加客户端连接时事件监听器
    public void addClientConnectedListener(ClientConnectedListener listener) {
        eventHelper.addListener(listener);
    }

    //客户端断开事件监听器
    public interface ClientDisconnectedListener extends SingleEventListener<ClientConnectionInfoArgs> {
    }

    //添加客户端断开事件监听器
    public void addClientDisconnectedListener(ClientDisconnectedListener listener) {
        eventHelper.addListener(listener);
    }

    //移除一个事件监听器
    public synchronized void removeListener(SingleEventListener listener) {
        eventHelper.removeListener(listener);
    }

    //==========================
    //      函数部分开始
    //==========================
    //构造函数
    public XiepServer(int tcpListenPort) {
        init(tcpListenPort);
    }

    //初始化
    private void init(int tcpListenPort) {
        connectedClientList = new LinkedList<Socket>();
        this.setRequestHandlerMap(new LinkedHashMap<String, AbstractRequestHandler>());
        this.tcpListenPort = tcpListenPort;
        tcpIpServer = new TcpIpServer();
        tcpIpServer.setTcpListenPort(tcpListenPort);
        tcpIpServer.setReadTimeOut(heartBeatTimeout * 1000);
        tcpIpServer.setWriteTimeOut(heartBeatTimeout * 1000);

        //添加事件绑定
        tcpIpServer.addNewTcpConnectedListener(new NewTcpConnectedListener() {

            @Override
            public void Perform(NewTcpConnectedArgs e) {
                tcpIpServer_NewTcpConnected(e);
            }
        });
    }

    // 启动服务端
    public void start() throws SocketException, IOException {
        tcpIpServer.start();
    }

    // 停止服务端
    public void stop() {
        tcpIpServer.stop();
        synchronized (connectedClientList) {
            for (Socket socket : connectedClientList) {
                try {
                    socket.close();
                } catch (Exception ex) {
                }
            }
        }
    }

    // 发送事件包
    public Boolean SendEvent(Socket socket, EventPackage eventPackage) {
        return XiepIoHelper.SendPackage(socket, eventPackage);
    }

    // 输出调试信息
    private void pushLog(String logText) {
        DebugInfoArgs dia = new DebugInfoArgs(this);
        dia.setDebugText(logText);
        eventHelper.performEvent(NewDebugInfoListener.class, dia);
    }

    private void tcpIpServer_NewTcpConnected(final NewTcpConnectedArgs e) {
        synchronized (connectedClientList) {
            connectedClientList.add(e.getSocket());
        }

        Thread trdNewTcp = new Thread(new Runnable() {

            @Override
            public void run() {
                receiveRequestThreadFunction(e.getSocket());
            }
        });
        trdNewTcp.start();
    }

    private void receiveRequestThreadFunction(Socket socket) {
        //连接的IP地址
        InetAddress remoteInetAddress = socket.getInetAddress();
        int remotePort = socket.getPort();

        //客户端连接信息参数对象
        ClientConnectionInfoArgs clientConnectionInfoArgs = new ClientConnectionInfoArgs(this, socket, remoteInetAddress, remotePort);

        //是否在发送完响应后断开连接
        boolean isDisconnectWhenSendResponseFinish = false;

        pushLog(String.format("%s:%s 连接到服务器。", remoteInetAddress, remotePort));
        try {
            //设置接受超时时间
            socket.setSoTimeout(heartBeatTimeout * 1000);

            ClientConnectedArgs clientConnectedArgs = new ClientConnectedArgs(this, clientConnectionInfoArgs);
            //触发客户端连接事件
            eventHelper.performEvent(ClientConnectedListener.class, clientConnectedArgs);

            if (!clientConnectedArgs.getIsAccept()) {
                throw new Exception("此连接不被接受，已断开。");
            }
            if (clientConnectedArgs.getEventPackage() != null) {
                XiepIoHelper.SendPackage(socket, clientConnectedArgs.getEventPackage());
            }
            while (true) {
                //接收请求包的XML字符串
                String xml = XiepIoHelper.ReceiveXml(socket);
                if (xml == null) {
                    throw new IOException("获取数据包字符串失败。");
                }
                //请求包
                RequestPackage requestPackage = RequestPackage.fromXml(xml);
                if (requestPackage == null) {
                    throw new IOException("字符串转换为请求数据包时失败。");
                }
                //响应包
                ResponsePackage responsePackage = null;

                String requestName = requestPackage.getRequest();
                //如果找到了对应的处理器
                if (this.requestHandlerMap.containsKey(requestName)) {
                    responsePackage = this.requestHandlerMap.get(requestName).execute(clientConnectionInfoArgs, requestPackage);
                }
                //如果是客户端发来的心跳消息包
                if (requestPackage.getRequest().equals("XiepPing")) {
                } else {
                    ReceiveRequestArgs receiveRequestArgs = new ReceiveRequestArgs(this, clientConnectionInfoArgs, requestPackage);
                    receiveRequestArgs.setResponsePackage(responsePackage);
                    //触发接收到客户端请求事件
                    eventHelper.performEvent(ReceiveRequestListener.class, receiveRequestArgs);
                    responsePackage = receiveRequestArgs.getResponsePackage();
                    isDisconnectWhenSendResponseFinish = receiveRequestArgs.getIsDisconnectWhenSendResponseFinish();

                    //发送响应包
                    if (responsePackage != null) {
                        //为响应数据包添加RequestId
                        responsePackage.setRequestId(requestPackage.getRequestId());

                        //发送响应包
                        if (!XiepIoHelper.SendPackage(socket, responsePackage)) {
                            pushLog("错误：发送响应包失败：" + remoteInetAddress + "   |   " + responsePackage);
                        }
                        //触发发送响应后事件
                        AfterSendResponseArgs afterSendResponseArgs = new AfterSendResponseArgs(this, clientConnectionInfoArgs, requestPackage, responsePackage);
                        eventHelper.performEvent(AfterSendResponseListener.class, afterSendResponseArgs);

                        if (isDisconnectWhenSendResponseFinish) {
                            return;
                        }
                    }
                }
            }
        } catch (Exception ex) {
            //Logger.getLogger(XiepServer.class.getName()).log(Level.SEVERE, null, ex);
        } finally {
            //触发与客户端连接断开事件
            eventHelper.performEvent(ClientDisconnectedListener.class, clientConnectionInfoArgs);

            synchronized (connectedClientList) {
                connectedClientList.remove(socket);
            }
            try {
                socket.close();
            } catch (Exception ex) {
                pushLog("关闭Socket时异常：" + ex.toString());
            }
        }
    }
}
