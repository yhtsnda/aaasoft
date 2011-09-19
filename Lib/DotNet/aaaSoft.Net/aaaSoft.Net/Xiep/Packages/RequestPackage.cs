using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Helpers;

namespace aaaSoft.Net.Xiep.Packages
{
    public class RequestPackage : AbstractXiepPackage
    {
        public override string GetPackageName()
        {
            return "RequestPackage";
        }

        //获取或设置请求编号
        public String RequestId
        {
            get { return this.GetAttributeValue("RequestId"); }
            set { this.SetAttribute("RequestId", value); }
        }

        //获取或设置此请求包的Request
        public String Request
        {
            get { return this.GetAttributeValue("Request"); }
            set { this.SetAttribute("Request", value); }
        }

        //构造函数
        public RequestPackage()
        {
            this.init(null, null);
        }

        //构造函数
        public RequestPackage(String request)
        {
            this.init(request, null);
        }

        //构造函数
        public RequestPackage(String request, Dictionary<String, String> mapArgs)
        {
            this.init(request, mapArgs);
        }

        private void init(String request, Dictionary<String, String> mapArgs)
        {
            RequestId = Guid.NewGuid().ToString();

            //设置请求
            if (!String.IsNullOrEmpty(request))
            {
                this.Request = request;
            }
            //添加参数
            if (mapArgs != null)
            {
                foreach (String key in mapArgs.Keys)
                {
                    this.AddArgument(key, mapArgs[key]);
                }
            }
        }

        //从XML得到请求包
        public static RequestPackage fromXml(String xml)
        {
            RequestPackage rtnPackage = null;

            XmlTreeNode treeNode = XmlTreeNode.FromXml(xml);
            String packageName = treeNode.Key;
            if (packageName.Equals("RequestPackage"))
            {
                rtnPackage = new RequestPackage();
            }
            else
            {
                return null;
            }

            rtnPackage.SetRootPackage(treeNode);
            return rtnPackage;
        }
    }
}
