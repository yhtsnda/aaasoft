/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep;

import com.scbeta.helpers.EventHelper;
import com.scbeta.helpers.SingleEventListener;
import com.scbeta.net.xiep.eventArgs.XiepClientEventArgs;
import com.scbeta.net.xiep.helpers.XiepIoHelper;
import com.scbeta.net.xiep.packages.AbstractXiepPackage;
import com.scbeta.net.xiep.packages.EventPackage;
import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.util.EventObject;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * XIEP(Xml-based Information Exchange Protocol)客户端
 * @author aaa
 */
public class XiepClient {
    //==========================
    //      属性部分开始
    //==========================

    private Socket socket;
    private String serverHostName;
    private int serverPort;
    private int heartBeatInterval = 10;
    private int heartBeatTimeout = 30;
    private Map<String, ResponsePackage> mapRequestResponse;

    //得到本地绑定的端口
    public int getLocalPort() {
        if (socket == null) {
            return -1;
        } else {
            return socket.getLocalPort();
        }
    }

    //获取服务器主机名称
    public String getServerHostName() {
        return serverHostName;
    }

    //设置服务器主机名称
    public void setServerHostName(String value) {
        serverHostName = value;
    }

    //获取服务端端口
    public int getServerPort() {
        return serverPort;
    }

    //设置服务端端口
    public void setServerPort(int value) {
        serverPort = value;
    }

    //获取心跳消息发送时间间隔(单位：秒)
    public int getHeartBeatInterval() {
        return heartBeatInterval;
    }

    //设置心跳消息发送时间间隔(单位：秒)
    public void setHeartBeatInterval(int value) {
        heartBeatInterval = value;
    }

    //获取心跳消息超时时间（单位：秒）
    public int getHeartBeatTimeout() {
        return heartBeatTimeout;
    }

    //设置获取心跳消息超时时间（单位：秒）
    //      根据我的经验来说“心跳消息超时时间”要
    //      大于“心跳消息发送时间间隔”的两倍以上
    //      且此超时时间最好与服务器的超时时间一致
    public void setHeartBeatTimeout(int value) {
        heartBeatTimeout = value;
    }

    //获取是否已连接到服务端
    public boolean getIsConnected() {
        return socket != null;
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

    //收到服务器事件的事件监听器
    public interface ServerEventCameListener extends SingleEventListener<XiepClientEventArgs> {
    }

    //添加收到服务器事件的事件监听器
    public void addServerEventCameListener(ServerEventCameListener listener) {
        eventHelper.addListener(listener);
    }

    //与服务器连接被断开事件监听器
    public interface ServerDisconnectedListener extends SingleEventListener<EventObject> {
    }

    //添加与服务器连接被断开事件监听器
    public void addServerDisconnectedListener(ServerDisconnectedListener listener) {
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
    public XiepClient(Socket socket) {
        this.socket = socket;
        init();
    }

    //构造函数
    public XiepClient(String serverHostName, int serverPort) {
        this.serverHostName = serverHostName;
        this.serverPort = serverPort;

        init();
    }

    private void init() {
        mapRequestResponse = new LinkedHashMap<String, ResponsePackage>();
    }

    //开始
    public void start() throws IOException {
        if (!this.getIsConnected()) {
            socket = new Socket();
            socket.setSoTimeout(0);
            socket.connect(new InetSocketAddress(serverHostName, serverPort));
        }

        //开启接收响应线程
        Thread trdRecv = new Thread(new Runnable() {

            @Override
            public void run() {
                receiveResponseThreadFunction();
            }
        });
        trdRecv.start();

        //开启心跳线程
        Thread trdHeartBeat = new Thread(new Runnable() {

            @Override
            public void run() {
                heartBeatThreadFunction();
            }
        });
        trdHeartBeat.start();
    }

    //停止
    public void stop() {
        try {
            if (socket != null) {
                socket.close();
            }
        } catch (IOException ex) {
            Logger.getLogger(XiepClient.class.getName()).log(Level.SEVERE, null, ex);
        }
        socket = null;
    }

    /// <summary>
    /// 获取响应是否成功
    /// </summary>
    /// <param name="responsePackage">响应包</param>
    /// <returns></returns>
    public static Boolean IsResponseSuccess(ResponsePackage responsePackage) {
        if (responsePackage == null) {
            return false;
        }
        return "Success".equals(responsePackage.getResponse());
    }

    // 发送请求数据包并得到响应包(默认超时时间为10秒)
    public ResponsePackage SendRequest(RequestPackage requestPackage) {
        return SendRequest(requestPackage, 10);
    }

    // 发送请求数据包并得到响应包
    //requestPackage:请求数据包
    //timeoutSeconds:超时时间(单位：秒)，如果超时时间小于等于0，则超时时间为无限大
    public ResponsePackage SendRequest(RequestPackage requestPackage, int timeoutSeconds) {
        String requestId = requestPackage.getRequestId();
        //发送Request包
        if (!XiepIoHelper.SendPackage(socket, requestPackage)) {
            //发送失败
            return null;
        }

        synchronized (mapRequestResponse) {
            mapRequestResponse.put(requestId, null);
        }

        long startWaitResponseTime = System.currentTimeMillis();
        ResponsePackage responsePackage = null;
        while (true) {
            synchronized (mapRequestResponse) {
                responsePackage = mapRequestResponse.get(requestId);
            }
            long usedSeconds = (System.currentTimeMillis() - startWaitResponseTime) / 1000;
            if ( //如果已经得到响应包
                    responsePackage != null
                    //或者等待已经超时
                    || usedSeconds > timeoutSeconds) {
                break;
            }
            try {
                Thread.sleep(100);
            } catch (InterruptedException ex) {
            }
        }
        //从等待字典中移除
        synchronized (mapRequestResponse) {
            if (mapRequestResponse.containsKey(requestId)) {
                mapRequestResponse.remove(requestId);
            }
        }
        return responsePackage;
    }

    //定时发送心跳消息线程函数
    private void heartBeatThreadFunction() {
        Socket currentSocket = socket;
        while (socket != null && currentSocket == socket) {
            //发送心跳线程
            SendRequest(new RequestPackage("XiepPing", null));
            //N秒发送一次心跳消息
            try {
                Thread.sleep(heartBeatInterval * 1000);
            } catch (InterruptedException ex) {
                Logger.getLogger(XiepClient.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }

    //接收服务器响应线程函数
    private void receiveResponseThreadFunction() {
        try {
            while (true) {
                //接收请求包
                AbstractXiepPackage recvPackage = XiepIoHelper.ReceivePackage(socket);
                if (recvPackage == null) {
                    throw new IOException("获取数据包失败。");
                }

                //如果是响应数据包
                if (ResponsePackage.class.isInstance(recvPackage)) {
                    ResponsePackage responsePackage = (ResponsePackage) recvPackage;
                    String requestId = responsePackage.getRequestId();
                    synchronized (mapRequestResponse) {
                        if (mapRequestResponse.containsKey(requestId)) {
                            mapRequestResponse.put(requestId, responsePackage);
                        }
                    }
                } //如果是事件数据包
                else if (EventPackage.class.isInstance(recvPackage)) {
                    EventPackage eventPackage = (EventPackage) recvPackage;
                    //触发收到服务器事件的事件
                    eventHelper.beginPerformEvent(ServerEventCameListener.class, new XiepClientEventArgs(this, eventPackage));
                }
            }
        } catch (Exception ex) {
            socket = null;

            //触发与服务器连接断开事件
            eventHelper.beginPerformEvent(ServerDisconnectedListener.class, null);
            return;
        }
    }
}
