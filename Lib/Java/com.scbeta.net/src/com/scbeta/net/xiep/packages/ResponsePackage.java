/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.packages;

import com.scbeta.helpers.XmlTreeNode;

/**
 * 响应包
 * @author aaa
 */
public class ResponsePackage extends AbstractXiepPackage {

    @Override
    public String getPackageName() {
        return "ResponsePackage";
    }

    //获取请求编号
    public String getRequestId() {
        return this.getAttributeValue("RequestId");
    }

    //设置请求编号
    public void setRequestId(String value) {
        this.setAttribute("RequestId", value);
    }

    //获取响应
    public String getResponse() {
        return this.getAttributeValue("Response");
    }

    //设置响应
    public void setResponse(String value) {
        this.setAttribute("Response", value);
    }

    //构造函数
    public ResponsePackage() {
    }

    //构造函数
    public ResponsePackage(String requestId) {
        init(requestId, null);
    }

    //构造函数
    public ResponsePackage(String requestId, String response) {
        init(requestId, response);
    }

    //构造函数
    public ResponsePackage(RequestPackage requestPackage) {
        init(requestPackage.getRequestId(), null);
    }

    //构造函数
    public ResponsePackage(RequestPackage requestPackage, String response) {
        init(requestPackage.getRequestId(), response);
    }

    private void init(String requestId, String response) {
        setRequestId(requestId);
        if (response == null || response.isEmpty()) {
        } else {
            this.setResponse(response);
        }
    }

    //从XML得到响应包
    public static ResponsePackage fromXml(String xml) {
        ResponsePackage rtnPackage = null;

        XmlTreeNode treeNode = XmlTreeNode.fromXml(xml);
        String packageName = treeNode.getKey();
        if (packageName.equals("ResponsePackage")) {
            rtnPackage = new ResponsePackage();
        } else {
            return null;
        }

        rtnPackage.setRootPackage(treeNode);
        return rtnPackage;
    }
}
