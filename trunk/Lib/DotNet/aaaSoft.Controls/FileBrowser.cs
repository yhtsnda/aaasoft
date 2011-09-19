using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using aaaSoft.Helpers;
using System.Diagnostics;
using System.Threading;

namespace aaaSoft.Controls
{
    public partial class FileBrowser : UserControl
    {
        private Environment.SpecialFolder _StartUpFolder = Environment.SpecialFolder.MyDocuments;
        /// <summary>
        /// 起始目录
        /// </summary>
        public Environment.SpecialFolder StartUpFolder
        {
            get
            {
                return _StartUpFolder;
            }
            set
            {
                _StartUpFolder = value;
            }
        }
        /// <summary>
        /// 当前目录路径
        /// </summary>
        public String CurrentFolderPath;
        /// <summary>
        /// 选中路径数组
        /// </summary>
        public String[] SelectedPathArray
        {
            get
            {
                List<String> SelectedPathList = new List<string>();
                var items = lvBrowser.SelectedItems;
                foreach (ListViewItem item in items)
                {
                    String Path = item.Name;
                    SelectedPathList.Add(Path);
                }
                return SelectedPathList.ToArray();
            }
        }
        /// <summary>
        /// 当前目录的文件系统监视器
        /// </summary>
        FileSystemWatcher fswCurrentFolder;

        public FileBrowser()
        {
            InitializeComponent();
            cbAddress.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            cbAddress.ComboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);

            fswCurrentFolder = new FileSystemWatcher();
            fswCurrentFolder.Changed += new FileSystemEventHandler(fswCurrentFolder_Changed);
            fswCurrentFolder.Created += new FileSystemEventHandler(fswCurrentFolder_Created);
            fswCurrentFolder.Deleted += new FileSystemEventHandler(fswCurrentFolder_Deleted);
            fswCurrentFolder.Renamed += new RenamedEventHandler(fswCurrentFolder_Renamed);
        }

        void fswCurrentFolder_Renamed(object sender, RenamedEventArgs e)
        {
            RemovePathFromList(e.OldFullPath);
            AddPathToList(e.FullPath);
        }

        void fswCurrentFolder_Deleted(object sender, FileSystemEventArgs e)
        {
            RemovePathFromList(e.FullPath);
        }

        void fswCurrentFolder_Created(object sender, FileSystemEventArgs e)
        {
            AddPathToList(e.FullPath);
        }

        void fswCurrentFolder_Changed(object sender, FileSystemEventArgs e)
        {
            RemovePathFromList(e.FullPath);
            AddPathToList(e.FullPath);
        }

        #region 控件加载时
        private void FileBrowser_Load(object sender, EventArgs e)
        {
            ListFolder(Environment.GetFolderPath(StartUpFolder));
        }
        #endregion

        #region ComboBox画对象事件
        void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var g = e.Graphics;
            e.DrawBackground();
            if (e.State == DrawItemState.Focus)
                e.DrawFocusRectangle();

            var item = cbAddress.Items[e.Index];
            String ImageKey = "";
            if (item is String)
            {
                switch (item.ToString())
                {
                    case "桌面":
                        ImageKey = "Desktop";
                        break;
                    default:
                        ImageKey = "Folder";
                        break;
                }
            }
            else if (item is DriveInfo)
            {
                var drive = item as DriveInfo;
                switch (drive.DriveType)
                {
                    case DriveType.Fixed:
                        ImageKey = "Hard_Drive";
                        break;
                    case DriveType.CDRom:
                        ImageKey = "CD_Drive";
                        break;
                    default:
                        ImageKey = "Hard_Drive";
                        break;
                }
            }

