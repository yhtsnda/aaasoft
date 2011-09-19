namespace aaaSoft.Helpers
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Windows.Forms;
    using System.Drawing;

    public class XmlHelper
    {
        public static void ReadString(XmlDocument doc, String XPath, ref String strVar)
        {
            if (doc == null) return;
            XmlNode tmpNode = doc.DocumentElement.SelectSingleNode(XPath);
            if (tmpNode != null)
            {
                strVar = tmpNode.InnerText;
            }
        }
        public static void ReadBoolean(XmlDocument doc, String XPath, ref bool boolVar)
        {
            String strVar = String.Empty;
            ReadString(doc, XPath, ref strVar);
            if (!String.IsNullOrEmpty(strVar))
            {
                boolVar = bool.Parse(strVar);
            }
        }
        public static void ReadInt(XmlDocument doc, String XPath, ref int intVar)
        {
            String strVar = String.Empty;
            ReadString(doc, XPath, ref strVar);
            if (!String.IsNullOrEmpty(strVar))
            {
                intVar = int.Parse(strVar);
            }
        }
        public static void ReadColor(XmlDocument doc, String XPath, ref Color clrVar)
        {
            String strVar = String.Empty;
            ReadString(doc, XPath, ref strVar);
            if (!String.IsNullOrEmpty(strVar))
            {
                try
                {
                    String[] args = strVar.Split(',');
                    switch (args.Length)
                    {
                        case 1:
                            clrVar = Color.FromArgb(Convert.ToInt32(args[0]));
                            break;
                        case 3:
                            clrVar = Color.FromArgb(
                                Convert.ToInt32(args[0]),
                                Convert.ToInt32(args[1]),
                                Convert.ToInt32(args[2])
                                );
                            break;
                        case 4:
                            clrVar = Color.FromArgb(
                                Convert.ToInt32(args[0]),
                                Convert.ToInt32(args[1]),
                                Convert.ToInt32(args[2]),
                                Convert.ToInt32(args[3])
                                );
                            break;
                    }
                }
                catch { }
            }
        }
        public static void ReadSize(XmlDocument doc, String XPath, ref Size szVar)
        {
            String strVar = String.Empty;
            ReadString(doc, XPath, ref strVar);
            if (!String.IsNullOrEmpty(strVar))
            {
                try
                {
                    String[] args = strVar.Split(',');
                    if (args.Length == 2)
                    {
                        szVar = new Size(
                            Convert.ToInt32(args[0]),
                            Convert.ToInt32(args[1])
                            );
                    }
                }
                catch { }
            }
        }
        public static void ReadStringAlignment(XmlDocument doc, String XPath, ref StringAlignment saVar)
        {
            String strVar = String.Empty;
            ReadString(doc, XPath, ref strVar);
            if (!String.IsNullOrEmpty(strVar))
            {
                try
                {
                    saVar = (StringAlignment)Enum.Parse(typeof(StringAlignment), strVar);
                }
                catch { }
            }
        }
        public static void ReadFont(XmlDocument doc, String XPath, ref Font ftVar)
        {
            String strVar = String.Empty;
            ReadString(doc, XPath, ref strVar);
            if (!String.IsNullOrEmpty(strVar))
            {
                try
                {
                    String[] args = strVar.Split(',');
                    switch (args.Length)
                    {
                        case 2:
                            ftVar = new Font(args[0], float.Parse(args[1]));
                            break;
                        case 3:
                            ftVar = new Font(args[0], float.Parse(args[1]), (FontStyle)Enum.Parse(typeof(FontStyle), args[2]));
                            break;
                    }
                }
                catch { }
            }
        }

        #region 根据TreeNode结构得到XmlDocument
        public static XmlDocument ReadTreeNode(TreeNode rootTN)
        {
            XmlDocument doc = new XmlDocument();
            ReadTreeNode(doc, rootTN);
            return doc;
        }

        //递归得到XML
        private static void ReadTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlDocument tmpDoc = xmlNode.OwnerDocument;
            if (xmlNode is XmlDocument) tmpDoc = (XmlDocument)xmlNode;

            XmlElement newXmlNode = tmpDoc.CreateElement(treeNode.Text);
            if (treeNode.Nodes.Count != 0)
            {
                //如果是支干                
                foreach (TreeNode tmpTN in treeNode.Nodes)
                {
                    ReadTreeNode(newXmlNode, tmpTN);
                }
            }
            else
            {
                //如果是叶子
                newXmlNode.InnerText = treeNode.Name;
            }
            xmlNode.AppendChild(newXmlNode);
        }
        #endregion
    }
}