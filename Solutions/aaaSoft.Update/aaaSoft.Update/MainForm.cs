using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using aaaSoft.Update.Helpers;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace aaaSoft.Update
{
    public partial class MainForm : Form
    {
        //要更新或安装的软件名称列表
        public List<String> lstSoftwareName = new List<string>();
        //要更新或安装的软件所在路径
        public String SoftwarePath;

        //保存软件到其文件列表的对应关系
        Dictionary<String, UpdateFile[]> dictSoftFile = new Dictionary<string, UpdateFile[]>();
        //保存要下载的文件
        Dictionary<String, UpdateFile> dictDownloadFiles = new Dictionary<string, UpdateFile>();

        //要更新的软件的进程ID
        public int ProcessId = -1;
        //要更新软件的主程序
        public UpdateFile SoftwareMainProgramFile;

        //代理服务器
        private WebProxy proxyServer = null;
        //服务器
        private static String UpdateApiUrl;
        //从服务器下载文件的URL格式
        private static String DownloadFileUrl;
        //从服务器获取软件信息URL格式
        private static String GetSoftwareInfoUrl;

        //放页面的列表
        List<Panel> lstPanels = new List<Panel>();
        int currentPage = 0;

        //程序运行类型
        private ActionEnum ProgramAction = ActionEnum.Update;
        /// <summary>
        /// 程序运行类型枚举
        /// </summary>
        private enum ActionEnum
        {
            Update,
            Install
        }

        //匿名委托
        delegate void UnnamedDelegate();
        #region 构造函数
        public MainForm()
        {
            InitializeComponent();
            //获取参数
            String[] args = Environment.GetCommandLineArgs();
            try
            {
                String configFileName = args[args.Length - 1];
                if (!File.Exists(configFileName)) throw new IOException("未找到配置文件！");
                InitProgram(configFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("参数错误，升级程序退出。请在软件中点击在线升级！\n\n错误详情：" + ex.Message
                    , Application.ProductName
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }

            switch (ProgramAction)
            {
                case ActionEnum.Update:
                    {
                        this.Text = String.Format("{0} - 在线升级", lstSoftwareName[0]);
                        break;
                    }
                case ActionEnum.Install:
                    {
                        this.Text = "软件安装";
                        break;
                    }
            }
        }
        #endregion

        #region 初始化程序
        private void InitProgram(String xmlFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            //删除配置文件
            File.Delete(xmlFilePath);
            XmlElement root = doc.DocumentElement;
            //读取主机地址
            UpdateApiUrl = root.SelectSingleNode("UpdateApiUrl").InnerText;
            DownloadFileUrl = UpdateApiUrl + "?Action=DownloadFile&FileName={0}";
            GetSoftwareInfoUrl = UpdateApiUrl + "?Action=GetSoftwareInfo&SoftwareName={0}";

            //读取运行方式
            ProgramAction = (ActionEnum)Enum.Parse(typeof(ActionEnum), root.SelectSingleNode("Action").InnerText);
            //读取软件目录
            SoftwarePath = root.SelectSingleNode("Folder").InnerText;
            //读取关于链接
            if (root.SelectSingleNode("AboutUsText") == null)
            {
                pnlAboutUs.Visible = false;
            }
            else
            {
                lblAboutUsText.Text = root.SelectSingleNode("AboutUsText").InnerText;
                lblAboutUsName.Text = root.SelectSingleNode("AboutUsName").InnerText;
                lklAboutUsUrl.Text = root.SelectSingleNode("AboutUsUrl").InnerText;
            }
            //读取代理服务器设置
            XmlNode proxySettingNode = root.SelectSingleNode("ProxySetting");
            {
                if (proxySettingNode != null)
                {
                    rbProxyConnect.Checked = true;

                    String proxyHost = proxySettingNode.SelectSingleNode("ProxyHost").InnerText;
                    String proxyPort = proxySettingNode.SelectSingleNode("ProxyPort").InnerText;

                    txtProxyServer.Text = proxyHost;
                    txtProxyPort.Text = proxyPort;

                    proxyServer = new WebProxy(proxyHost, int.Parse(proxyPort));
                    //网络认证信息
                    XmlNode proxyUserNameNode = proxySettingNode.SelectSingleNode("ProxyUserName");
                    if (proxyUserNameNode != null)
                    {
                        String proxyUserName = proxyUserNameNode.InnerText;
                        String proxyPassword = proxySettingNode.SelectSingleNode("ProxyPassword").InnerText;
                        String proxyDomain = String.Empty;
                        XmlNode proxyDomainNode = proxySettingNode.SelectSingleNode("ProxyDomain");
                        if (proxyDomainNode != null)
                        {
                            proxyDomain = proxyDomainNode.InnerText;
                        }
                        NetworkCredential nc = new NetworkCredential(proxyUserName, proxyPassword, proxyDomain);
                        proxyServer.Credentials = nc;

                        txtProxyUserName.Text = proxyUserName;
                        txtProxyPassword.Text = proxyPassword;
                        txtProxyDomain.Text = proxyDomain;
                    }                    
                }
            }
            //读取软件名称列表
            XmlNodeList softwareNodes = root.SelectNodes("SoftwareList/Software");
            foreach (XmlNode softwareNode in softwareNodes)
            {
                lstSoftwareName.Add(softwareNode.SelectSingleNode("Name").InnerText);
            }

            //额外的设置
            switch (ProgramAction)
            {
                case ActionEnum.Update:
                    {
                        if (root.SelectSingleNode("ProcessId") != null)
                            int.TryParse(root.SelectSingleNode("ProcessId").InnerText, out ProcessId);

                        DirectoryInfo di = new DirectoryInfo(SoftwarePath);
                        if (!di.Exists)
                        {
                            throw new IOException("软件目录不存在。");
                        }
                        SoftwarePath = di.FullName;
                        break;
                    }
                case ActionEnum.Install:
                    {
                        break;
                    }
            }


        }
        #endregion

        #region 窗体加载时
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Controls.Remove(tabControl1);

            switch (ProgramAction)
            {
                case ActionEnum.Update:
                    {
                        lstPanels.Add(panel1);
                        lstPanels.Add(panel2);
                        lstPanels.Add(panel3);
                        lstPanels.Add(panel4);
                        lstPanels.Add(panel5);
                        lstPanels.Add(panel6);
                        lstPanels.Add(panel7);

                        cbRunSoftwareAfterUpdate.Text = "运行" + lstSoftwareName[0];
                        break;
                    }
                case ActionEnum.Install:
                    {
                        lstPanels.Add(panel3);
                        lstPanels.Add(panel5);
                        lstPanels.Add(panel8);

                        StringBuilder sb = new StringBuilder();
                        foreach (String softName in lstSoftwareName)
                        {
                            sb.AppendLine(softName);
                        }
                        lblSuccessInstallSoftwares.Text = sb.ToString();
                        break;
                    }
            }


            Point offsetLocation = tabControl1.Location;
            foreach (Panel pnl in lstPanels)
            {
                pnl.Parent = this;
                pnl.Location = offsetLocation;
                pnl.Visible = false;
            }

            

            ShowPage(0);
        }
        #endregion

        #region 显示某一页面
        private void ShowPage(int index)
        {
            switch (ProgramAction)
            {
                case ActionEnum.Update:
                    {
                        //设置完网络类型点下一步时，开始获取服务器上文件列表
                        if (currentPage == 1 && index == 2)
                        {
                            DisableBottomButtons();
                            //检查是否设置代理
                            if (rbDirectConnect.Checked)
                            {
                                proxyServer = null;
                            }
                            else
                            {
                                proxyServer = GetProxy();
                            }
                            //打开获取文件列表线程
                            Thread trdGetFileList = new Thread(GetUpdateSoftwareInfoThread);
                            trdGetFileList.Start();
                        }

                        //当来到第5个页面（下载更新页面）时
                        if (index == 4)
                        {
                            //询问是否杀进程
                            if (ProcessId > 0)
                            {
                                Process prcSoft = null;
                                try
                                {
                                    prcSoft = Process.GetProcessById(ProcessId);
                                }
                                catch { }
                                if (prcSoft != null)
                                {
                                    while (!prcSoft.HasExited)
                                    {
                                        DialogResult dr = MessageBox.Show(String.Format("检测到{0}还在运行，请关闭{0}后点击重试按钮。如果点击终止按钮则强制关闭。", lstSoftwareName[0]), this.Text, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
                                        if (dr == System.Windows.Forms.DialogResult.Abort)
                                        {
                                            prcSoft.Kill();
                                            prcSoft.WaitForExit();
                                        }
                                        else if (dr == System.Windows.Forms.DialogResult.Retry)
                                        {
                                            continue;
                                        }
                                        else if (dr == System.Windows.Forms.DialogResult.Ignore)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            DisableBottomButtons();
                            List<ListViewItem> lvis = new List<ListViewItem>();
                            foreach (ListViewItem newLvi in lvSelectUpdate.CheckedItems)
                            {
                                lvis.Add(newLvi);
                            }
                            foreach (ListViewItem newLvi in lvis)
                            {
                                lvSelectUpdate.Items.Remove(newLvi);
                            }
                            lvDownloadUpdate.Items.AddRange(lvis.ToArray());
                            //开启下载线程
                            Thread trdDownload = new Thread(DownloadThread);
                            trdDownload.Start();
                        }

                        if (index == 2 && currentPage == 3)
                        {
                            ShowPage(1);
                            pbConnectServer.Style = ProgressBarStyle.Marquee;
                            lblConnectUpdateServer.Text = "连接更新服务器中。。。";
                            return;
                        }

                        //一般判断
                        if (index == 0)
                        {
                            btnBack.Visible = false;
                            btnNext.Visible = true;
                        }
                        else if (index >= lstPanels.Count - 2)
                        {
                            btnBack.Visible = false;
                            btnNext.Visible = false;
                            btnCanel.Text = "完成";
                        }
                        else if (index >= 4)
                        {
                            btnBack.Visible = false;
                            btnNext.Visible = false;
                        }
                        else
                        {
                            btnCanel.Text = "取消";
                        }
                        break;
                    }
                case ActionEnum.Install:
                    {
                        btnBack.Visible = false;
                        btnNext.Visible = false;

                        if (index == 0)
                        {
                            Thread trdInstall = new Thread(GetInstallSoftwareInfoThread);
                            trdInstall.Start();
                        }
                        else if (index == 1)
                        {
                            List<ListViewItem> lvis = new List<ListViewItem>();
                            foreach (ListViewItem newLvi in lvSelectUpdate.CheckedItems)
                            {
                                lvis.Add(newLvi);
                            }
                            foreach (ListViewItem newLvi in lvis)
                            {
                                lvSelectUpdate.Items.Remove(newLvi);
                            }
                            lvDownloadUpdate.Items.AddRange(lvis.ToArray());
                            //开启下载线程
                            Thread trdDownload = new Thread(DownloadThread);
                            trdDownload.Start();
                        }

                        //一般判断
                        if (index >= lstPanels.Count - 1)
                        {
                            btnCanel.Text = "完成";
                        }
                        else
                        {
                            btnCanel.Text = "取消";
                        }
                        break;
                    }
            }
            lstPanels[currentPage].Visible = false;
            lstPanels[index].Visible = true;
            currentPage = index;
        }
        #endregion

        #region 让下方三按钮不可用
        private void DisableBottomButtons()
        {
            btnBack.Enabled = false;
            btnNext.Enabled = false;
        }
        #endregion

        #region 让下方三按钮可用
        private void EnableBottomButtons()
        {
            btnBack.Enabled = true;
            btnNext.Enabled = true;
        }
        #endregion

        #region 获取软件文件列表(第一个文件为主程序文件)
        private UpdateFile[] GetSoftwareFileList(String softwareName)
        {
            List<UpdateFile> lstUpdateFiles = new List<UpdateFile>();

            //得到文件信息XML字符串
            String softwareInfoUrl = String.Format(GetSoftwareInfoUrl, softwareName);
            String softwareInfoXml = HttpHelper.GetUrlData(softwareInfoUrl, Encoding.UTF8, proxyServer, 5);


            if (String.IsNullOrEmpty(softwareInfoXml))
            {
                this.Invoke(new UnnamedDelegate(delegate()
                {
                    MessageBox.Show("未能从服务器获取到软件信息，程序退出！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(0);
                }));
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(softwareInfoXml);
            }
            catch
            {
                this.Invoke(new UnnamedDelegate(delegate()
                {
                    MessageBox.Show("错误的软件信息格式，程序退出！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(0);
                }));
            }
            XmlElement root = doc.DocumentElement;

            this.Invoke(new UnnamedDelegate(delegate()
            {
                lvSelectUpdate.Items.Clear();
                lblConnectUpdateServer.Text = "获取文件列表完成，开始检测组件版本...";
                pbConnectServer.Style = ProgressBarStyle.Blocks;
            }));

            //分析检测主程序文件版本
            SoftwareMainProgramFile = new UpdateFile();
            SoftwareMainProgramFile.FileName = root.SelectSingleNode("MainExecuteFile").InnerText;
            SoftwareMainProgramFile.FilePath = "/";
            SoftwareMainProgramFile.FileVersion = new Version(root.SelectSingleNode("MainExecuteFileVersion").InnerText);
            SoftwareMainProgramFile.FileDescription = root.SelectSingleNode("Description").InnerText;
            SoftwareMainProgramFile.Length = Int64.Parse(root.SelectSingleNode("MainExecuteFileSize").InnerText);
            SoftwareMainProgramFile.DownloadUrl = String.Format(DownloadFileUrl, SoftwareMainProgramFile.FileName);
            SoftwareMainProgramFile.Status = "等待开始";
            switch (ProgramAction)
            {
                case ActionEnum.Update:
                    {
                        SoftwareMainProgramFile.FullFileName = GetUpdateFileFullPath(SoftwareMainProgramFile);
                        break;
                    }
                case ActionEnum.Install:
                    {
                        SoftwareMainProgramFile.FullFileName = Path.Combine(SoftwarePath, SoftwareMainProgramFile.FileName);
                        break;
                    }
            }
            

            lstUpdateFiles.Add(SoftwareMainProgramFile);

            //分析检测组件版本
            XmlNodeList subAssemblyNodes = root.SelectNodes("SubAssemblys/SubAssembly");
            for (int i = 0; i <= subAssemblyNodes.Count - 1; i++)
            {
                XmlNode subAssemblyNode = subAssemblyNodes[i];

                //显示分析进度
                this.Invoke(new UnnamedDelegate(delegate()
                {
                    pbConnectServer.Value = (i + 1) * pbConnectServer.Maximum / subAssemblyNodes.Count;
                }));

                UpdateFile newFile = new UpdateFile();
                newFile.FileName = subAssemblyNode.SelectSingleNode("FileName").InnerText;
                newFile.FilePath = subAssemblyNode.SelectSingleNode("FilePath").InnerText;
                newFile.FileVersion = new Version(subAssemblyNode.SelectSingleNode("Version").InnerText);
                newFile.FileDescription = subAssemblyNode.SelectSingleNode("Description").InnerText;
                newFile.Length = Int64.Parse(subAssemblyNode.SelectSingleNode("FileSize").InnerText);
                switch (ProgramAction)
                {
                    case ActionEnum.Update:
                        {
                            newFile.FullFileName = GetUpdateFileFullPath(newFile);
                            break;
                        }
                    case ActionEnum.Install:
                        {
                            newFile.FullFileName = Path.Combine(SoftwarePath, newFile.FileName);
                            break;
                        }
                }

                newFile.DownloadUrl = String.Format(DownloadFileUrl, newFile.FileName);
                newFile.Status = "等待开始";

                lstUpdateFiles.Add(newFile);
            }

            return lstUpdateFiles.ToArray();
        }
        #endregion

        #region 获取更新文件全路径
        
        //更新软件用
        private String GetUpdateFileFullPath(UpdateFile newFile)
        {
            FileInfo newFileInfo = new FileInfo(String.Format("{0}/{1}/{2}", SoftwarePath, newFile.FilePath, newFile.FileName));
            return newFileInfo.FullName;
        }

        //下载软件用
        private String GetUpdateFileFullPath(UpdateFile newFile,String SoftwareName)
        {
            FileInfo newFileInfo = new FileInfo(String.Format("{0}/{1}/{2}/{3}", SoftwarePath, SoftwareName, newFile.FilePath, newFile.FileName));
            return newFileInfo.FullName;
        }
        #endregion

        #region 获取更新软件信息线程(包括检测分析新版本)
        private void GetUpdateSoftwareInfoThread()
        {
            UpdateFile[] updateFiles = GetSoftwareFileList(lstSoftwareName[0]);
            //分析文件是否需要更新
            foreach (UpdateFile newFile in updateFiles)
            {
                CheckUpdateFile(newFile);
            }

            this.Invoke(new UnnamedDelegate(delegate()
                    {
                        EnableBottomButtons();
                        if (lvSelectUpdate.CheckedItems.Count == 0)
                        {
                            ShowPage(lstPanels.Count - 1);
                        }
                        else
                        {
                            ShowPage(currentPage + 1);
                        }
                    }));
        }
        #endregion

        #region 获取安装软件信息线程
        private void GetInstallSoftwareInfoThread()
        {
            //获取软件信息
            foreach (String softName in lstSoftwareName)
            {
                this.Invoke(new UnnamedDelegate(delegate()
                {
                    lblConnectUpdateServer.Text = String.Format("获取软件 {0} 的信息中...", softName);
                }));
                UpdateFile[] softFiles = GetSoftwareFileList(softName);
                dictSoftFile.Add(softName, softFiles);

                foreach (UpdateFile newFile in softFiles)
                {
                    if(dictDownloadFiles.ContainsKey(newFile.FileName)) continue;
                    dictDownloadFiles.Add(newFile.FileName, newFile);
                }
            }

            //加入到选择更新文件列表
            foreach (UpdateFile newFile in dictDownloadFiles.Values)
            {
                PushNewUpdateFile(newFile);
            }

            this.Invoke(new UnnamedDelegate(delegate()
                    {
                        ShowPage(1);
                    }));
        }
        #endregion

        #region 检查分析该文件是否需要更新，如果需要则添加到列表中
        private void CheckUpdateFile(UpdateFile newFile)
        {
            FileInfo newFileInfo = new FileInfo(newFile.FullFileName);
            if (newFileInfo.Exists)
            {
                if (newFile.FileVersion <= GetFileVersion(newFileInfo))
                {
                    return;
                }
            }
            this.Invoke(new UnnamedDelegate(delegate()
            {
                PushNewUpdateFile(newFile);
            }));
        }
        #endregion

        #region 将要需要更新的文件放入选择更新列表中
        private void PushNewUpdateFile(UpdateFile newFile)
        {
            ListViewItem newLvi = lvSelectUpdate.Items.Add(newFile.FileName);
            newLvi.SubItems.Add(newFile.FileVersion.ToString());
            newLvi.SubItems.Add(IoHelper.getFileLengthLevel(newFile.Length, 2));
            newLvi.Tag = newFile;
            newLvi.Checked = true;
        }
        #endregion

        #region 点击取消或完成时
        private void btnCanel_Click(object sender, EventArgs e)
        {
            switch (ProgramAction)
            {
                case ActionEnum.Update:
                    {
                        //如果不是最后一页
                        if (currentPage < lstPanels.Count - 2)
                        {
                            if (MessageBox.Show("您确定退出在线升级程序吗？", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        break;
                    }
                case ActionEnum.Install:
                    {
                        //如果不是最后一页
                        if (currentPage < lstPanels.Count - 1)
                        {
                            if (MessageBox.Show("您确定退出在线安装程序吗？", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (cbMakeShortCutOnDesktop.Checked)
                            {
                                foreach (String key in dictSoftFile.Keys)
                                {
                                    UpdateFile[] files = dictSoftFile[key];
                                    UpdateFile mainExeFile = files[0];

                                    String mainExeFileFullPath = GetUpdateFileFullPath(mainExeFile, key);
                                    aaaSoft.Update.Helpers.ShellHelper.CreateShortCut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), key), mainExeFileFullPath, "", Path.GetDirectoryName(mainExeFileFullPath), 1, mainExeFile.FileDescription, "");
                                }
                            }
                        }
                        break;
                    }
            }
            this.Close();
        }
        #endregion

        #region 点击上一步时
        private void btnBack_Click(object sender, EventArgs e)
        {
            ShowPage(currentPage - 1);
        }
        #endregion

        #region 点击下一步时
        private void btnNext_Click(object sender, EventArgs e)
        {
            ShowPage(currentPage + 1);
        }
        #endregion

        #region 是否使用代理改变时
        private void rbProxyConnect_CheckedChanged(object sender, EventArgs e)
        {
            gbProxyServer.Visible = rbProxyConnect.Checked;
        }
        #endregion

        #region 打开链接
        private void LinkLabel_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lkl = (LinkLabel)sender;
            Process.Start(lkl.Text);
        }
        #endregion

        #region 点击测试按钮，测试代理服务器连接时
        private void btnTestProxy_Click(object sender, EventArgs e)
        {
            btnTestProxy.Enabled = false;

            WebProxy proxy = GetProxy();
            if (proxy == null)
            {
                btnTestProxy.Enabled = true;
                return;
            }

            String httpString = HttpHelper.GetUrlData("http://aaaSoft.blog.sohu.com", Encoding.Default, proxy, 0);
            if (String.IsNullOrEmpty(httpString))
            {
                MessageBox.Show("无法连接到代理服务器！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("成功连接到代理服务器！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            btnTestProxy.Enabled = true;
        }
        #endregion

        #region 获取WebProxy对象
        private WebProxy GetProxy()
        {
            int proxyPort;
            if (!Int32.TryParse(txtProxyPort.Text.Trim(), out proxyPort))
            {
                MessageBox.Show("请输入正确的端口号(0 - 65535)！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            if (proxyPort < 0 || proxyPort > 65535)
            {
                MessageBox.Show("请输入正确的端口号(0 - 65535)！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            String proxyServer = txtProxyServer.Text.Trim();
            if (String.IsNullOrEmpty(proxyServer))
            {
                MessageBox.Show("请输入代理服务器地址！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            WebProxy proxy = new WebProxy(proxyServer, proxyPort);

            //网络认证信息
            NetworkCredential nc = new NetworkCredential(txtProxyUserName.Text, txtProxyPassword.Text, txtProxyDomain.Text);
            proxy.Credentials = nc;

            return proxy;
        }
        #endregion

        #region 窗体关闭时
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentPage == 5)
            {
                if (cbRunSoftwareAfterUpdate.Checked)
                {
                    ProcessStartInfo psi = new ProcessStartInfo(SoftwareMainProgramFile.FullFileName);
                    psi.WorkingDirectory = Path.GetDirectoryName(SoftwareMainProgramFile.FullFileName);
                    Process.Start(psi);
                }                
            }
            DeleteSelf();
            Environment.Exit(0);
        }
        #endregion

        #region 删除自己
        [DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        private void DeleteSelf()
        {
            String BatString = ":del" + Environment.NewLine +
                                "del \"{0}\"" + Environment.NewLine +
                                "if exist \"{0}\" goto del" + Environment.NewLine +
                                "del %0";
            BatString = String.Format(BatString,Application.ExecutablePath);

            String BatFileName = Path.GetTempFileName() + ".bat";
            FileStream fStream = File.Create(BatFileName);
            StreamWriter writer = new StreamWriter(fStream, Encoding.Default);
            writer.Write(BatString);
            writer.Close();
            fStream.Close();
            WinExec(BatFileName, 0);
        }
        #endregion

        #region 得到文件版本
        /// <summary>
        /// 得到文件版本
        /// </summary>
        /// <param name="file">文件信息对象</param>
        /// <returns></returns>
        private Version GetFileVersion(FileInfo file)
        {
            String version;
            if (file.Extension.ToUpper().Equals(".DLL") || file.Extension.ToUpper().Equals(".EXE"))
            {
                FileVersionInfo fileVer = FileVersionInfo.GetVersionInfo(file.FullName);
                version = String.Format("{0}.{1}.{2}.{3}", fileVer.FileMajorPart, fileVer.FileMinorPart, fileVer.FileBuildPart, fileVer.FilePrivatePart);
            }
            else
            {
                DateTime modifyTime = file.LastWriteTime;
                version = String.Format("{0}.{1}.{2}.{3}", modifyTime.Year, modifyTime.Month, modifyTime.Day, (int)modifyTime.TimeOfDay.TotalSeconds);
            }
            return new Version(version);
        }
        #endregion

        #region 下载列表_画对象
        private void lvDownloadUpdate_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle AllProcessRect = e.Item.Bounds;
            int borderWidth = 1;
            AllProcessRect.X += borderWidth;
            AllProcessRect.Y += borderWidth;
            AllProcessRect.Width -= 2 * borderWidth - 1;
            AllProcessRect.Height -= 2 * borderWidth - 1 - 1;

            Rectangle BorderRect = e.Item.Bounds;
            BorderRect.Width -= 1;
            BorderRect.Height -= 1;

            UpdateFile si = (UpdateFile)e.Item.Tag;
            if (si != null)
            {
                Rectangle CurrentProcessRect = AllProcessRect;
                CurrentProcessRect.Width = Convert.ToInt32(AllProcessRect.Width * si.DownloadPercent);

                SolidBrush sb;

                //画边框
                Color BorderColor = Color.FromArgb(185, 232, 254);
                if (e.Item.Selected)
                {
                    sb = new SolidBrush(BorderColor);
                    g.FillRectangle(sb, BorderRect);
                }

                //画进度
                sb = new SolidBrush(Color.FromArgb(213, 251, 134));
                g.FillRectangle(sb, CurrentProcessRect);
                //画状态
                if (si.DownloadPercent == 1)
                {
                    sb = new SolidBrush(Color.Green);
                }
                else
                {
                    sb = new SolidBrush(Color.DarkGray);
                }
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Far;
                sf.LineAlignment = StringAlignment.Center;
                Rectangle tmpRect = AllProcessRect;
                tmpRect.Height += 2;
                g.DrawString(si.Status, this.Font, sb, tmpRect, sf);

                //画文字
                SizeF statusSize = g.MeasureString(si.Status, this.Font);
                Rectangle TextRect = e.Item.Bounds;
                TextRect.Height += 2;
                TextRect.Width -= Convert.ToInt32(statusSize.Width);
                g.DrawString(e.Item.Text, this.Font, Brushes.Black, TextRect);
            }
            else
            {
                e.DrawText();
            }
        }
        #endregion

        #region 下载线程
        private void DownloadThread()
        {
            List<ListViewItem> lvis = new List<ListViewItem>();
            this.Invoke(new UnnamedDelegate(delegate()
            {
                foreach (ListViewItem tmpLvi in lvDownloadUpdate.Items)
                {
                    lvis.Add(tmpLvi);
                }
            }));

            foreach (ListViewItem tmpLvi in lvis)
            {
                ListViewItem newLvi = tmpLvi;
                UpdateFile si = (UpdateFile)newLvi.Tag;
                this.Invoke(new UnnamedDelegate(delegate()
                {
                    newLvi.EnsureVisible();
                }));
                si.Status = "连接中...";

                //打开显示进度线程
                Thread trdDisplay = new Thread(DisplayProgressThread);
                trdDisplay.Name = "显示进度线程";
                trdDisplay.IsBackground = true;
                trdDisplay.Start(newLvi);
                si.Status = "下载中";
                //开始下载
                int rtnValue = DownloadUpdateFile(si);
                if (rtnValue != 1)
                {
                    si.Status = "错误";
                    this.Invoke(new UnnamedDelegate(delegate()
                    {
                        MessageBox.Show(String.Format("下载文件 {0} 时出错，更新失败。", si.FileName), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Environment.Exit(0);
                    }));
                }
                si.Status = "完成";
            }

            //如果是安装动作
            if (ProgramAction == ActionEnum.Install)
            {
                //分发文件
                foreach (String softName in dictSoftFile.Keys)
                {
                    UpdateFile[] files = dictSoftFile[softName];
                    foreach(UpdateFile newFile in files)
                    {
                        String newFileFullName = GetUpdateFileFullPath(newFile, softName);
                        IoHelper.CreateMultiFolder(Path.GetDirectoryName(newFileFullName));
                        if (File.Exists(newFileFullName))
                        {
                            File.Delete(newFileFullName);
                        }
                        File.Copy(newFile.FullFileName, newFileFullName);
                    }
                }

                //删除临时文件
                foreach (UpdateFile newFile in dictDownloadFiles.Values)
                {
                    File.Delete(newFile.FullFileName);
                }
            }            

            this.Invoke(new UnnamedDelegate(delegate()
            {
                ShowPage(currentPage + 1);
            }));
        }
        #endregion

        #region 显示进度线程
        private void DisplayProgressThread(Object obj)
        {
            ListViewItem newLvi = (ListViewItem)obj;
            UpdateFile si = (UpdateFile)newLvi.Tag;

            long lastPos = 0;
            DateTime lastTime = DateTime.Now;
            while (si.DownloadPercent != 1)
            {
                if (si.Length > 0)
                {
                    double seconds = (DateTime.Now - lastTime).TotalSeconds;
                    if (seconds == 0.0) continue;
                    long bytes = Convert.ToInt64((si.DownloadPercent * si.Length)) - lastPos;
                    long speed = Convert.ToInt64(bytes / seconds);
                    si.Status = String.Format("{0}/s", IoHelper.getFileLengthLevel(speed, 1));
                }
                lastTime = DateTime.Now;
                lastPos = Convert.ToInt64((si.DownloadPercent * si.Length));
                InvalidateListViewItem(newLvi);
                Thread.Sleep(1000);
            }
            si.Status = "完成";
            InvalidateListViewItem(newLvi);
        }
        #endregion

        #region 让ListViewItem重绘
        private void InvalidateListViewItem(ListViewItem newLvi)
        {
            if (newLvi.ListView != null)
            {
                this.Invoke(new UnnamedDelegate(delegate()
                {
                    newLvi.ListView.Invalidate(newLvi.Bounds);
                }));
            }
        }
        #endregion

        #region 下载更新文件
        public int DownloadUpdateFile(UpdateFile si)
        {
            return DownloadUpdateFile(si, 5);
        }
        public int DownloadUpdateFile(UpdateFile si, int retryTimes)
        {
            HttpWebRequest httpReq;
            HttpWebResponse httpResp;
            List<Cookie> Cookies = new List<Cookie>();
            Uri httpURL = new Uri(si.DownloadUrl);
            int respInfo = 0;
            Int32 n;
            httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
            httpReq.Proxy = proxyServer;
            httpReq.Method = "GET";
            httpReq.Timeout = 20 * 1000;
            httpReq.Referer = "http://www.scbeta.com";
            //检查 Cookie 信息是否为空
            if (Cookies.Count > 0)
            {
                httpReq.CookieContainer = new CookieContainer();
                for (n = 0; n <= Cookies.Count - 1; n++)
                {
                    httpReq.CookieContainer.Add(Cookies[n]);
                }
            }

            FileStream fStream = null;
            //向服务器发送请求和收取服务器回复消息
            try
            {
                httpResp = (HttpWebResponse)httpReq.GetResponse();
                //得到网络流
                Stream revStream = httpResp.GetResponseStream();
                //创建文件夹
                IoHelper.CreateMultiFolder(Path.GetDirectoryName(si.FullFileName));
                //创建文件
                fStream = new FileStream(si.FullFileName + ".tmp", FileMode.OpenOrCreate);
                fStream.SetLength(0);

                int revCount = 0;
                //缓存
                byte[] buffer = new byte[1024];
                //当前已下载字节数
                long currentCount = 0;
                //总字节数
                long totalCount = httpResp.ContentLength;
                si.Length = totalCount;
                do
                {
                    revCount = revStream.Read(buffer, 0, buffer.Length);
                    fStream.Write(buffer, 0, revCount);
                    currentCount += revCount;
                    si.DownloadPercent = Convert.ToDouble(currentCount) / totalCount;
                } while (currentCount != totalCount);

                httpResp.Close();
                fStream.Close();
                
                //删除旧文件
                if (System.IO.File.Exists(si.FullFileName))
                {
                    try
                    {
                        System.IO.File.Delete(si.FullFileName);
                    }
                    catch
                    {
                        this.Invoke(new UnnamedDelegate(delegate()
                        {
                            MessageBox.Show(String.Format("无法替换文件 {0} ，更新失败。", si.FileName), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Environment.Exit(0);
                        }));
                        return -1;
                    }
                }

                System.IO.File.Move(si.FullFileName + ".tmp", si.FullFileName);
                respInfo = 1;
            }
            catch
            {
                try
                {
                    fStream.Close();
                    System.IO.File.Delete(si.FullFileName + ".tmp");
                }
                catch { }
                if (retryTimes > 0)
                {
                    System.Diagnostics.Debug.Print("重试次数：" + retryTimes.ToString());
                    respInfo = DownloadUpdateFile(si, retryTimes - 1);
                }
            }
            finally
            {
                httpResp = null;
            }
            return respInfo;
        }
        #endregion

    }
}
