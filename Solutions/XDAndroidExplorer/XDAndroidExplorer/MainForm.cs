using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using aaaSoft.Helpers;

namespace XDAndroidExplorer
{
	public partial class MainForm : Form
	{
		private UnitStringConverting susc = UnitStringConverting.StorageUnitStringConverting;
		private XDAndroidExplorer.Core.IO.Folder _CurrentFolder;
		public XDAndroidExplorer.Core.IO.Folder CurrentFolder
		{
			get
			{
				return _CurrentFolder;
			}
			set
			{
				_CurrentFolder = value;
				GotoFolder(value);
				txtAddress.Text = CurrentFolder.FullName;
			}
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			lblVersion.Text = Application.ProductVersion;

			XDAndroidExplorer.Core.NativeMethod.InitAdb();
			Thread.Sleep(1000);
			CurrentFolder = new XDAndroidExplorer.Core.IO.Folder("/");
		}

		#region 文件管理选项卡

		#region 转到目录
		//转到目录
		private void GotoFolder(String folderPath)
		{
			CurrentFolder = new XDAndroidExplorer.Core.IO.Folder(folderPath);
		}
		private void GotoFolder(XDAndroidExplorer.Core.IO.Folder folder)
		{
			lvExplorer.Items.Clear();
			List<XDAndroidExplorer.Core.IO.BaseFile> baseFileList = folder.SubBaseFiles;
			foreach (XDAndroidExplorer.Core.IO.BaseFile baseFile in baseFileList)
			{
				ListViewItem newLvi = lvExplorer.Items.Add(baseFile.Name);
				newLvi.SubItems.Add(baseFile.LastModifyTime.ToString());
				//如果是目录
				if(baseFile is XDAndroidExplorer.Core.IO.Folder)
				{
					newLvi.SubItems.Add("文件夹");
					newLvi.SubItems.Add("");
				}
				//否则是文件
				else
				{
					String extension = Path.GetExtension(baseFile.Name).ToUpper();
					if(extension.Contains(".")){
						extension = extension.Substring(1) + " ";
					}
					newLvi.SubItems.Add(extension + "文件");
					newLvi.SubItems.Add(susc.GetString(baseFile.Size,"K",0,true) + "B");
				}
				
				newLvi.SubItems.Add(baseFile.Property);
				newLvi.SubItems.Add(baseFile.Ext);
				newLvi.SubItems.Add(baseFile.Owner);
				newLvi.SubItems.Add(baseFile.OwnerGroup);
				
				int imageIndex = 0;
				if (baseFile is XDAndroidExplorer.Core.IO.Folder)
				{
					if (baseFile.FullName.ToLower() == "/sdcard")
					{
						imageIndex = 2;
					}
					else
					{
						imageIndex = 0;
					}
				}
				else
				{
					imageIndex = 1;
				}
				newLvi.ImageIndex = imageIndex;
				newLvi.Tag = baseFile;
			}
		}
		#endregion

		#region 点击“刷新”按钮时
		private void btnRefrush_Click(object sender, EventArgs e)
		{
			RefrushCurrentFolder();
		}

		private void RefrushCurrentFolder()
		{
			CurrentFolder = CurrentFolder;
		}
		#endregion

		#region 点击“向上”按钮时
		private void btnUp_Click(object sender, EventArgs e)
		{
			if (CurrentFolder.FullName == "/") return;
			CurrentFolder = CurrentFolder.ParentFolder;
		}
		#endregion

		#region 点击“新建文件夹”按钮时
		private void btnNewFolder_Click(object sender, EventArgs e)
		{
			String newFolderName = InputForm.GetInput("新建文件夹", "请输入文件夹名称：", "");
			if (newFolderName == null) return;

			newFolderName = newFolderName.Trim();
			if (String.IsNullOrEmpty(newFolderName))
			{
				MessageBox.Show("请输入正确的文件夹名称！");
				return;
			}
			CurrentFolder.CreateFolder(newFolderName);
			RefrushCurrentFolder();
		}
		#endregion

		#region 双击列表时
		private void lvExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (lvExplorer.SelectedItems.Count == 0) return;
			XDAndroidExplorer.Core.IO.BaseFile baseFile = lvExplorer.SelectedItems[0].Tag as XDAndroidExplorer.Core.IO.BaseFile;
			if (baseFile is XDAndroidExplorer.Core.IO.File)
			{
			}
			else if (baseFile is XDAndroidExplorer.Core.IO.Folder)
			{
				CurrentFolder = baseFile as XDAndroidExplorer.Core.IO.Folder;
			}
		}
		#endregion

		#region 点击“转到”按钮时
		private void btnGotoFolder_Click(object sender, EventArgs e)
		{
			GotoFolder(txtAddress.Text.Trim());
		}
		#endregion

