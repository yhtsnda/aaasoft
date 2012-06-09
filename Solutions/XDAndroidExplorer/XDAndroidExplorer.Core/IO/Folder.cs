using System;
using System.Collections.Generic;
using System.Text;

namespace XDAndroidExplorer.Core.IO
{
    public class Folder : BaseFile
    {
        public Folder() { }
        public Folder(String folderName) { this.FullName = folderName; }
        public List<BaseFile> SubBaseFiles
        {
            get
            {
                return GetSubBaseFiles();
            }
        }

        public bool CreateFolder(String FolderName)
        {
            String cmdStr = String.Format("mkdir \"{0}/{1}\"", this.FullName, FolderName);
            String rtnStr = NativeMethod.ExecuteShellCommand(cmdStr);
            return String.IsNullOrEmpty(rtnStr);
        }

        private List<BaseFile> GetSubBaseFiles()
        {
            List<BaseFile> baseFileList = new List<BaseFile>();

            try
            {
                String result = NativeMethod.ExecuteShellCommand(String.Format("ls -l -e \"{0}\"", this.FullName));
                String[] lines = result.Split('\n');
                foreach (String line in lines)
                {
                    if (String.IsNullOrEmpty(line.Trim())) continue;
                    if (line.StartsWith("/")) continue;
                    if (line.StartsWith("ls:")) continue;
                    if (line.StartsWith("l:")) continue;

                    //文件属性
                    String fileProperty = line.Substring(0, 10).Trim();
                    //文件链接数
                    Int32 fileConnectionCount = Convert.ToInt32(line.Substring(10, 5).Trim());
                    //文件大小
                    Int64 fileSize = Convert.ToInt64(line.Substring(35, 7).Trim());
                    //文件修改时间
                    DateTime fileLastWriteTime = NativeMethod.ConvertLinuxTimeStringToDateTime(line.Substring(42, 25).Trim());
                    //文件名
                    String fileName = line.Substring(67).Trim();
                    if (fileName[0] < '0')
                    {
                        fileName = fileName.Substring(7, fileName.Length - 7 - 4);
                    }

                    //加上全路径
                    fileName = this.FullName + "/" + fileName;
                    fileName = fileName.Replace("//", "/");

                    baseFileList.Add(BaseFile.GetBaseFile(fileName, fileSize, fileLastWriteTime, fileProperty));
                }
            }
            catch { }
            return baseFileList;
        }
    }
}
