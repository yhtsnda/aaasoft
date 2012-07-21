using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace aaaSoft.Helpers
{
    public static class SerializeHelper
    {
        #region XML序列化对象为字节数组
        private static System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
        /// <summary>
        /// XML序列化对象为字节数组
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns></returns>
        public static byte[] XmlSerializeObject(object obj)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                MemoryStream ms = new MemoryStream();
                xs.Serialize(ms, obj);
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region XML反序列化字节数组为对象
        /// <summary>
        /// XML反序列化字节数组为对象
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="objType">对象类型</param>
        /// <returns></returns>
        public static object XmlDeserializeObject(byte[] buffer, Type objType)
        {
            return XmlDeserializeObject(buffer, objType, false);
        }
        /// <summary>
        /// XML反序列化字节数组为对象
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="objType">对象类型</param>
        /// <param name="IsCheckTypeName">是否检查类型名称</param>
        /// <returns></returns>
        public static object XmlDeserializeObject(byte[] buffer, Type objType, Boolean IsCheckTypeName)
        {
            Encoding encoding = Encoding.UTF8;

            String newStr;
            try
            {
                string xmlHeaderStr = "<?xml";
                byte[] xmlHeaderArray = encoding.GetBytes(xmlHeaderStr);
                for (int i = 0; i <= xmlHeaderArray.Length - 1; i++)
                {
                    if (xmlHeaderArray[i] != buffer[i])
                    {
                        return null;
                    }
                }

                lock (xd)
                {
                    newStr = encoding.GetString(buffer);
                    xd.LoadXml(newStr);
                    if (IsCheckTypeName)
                    {
                        if (objType.Name.ToString().ToUpper() != xd.DocumentElement.Name.ToUpper())
                        {
                            return null;
                        }
                    }
                    XmlSerializer xs = new XmlSerializer(objType);
                    MemoryStream ms = new MemoryStream(buffer);
                    return xs.Deserialize(ms);
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 二进制序列化对象为字节数组
        /// <summary>
        /// 二进制序列化对象为字节数组
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns></returns>
        public static byte[] BinarySerializeObject(object obj)
        {
            try
            {
                MemoryStream mStream = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(mStream, obj);
                mStream.Position = 0;
                return mStream.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 二进制反序列化字节数组为对象
        /// <summary>
        /// 二进制反序列化字节数组为对象
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns></returns>
        public static object BinaryDeserializeObject(byte[] buffer)
        {
            try
            {
                MemoryStream mStream;
                mStream = new MemoryStream(buffer);
                return BinaryDeserializeObject(mStream);
            }
            catch
            {
                return null;
            }
        }

        public static object BinaryDeserializeObject(MemoryStream ms)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                ms.Position = 0;
                return bf.Deserialize(ms);
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
