/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.packages;

import com.scbeta.helpers.XmlTreeNode;

/**
 *
 * @author aaa
 */
public class EventPackage extends AbstractXiepPackage {

    @Override
    public String getPackageName() {
        return "EventPackage";
    }

    //获取此请求包的Event
    public String getEvent() {
        return this.getAttributeValue("Event");
    }

    //设置此请求包的Event
    public void setEvent(String event) {
        this.setAttribute("Event", event);
    }

    //构造函数
    public EventPackage() {
    }

    //构造函数
    public EventPackage(String event) {
        init(event);
    }

    private void init(String event) {
        setEvent(event);
    }

    //从XML得到事件包
    public static EventPackage fromXml(String xml) {
        EventPackage rtnPackage = null;

        XmlTreeNode treeNode = XmlTreeNode.fromXml(xml);
        String packageName = treeNode.getKey();
        if (packageName.equals("EventPackage")) {
            rtnPackage = new EventPackage();
        } else {
            return null;
        }

        rtnPackage.setRootPackage(treeNode);
        return rtnPackage;
    }
}
