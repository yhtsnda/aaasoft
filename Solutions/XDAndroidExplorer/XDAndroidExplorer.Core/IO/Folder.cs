using System;
using System.Collections.Generic;
using System.Text;

namespace XDAndroidExplorer.Core.IO
{
	public class Folder : BaseFile
	{
		public Folder() { }
		public Folder(String folderName) { this.FullName = folderName; }
		public List<BaseFile> SubBaseFiles
		{
			get
			{
				return GetSubBaseFiles();
			}
		}

		public bool CreateFolder(String FolderName)
		{
			String cmdStr = String.Format("mkdir \"{0}/{1}\"", this.FullName, FolderName);
			String rtnStr = NativeMethod.ExecuteShellCommand(cmdStr);
			return String.IsNullOrEmpty(rtnStr);
		}

		private List<BaseFile> GetSubBaseFiles()
		{
			List<BaseFile> baseFileList = new List<BaseFile>();

			String tmpFullName = this.FullName;
			if(!tmpFullName.EndsWith("/"))
				tmpFullName += "/";
			String result = NativeMethod.ExecuteShellCommand(String.Format("ls -l \"{0}\"", tmpFullName));
			String[] lines = result.Split('\n');
			foreach (String line in lines)
			{
				try{
				BaseFile baseFile = BaseFile.GetBaseFile(line,this.FullName);
				if(baseFile == null)
					continue;
				baseFileList.Add(baseFile);
				}catch(Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
			return baseFileList;
		}
	}
}
