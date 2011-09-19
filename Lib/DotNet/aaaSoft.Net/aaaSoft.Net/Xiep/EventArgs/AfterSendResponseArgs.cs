using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Xiep.Packages;

namespace aaaSoft.Net.Xiep.EventArgs
{
    public class AfterSendResponseArgs : System.EventArgs
    {

        private ClientConnectionInfoArgs clientConnectionInfoArgs;
        private RequestPackage requestPackage;
        private ResponsePackage responsePackage;

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

        //构造函数
        public AfterSendResponseArgs(ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage, ResponsePackage responsePackage)
        {
            this.clientConnectionInfoArgs = clientConnectionInfoArgs;
            this.requestPackage = requestPackage;
            this.responsePackage = responsePackage;
        }
    }
}
