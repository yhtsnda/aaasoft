using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Net.Ftp.ListAnalyzer
{
    /// <summary>
    /// FTP的LIST命令得到的文件列表分析器接口
    /// </summary>
    public abstract class FtpListAnalyzer
    {
        /// <summary>
        /// 分析行字符串
        /// </summary>
        /// <param name="LineString">行字符串</param>
        /// <returns></returns>
        public abstract FtpBaseFileInfo AnalyzeLine(String Line);

        public static FtpListAnalyzer GetFtpListAnalyzer(FtpClient site)
        {
            //如果使用的是MLSD命令
            if (site.IsUseMlsdToListFolder && site.IsSupportMLSD)
            {
                return new MlsdLineAnalyzer();
            }
            //如果使用的是LIST命令
            else
            {
                if (site.FtpServerWelcomeText.Contains("Serv-U FTP Server"))
                {
                    return new ServuFtpListAnalyzer();
                }
                else if (site.FtpServerWelcomeText.Contains("Microsoft FTP Service"))
                {
                    return new MicrosoftFtpListAnalyzer();
                }
            }
            return new ServuFtpListAnalyzer();
        }

        public FtpBaseFileInfo GetFtpBaseFileInfo(String FileName, Int64 FileSize, bool IsFolder, String FileProperty, DateTime FileLastWriteTime)
        {
            //过滤
            if (FileName == "." ||
                FileName == "..")
                return null;

            FtpBaseFileInfo baseFileInfo;
            baseFileInfo = new FtpBaseFileInfo();
            if (!IsFolder)
                baseFileInfo.Length = FileSize;
            baseFileInfo.IsFolder = IsFolder;
            baseFileInfo.Name = FileName;
            baseFileInfo.Property = FileProperty;
            baseFileInfo.LastModifyTime = FileLastWriteTime;

            return baseFileInfo;
        }
    }
}
