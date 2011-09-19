/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.packages;

import com.scbeta.helpers.XmlTreeNode;
import java.util.List;

/**
 * 抽象Xiep包
 * @author aaa
 */
public abstract class AbstractXiepPackage {

    private XmlTreeNode rootPackage;

    //设置根包
    public void setRootPackage(XmlTreeNode rootPackage) {
        this.rootPackage = rootPackage;
    }

    //设置属性
    public void setAttribute(String key, String value) {
        rootPackage.setAttribute(key, value);
    }

    //获取属性的值
    public String getAttributeValue(String key) {
        return rootPackage.getAttributeValue(key);
    }

    //移除包属性
    public void removeAttribute(String key) {
        rootPackage.removeAttribute(key);
    }

    //得到参数列表
    public List<XmlTreeNode> getArguments() {
        return rootPackage.getItems();
    }

    //得到参数节点
    public XmlTreeNode getArgument(String path) {
        return rootPackage.getItem(path);
    }

    //根据path得到参数的值
    public String getArgumentValue(String path) {
        return rootPackage.getItemValue(path);
    }

    //添加参数    
    public XmlTreeNode addArgument(String key, String value) {
        return rootPackage.addItem(key, value);
    }

    //添加参数
    public XmlTreeNode addArgument(XmlTreeNode node) {
        return rootPackage.addItem(node);
    }

    //得到包类型名称(抽象函数，由实现类实现。用来作为XML根节点名称)
    public abstract String getPackageName();

    //构造函数
    public AbstractXiepPackage() {
        init();
    }

    //初始化
    private void init() {
        rootPackage = new XmlTreeNode(getPackageName());
    }

    // 输出XML
    public String toXml() {
        return rootPackage.toXml();
    }

    // 输出XML
    public String toXml(String encoding) {
        return rootPackage.toXml(encoding);
    }
}
