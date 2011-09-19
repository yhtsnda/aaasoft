/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 *
 * @author aaa
 */
public class TextEncoder {

    private Map<String, String> textReplaceMap;

    /**
     * 得到XML文本编码器
     * @return 
     */
    public static TextEncoder getXmlTextEncoder() {
        Map<String, String> textReplaceMap = new LinkedHashMap<String, String>();
        textReplaceMap.put("&", "&amp;");
        textReplaceMap.put("<", "&lt;");
        textReplaceMap.put(">", "&gt;");
        textReplaceMap.put("'", "&apos;");
        textReplaceMap.put("\"", "&quot;");

        textReplaceMap.put("\t", "&#x09;");
        textReplaceMap.put(" ", "&#x20;");
        textReplaceMap.put("\n", "&#x0A;");
        textReplaceMap.put("\r", "&#x0D;");
        return new TextEncoder(textReplaceMap);
    }

    /**
     * 得到JSON文本编码器
     * @return 
     */
    public static TextEncoder getJsonTextEncoder() {
        Map<String, String> textReplaceMap = new LinkedHashMap<String, String>();
        textReplaceMap.put("\"", "\\\"");
        return new TextEncoder(textReplaceMap);
    }

    public TextEncoder(Map<String, String> textReplaceMap) {
        this.textReplaceMap = textReplaceMap;
    }

    public TextEncoder(String[] srcTextArray, String[] desTextArray) {
        this.textReplaceMap = new LinkedHashMap<String, String>();
        for (int i = 0; i <= srcTextArray.length - 1; i++) {
            this.textReplaceMap.put(srcTextArray[i], desTextArray[i]);
        }
    }

    //编码字符串
    public String encodeText(String text) {
        for (String srcText : textReplaceMap.keySet()) {
            String desText = textReplaceMap.get(srcText);
            text = text.replaceAll(srcText, desText);
        }
        return text;
    }

    //解码字符串
    public String decodeText(String text) {
        for (String srcText : textReplaceMap.keySet()) {
            String desText = textReplaceMap.get(srcText);
            text = text.replaceAll(desText, srcText);
        }
        return text;
    }
}
