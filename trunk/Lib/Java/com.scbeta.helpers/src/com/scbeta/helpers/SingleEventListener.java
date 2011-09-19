/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.EventListener;
import java.util.EventObject;

/**
 * 单事件监听器接口
 * @author aaa
 */
public interface SingleEventListener<T extends EventObject> extends EventListener {

    //执行事件方法
    public void Perform(T e);
}
