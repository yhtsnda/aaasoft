/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.util.Random;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * 网络辅助类
 * @author aaa
 */
public class NetHelper {

    // 获取随机端口(1024~65535)
    public static int GetRandomPort() {
        return RandomHelper.GetRandomInt(1024, 65535);
    }

    /**
     * 测试TCP端口是否被监听
     * @param serverHostName
     * @param serverPort
     * @return 
     */
    public static boolean testTcpPortListened(String serverHostName, int serverPort) {
        return testTcpPortListened(serverHostName, serverPort, 2 * 1000);
    }

    /**
     * 测试TCP端口是否被监听
     * @param serverHostName
     * @param serverPort
     * @param timeOut
     * @return 
     */
    public static boolean testTcpPortListened(String serverHostName, int serverPort, int timeOut) {
        try {
            Socket socket = new Socket();
            socket.setSoTimeout(timeOut);
            socket.connect(new InetSocketAddress(serverHostName, serverPort));
            socket.close();
            socket = null;
            return true;
        } catch (IOException ex) {
            return false;
        }
    }
}
