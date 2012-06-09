using System;
using System.Collections.Generic;
using System.Text;
using XDAndroidExplorer.Core.Helpers;

namespace XDAndroidExplorer.Core.IO
{
    public abstract class BaseFile
    {
        private String _FullName;
        /// <summary>
        /// 全路径
        /// </summary>
        public String FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
                if (String.IsNullOrEmpty(_FullName))
                {
                    _FullName = "/";
                }
                while (_FullName.Contains("//"))
                {
                    _FullName = FullName.Replace("//", "/");
                }
                
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name
        {
            get
            {
            	String[] strs = FullName.Split('/');
                if (strs.Length == 0)
                {
                    return "/";
                }
                else
                {
                    return strs[strs.Length - 1];
                }
            }
        }

        public Int64 Size;
        public DateTime LastWriteTime;
        public String Property;

        /// <summary>
        /// 获取父路径
        /// </summary>
        public String ParentPath
        {
            get
            {
                if (this.FullName == "/") return null;
                return IoHelper.GetParentPath(this.FullName, '/');
            }
        }

        /// <summary>
        /// 获取父目录对象
        /// </summary>
        public Folder ParentFolder
        {
            get
            {
                if (this.FullName == "/") return null;
                return new Folder(ParentPath);
            }
        }

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            String deleteCmd = "";
            if (this is File)
            {
                deleteCmd = String.Format("rm -f \"{0}\"", this.FullName);
            }
            else if (this is Folder)
            {
                deleteCmd = String.Format("rm -rf \"{0}\"", this.FullName);
            }
            String rtnStr = NativeMethod.ExecuteShellCommand(deleteCmd);
            return String.IsNullOrEmpty(rtnStr);
        }
        #endregion

        #region 得到BaseFile对象
        /// <summary>
        /// 得到BaseFile对象
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="LastWriteTime"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public static BaseFile GetBaseFile(String Name, Int64 Size, DateTime LastWriteTime, String Property)
        {
            BaseFile baseFile = null;
            if (Property.StartsWith("d"))
            {
                //目录
                baseFile = new Folder();
            }
            else
            {
                baseFile = new File();
                //文件
            }
            baseFile.FullName = Name;
            baseFile.Size = Size;
            baseFile.LastWriteTime = LastWriteTime;
            baseFile.Property = Property;
            return baseFile;
        }
        #endregion
    }
}
