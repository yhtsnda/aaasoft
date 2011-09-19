using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Xiep.Packages;
using aaaSoft.Net.Xiep.EventArgs;

namespace aaaSoft.Net.Xiep.Handlers
{
    /// <summary>
    /// 抽象请求处理器
    /// </summary>
    public abstract class AbstractRequestHandler
    {
        public abstract ResponsePackage execute(ClientConnectionInfoArgs clientConnectionInfoArgs, RequestPackage requestPackage);
    }
}
