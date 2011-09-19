using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace aaaSoft.Net.Ftp.ListAnalyzer
{
    public class ServuFtpListAnalyzer : FtpListAnalyzer
    {
        public override FtpBaseFileInfo AnalyzeLine(String Line)
        {
            String FileName = "";
            Int64 FileSize = -1;
            bool IsFolder = false;
            String FileProperty = "";
            DateTime FileLastWriteTime = DateTime.MinValue;
            try
            {
                //文件属性
                FileProperty = Line.Substring(0, 10).Trim();
                //文件大小
                Int64.TryParse(Line.Substring(29, 12), out FileSize);
                //修改时间
                String DateTimeStirng = Line.Substring(42, 12).Trim();
                DateTimeStirng = String.Format("{0} {1} {2}", DateTimeStirng.Substring(0, 6), DateTime.Now.Year, DateTimeStirng.Substring(7));

                DateTime.TryParse(DateTimeStirng, out FileLastWriteTime);
                //文件名称
                FileName = Line.Substring(55).Trim();

                //是否是目录
                IsFolder = FileProperty.StartsWith("d");

                return base.GetFtpBaseFileInfo(FileName, FileSize, IsFolder, FileProperty, FileLastWriteTime);
            }
            catch (Exception ex)
            {
                Debug.Print("Error From aaaSoft.Ftp.FtpClient.AnalyzeList_ServUFtpServer(),Reason:" + ex.Message);
                return null;
            }
        }
    }
}
