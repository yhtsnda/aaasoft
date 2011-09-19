/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.packages;

import com.scbeta.helpers.XmlTreeNode;
import java.util.Map;
import java.util.UUID;

/**
 * 请求包
 * @author aaa
 */
public class RequestPackage extends AbstractXiepPackage {

    @Override
    public String getPackageName() {
        return "RequestPackage";
    }

    //获取请求编号
    public String getRequestId() {
        return this.getAttributeValue("RequestId");
    }

    //设置请求编号
    private void setRequestId(String value) {
        this.setAttribute("RequestId", value);
    }

    //获取此请求包的Request
    public String getRequest() {
        return this.getAttributeValue("Request");
    }

    //设置此请求包的Request
    public void setRequest(String action) {
        this.setAttribute("Request", action);
    }

    //构造函数
    public RequestPackage() {
        this.init(null, null);
    }

    //构造函数
    public RequestPackage(String request) {
        this.init(request, null);
    }

    //构造函数
    public RequestPackage(String request, Map<String, String> mapArgs) {
        this.init(request, mapArgs);
    }

    private void init(String request, Map<String, String> mapArgs) {
        setRequestId(UUID.randomUUID().toString());

        //设置请求
        if (request != null && !request.isEmpty()) {
            this.setRequest(request);
        }
        //添加参数
        if (mapArgs != null) {
            for (String key : mapArgs.keySet()) {
                this.addArgument(key, mapArgs.get(key));
            }
        }
    }

    //从XML得到请求包
    public static RequestPackage fromXml(String xml) {
        RequestPackage rtnPackage = null;

        XmlTreeNode treeNode = XmlTreeNode.fromXml(xml);
        String packageName = treeNode.getKey();
        if (packageName.equals("RequestPackage")) {
            rtnPackage = new RequestPackage();
        } else {
            return null;
        }

        rtnPackage.setRootPackage(treeNode);
        return rtnPackage;
    }
}
