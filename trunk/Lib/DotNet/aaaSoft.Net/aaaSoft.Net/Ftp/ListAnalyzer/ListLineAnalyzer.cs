using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace aaaSoft.Net.Ftp.ListAnalyzer
{
    public class ListLineAnalyzer : FtpListAnalyzer
    {
        private static String[] getPart1String(String line)
        {
            line = line.Trim();
            Int32 index = line.IndexOf(" ");
            String[] rtnArray = new String[2];
            rtnArray[0] = line.Substring(0, index);
            rtnArray[1] = line.Substring(index);
            return rtnArray;
        }

        public override FtpBaseFileInfo AnalyzeLine(string line)
        {
            String srcLine = line;
            try
            {
                if (String.IsNullOrEmpty(line.Trim())) return null;

                String FileName = "";
                Int64 FileSize = -1;
                bool IsFolder = false;
                String FileProperty = "";
                DateTime FileLastWriteTime = DateTime.MinValue;

                Char lineFirstChar = line[0];

                //如果是UNIX风格
                if (lineFirstChar == 'd' || lineFirstChar == '-')
                {
                    //LS的输出风格
                    //0:drwxrwx--x system   system            2012-06-10 20:58 dvp
                    //1:drwxr-xr-x    2 root     root             0 Jun 10 12:59 boot
                    Int32 lsOutputStyle = 0;

                    String[] lineArray;

                    //文件属性
                    lineArray = getPart1String(line);
                    FileProperty = lineArray[0].Trim();
                    line = lineArray[1];

                    //判断风格
                    lineArray = getPart1String(line);
                    Int32 tmpInt;
                    if (Int32.TryParse(lineArray[0], out tmpInt))
                    {
                        lsOutputStyle = 1;
                        line = lineArray[1];
                    }
                    else
                    {
                        lsOutputStyle = 0;
                    }

                    //文件拥有者
                    lineArray = getPart1String(line);
                    String fileOwner = lineArray[0].Trim();
                    line = lineArray[1];
                    //文件所属用户组
                    lineArray = getPart1String(line);
                    String fileOwnerGroup = lineArray[0].Trim();
                    line = lineArray[1];

                    if (lsOutputStyle == 0)
                    {
                        lineArray = getPart1String(line);
                        IsFolder = lineArray[0].Contains("-");
                    }
                    else if (lsOutputStyle == 1)
                    {
                        IsFolder = FileProperty.StartsWith("d");
                        if (IsFolder)
                        {
                            lineArray = getPart1String(line);
                            line = lineArray[1];
                        }
                    }

                    //如果是文件
                    if (!IsFolder)
                    {
                        lineArray = getPart1String(line);
                        String fileSizeStr = lineArray[0].Trim();
                        line = lineArray[1];

                        if (fileSizeStr.Contains(","))
                        {
                            lineArray = getPart1String(line);
                            fileSizeStr += lineArray[0].Trim();
                            line = lineArray[1];

                            fileSizeStr = fileSizeStr.Replace(",", "");
                        }

                        FileSize = Int64.Parse(fileSizeStr);
                    }

                    //文件修改时间
                    Int32 timeStringPartCount = 0;
                    if (lsOutputStyle == 0)
                        timeStringPartCount = 2;
                    else if (lsOutputStyle == 1)
                        timeStringPartCount = 3;

                    String fileModifyTime = "";
                    for (int i = 0; i < timeStringPartCount; i++)
                    {
                        lineArray = getPart1String(line);
                        fileModifyTime += lineArray[0].Trim() + " ";
                        line = lineArray[1];
                    }
                    fileModifyTime = fileModifyTime.Trim();
                    if (lsOutputStyle == 1)
                    {
                        if (fileModifyTime.Contains(":"))
                        {
                            fileModifyTime = String.Format("{0} {1} {2}", fileModifyTime.Substring(0, 6), DateTime.Now.Year, fileModifyTime.Substring(7));
                        }
                    }
                    FileLastWriteTime = DateTime.Parse(fileModifyTime);

                    //文件名称
                    FileName = line.Trim();
                }
                //如果是MS-DOS风格
                else if (lineFirstChar >= '0' && lineFirstChar <= '9')
                {
                    String[] lineArray = getPart1String(line);
                    String dateString = lineArray[0].Trim();
                    line = lineArray[1];

                    lineArray = dateString.Split('-');
                    dateString = lineArray[2] + "-" + lineArray[0] + "-" + lineArray[1];

                    lineArray = getPart1String(line);
                    String timeString = lineArray[0].Trim();
                    line = lineArray[1];

                    FileLastWriteTime = DateTime.Parse(dateString + " " + timeString);


                    lineArray = getPart1String(line);
                    String tmpString = lineArray[0].Trim();
                    line = lineArray[1];

                    Int64 tmpInt64 = 0;
                    if (Int64.TryParse(tmpString, out tmpInt64))
                    {
                        FileSize = tmpInt64;
                        IsFolder = false;
                    }
                    else
                    {
                        IsFolder = true;
                    }
                    FileName = line.Trim();
                }
                return base.GetFtpBaseFileInfo(FileName, FileSize, IsFolder, FileProperty, FileLastWriteTime);
            }
            catch (Exception ex)
            {
                Debug.Print(srcLine);
                return null;
            }
        }
    }
}
