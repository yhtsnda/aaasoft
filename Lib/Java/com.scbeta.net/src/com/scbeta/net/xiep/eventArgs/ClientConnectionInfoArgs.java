/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.eventArgs;

import java.net.InetAddress;
import java.net.Socket;
import java.util.EventObject;

/**
 *
 * @author aaa
 */
public class ClientConnectionInfoArgs extends EventObject {

    private Socket socket;
    private InetAddress inetAddress;
    private int port;

    //获取Socket对象
    public Socket getSocket() {
        return socket;
    }

    //获取IP地址
    public InetAddress getInetAddress() {
        return inetAddress;
    }

    //获取端口
    public int getPort() {
        return port;
    }

    @Override
    public String toString() {
        return String.format("%s:%s", inetAddress.toString(), port);
    }

    //构造函数
    public ClientConnectionInfoArgs(Object source, Socket socket, InetAddress inetAddress, int port) {
        super(source);
        this.socket = socket;
        this.inetAddress = inetAddress;
        this.port = port;
    }
}
