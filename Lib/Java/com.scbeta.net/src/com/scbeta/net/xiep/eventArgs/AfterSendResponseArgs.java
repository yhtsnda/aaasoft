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
public class AfterSendResponseArgs extends EventObject {

    private ClientConnectionInfoArgs clientConnectionInfoArgs;
    private RequestPackage requestPackage;
    private ResponsePackage responsePackage;

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

    //构造函数
    public AfterSendResponseArgs(Object source, ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage, ResponsePackage responsePackage) {
        super(source);
        this.clientConnectionInfoArgs = clientConnectionInfoArgs;
        this.requestPackage = requestPackage;
        this.responsePackage = responsePackage;
    }
}
