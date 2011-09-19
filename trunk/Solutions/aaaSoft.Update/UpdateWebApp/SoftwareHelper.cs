using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace UpdateWebApp
{
    /// <summary>
    ///SoftwareHelper 的摘要说明
    /// </summary>
    public class SoftwareHelper
    {
        String RootPath;
        public SoftwareHelper(String rootPath)
        {
            RootPath = rootPath;
        }

        #region 获取软件列表XML字符串
        public String GetSoftwareListXml()
        {
            List<DirectoryInfo> softList = new List<DirectoryInfo>();
            DirectoryInfo[] dirs = GetAllDirectoryInfos();
            foreach (DirectoryInfo dir in dirs)
            {
                if (File.Exists(Path.Combine(dir.FullName, "SoftwareInfo.xml")))
                {
                    softList.Add(dir);
                }
            }

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("SoftwareList"); // 创建根节点
            doc.AppendChild(root);

            //添加XML节点
            foreach (DirectoryInfo dir in softList)
            {
                XmlNode soft = GetSoftwareSimpleInfoXmlNode(doc, dir);
                if (soft != null)
                {
                    root.AppendChild(soft);
                }
            }
            return doc.OuterXml;
        }
        #endregion

        #region 获取软件简单信息XML字符串节点
        public XmlNode GetSoftwareSimpleInfoXmlNode(XmlDocument doc, DirectoryInfo dir)
        {
            try
            {
                XmlDocument softInfoDoc = new XmlDocument();
                softInfoDoc.Load(Path.Combine(dir.FullName, "SoftwareInfo.xml"));
                XmlNode softwareInfoNode = softInfoDoc.DocumentElement;

                XmlElement software = doc.CreateElement("Software");
                {
                    //主执行文件名称
                    String softwareMainExeFileName = softwareInfoNode.SelectSingleNode("MainExecuteFile").InnerText;
                    //主执行文件全路径
                    String softwareMainExeFileFullPath = Path.Combine(dir.FullName, softwareMainExeFileName);
                    //主执行文件FileInfo对象
                    FileInfo softwareMainExeFileInfo = new FileInfo(softwareMainExeFileFullPath);

                    //名称
                    XmlElement name = doc.CreateElement("Name");
                    {
                        name.InnerText = softwareInfoNode.SelectSingleNode("Name").InnerText;
                    }
                    software.AppendChild(name);

                    //描述
                    XmlElement description = doc.CreateElement("Description");
                    {
                        description.InnerText = softInfoDoc.DocumentElement.SelectSingleNode("Description").InnerText;
                    }
                    software.AppendChild(description);

                    //版本
                    XmlElement version = doc.CreateElement("Version");
                    {
                        version.InnerText = GetFileVersion(softwareMainExeFileInfo);
                    }
                    software.AppendChild(version);

                    //大小
                    XmlElement size = doc.CreateElement("Size");
                    {
                        size.InnerText = GetSoftwareSize(softInfoDoc).ToString();
                    }
                    software.AppendChild(size);

                    //是否有Logo
                    XmlElement hasLogo = doc.CreateElement("HasLogo");
                    {
                        String logoFileFullPath = Path.Combine(dir.FullName, "Logo.png");
                        hasLogo.InnerText = File.Exists(logoFileFullPath).ToString();
                    }
                    software.AppendChild(hasLogo);

                    //更新时间
                    XmlElement UpdateTime = doc.CreateElement("UpdateTime");
                    {
                        UpdateTime.InnerText = softwareMainExeFileInfo.LastWriteTime.ToString();
                    }
                    software.AppendChild(UpdateTime);
                }
                return software;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取软件信息XML字符串
        public String GetSoftwareInfoXml(String softwareName, out String ErrMsg)
        {
            ErrMsg = String.Empty;
            try
            {
                DirectoryInfo dir = GetSoftwareDirectoryInfo(softwareName);
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(dir.FullName, "SoftwareInfo.xml"));
                XmlElement root = doc.DocumentElement;

                //主执行文件文件信息
                FileInfo MainExeFileInfo = new FileInfo(Path.Combine(dir.FullName, root.SelectSingleNode("MainExecuteFile").InnerText));

                //主执行文件版本节点
                XmlElement softwareMainExeFileVerNode = doc.CreateElement("MainExecuteFileVersion");
                {
                    softwareMainExeFileVerNode.InnerText = GetFileVersion(MainExeFileInfo);
                }
                root.AppendChild(softwareMainExeFileVerNode);

                //主执行文件大小
                XmlElement softwareMainExeFileSizeNode = doc.CreateElement("MainExecuteFileSize");
                {
                    softwareMainExeFileSizeNode.InnerText = MainExeFileInfo.Length.ToString();
                }
                root.AppendChild(softwareMainExeFileSizeNode);

                //组件版本
                XmlNodeList subAssemblyNodes = root.SelectNodes("SubAssemblys/SubAssembly");
                foreach (XmlNode subAssemblyNode in subAssemblyNodes)
                {
                    String tmpFileName = subAssemblyNode.SelectSingleNode("FileName").InnerText;
                    FileInfo tmpFileInfo = GetFileInfo(tmpFileName, RootPath, SearchOption.AllDirectories);

                    if (tmpFileInfo == null)
                    {
                        throw new IOException(String.Format("未找到文件名为[{0}]的文件", tmpFileName));
                    }
                    //版本
                    XmlElement subAssemblyVersionNode = doc.CreateElement("Version");
                    {
                        String tmpVersion = GetFileVersion(tmpFileInfo);
                        subAssemblyVersionNode.InnerText = tmpVersion;
                    }
                    subAssemblyNode.AppendChild(subAssemblyVersionNode);

                    //大小
                    XmlElement subAssemblyFileSizeNode = doc.CreateElement("FileSize");
                    {
                        subAssemblyFileSizeNode.InnerText = tmpFileInfo.Length.ToString();
                    }
                    subAssemblyNode.AppendChild(subAssemblyFileSizeNode);
                }

                return doc.OuterXml;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return null;
            }
        }
        #endregion

        #region 获取软件大小
        public long GetSoftwareSize(XmlDocument softInfoDoc)
        {
            long totalSize = 0;
            String MainExeFileName = softInfoDoc.DocumentElement.SelectSingleNode("MainExecuteFile").InnerText;
            FileInfo tmpFileInfo = GetFileInfo(MainExeFileName, RootPath, SearchOption.AllDirectories);
            totalSize += tmpFileInfo.Length;

            XmlNodeList SubAssemblyNodes = softInfoDoc.DocumentElement.SelectNodes("SubAssemblys/SubAssembly");
            foreach (XmlNode tmpNode in SubAssemblyNodes)
            {
                String tmpFileName = tmpNode.SelectSingleNode("FileName").InnerText;
                tmpFileInfo = GetFileInfo(tmpFileName, RootPath, SearchOption.AllDirectories);
                if (tmpFileInfo == null) continue;
                totalSize += tmpFileInfo.Length;
            }
            return totalSize;
        }

        private long GetSoftwareSize(DirectoryInfo softwareDir)
        {
            if (softwareDir == null) return -1;
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(softwareDir.FullName, "SoftwareInfo.xml"));
            return GetSoftwareSize(doc);
        }

        private long GetSoftwareSize(String softwareName)
        {
            DirectoryInfo dir = GetSoftwareDirectoryInfo(softwareName);
            return GetSoftwareSize(dir);
        }
        #endregion

        #region 获取软件目录信息
        public DirectoryInfo GetSoftwareDirectoryInfo(String softwareName)
        {
            DirectoryInfo[] dirs = GetAllDirectoryInfos();
            foreach (DirectoryInfo dir in dirs)
            {
                String xmlFileName = Path.Combine(dir.FullName, "SoftwareInfo.xml");
                //如果该目录没有软件信息XML文件
                if (!File.Exists(xmlFileName)) continue;

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFileName);
                XmlElement root = doc.DocumentElement;

                XmlNode NameNode = root.SelectSingleNode("Name");
                if (NameNode == null) continue;

                if (NameNode.InnerText == softwareName)
                {
                    return dir;
                }
            }
            return null;
        }
        #endregion

        #region 获取所有目录信息
        public DirectoryInfo[] GetAllDirectoryInfos()
        {
            DirectoryInfo updateDir = new DirectoryInfo(RootPath);
            return updateDir.GetDirectories("*", SearchOption.AllDirectories);
        }
        #endregion

        #region 获取文件信息

        public FileInfo GetFileInfo(String searchPattern, String FolderPath, SearchOption searchOption)
        {
            DirectoryInfo updateDir = new DirectoryInfo(FolderPath);
            FileInfo[] files = updateDir.GetFiles(searchPattern, searchOption);
            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 得到文件版本
        /// <summary>
        /// 得到文件版本
        /// </summary>
        /// <param name="file">文件信息对象</param>
        /// <returns></returns>
        public String GetFileVersion(FileInfo file)
        {
            String version;
            if (file.Extension.ToUpper().Equals(".DLL") || file.Extension.ToUpper().Equals(".EXE"))
            {
                version = GetPeFileVersion(file.FullName);
            }
            else
            {
                DateTime modifyTime = file.LastWriteTime;
                version = String.Format("{0}.{1}.{2}.{3}", modifyTime.Year, modifyTime.Month, modifyTime.Day, (int)modifyTime.TimeOfDay.TotalSeconds);
            }
            return version;
        }
        #endregion

        #region 获取PE文件版本号
        public static String GetPeFileVersion(String FileName)
        {
            try
            {
                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(FileName);
                return ver.ProductVersion;
            }
            catch
            {
                FileStream f = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                int[] FileVersionHead = new int[] { 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x56, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x00 };


                byte[] buffer = new byte[32];

                int p = 0;
                while (true)
                {
                    int tmpInt = f.ReadByte();
                    if (tmpInt == FileVersionHead[p])
                    {
                        p += 1;
                    }
                    else
                    {
                        p = 0;
                    }
                    if (p == FileVersionHead.Length)
                    {
                        f.Read(buffer, 0, buffer.Length);
                        break;
                    }
                    if (f.Position == f.Length)
                    {
                        break;
                    }
                }
                f.Close();

                String tmpVersionString = System.Text.Encoding.ASCII.GetString(buffer).Replace("\0", "");

                Char[] charArray = tmpVersionString.ToCharArray();
                for (int i = 0; i <= charArray.Length - 1; i++)
                {
                    if ((charArray[i] >= '0' && charArray[i] <= '9') || charArray[i] == '.')
                    {
                    }
                    else
                    {
                        tmpVersionString = tmpVersionString.Substring(0, i);
                        break;
                    }
                }
                return tmpVersionString;
            }
        }
        #endregion
    }
}