            var textRect = e.Bounds;
            var image = ilAddress.Images[ImageKey];
            if (image != null)
            {
                Int32 sp = (e.Bounds.Height - ilAddress.ImageSize.Height) / 2;
                Point location = new Point(sp + e.Bounds.Left, sp + e.Bounds.Top);
                var rect = new Rectangle(location, ilAddress.ImageSize);
                g.DrawImage(image, rect);

                textRect.X += ilAddress.ImageSize.Width + 2 * sp;
            }
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            g.DrawString(item.ToString(), e.Font, new SolidBrush(e.ForeColor), textRect, sf);
        }
        #endregion

        #region 地址栏下拉列表打开时
        private void cbAddress_DropDown(object sender, EventArgs e)
        {
            cbAddress.Items.Clear();
            cbAddress.Items.Add("桌面");
            cbAddress.Items.Add("我的文档");
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                cbAddress.Items.Add(drive);
            }
        }
        #endregion

        #region 地址栏选择改变时
        private void cbAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = cbAddress.SelectedItem;
            String Path = selectedItem.ToString();
            ListFolder(Path);
        }
        #endregion

        #region 地址栏键盘按下时
        private void cbAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String Path = cbAddress.Text.Trim();
                if (String.IsNullOrEmpty(Path))
                    return;
                ListFolder(Path);
            }
        }
        #endregion


        #region 列出目录
        /// <summary>
        /// 跳转到并列出目录
        /// </summary>
        /// <param name="Path">目录路径</param>
        public void ListFolder(String Path)
        {
            //如果当前线程是主线程
            if (ThreadHelper.IsCurrentThreadMainThread())
                _ListFolder(Path);
            else
            {
                this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate()
                {
                    _ListFolder(Path);
                }));
            }
        }
        private void _ListFolder(String Path)
        {
            try
            {
                if (!this.DesignMode)
                {
                    fswCurrentFolder.EnableRaisingEvents = false;
                }

                switch (Path)
                {
                    case "桌面":
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        break;
                    case "我的文档":
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        break;
                }

                if (!Directory.Exists(Path))
                {
                    MessageBox.Show("目录不存在!");
                    return;
                }

                DirectoryInfo di = new DirectoryInfo(Path);
                //得到目录列表
                var dirs = di.GetDirectories();
                //得到文件列表
                var files = di.GetFiles();

                lvBrowser.Items.Clear();
                foreach (var dir in dirs)
                {
                    AddPathToList(dir.FullName);
                }

                foreach (var file in files)
                {
                    AddPathToList(file.FullName);
                }
                CurrentFolderPath = di.FullName;
                cbAddress.Text = CurrentFolderPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("列目录时出错，原因：" + ex.Message);
            }
            finally
            {
                if (!this.DesignMode)
                {
                    fswCurrentFolder.Path = CurrentFolderPath;
                    fswCurrentFolder.EnableRaisingEvents = true;
                }
            }
        }
        #endregion

        #region 从列表中移除路径
        private void RemovePathFromList(String Path)
        {
            //如果当前线程是主线程
            if (ThreadHelper.IsCurrentThreadMainThread())
                _RemovePathFromList(Path);
            else
            {
                this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate()
                {
                    _RemovePathFromList(Path);
                }));
            }
        }
        private void _RemovePathFromList(String Path)
        {
            try { lvBrowser.Items.RemoveByKey(Path); }
            catch { }
        }
        #endregion

        #region 添加路径到列表中
        private void AddPathToList(String Path)
        {
            //如果当前线程是主线程
            if (ThreadHelper.IsCurrentThreadMainThread())
                _AddPathToList(Path);
            else
            {
                this.BeginInvoke(new ThreadHelper.UnnamedDelegate(delegate()
                {
                    _AddPathToList(Path);
                }));
            }
        }
        private void _AddPathToList(String Path)
        {
            if (Directory.Exists(Path))
            {
                var dir = new DirectoryInfo(Path);
                var newLvi = lvBrowser.Items.Add(dir.FullName, dir.Name, "Folder");
                newLvi.SubItems.Add("");
                newLvi.SubItems.Add(dir.LastWriteTime.ToString());
            }
            else if (File.Exists(Path))
            {
                var file = new FileInfo(Path);
                var newLvi = lvBrowser.Items.Add(file.FullName, file.Name, "File");
                newLvi.SubItems.Add(UnitStringConverting.StorageUnitStringConverting.GetString(file.Length, 0, true));
                newLvi.SubItems.Add(file.LastWriteTime.ToString());
            }
            //排序
            if (lvBrowser.Sorting != SortOrder.None)
                lvBrowser.Sort();
        }
        #endregion

        #region 刷新当前目录
        /// <summary>
        /// 刷新当前目录
        /// </summary>
        public void RefreshCurrentFolder()
        {
            ListFolder(CurrentFolderPath);
        }
        #endregion

        #region 返回上层目录
        private void btnUp_Click(object sender, EventArgs e)
        {
            var parentPath = CurrentFolderPath;
            do
            {
                var tmpPath = Path.GetDirectoryName(parentPath);
                if (String.IsNullOrEmpty(tmpPath))
                    break;
                parentPath = tmpPath;
            } while (!Directory.Exists(parentPath));
            ListFolder(parentPath);
        }
        #endregion



        #region ListView对象拖出事件
        private void lvBrowser_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (lvBrowser.SelectedItems.Count <= 0)
                {
                    return;
                }

                // put selected files into a string array 
                string[] files = new String[lvBrowser.SelectedItems.Count];

                int i = 0;
                foreach (ListViewItem item in lvBrowser.SelectedItems)
                {
                    files[i++] = item.Name;
                }
                // create a dataobject holding this array as a filedrop
                DataObject data = new DataObject(DataFormats.FileDrop, files);

                // also add the selection as textdata
                data.SetData(DataFormats.StringFormat, files[0]);

                // Do DragDrop 
                DoDragDrop(data, DragDropEffects.Copy);
            }
        }
        #endregion

        #region ListView鼠标双击事件
        private void lvBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvBrowser.SelectedItems.Count == 0)
                return;
            var newLvi = lvBrowser.SelectedItems[0];
            var Path = newLvi.Name;
            if (Directory.Exists(Path))
            {
                ListFolder(Path);
            }
            else if (File.Exists(Path))
            {
                Win32ApiHelper.ShellExecute(IntPtr.Zero, "", Path, "", "", Win32ApiHelper.ShowCommands.SW_SHOWNORMAL);
            }
            else
            {
                MessageBox.Show("路径无效");
            }
        }
        #endregion

        #region ListView键盘按下时
        private void lvBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            Int32 SelectedItemCount = lvBrowser.SelectedItems.Count;
            switch (e.KeyCode)
            {
                    //重命名
                case Keys.F2:
                    if (SelectedItemCount == 1)
                        重命名RToolStripMenuItem_Click(sender, e);
                    break;
                    //刷新当前目录
                case Keys.F5:
                    刷新XToolStripMenuItem_Click(sender, e);
                    break;
                    //删除
                case Keys.Delete:
                    删除DToolStripMenuItem_Click(sender, e);
                    break;
                    //进入目录或打开
                case Keys.Enter:
                    if (SelectedItemCount == 1)
                        lvBrowser_MouseDoubleClick(sender, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
                    break;
                    //返回上层目录
                case Keys.Back:
                    btnUp_Click(sender, e);
                    break;
                    //Ctrl + A 全选
                case Keys.A:
                    if (e.Control)
                        foreach (ListViewItem item in lvBrowser.Items)
                            item.Selected = true;
                    break;
            }
        }
        #endregion

        #region ListView拖拽进入时
        private void lvBrowser_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region ListView拖拽放下时
        private void lvBrowser_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                String[] pathArray = e.Data.GetData(DataFormats.FileDrop, false) as String[];

                //Copy file from external application
                foreach (string path in pathArray)
                {
                    string destPath = Path.Combine(CurrentFolderPath, System.IO.Path.GetFileName(path));

                    if (File.Exists(path))
                    {
                        //如果是在Windows平台，则调用系统的文件操作API
                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            WindowsFileOperationHelper.CopyPath(path, destPath);
                        else
                            File.Copy(path, destPath, true);
                    }
                    else if (Directory.Exists(path))
                    {
                        WindowsFileOperationHelper.CopyPath(path, destPath);
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region 菜单弹出时
        private void cmsMain_Opening(object sender, CancelEventArgs e)
        {
            foreach (ToolStripItem item in cmsMain.Items)
            {
                item.Enabled = false;
            }
            
            新建文件夹NToolStripMenuItem.Enabled = true;
            刷新XToolStripMenuItem.Enabled = true;

            switch (lvBrowser.SelectedItems.Count)
            {
                case 0:
                    break;
                case 1:
                    打开ToolStripMenuItem.Enabled = true;
                    删除DToolStripMenuItem.Enabled = true;
                    重命名RToolStripMenuItem.Enabled = true;
                    break;
                default:
                    打开ToolStripMenuItem.Enabled = true;
                    删除DToolStripMenuItem.Enabled = true;
                    break;
            }
        }
        #endregion

        #region 菜单-打开
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var Path in SelectedPathArray)
            {
                Win32ApiHelper.ShellExecute(IntPtr.Zero, "", Path, "", "", Win32ApiHelper.ShowCommands.SW_SHOWNORMAL);
            }
        }
        #endregion

        #region 菜单-删除
        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("您确定要删除选定的项?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Cancel) return;
            foreach (var Path in SelectedPathArray)
            {
                try
                {
                    if (Directory.Exists(Path))
                    {
                        Directory.Delete(Path, true);
                    }
                    else if (File.Exists(Path))
                    {
                        File.Delete(Path);
                    }
                }
                catch { }
            }
        }
        #endregion

        #region 菜单-重命名
        private void 重命名RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = lvBrowser.SelectedItems[0];
            var oldPath = item.Name;

            InputForm frmInput = new InputForm("重命名", "请输入新的名称:", item.Text);
            var dr = frmInput.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel) return;
            String newName = frmInput.Value;
            
            try
            {
                String newPath = Path.Combine(CurrentFolderPath, newName);
                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newPath);
                }
                else if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                }
            }
            catch { }
        }
        #endregion

        #region 菜单-新建文件夹
        private void 新建文件夹NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm frmInput = new InputForm("新建文件夹", "请输入新建文件夹的名称:", "新建文件夹");
            var dr = frmInput.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel) return;
            String newName = frmInput.Value;
            try
            {
                String newPath = Path.Combine(CurrentFolderPath, newName);
                Directory.CreateDirectory(newPath);
            }
            catch { }
        }
        #endregion

        #region 菜单-刷新
        private void 刷新XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshCurrentFolder();
        }
        #endregion
    }
}
