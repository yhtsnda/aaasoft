using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace aaaSoft.Helpers
{
    public class XmlTreeNode
    {
        private String _key;
        private String value;
        private List<XmlTreeNode> items;
        private Dictionary<String, String> attribs;


        /// <summary>
        /// 获取或设置结点名称
        /// </summary>
        public String Key
        {
            get { return _key; }
            set { this._key = value; }
        }
                
        /// <summary>
        /// 获取或设置结点的值(Value与Items同时只能有一个有效)
        /// </summary>
        public String Value
        {
            get
            {
                if (String.IsNullOrEmpty(value))
                {
                    return String.Empty;
                }
                return value;
            }
            set
            {
            if (value == null)
            {
                this.value = String.Empty;
            }
            else
            {
                this.value = value;
            }
            }
        }        

        
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public XmlTreeNode()
            : this(String.Empty, String.Empty)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Key">结点名称</param>
        public XmlTreeNode(String Key)
            : this(Key, String.Empty)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">结点名称</param>
        /// <param name="value">结点值</param>
        public XmlTreeNode(String key, String value)
        {
            this._key = key;
            this.value = value;
            items = new List<XmlTreeNode>();
            attribs = new Dictionary<string, string>();
        }
        #endregion

        #region 根据Key得到属性
        public String GetAttributeValue(String key)
        {
            foreach (var attrib in attribs)
            {
                if (attrib.Key == key)
                    return attrib.Value;
            }
            return null;
        }
        #endregion

        #region 添加属性
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAttribute(String key, String value)
        {
            if (attribs.ContainsKey(key))
                attribs.Remove(key);
            attribs.Add(key, value);
        }
        #endregion

        //移除属性
        public void RemoveAttribute(String key)
        {
            attribs.Remove(key);
        }

        #region 根据Key得到对象
        /// <summary>
        /// 根据Key得到其子对象
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>
        private XmlTreeNode GetChildItem(String key)
        {
            foreach (var item in items)
            {
                if (item._key == key)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据Key得到其子对象列表
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>
        private List<XmlTreeNode> GetChildItems(String key)
        {
            List<XmlTreeNode> lstNodes = new List<XmlTreeNode>();
            foreach (var item in items)
            {
                if (item._key == key)
                {
                    lstNodes.Add(item);
                }
            }
            return lstNodes;
        }

        /// <summary>
        /// 根据路径得到对象
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns></returns>
        public XmlTreeNode GetItem(String Path)
        {
            if (String.IsNullOrEmpty(Path))
                return this;

            XmlTreeNode TmpTn = this;

            var TmpKeys = Path.Split('/');
            foreach (var tmpKey in TmpKeys)
            {
                String tmpKeyStr = tmpKey.Trim();
                if (String.IsNullOrEmpty(tmpKeyStr)) continue;

                TmpTn = TmpTn.GetChildItem(tmpKeyStr);
                if (TmpTn == null) return null;
            }
            return TmpTn;
        }

        /// <summary>
        /// 得到对象集合
        /// </summary>
        /// <returns></returns>
        public List<XmlTreeNode> GetItems()
        {
            return this.items;
        }

        /// <summary>
        /// 根据路径得到对象集合
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns></returns>
        public List<XmlTreeNode> GetItems(String Path)
        {
            var ParentNode = this;
            var TmpKeys = Path.Split('/');
            var LastKey = TmpKeys[TmpKeys.Length - 1];
            if (TmpKeys.Length > 1)
            {
                var ParentPath = Path.Substring(0, Path.Length - (LastKey.Length + 1));
                ParentNode = GetItem(ParentPath);
                if (ParentNode == null)
                    return new List<XmlTreeNode>();
            }
            return ParentNode.GetChildItems(LastKey);
        }

        /// <summary>
        /// 根据路径得到对象的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public String GetItemValue(String path)
        {
            var TmpTn = GetItem(path);
            if (TmpTn == null) return null;
            return TmpTn.value;
        }
        #endregion

        #region 添加子结点
        /// <summary>
        /// 添加子结点
        /// </summary>
        /// <param name="tn">TreeNode对象</param>
        /// <returns>TreeNode对象</returns>
        public XmlTreeNode AddItem(XmlTreeNode tn)
        {
            items.Add(tn);
            return tn;
        }
        /// <summary>
        /// 添加子结点
        /// </summary>
        /// <param name="Key">结点名称</param>
        /// <returns>TreeNode对象</returns>
        public XmlTreeNode AddItem(String Key)
        {
            return AddItem(Key, String.Empty);
        }
        /// <summary>
        /// 添加子结点
        /// </summary>
        /// <param name="key">结点名称</param>
        /// <param name="value">结点值</param>
        /// <returns>TreeNode对象</returns>
        public XmlTreeNode AddItem(String key, String value)
        {
            XmlTreeNode tn = new XmlTreeNode(key);
            tn.value = value;
            AddItem(tn);
            return tn;
        }

        /// <summary>
        /// 添加子结点
        /// </summary>
        /// <param name="key">结点名称</param>
        /// <param name="node">结点</param>
        /// <returns></returns>
        public XmlTreeNode AddItem(String key, XmlTreeNode node)
        {
            XmlTreeNode tn = new XmlTreeNode(key);
            tn.AddItem(node);
            AddItem(tn);
            return tn;
        }
        #endregion


        #region ToString方法
        /// <summary>
        /// 此方法仅供调试时查看对象的值，要生成XML请用GenerateXml方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToXml(this, Encoding.Default);
        }
        #endregion

        #region 生成XML
        /// <summary>
        /// 生成XML
        /// </summary>
        /// <param name="root">根结点</param>
        /// <returns></returns>
        public static String ToXml(XmlTreeNode root, Encoding encoding)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = new System.Xml.XmlTextWriter(ms, encoding);

            xw.WriteStartDocument();
            _WriteTreeNode(root, xw);
            xw.WriteEndDocument();

            xw.Flush();
            xw.Close();

            var xml = encoding.GetString(ms.ToArray());
            return xml;
        }

        private static void _WriteTreeNode(XmlTreeNode tn, XmlWriter xw)
        {
            xw.WriteStartElement(tn._key);
            //先写属性
            foreach (var attrib in tn.attribs)
            {
                xw.WriteStartAttribute(attrib.Key);
                xw.WriteValue(attrib.Value);
                xw.WriteEndAttribute();
            }
            //再写值或子结点
            if (tn.items.Count == 0)
            {
                String tmpValue = tn.value;
                if (tmpValue == null) tmpValue = String.Empty;
                xw.WriteValue(tmpValue);
            }
            else
            {
                foreach (var item in tn.items)
                {
                    _WriteTreeNode(item, xw);
                }
            }
            xw.WriteEndElement();
        }
        #endregion

        #region 从XML得到TreeNode对象

        public String ToXml()
        {
            return XmlTreeNode.ToXml(this, new UTF8Encoding(false));
        }

        public String ToXml(Encoding encoding)
        {
            return XmlTreeNode.ToXml(this, encoding);
        }


        /// <summary>
        /// 从XML得到TreeNode对象
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static XmlTreeNode FromXml(String xml)
        {
            XmlDocument document = new XmlDocument();
            try { document.LoadXml(xml); }
            catch { return null; }
            var XmlRoot = document.DocumentElement;
            return _fromXml(XmlRoot);
        }
        private static XmlTreeNode _fromXml(XmlNode XmlNode)
        {
            XmlTreeNode TnNode = new XmlTreeNode(XmlNode.Name);
            //先写属性
            foreach (XmlAttribute attrib in XmlNode.Attributes)
            {
                TnNode.SetAttribute(attrib.Name, attrib.Value);
            }
            //再写子结点
            if (XmlNode.ChildNodes.Count == 0)
            {
                String tmpValue = XmlNode.Value;
                if (tmpValue == null) tmpValue = String.Empty;
                TnNode.value = tmpValue;
            }
            else if (XmlNode.ChildNodes.Count == 1 && XmlNode.ChildNodes[0] is XmlText)
            {
                TnNode.value = XmlNode.ChildNodes[0].Value;
            }
            else
            {
                foreach (XmlNode childNode in XmlNode.ChildNodes)
                {
                    if (childNode is XmlElement)
                        TnNode.AddItem(_fromXml(childNode));
                }
            }
            return TnNode;
        }
        #endregion
    }
}
