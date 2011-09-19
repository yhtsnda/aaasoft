using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Update
{
    //更新文件类
    public class UpdateFile
    {
        //文件名称
        public String FileName;
        //文件相对路径
        public String FilePath;
        //文件版本
        public Version FileVersion;
        //文件描述
        public String FileDescription;
        //本地文件路径
        public String FullFileName;
        
        //文件下载URL
        public String DownloadUrl;
        /**********/
        /*下载部分*/
        /**********/
        //文件大小
        public Int64 Length;
        //下载状态
        public String Status;
        //下载进度
        private Double _DownloadPercent = 0;
        public Double DownloadPercent
        {
            get
            {
                return _DownloadPercent;
            }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _DownloadPercent = value;
                }
            }
        }
    }
}
