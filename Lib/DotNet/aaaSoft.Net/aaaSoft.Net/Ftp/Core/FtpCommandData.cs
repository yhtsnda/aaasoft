using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Net.Ftp.Core
{
    /// <summary>
    /// FTP命令数据
    /// </summary>
    public class FtpCommandData
    {
        /// <summary>
        /// FTP命令名称
        /// </summary>
        public String FtpCommandName;
        /// <summary>
        /// FTP命令参数
        /// </summary>
        public String FtpCommandArgs;
        /// <summary>
        /// FTP命令其他数据
        /// </summary>
        public String FtpCommandOther;

        public FtpCommandData(String FtpCommandName)
        {
            this.FtpCommandName = FtpCommandName;
        }
        public FtpCommandData(String FtpCommandName, String FtpCommandArgs)
            : this(FtpCommandName)
        {
            this.FtpCommandArgs = FtpCommandArgs;
        }

        public static FtpCommandData FromCommandText(String FtpCommandText)
        {
            FtpCommandText = FtpCommandText.Trim();
            if (FtpCommandText.Contains(" "))
            {
                //包含参数
                Int32 spIndex = FtpCommandText.IndexOf(' ');
                String FtpCommandName = FtpCommandText.Substring(0, spIndex);
                String FtpCommandArgs = FtpCommandText.Substring(spIndex + 1);
                return new FtpCommandData(FtpCommandName, FtpCommandArgs);
            }
            else
            {
                //不包含参数
                String FtpCommandName = FtpCommandText;
                return new FtpCommandData(FtpCommandName);
            }
        }

        /// <summary>
        /// 得到命令文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(FtpCommandOther))
                sb.Append(FtpCommandOther);
            sb.Append(FtpCommandName);
            if (!String.IsNullOrEmpty(FtpCommandArgs))
            {
                sb.Append(" ");
                sb.Append(FtpCommandArgs);
            }
            return sb.ToString();
        }
    }
}
