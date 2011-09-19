/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.handlers;

import com.scbeta.net.xiep.eventArgs.ClientConnectionInfoArgs;
import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;

/**
 * 抽象请求处理器
 * @author aaa
 */
public abstract class AbstractRequestHandler {
    /**
     * 执行
     * @param clientConnectionInfoArgs 客户端连接信息
     * @param requestPackage 请求包
     * @return 响应包
     */
    public abstract ResponsePackage execute(ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage);
}
