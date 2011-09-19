namespace aaaSoft.Update.Helpers
{
    using System;
    using System.IO;

    internal class IoHelper
    {
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

        #region 得到文件大小字符串
        public static string getFileLengthLevel(long fileLength, int pointPostion)
        {
            float num = Convert.ToSingle(fileLength);
            if (fileLength < 0L)
            {
                return "未知";
            }
            if ((fileLength >= 0L) && (fileLength <= 0x3ffL))
            {
                return (num.ToString("N0") + " B");
            }
            if ((fileLength >= 0x400L) && (fileLength <= 0xfffffL))
            {
                float num2 = num / 1024f;
                return (num2.ToString("N" + pointPostion.ToString()) + " KB");
            }
            if ((fileLength >= 0x100000L) && (fileLength <= 0x3fffffffL))
            {
                float num3 = (num / 1024f) / 1024f;
                return (num3.ToString("N" + pointPostion.ToString()) + " MB");
            }
            float num4 = ((num / 1024f) / 1024f) / 1024f;
            return (num4.ToString("N" + pointPostion.ToString()) + " GB");
        }
        #endregion
    }
}

