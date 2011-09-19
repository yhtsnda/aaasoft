using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Xiep.Packages;

namespace aaaSoft.Net.Xiep.EventArgs
{
    public class ReceiveRequestArgs:System.EventArgs
    {
        private ClientConnectionInfoArgs clientConnectionInfoArgs;
        private RequestPackage requestPackage;
        private ResponsePackage responsePackage;
        private Boolean isDisconnectWhenSendResponseFinish;

        //获取客户端连接信息
        public ClientConnectionInfoArgs getClientConnectionInfoArgs()
        {
            return clientConnectionInfoArgs;
        }

        //获取请求包
        public RequestPackage getRequestPackage()
        {
            return requestPackage;
        }

        //获取响应包
        public ResponsePackage getResponsePackage()
        {
            return responsePackage;
        }

        //设置响应包
        public void setResponsePackage(ResponsePackage value)
        {
            responsePackage = value;
        }

        //获取是否在发送完响应包后断开连接
        public Boolean getIsDisconnectWhenSendResponseFinish()
        {
            return isDisconnectWhenSendResponseFinish;
        }

        //设置是否在发送完响应包后断开连接
        public void setIsDisconnectWhenSendResponseFinish(Boolean value)
        {
            isDisconnectWhenSendResponseFinish = value;
        }

        //构造函数
        public ReceiveRequestArgs(ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage)
        {
            this.clientConnectionInfoArgs = clientConnectionInfoArgs;
            this.requestPackage = requestPackage;
        }
    }
}
