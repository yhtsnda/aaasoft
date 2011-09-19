/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.handlers;

import com.scbeta.net.xiep.eventArgs.ClientConnectionInfoArgs;
import com.scbeta.net.xiep.packages.RequestPackage;
import com.scbeta.net.xiep.packages.ResponsePackage;

/**
 * 心跳请求
 * @author aaa
 */
public class XiepPingRequestHandler extends AbstractRequestHandler {

    @Override
    public ResponsePackage execute(ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage) {
        ResponsePackage responsePackage = new ResponsePackage();
        responsePackage.setResponse("XiepPong");
        return responsePackage;
    }
}
