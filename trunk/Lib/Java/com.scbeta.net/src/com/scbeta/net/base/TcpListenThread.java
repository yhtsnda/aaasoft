/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.base;

import com.scbeta.net.base.TcpIpServer.NewTcpConnectedListener;
import com.scbeta.net.base.eventArgs.NewTcpConnectedArgs;
import java.io.IOException;
import java.net.Socket;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author aaa
 */
class TcpListenThread extends Thread {

    TcpIpServer server;

    TcpListenThread(TcpIpServer server) {
        this.server = server;
    }

    @Override
    public void run() {

        while (server.getTcpListenPort() > 0) {
            try {
                Socket newSocket = server.getTcpListener().accept();
                newSocket.setSoTimeout(server.getReadTimeOut());

                // 触发新TCP连接事件
                NewTcpConnectedArgs e = new NewTcpConnectedArgs(this, newSocket.getInetAddress(), newSocket.getPort(), newSocket);
                server.getEventHelper().performEvent(NewTcpConnectedListener.class, e);
            } catch (IOException ex) {
            }
        }
    }
}
