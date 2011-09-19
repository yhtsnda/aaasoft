/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.EventObject;
import java.util.HashSet;
import java.util.Set;

/**
 *
 * @author aaa
 */
public class EventHelper {

    //用来存放已注册的监听对象的集合
    private Set<SingleEventListener> eventListeners;

    public EventHelper() {
        init();
    }

    private void init() {
        eventListeners = new HashSet<SingleEventListener>();
    }

    //添加事件监听器
    public synchronized void addListener(SingleEventListener listener) {
        eventListeners.add(listener);
    }

    //移除事件监听器
    public synchronized void removeListener(SingleEventListener listener) {
        eventListeners.remove(listener);
    }

    //同步触发事件
    public void performEvent(Class listenerClass, EventObject e) {
        for (SingleEventListener listener : eventListeners) {
            if (listenerClass.isInstance(listener)) {
                listener.Perform(e);
            }
        }
    }

    //异步触发事件(多线程实现)
    public void beginPerformEvent(final Class listenerClass, final EventObject e) {
        Thread trd = new Thread(new Runnable() {

            @Override
            public void run() {
                performEvent(listenerClass, e);
            }
        });
        trd.start();
    }
}