		#region 主窗体拖放
		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			String[] paths = e.Data.GetData(DataFormats.FileDrop, false) as String[];
			foreach (String path in paths)
			{
				if (File.Exists(path))
				{
					XDAndroidExplorer.Core.NativeMethod.PushFile(path, CurrentFolder.FullName);
				}
				else
				{
					//XDAndroidExplorer.Core.NativeMethod.PushFolder(path, CurrentFolder.FullName);
					XDAndroidExplorer.Core.NativeMethod.PushFile(path, CurrentFolder.FullName + "/" + Path.GetFileName(path));
				}
			}
			RefrushCurrentFolder();
		}
		#endregion

		#region 列表菜单部分

		private void cmsMain_Opening(object sender, CancelEventArgs e)
		{
			if (lvExplorer.SelectedItems.Count == 0) e.Cancel = true;
			重命名ToolStripMenuItem.Enabled = lvExplorer.SelectedItems.Count == 1;
		}

		private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Core.IO.BaseFile baseFile = lvExplorer.SelectedItems[0].Tag as Core.IO.BaseFile;

			String newName = null;
			if (baseFile is Core.IO.File)
			{
				newName = InputForm.GetInput("重命名文件", "请输入新的文件名称", baseFile.Name);
			}
			else if (baseFile is Core.IO.Folder)
			{
				newName = InputForm.GetInput("重命名文件夹", "请输入新的文件夹名称", baseFile.Name);
			}

			if (newName == null) return;
			newName = newName.Trim();
			if (newName == "")
			{
				MessageBox.Show("输入为空，已取消重命名！");
				return;
			}

			XDAndroidExplorer.Core.NativeMethod.Move(baseFile.FullName, baseFile.ParentPath + "/" + newName);
			RefrushCurrentFolder();
		}

		private void 下载ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<XDAndroidExplorer.Core.IO.BaseFile> baseFiles = new List<XDAndroidExplorer.Core.IO.BaseFile>();
			foreach (ListViewItem newLvi in lvExplorer.SelectedItems)
			{
				Core.IO.BaseFile baseFile = newLvi.Tag as Core.IO.BaseFile;
				if (baseFile != null) baseFiles.Add(baseFile);
			}

			foreach (Core.IO.BaseFile baseFile in baseFiles)
			{
				if (baseFile is XDAndroidExplorer.Core.IO.File)
				{
					XDAndroidExplorer.Core.NativeMethod.PullFile(baseFile.FullName, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
				}
				else if (baseFile is XDAndroidExplorer.Core.IO.Folder)
				{
					XDAndroidExplorer.Core.NativeMethod.PullFile(baseFile.FullName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), baseFile.Name));
				}
			}
			MessageBox.Show("选中的文件/文件夹已全部成功下载到桌面！");
		}

		private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult dr = MessageBox.Show("您确定要删除选中的项？", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.Cancel) return;
			foreach (ListViewItem newLvi in lvExplorer.SelectedItems)
			{
				XDAndroidExplorer.Core.IO.BaseFile baseFile = newLvi.Tag as XDAndroidExplorer.Core.IO.BaseFile;
				baseFile.Delete();
			}
			RefrushCurrentFolder();
		}
		#endregion

		#endregion

		#region Shell控制台选项卡
		private void btnShellSendCmd_Click(object sender, EventArgs e)
		{
			ExecuteShellCommand(txtShellInput.Text.Trim());
		}
		private void btnShellHelp_Click(object sender, EventArgs e)
		{
			ExecuteShellCommand("busybox");
		}

		private void ExecuteShellCommand(String cmdStr)
		{
			txtShellOutput.Text = XDAndroidExplorer.Core.NativeMethod.ExecuteShellCommand(cmdStr);
		}

		private void txtShellInput_Enter(object sender, EventArgs e)
		{
			this.AcceptButton = btnShellSendCmd;
		}

		private void Input_Leave(object sender, EventArgs e)
		{
			this.AcceptButton = null;
		}
		#endregion

		#region ADB控制台选项卡
		private void btnAdbSendCmd_Click(object sender, EventArgs e)
		{
			ExecuteAdbCommand(txtAdbInput.Text.Trim());
		}

		private void btnAdbHelp_Click(object sender, EventArgs e)
		{
			ExecuteAdbCommand("help");
		}
		private void ExecuteAdbCommand(String cmdStr)
		{
			txtAdbOutput.Text = XDAndroidExplorer.Core.NativeMethod.ExecuteCommand(cmdStr);
		}
		private void txtAdbInput_Enter(object sender, EventArgs e)
		{
			this.AcceptButton = btnAdbSendCmd;
		}
		#endregion

		#region 常用功能选项卡
		private void btnReboot_Click(object sender, EventArgs e)
		{
			ExecuteShellCommand("reboot");
		}
		
		private void btnRebootRecovery_Click(object sender, EventArgs e)
		{
			ExecuteShellCommand("reboot recovery");
		}

		private void btnPoweroff_Click(object sender, EventArgs e)
		{
			ExecuteShellCommand("poweroff -f");
		}
		#endregion

		#region 关于功能选项卡
		private void pbScbeta_Click(object sender, EventArgs e)
		{
			Process.Start("http://bbs.scbeta.com");
		}
		#endregion
	}
}
