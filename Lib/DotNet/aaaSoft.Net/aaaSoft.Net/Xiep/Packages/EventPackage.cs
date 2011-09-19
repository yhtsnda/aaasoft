using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Helpers;

namespace aaaSoft.Net.Xiep.Packages
{
    public class EventPackage : AbstractXiepPackage
    {
        public override string GetPackageName()
        {
            return "EventPackage";
        }

        //获取或设置此请求包的Event
        public String Event
        {
            get { return this.GetAttributeValue("Event"); }
            set { this.SetAttribute("Event", value); }
        }
        
        //构造函数
        public EventPackage()
        {
        }

        //构造函数
        public EventPackage(String eventName)
        {
            init(eventName);
        }

        private void init(String eventName)
        {
            this.Event = eventName;
        }

        //从XML得到事件包
        public static EventPackage FromXml(String xml)
        {
            EventPackage rtnPackage = null;

            XmlTreeNode treeNode = XmlTreeNode.FromXml(xml);
            String packageName = treeNode.Key;
            if (packageName.Equals("EventPackage"))
            {
                rtnPackage = new EventPackage();
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