/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.LinkedHashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.dom4j.Attribute;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;

/**
 *
 * @author aaa
 */
public class XmlTreeNode {

    private String key;
    private String value;
    private List<XmlTreeNode> items;
    private Map<String, String> attribs;
    private static TextEncoder xmlTextEncoder = TextEncoder.getXmlTextEncoder();
    private static TextEncoder jsonTextEncoder = TextEncoder.getJsonTextEncoder();
    // 获取结点名称

    public String getKey() {
        return key;
    }

    public String getEncodedKey() {
        return xmlTextEncoder.encodeText(key);
    }

    //设置结点名称
    public void setKey(String value) {
        key = value;
    }

    // 获取结点的值
    public String getValue() {
        if (value == null) {
            return "";
        }
        return value;
    }

    public String getEncodedValue() {
        if (value == null) {
            return "";
        }
        return xmlTextEncoder.encodeText(value);
    }

    // 设置结点的值(Value与Items同时只能有一个有效)
    public void setValue(String value) {
        if (value == null) {
            this.value = "";
        } else {
            this.value = xmlTextEncoder.decodeText(value);
        }
    }

    // 子结点集合(Value与Items同时只能有一个有效)
    public List<XmlTreeNode> getItems() {
        return items;
    }

    // 属性集合
    public Map<String, String> getAttribs() {
        return attribs;
    }

    // 构造函数
    public XmlTreeNode() {
        init("", "");
    }

    // 构造函数
    public XmlTreeNode(String key) {
        init(key, "");
    }

    // 构造函数
    public XmlTreeNode(String key, String value) {
        init(key, value);
    }

    // 初始化
    private void init(String key, String value) {
        this.setKey(key);
        this.setValue(value);

        items = new LinkedList<XmlTreeNode>();
        attribs = new LinkedHashMap<String, String>();
    }

    // 根据Key得到属性
    public String getAttributeValue(String key) {
        if (attribs.containsKey(key)) {
            return attribs.get(key);
        } else {
            return null;
        }
    }

    // 设置属性
    public void setAttribute(String key, String value) {
        if (attribs.containsKey(key)) {
            attribs.remove(key);
        }
        attribs.put(key, value);
    }

    //移除属性
    public void removeAttribute(String key) {
        attribs.remove(key);
    }

    public void removeAllAttribute() {
        attribs.clear();
    }

    // 根据Key得到对象
    private XmlTreeNode getChildItem(String key) {
        for (XmlTreeNode item : items) {
            if (item.getKey().equals(key)) {
                return item;
            }
        }
        return null;
    }

    // 根据Key得到其子对象列表
    private List<XmlTreeNode> getChildItems(String key) {
        List<XmlTreeNode> lstNodes = new LinkedList<XmlTreeNode>();
        for (XmlTreeNode item : items) {
            if (item.getKey().equals(key)) {
                lstNodes.add(item);
            }
        }
        return lstNodes;
    }

    // 根据路径得到对象
    public XmlTreeNode getItem(String path) {
        if (path == null || path.isEmpty()) {
            return this;
        }
        XmlTreeNode TmpTn = this;

        String[] TmpKeys = path.split("/");
        for (String tmpKey : TmpKeys) {
            String tmpKeyStr = tmpKey.trim();
            if (tmpKeyStr == null || tmpKeyStr.equals("")) {
                continue;
            }

            TmpTn = TmpTn.getChildItem(tmpKeyStr);
            if (TmpTn == null) {
                return null;
            }
        }
        return TmpTn;
    }

    //移除指定的对象
    public void removeItem(XmlTreeNode node) {
        items.remove(node);
    }

    //移除所有的对象
    public void removeAllItems() {
        items.clear();
    }

    /// <summary>
    /// 根据路径得到对象集合
    /// </summary>
    /// <param name="Path">路径</param>
    /// <returns></returns>
    public List<XmlTreeNode> getItems(String path) {
        XmlTreeNode ParentNode = this;
        String[] TmpKeys = path.split("/");
        String LastKey = TmpKeys[TmpKeys.length - 1];
        if (TmpKeys.length > 1) {
            String ParentPath = path.substring(0, path.length() - (LastKey.length() + 1));
            ParentNode = getItem(ParentPath);
            if (ParentNode == null) {
                return new LinkedList<XmlTreeNode>();
            }
        }
        return ParentNode.getChildItems(LastKey);
    }

    /// <summary>
    /// 根据路径得到对象的值
    /// </summary>
    /// <param name="Path">路径</param>
    /// <returns></returns>
    public String getItemValue(String path) {
        XmlTreeNode TmpTn = getItem(path);
        if (TmpTn == null) {
            return null;
        }
        return TmpTn.getValue();
    }

    // 添加子结点
    public XmlTreeNode addItem(XmlTreeNode tn) {
        items.add(tn);
        return tn;
    }

    // 添加子结点
    public XmlTreeNode addItem(String key) {
        return addItem(key, "");
    }

    // 添加子结点
    public XmlTreeNode addItem(String key, String value) {
        XmlTreeNode tn = new XmlTreeNode(key);
        tn.setValue(value);
        return addItem(tn);
    }

