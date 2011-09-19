using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Xiep.Packages;

namespace aaaSoft.Net.Xiep.EventArgs
{
    public class ClientConnectedArgs : System.EventArgs
    {

        private ClientConnectionInfoArgs clientConnectionInfoArgs;
        private Boolean isAccept;
        private EventPackage eventPackage;

        //获取客户端连接信息
        public ClientConnectionInfoArgs getClientConnectionInfoArgs()
        {
            return clientConnectionInfoArgs;
        }

        //获取是否接受此连接，默认为true，如果为false，则会断开与此客户端的连接
        public Boolean getIsAccept()
        {
            return isAccept;
        }

        //设置是否接受此连接，默认为true，如果为false，则会断开与此客户端的连接
        public void setIsAccept(Boolean value)
        {
            isAccept = value;
        }

        //获取EventPackage对象，如果需要向客户端发送EventPackage对象，则要在参数中赋值。比如发送点服务端信息事件
        public EventPackage getEventPackage()
        {
            return eventPackage;
        }

        //设置EventPackage对象，如果需要向客户端发送EventPackage对象，则要在参数中赋值。比如发送点服务端信息事件
        public void setEventPackage(EventPackage value)
        {
            this.eventPackage = value;
        }

        //构造函数
        public ClientConnectedArgs(ClientConnectionInfoArgs clientConnectionInfoArgs)
        {
            this.clientConnectionInfoArgs = clientConnectionInfoArgs;
            isAccept = true;
        }
    }
}
