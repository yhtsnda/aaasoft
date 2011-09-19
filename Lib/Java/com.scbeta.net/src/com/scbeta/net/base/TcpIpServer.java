/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.base;

import com.scbeta.helpers.EventHelper;
import com.scbeta.helpers.NetHelper;
import com.scbeta.helpers.SingleEventListener;
import com.scbeta.net.base.eventArgs.*;
import java.io.IOException;
import java.net.*;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * 基于TCP/IP协议网络平台类
 * @author aaa
 */
public class TcpIpServer {

    //==========================
    //      属性部分开始
    //==========================
    private ServerSocket tcpListener;
    private DatagramSocket udpListener;
    private int bufferSize = 1024;
    private int writeTimeOut = 0;
    private int readTimeOut = 0;
    private int backlog = 50;
    private int tcpListenPort = -1;
    private int udpListenPort = -1;
    private boolean udpListenBroadcast = false;

    // 获取TCP监听器
    public ServerSocket getTcpListener() {

        return tcpListener;
    }

    // 获取UDP监听器
    public DatagramSocket getUdpListener() {
        return udpListener;
    }

    // 获取缓冲区大小
    public int getBufferSize() {
        return bufferSize;
    }

    // 设置缓冲区大小
    public void setBufferSize(int value) {
        bufferSize = value;
    }

    // 获取发送超时时间
    public int getWriteTimeOut() {
        return writeTimeOut;
    }

    // 设置发送超时时间
    public void setWriteTimeOut(int value) {
        if (value <= 0) {
            value = 10 * 1000;
        }
        writeTimeOut = value;
    }

    // 获取接收超时时间
    public int getReadTimeOut() {
        return readTimeOut;
    }

    // 设置接收超时时间
    public void setReadTimeOut(int value) {
        if (value <= 0) {
            value = 10 * 1000;
        }
        readTimeOut = value;
    }

    // 获取等待连接队列长度
    public int getBacklog() {
        return backlog;
    }

    // 设置等待连接队列长度
    public void setBacklog(int value) {
        backlog = value;
    }

    // 获取TCP监听端口
    public int getTcpListenPort() {
        return tcpListenPort;
    }

    // 获取正在监听的TCP端口
    public int getTcpListeningPort() {
        if (tcpListener == null) {
            return -1;
        }
        if (tcpListenPort != 0) {
            return tcpListenPort;
        }
        return tcpListener.getLocalPort();
    }
    // 设置TCP监听端口

    public void setTcpListenPort(int value) {
        if (value > 65535 || value < -1) {
            value = 0;
        }
        tcpListenPort = value;
    }

    // 获取UDP监听端口
    public int getUdpListenPort() {
        return udpListenPort;
    }

    // 获取正在监听的UDP端口
    public int getUdpListeningPort() {
        if (udpListener == null) {
            return -1;
        }
        if (udpListenPort != 0) {
            return udpListenPort;
        }
        return udpListener.getLocalPort();
    }
    // 设置UDP监听端口

    public void setUdpListenPort(int value) {
        if (value > 65535 || value < -1) {
            value = 0;
        }
        udpListenPort = value;
    }

    // 获取UDP是否监听广播
    public boolean getUdpListenBroadcast() {
        return udpListenBroadcast;
    }

    // 设置UDP是否监听广播
    public void setUdpListenBroadcast(boolean value) {
        udpListenBroadcast = value;
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

    // 新TCP连接事件监听器
    public interface NewTcpConnectedListener extends SingleEventListener<NewTcpConnectedArgs> {
    }

    //添加一个NewTcpConnectedListener事件监听器
    public void addNewTcpConnectedListener(NewTcpConnectedListener listener) {
        eventHelper.addListener(listener);
    }

    // 新UDP连接事件监听器
    public interface NewUdpConnectedListener extends SingleEventListener<NewUdpConnectedArgs> {
    }

    //添加一个NewUdpConnectedListener事件监听器
    public void addNewUdpConnectedListener(NewUdpConnectedListener listener) {
        eventHelper.addListener(listener);
    }

    //移除一个事件监听器
    public synchronized void removeListener(SingleEventListener listener) {
        eventHelper.removeListener(listener);
    }

    //==========================
    //      函数部分开始
    //==========================
    // 开始
    public void start() throws SocketException, IOException {
        //本机地址
        InetAddress localAddress = null;//InetAddress.getLocalHost()

        if (getUdpListenPort() != -1) {
            if (getUdpListenPort() == 0) {
                while (true) {
                    int randomPort = NetHelper.GetRandomPort();
                    try {
                        udpListener = new DatagramSocket(randomPort, localAddress);
                        break;
                    } catch (SocketException ex) {
                        Logger.getLogger(TcpIpServer.class.getName()).log(Level.SEVERE, null, ex);
                        try {
                            Thread.sleep(10);
                        } catch (InterruptedException ex1) {
                        }
                    }
                }
            } else {
                udpListener = new DatagramSocket(getUdpListenPort(), localAddress);
            }
            udpListener.setBroadcast(getUdpListenBroadcast());
            Thread trdListenUdp = new UdpListenThread(this);
            trdListenUdp.start();
        }

        if (getTcpListenPort() != -1) {
            if (getTcpListenPort() == 0) {
                //如果是随机端口
                while (true) {
                    int randomPort = NetHelper.GetRandomPort();
                    try {
                        tcpListener = new ServerSocket(randomPort, getBacklog(), localAddress);
                        break;
                    } catch (IOException ex) {
                        Logger.getLogger(TcpIpServer.class.getName()).log(Level.SEVERE, null, ex);
                        try {
                            Thread.sleep(10);
                        } catch (InterruptedException ex1) {
                        }
                    }
                }
            } else {
                tcpListener = new ServerSocket(getTcpListenPort(), getBacklog(), localAddress);
            }

            //设置超时值
            tcpListener.setSoTimeout(this.getReadTimeOut());

            Thread trdListenTcp = new TcpListenThread(this);
            trdListenTcp.start();
        }
    }

    // 停止
    public void stop() {
        if (udpListener != null && getUdpListenPort() != -1) {
            udpListener.close();
            udpListener = null;
        }

        if (tcpListener != null && getTcpListenPort() != -1) {
            try {
                tcpListener.close();
            } catch (IOException ex) {
                Logger.getLogger(TcpIpServer.class.getName()).log(Level.SEVERE, null, ex);
            }
            tcpListener = null;
        }
    }
}
