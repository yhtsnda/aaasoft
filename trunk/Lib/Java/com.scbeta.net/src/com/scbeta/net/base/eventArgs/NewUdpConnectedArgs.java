/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.base.eventArgs;

import java.net.InetAddress;
import java.util.EventObject;

/**
 *新UDP连接事件参数
 * @author aaa
 */
public class NewUdpConnectedArgs extends EventObject {

    private InetAddress remoteIP;
    private int remotePort;
    private byte[] data;

    // 对方IP地址
    public InetAddress getRemoteIP() {
        return remoteIP;
    }

    // 对方端口
    public int getRemotePort() {
        return remotePort;
    }

    // 数据
    public byte[] getData() {
        return data;
    }

    public NewUdpConnectedArgs(Object source, InetAddress remoteIP, int remotePort, byte[] data) {
        super(source);
        this.remoteIP = remoteIP;
        this.remotePort = remotePort;
        this.data = data;
    }
}
