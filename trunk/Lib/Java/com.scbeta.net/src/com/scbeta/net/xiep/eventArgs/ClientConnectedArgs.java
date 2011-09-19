/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.eventArgs;

import com.scbeta.net.xiep.packages.EventPackage;
import java.util.EventObject;

/**
 *
 * @author aaa
 */
public class ClientConnectedArgs extends EventObject {

    private ClientConnectionInfoArgs clientConnectionInfoArgs;
    private boolean isAccept;
    private EventPackage eventPackage;

    //获取客户端连接信息
    public ClientConnectionInfoArgs getClientConnectionInfoArgs() {
        return clientConnectionInfoArgs;
    }

    //获取是否接受此连接，默认为true，如果为false，则会断开与此客户端的连接
    public boolean getIsAccept() {
        return isAccept;
    }

    //设置是否接受此连接，默认为true，如果为false，则会断开与此客户端的连接
    public void setIsAccept(boolean value) {
        isAccept = value;
    }

    //获取EventPackage对象，如果需要向客户端发送EventPackage对象，则要在参数中赋值。比如发送点服务端信息事件
    public EventPackage getEventPackage() {
        return eventPackage;
    }

    //设置EventPackage对象，如果需要向客户端发送EventPackage对象，则要在参数中赋值。比如发送点服务端信息事件
    public void setEventPackage(EventPackage value) {
        this.eventPackage = value;
    }

    //构造函数
    public ClientConnectedArgs(Object source, ClientConnectionInfoArgs clientConnectionInfoArgs) {
        super(source);
        this.clientConnectionInfoArgs = clientConnectionInfoArgs;
        isAccept = true;
    }
}
