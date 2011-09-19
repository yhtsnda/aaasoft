using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace aaaSoft.Net.Ftp.ListAnalyzer
{
    public class MlsdLineAnalyzer : FtpListAnalyzer
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
                Dictionary<String, String> dictLine = new Dictionary<string, string>();
                var tmpLines = Line.Split(';');
                foreach (var tmpLine in tmpLines)
                {
                    if (String.IsNullOrEmpty(tmpLine.Trim())) continue;
                    if (!tmpLine.Contains("="))
                    {
                        //文件/目录名
                        FileName = tmpLine.Trim();
                        continue;
                    }
                    var tmpKeyValue = tmpLine.Split('=');
                    dictLine.Add(tmpKeyValue[0].Trim(), tmpKeyValue[1].Trim());
                }
                //类型
                switch (dictLine["Type"])
                {
                        //上层目录
                    case "cdir": return null;
                    case "file":
                        IsFolder = false;
                        break;
                    case "dir":
                        IsFolder = true;
                        break;
                }
                //最后修改时间
                String srcDateTimeString = dictLine["Modify"];
                var DataTimeString = String.Format(
                        "{0}/{1}/{2} {3}:{4}:{5}",
                        srcDateTimeString.Substring(0, 4),
                        srcDateTimeString.Substring(4, 2),
                        srcDateTimeString.Substring(6, 2),
                        srcDateTimeString.Substring(8, 2),
                        srcDateTimeString.Substring(10, 2),
                        srcDateTimeString.Substring(12, 2)
                        );
                if (DateTime.TryParse(DataTimeString, out FileLastWriteTime))
                    FileLastWriteTime = FileLastWriteTime.ToLocalTime();
                //大小
                if (!IsFolder)
                {
                    FileSize = Convert.ToInt64(dictLine["Size"]);
                }
                //属性
                if (dictLine.ContainsKey("Win32.ea"))
                    FileProperty = dictLine["Win32.ea"];

                return base.GetFtpBaseFileInfo(FileName, FileSize, IsFolder, FileProperty, FileLastWriteTime);
            }
            catch (Exception ex)
            {
                Debug.Print("Error From aaaSoft.Ftp.Analyzer.MlsdLineAnalyzer.AnalyzeLine(),Reason:" + ex.Message);
                return null;
            }
        }
    }
}
