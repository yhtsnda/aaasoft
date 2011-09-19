/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.base;

import com.scbeta.net.base.TcpIpServer.NewUdpConnectedListener;
import com.scbeta.net.base.eventArgs.NewUdpConnectedArgs;
import java.io.IOException;
import java.net.DatagramPacket;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author aaa
 */
class UdpListenThread extends Thread {

    TcpIpServer server;

    UdpListenThread(TcpIpServer server) {
        this.server = server;
    }

    @Override
    public void run() {
        while (server.getUdpListeningPort() > 0) {
            try {
                byte[] buffer = new byte[1500];
                DatagramPacket p = new DatagramPacket(buffer, buffer.length);
                server.getUdpListener().receive(p);

                byte[] data = new byte[p.getLength()];
                for (int i = 0; i <= data.length - 1; i++) {
                    data[i] = buffer[i];
                }

                //触发事件
                NewUdpConnectedArgs e = new NewUdpConnectedArgs(this, p.getAddress(), p.getPort(), data);
                server.getEventHelper().performEvent(NewUdpConnectedListener.class, e);
            } catch (IOException ex) {
                Logger.getLogger(UdpListenThread.class.getName()).log(Level.SEVERE, null, ex);
                continue;
            }
        }
    }
}
