using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Helpers;

namespace aaaSoft.Net.Xiep.Packages
{
    public class ResponsePackage:AbstractXiepPackage
    {
        public override string GetPackageName()
        {
            return "ResponsePackage";
        }

        //获取或设置请求编号
        public String RequestId
        {
            get { return this.GetAttributeValue("RequestId"); }
            set { this.SetAttribute("RequestId", value); }
        }

        //获取或设置响应
        public String Response
        {
            get { return this.GetAttributeValue("Response"); }
            set { this.SetAttribute("Response", value); }
        }

        //构造函数
        public ResponsePackage()
        {
        }

        //构造函数
        public ResponsePackage(String requestId)
        {
            init(requestId, null);
        }

        //构造函数
        public ResponsePackage(String requestId, String response)
        {
            init(requestId, response);
        }

        //构造函数
        public ResponsePackage(RequestPackage requestPackage)
        {
            init(requestPackage.RequestId, null);
        }

        //构造函数
        public ResponsePackage(RequestPackage requestPackage, String response)
        {
            init(requestPackage.RequestId, response);
        }

        private void init(String requestId, String response)
        {
            RequestId = requestId;
            if (String.IsNullOrEmpty(response))
            {
            }
            else
            {
                this.Response = response;
            }
        }

        //从XML得到响应包
        public static ResponsePackage fromXml(String xml)
        {
            ResponsePackage rtnPackage = null;

            XmlTreeNode treeNode = XmlTreeNode.FromXml(xml);
            String packageName = treeNode.Key;
            if (packageName.Equals("ResponsePackage"))
            {
                rtnPackage = new ResponsePackage();
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
