using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Helpers;

namespace aaaSoft.Net.Xiep.Packages
{
    public abstract class AbstractXiepPackage
    {
        private XmlTreeNode rootPackage;

        /// <summary>
        /// 设置根包
        /// </summary>
        /// <param name="rootPackage">根包</param>
        public void SetRootPackage(XmlTreeNode rootPackage)
        {
            this.rootPackage = rootPackage;
        }

        //设置属性
        public void SetAttribute(String key, String value)
        {
            rootPackage.SetAttribute(key, value);
        }

        //获取属性的值
        public String GetAttributeValue(String key)
        {
            return rootPackage.GetAttributeValue(key);
        }

        //移除包属性
        public void RemoveAttribute(String key)
        {
            rootPackage.RemoveAttribute(key);
        }

        //得到参数列表
        public List<XmlTreeNode> GetArguments()
        {
            return rootPackage.GetItems();
        }


        //得到参数节点
        public XmlTreeNode GetArgument(String path)
        {
            return rootPackage.GetItem(path);
        }

        //根据path得到参数的值
        public String GetArgumentValue(String path)
        {
            return rootPackage.GetItemValue(path);
        }

        //添加参数
        public XmlTreeNode AddArgument(String key, String value)
        {
            return rootPackage.AddItem(key, value);
        }

        //添加参数
        public XmlTreeNode AddArgument(String key, XmlTreeNode node)
        {
            return rootPackage.AddItem(key, node);
        }

        //得到包类型名称(抽象函数，由实现类实现。用来作为XML根节点名称)
        public abstract String GetPackageName();

        //构造函数
        public AbstractXiepPackage()
        {
            init();
        }

        //初始化
        private void init()
        {
            rootPackage = new XmlTreeNode(GetPackageName());
        }

        // 输出XML
        public String ToXml()
        {
            return rootPackage.ToXml();
        }

        // 输出XML
        public String ToXml(Encoding encoding)
        {
            return rootPackage.ToXml(encoding);
        }
    }
}
