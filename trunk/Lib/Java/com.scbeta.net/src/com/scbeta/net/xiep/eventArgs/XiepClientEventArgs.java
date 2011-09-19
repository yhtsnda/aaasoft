/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.net.xiep.eventArgs;

import com.scbeta.net.xiep.packages.EventPackage;
import java.util.EventObject;

/**
 * XiepClient事件参数类
 * @author aaa
 */
public class XiepClientEventArgs extends EventObject {

    private EventPackage eventPackage;

    //获取事件包
    public EventPackage getEventPackage() {
        return eventPackage;
    }

    //构造函数
    public XiepClientEventArgs(Object source, EventPackage eventPackage) {
        super(source);
        this.eventPackage = eventPackage;
    }
}
