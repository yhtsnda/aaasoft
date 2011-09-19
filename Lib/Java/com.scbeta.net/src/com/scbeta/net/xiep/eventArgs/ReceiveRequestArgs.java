/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.eventArgs;

import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;
import java.util.EventObject;

/**
 *
 * @author aaa
 */
public class ReceiveRequestArgs extends EventObject {

    private ClientConnectionInfoArgs clientConnectionInfoArgs;
    private RequestPackage requestPackage;
    private ResponsePackage responsePackage;
    private boolean isDisconnectWhenSendResponseFinish;

    //获取客户端连接信息
    public ClientConnectionInfoArgs getClientConnectionInfoArgs() {
        return clientConnectionInfoArgs;
    }

    //获取请求包
    public RequestPackage getRequestPackage() {
        return requestPackage;
    }

    //获取响应包
    public ResponsePackage getResponsePackage() {
        return responsePackage;
    }

    //设置响应包
    public void setResponsePackage(ResponsePackage value) {
        responsePackage = value;
    }

    //获取是否在发送完响应包后断开连接
    public boolean getIsDisconnectWhenSendResponseFinish() {
        return isDisconnectWhenSendResponseFinish;
    }

    //设置是否在发送完响应包后断开连接
    public void setIsDisconnectWhenSendResponseFinish(boolean value) {
        isDisconnectWhenSendResponseFinish = value;
    }

    //构造函数
    public ReceiveRequestArgs(Object source, ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage) {
        super(source);
        this.clientConnectionInfoArgs = clientConnectionInfoArgs;
        this.requestPackage = requestPackage;
    }
}