    //添加子结点
    public XmlTreeNode addItem(String key, XmlTreeNode node) {
        XmlTreeNode tn = new XmlTreeNode(key);
        tn.addItem(node);
        addItem(tn);
        return tn;
    }

    // ToString方法(此方法仅供调试时查看对象的值，要生成XML请用toXml方法)
    @Override
    public String toString() {
        return this.toXml();
    }

    //输出XML
    public String toXml() {
        return XmlTreeNode.toXml(this, "utf-8");
    }

    public String toXml(String encoding) {
        return XmlTreeNode.toXml(this, encoding);
    }

    //输出Json
    public String toJson() {
        return XmlTreeNode.toJson(this);
    }

    // 输出XML
    public static String toXml(XmlTreeNode root, String encoding) {
        String xml = "";

        org.dom4j.DocumentFactory factory = org.dom4j.DocumentFactory.getInstance();
        Document document = factory.createDocument();
        //根结点
        Element rootElement = document.addElement(root.getKey());
        //设置XML文档编码
        document.setXMLEncoding(encoding);

        _toXml(root, rootElement);
        xml = document.asXML();
        return xml;
    }

    private static void _toXml(XmlTreeNode treeNode, Element parentXmlElement) {
        //先添加属性
        for (String key : treeNode.getAttribs().keySet()) {
            parentXmlElement.addAttribute(key, treeNode.getAttribs().get(key));
        }

        //再添加值或子结点
        if (treeNode.getItems().isEmpty()) {
            String tmpValue = treeNode.getEncodedValue();
            if (tmpValue == null) {
                tmpValue = "";
            }
            parentXmlElement.setText(tmpValue);
        } else {
            for (XmlTreeNode item : treeNode.getItems()) {
                Element newXmlElement = parentXmlElement.addElement(item.getKey());
                _toXml(item, newXmlElement);
            }
        }
    }

    // 从XML得到TreeNode对象
    public static XmlTreeNode fromXml(String xml) {
        if (xml == null || xml.isEmpty()) {
            return null;
        }
        Document document = null;
        try {
            document = DocumentHelper.parseText(xml);
        } catch (DocumentException ex) {
            ConsoleHelper.println("fromXml:" + xml);
            Logger.getLogger(Logger.GLOBAL_LOGGER_NAME).log(Level.SEVERE, xml, ex);
            return null;
        }

        Element XmlRoot = document.getRootElement();
        return _fromXml(XmlRoot);
    }

    private static XmlTreeNode _fromXml(Element parentXmlElement) {
        XmlTreeNode TnNode = new XmlTreeNode(parentXmlElement.getName());

        //先写属性
        for (Attribute attrib : (List<Attribute>) parentXmlElement.attributes()) {
            TnNode.setAttribute(attrib.getName(), attrib.getValue());
        }

        //再写子结点
        if (parentXmlElement.elements().isEmpty()) {
            String tmpValue = parentXmlElement.getTextTrim();
            if (tmpValue == null) {
                tmpValue = "";
            }
            TnNode.setValue(tmpValue);
        } else {
            for (Element childElement : (List<Element>) parentXmlElement.elements()) {
                TnNode.addItem(_fromXml(childElement));
            }
        }
        return TnNode;
    }

    public static String toJson(XmlTreeNode treeNode) {
        StringBuilder sb = new StringBuilder();
        _toJson(treeNode, sb);
        return sb.toString();
    }

    private static void _toJson(XmlTreeNode treeNode, StringBuilder sb) {
        //如果当前是根结点
        boolean isRootNode = sb.length() == 0;
        //如果当前根结点的名字是root
        boolean isRootNodeName_root = "root".equals(treeNode.getKey());

        sb.append("{");
        if (isRootNode && !isRootNodeName_root) {
            sb.append(String.format("\"%s\":{", treeNode.getKey()));
        }
        //添加属性
        boolean isFirstAttrib = true;
        for (String attribKey : treeNode.getAttribs().keySet()) {
            if (!isFirstAttrib) {
                sb.append(",");
            }
            isFirstAttrib = false;

            sb.append(String.format("\"%s\":\"%s\"", attribKey, jsonTextEncoder.encodeText(treeNode.getAttributeValue(attribKey))));
        }
        //添加子结点
        boolean isFirstNode = true;
        for (XmlTreeNode childNode : treeNode.getItems()) {
            if (treeNode.getAttribs().keySet().size() > 0 || !isFirstNode) {
                sb.append(",");
            }
            isFirstNode = false;

            if (childNode.getKey() != null || !childNode.getKey().isEmpty()) {
                sb.append(String.format("\"%s\":", childNode.getKey()));
            }
            _toJson(childNode, sb);
        }
        //添加大括号结束
        if (isRootNode && !isRootNodeName_root) {
            sb.append("}");
        }
        sb.append("}");
    }

    public static XmlTreeNode fromJson(String json) throws Exception {
        throw new Exception("未实现的方法！此方法还未实现");
    }
}
