/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.eventArgs;

import java.util.EventObject;

/**
 *
 * @author aaa
 */
public class DebugInfoArgs extends EventObject {
    
    private String debugText;
    
    //获取调试文本
    public String getDebugText(){
        return debugText;
    }
    
    //设置调试文本
    public void setDebugText(String value){
        debugText = value;
    }
    
    //构造函数
    public DebugInfoArgs(Object source) {
        super(source);
    }
}
