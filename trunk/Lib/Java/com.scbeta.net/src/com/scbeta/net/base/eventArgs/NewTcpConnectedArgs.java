/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.base.eventArgs;

import java.net.InetAddress;
import java.net.Socket;
import java.util.EventObject;

/**
 *新TCP连接事件参数
 * @author aaa
 */
public class NewTcpConnectedArgs extends EventObject {

    private InetAddress remoteIP;
    private int remotePort;
    private Socket socket;

    // 对方IP地址
    public InetAddress getRemoteIP() {
        return remoteIP;
    }

    // 对方端口
    public int getRemotePort() {
        return remotePort;
    }

    // Socket对象
    public Socket getSocket() {
        return socket;
    }

    // 构造函数
    public NewTcpConnectedArgs(Object source, InetAddress remoteIP, int remotePort, Socket socket) {
        super(source);
        this.remoteIP = remoteIP;
        this.remotePort = remotePort;
        this.socket = socket;
    }
}
