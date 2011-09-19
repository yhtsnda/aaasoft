using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using aaaSoft.Helpers;
using System.Diagnostics;
using aaaSoft.Net.Xiep.Packages;
using System.IO;

namespace aaaSoft.Net.Xiep.Helpers
{
    /// <summary>
    /// XIEP协议的输入输出辅助类
    /// </summary>
    public class XiepIoHelper
    {
        //传输编码
        public static String transferEncoding = "utf-8";
        //最大接收包大小
        public static int maxReceivePackageSize = 10 * 1024 * 1024;

        //得到Encoding对象
        private static Encoding getEncoding()
        {
            Encoding encoding = new UTF8Encoding(false);
            try
            {
                encoding = Encoding.GetEncoding(transferEncoding);
            }
            catch
            {
            }
            return encoding;
        }

        /// <summary>
        /// 发送XmlTreeNode包
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool SendPackage(NetworkStream ns, AbstractXiepPackage xiepPackage)
        {
            if (ns == null || xiepPackage == null)
                return false;

            lock (ns)
            {
                String responseNodeString = xiepPackage.ToXml();
                try
                {
                    Byte[] buffer = XiepIoHelper.getEncoding().GetBytes(responseNodeString);
                    //写数据包大小
                    Byte[] packageSizeBuffer = NumberHelper.intToByte(buffer.Length);
                    ns.Write(packageSizeBuffer, 0, packageSizeBuffer.Length);
                    //写数据包内容
                    ns.Write(buffer, 0, buffer.Length);
                    ns.Flush();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static String ReceiveXml(NetworkStream ns)
        {
            try
            {
                int packageSizeNumberBytesCount = 4;
                byte[] packageSizeByteArray = new byte[packageSizeNumberBytesCount];
                //读取包大小
                while (true)
                {
                    int readPackageSizeCount = ns.Read(packageSizeByteArray, 0, packageSizeNumberBytesCount);
                    if (readPackageSizeCount <= 0)
                    {
                        throw new IOException(String.Format("readPackageSizeCount为{0}！", readPackageSizeCount));
                    }
                    if (readPackageSizeCount != packageSizeNumberBytesCount)
                    {
                        Debug.Print("readPackageSizeCount:" + readPackageSizeCount);
                        continue;
                    }
                    break;
                }

                int packageSize = NumberHelper.bytesToInt(packageSizeByteArray);

                if (packageSize <= 0)
                {
                    throw new IOException("包大小不能为负数，已丢弃！", maxReceivePackageSize);
                }
                if (packageSize > maxReceivePackageSize)
                {
                    throw new IOException(String.Format("包大小超过 {0}，已丢弃！", maxReceivePackageSize));
                }

                MemoryStream ms = new MemoryStream(packageSize);
                //读取数据
                IoHelper.CopyStream(ns, ms, Convert.ToInt64(packageSize));
                ms.Position = 0;

                return XiepIoHelper.getEncoding().GetString(ms.ToArray());
            }
            catch
            {
                return null;
            }
        }

        // 接收AbstractXiepPackage包
        public static AbstractXiepPackage ReceivePackage(NetworkStream ns)
        {

            String xml = ReceiveXml(ns);
            if (String.IsNullOrEmpty(xml))
            {
                return null;
            }

            AbstractXiepPackage rtnPackage = null;

            XmlTreeNode treeNode = XmlTreeNode.FromXml(xml);
            String packageName = treeNode.Key;

            if (packageName.Equals("RequestPackage"))
            {
                rtnPackage = new RequestPackage();
            }
            else if (packageName.Equals("ResponsePackage"))
            {
                rtnPackage = new ResponsePackage();
            }
            else if (packageName.Equals("EventPackage"))
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
