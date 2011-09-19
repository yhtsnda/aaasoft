using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aaaSoft.Helpers
{
    public class IoHelper
    {
        #region 得到父路径
        /// <summary>
        /// 得到父路径
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="spStr">路径分隔符</param>
        /// <returns></returns>
        public static String GetParentPath(String Path, Char spStr)
        {
            var strs = Path.Split(spStr);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= strs.Length - 1 - 1; i++)
            {
                if (String.IsNullOrEmpty(strs[i])) continue;
                sb.Append(spStr);
                sb.Append(strs[i]);
            }
            if (sb.Length == 0)
                sb.Append(spStr);
            return sb.ToString();
        }
        #endregion

        #region 得到文件或目录名
        /// <summary>
        /// 得到文件或目录名
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="spStr">分隔符</param>
        /// <returns></returns>
        public static String GetFileOrFolderName(String Path, Char spStr)
        {
            var strs = Path.Split(spStr);
            for (int i = strs.Length - 1; i >= 0; i--)
            {
                if (String.IsNullOrEmpty(strs[i])) continue;
                return strs[i];
            }
            return String.Empty;
        }
        #endregion

        #region 创建多级目录
        public static Boolean CreateMultiFolder(string folderName)
        {
            try
            {
                if (Directory.Exists(folderName))
                {
                    return true;
                }

                string pDirName = Path.GetDirectoryName(folderName);

                //如果父目录存在 
                if (!Directory.Exists(pDirName))
                {
                    if (!CreateMultiFolder(pDirName))
                    {
                        return false;
                    }
                }

                Directory.CreateDirectory(folderName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        
        #region 复制目录
        public static void CopyFolder(String srcPath, String desPath)
        {
            //创建目的目录
            CreateMultiFolder(desPath);

            //先复制文件
            DirectoryInfo srcDirInfo = new DirectoryInfo(srcPath);
            if (!srcDirInfo.Exists)
                return;
            FileInfo[] fileInfoArray = srcDirInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfoArray)
            {
                fileInfo.CopyTo(Path.Combine(desPath, fileInfo.Name));
            }

            //然后复制子目录
            DirectoryInfo[] directoryInfoArray = srcDirInfo.GetDirectories();
            foreach (DirectoryInfo directoryInfo in directoryInfoArray)
            {
                CopyFolder(directoryInfo.FullName, Path.Combine(desPath, directoryInfo.Name));
            }
        }
        #endregion

        #region 复制流

        #region 复制流(未知要复制的数据大小，不提供进度报告，适用于FileStream，MemoryStream；不适用于NetworkStream)
        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">输入流</param>
        /// <param name="output">输出流</param>
        public static void CopyStream(Stream input, Stream output)
        {
            int bufferSize = 4096;
            CopyStream(input, output, bufferSize);
        }

        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">输入流</param>
        /// <param name="output">输出流</param>
        /// <param name="bufferSize">缓冲区大小</param>
        public static void CopyStream(Stream input, Stream output, Int32 bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    return;
                }
                output.Write(buffer, 0, read);
            }
        }
        #endregion

        #region 复制流(已知要复制的数据大小，提供进度报告，适用于所有Stream)

        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">输入流</param>
        /// <param name="output">输出流</param>
        /// <param name="totalLength">总大小</param>
        public static void CopyStream(Stream input, Stream output, Int64 totalLength)
        {
            Int64 position = 0;
            CopyStream(input, output, totalLength, ref position);
        }

        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">输入流</param>
        /// <param name="output">输出流</param>
        /// <param name="totalLength">总大小</param>
        /// <param name="position">已复制数据大小</param>
        public static void CopyStream(Stream input, Stream output, Int64 totalLength, ref Int64 position)
        {
            int bufferSize = 4096;
            CopyStream(input, output, bufferSize, totalLength, ref position);
        }


        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="input">输入流</param>
        /// <param name="output">输出流</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="totalLength">要复制数据的总大小</param>
        /// <param name="transferedLength">已经传输的数据长度</param>
        public static void CopyStream(Stream input, Stream output, Int32 bufferSize, Int64 totalLength, ref Int64 transferedLength)
        {
            Int64 position = 0;
            byte[] buffer = new byte[bufferSize];
            do
            {
                var CurrentReadSize = bufferSize;
                if ((totalLength - position) < bufferSize)
                {
                    CurrentReadSize = Convert.ToInt32(totalLength - position);
                }

                int readCount = input.Read(buffer, 0, CurrentReadSize);

                output.Write(buffer, 0, readCount);
                position += readCount;
                transferedLength += readCount;
            } while (position < totalLength);
        }
        #endregion

        #endregion
    }
}
