using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Xiep.Packages;
using aaaSoft.Net.Xiep.EventArgs;

namespace aaaSoft.Net.Xiep.Handlers
{
    /**
     * 心跳请求处理器
     * @author aaa
     */
    public class XiepPingRequestHandler : AbstractRequestHandler
    {

        public override ResponsePackage execute(ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            responsePackage.Response = "XiepPong";
            return responsePackage;
        }
    }
}
