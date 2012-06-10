using System;
using System.Collections.Generic;
using System.Text;
using XDAndroidExplorer.Core.Helpers;

namespace XDAndroidExplorer.Core.IO
{
	public abstract class BaseFile
	{
		/// <summary>
		/// 全路径
		/// </summary>
		public String FullName;

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
		//大小
		public Int64 Size;
		//最后写入时间
		public String LastModifyTime;
		//属性
		public String Property;
		//其他
		public String Ext;
		//文件拥有者
		public String Owner;
		//文件所属组
		public String OwnerGroup;

		/// <summary>
		/// 获取父路径
		/// </summary>
		public String ParentPath
		{
			get
			{
				if (this.FullName == "/") return null;
				String tmpPath = IoHelper.GetParentPath(this.FullName, '/');
				if(String.IsNullOrEmpty(tmpPath))
					tmpPath  = "/";
				return tmpPath;
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
				deleteCmd = String.Format("rm \"{0}\"", this.FullName);
			}
			else if (this is Folder)
			{
				deleteCmd = String.Format("rm -r \"{0}\"", this.FullName);
			}
			String rtnStr = NativeMethod.ExecuteShellCommand(deleteCmd);
			return String.IsNullOrEmpty(rtnStr);
		}
		#endregion

		private static String[] getPart1String(String line)
		{
			line = line.Trim();
			Int32 index = line.IndexOf(" ");
			String[] rtnArray = new String[2];
			rtnArray[0] = line.Substring(0,index);
			rtnArray[1] = line.Substring(index);
			return rtnArray;
		}
		
		#region 得到BaseFile对象
		/// <summary>
		/// 得到BaseFile对象
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Size"></param>
		/// <param name="LastWriteTime"></param>
		/// <param name="Property"></param>
		/// <returns></returns>
		public static BaseFile GetBaseFile(String line,String parentFullPath)
		{
			if (String.IsNullOrEmpty(line.Trim())) return null;
			if (line.StartsWith("/")) return null;
			if (line.StartsWith("ls:")) return null;
			if (line.StartsWith("l:")) return null;
			
			//LS的输出风格
			//0:drwxrwx--x system   system            2012-06-10 20:58 dvp
			//1:drwxr-xr-x    2 root     root             0 Jun 10 12:59 boot
			Int32 lsOutputStyle = 0;
			
			String[] lineArray;
			
			//文件属性
			lineArray = getPart1String(line);
			String fileProperty = lineArray[0].Trim();
			line = lineArray[1];
			
			//判断风格
			lineArray = getPart1String(line);
			Int32 tmpInt;
			if(Int32.TryParse(lineArray[0],out tmpInt))
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
			
			BaseFile baseFile = null;
			
			Boolean isFolder = false;
			if(lsOutputStyle == 0){
				lineArray = getPart1String(line);
				isFolder = lineArray[0].Contains("-");
			}else if(lsOutputStyle == 1){
				isFolder = fileProperty.StartsWith("d");
				if(isFolder)
				{
					lineArray = getPart1String(line);
					line = lineArray[1];
				}
			}
			
			//如果是目录
			if(isFolder)
			{
				baseFile = new Folder();
			}
			//否则是文件
			else
			{
				lineArray = getPart1String(line);
				String fileSizeStr = lineArray[0].Trim();
				line = lineArray[1];
				
				if(fileSizeStr.Contains(","))
				{
					lineArray = getPart1String(line);
					fileSizeStr += lineArray[0].Trim();
					line = lineArray[1];
					
					fileSizeStr = fileSizeStr.Replace(",","");
				}
				
				Int64 fileSize = Int64.Parse(fileSizeStr);
				
				baseFile = new File();
				baseFile.Size = fileSize;
			}
			
			//文件修改时间
			Int32 timeStringPartCount=0;
			if(lsOutputStyle == 0)
				timeStringPartCount = 2;
			else if(lsOutputStyle == 1)
				timeStringPartCount = 3;
			
			String fileModifyTime = "";
			for(int i=0;i<timeStringPartCount;i++)
			{
				lineArray = getPart1String(line);
				fileModifyTime += lineArray[0].Trim() + " ";
				line = lineArray[1];
			}
			fileModifyTime=fileModifyTime.Trim();
			
			//文件名称
			String fileName = line.Trim();
			//文件其他属性
			String fileExt = "";
			
			if(fileName.Contains("->"))
			{
				String[] tmpArray2 = fileName.Split(new String[]{ "->"}, StringSplitOptions.None);
				fileName = tmpArray2[0].Trim();
				fileExt = tmpArray2[1].Trim();
			}
			
			//加上全路径
			fileName = parentFullPath + "/" + fileName;
			fileName = fileName.Replace("//", "/");
			
			baseFile.FullName = fileName;
			baseFile.LastModifyTime = fileModifyTime;
			baseFile.Property = fileProperty;
			baseFile.Ext = fileExt;
			baseFile.Owner = fileOwner;
			baseFile.OwnerGroup = fileOwnerGroup;
			return baseFile;
		}
		#endregion
	}
}
