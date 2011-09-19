using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace aaaSoft.Net.Ftp.ListAnalyzer
{
    public class MicrosoftFtpListAnalyzer : FtpListAnalyzer
    {
        public override FtpBaseFileInfo AnalyzeLine(string Line)
        {
            String FileName = "";
            Int64 FileSize = -1;
            bool IsFolder = false;
            String FileProperty = "";
            DateTime FileLastWriteTime = DateTime.MinValue;
            try
            {
                //修改时间
                FileLastWriteTime = ConvertMsFtpTimeStringToDateTime(Line.Substring(0, 18).Trim());
                //文件属性
                FileProperty = Line.Substring(19, 10).Trim();
                //文件大小
                Int64.TryParse(Line.Substring(29, 9), out FileSize);
                //文件名称
                FileName = Line.Substring(39).Trim();
                //是否是目录
                IsFolder = FileProperty.Contains("<DIR>");

                return base.GetFtpBaseFileInfo(FileName, FileSize, IsFolder, FileProperty, FileLastWriteTime);
            }
            catch (Exception ex)
            {
                Debug.Print("Error From aaaSoft.Ftp.FtpClient.AnalyzeList_MicrosoftFtpServer(),Reason:" + ex.Message);
                return null;
            }
        }

        #region 转换MsFtp的时间格式为.NET时间格式
        public static DateTime ConvertMsFtpTimeStringToDateTime(String str)
        {
            String month = str.Substring(0, 2);
            String day = str.Substring(3, 2);
            String year = str.Substring(6, 2);
            String time = str.Substring(10);

            String netString = String.Format("{0}-{1}-{2} {3}", year, month, day, time);

            var returnValue = DateTime.MinValue;
            DateTime.TryParse(netString, out returnValue);
            return returnValue;
        }
        #endregion
    }
}
