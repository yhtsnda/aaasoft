using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using aaaSoft.Net.Base;
using System.Net.Sockets;
using aaaSoft.Helpers;
using System.Diagnostics;
using System.Threading;
using aaaSoft.Net.Http;
using System.Net;

namespace ShanZhaiF5
{
    public partial class MainForm : Form
    {
        //最后修改时间
        private DateTime lastModifyTime = DateTime.Now;
        private HttpServer httpServer;
        //最后的目录路径
        private String lastFolderPath;
        //配置文件路径
        private String configFilePath = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),"aaaSoft/ShanZhaiF5/config.xml");
        
        public MainForm()
        {
            InitializeComponent();
        }
        
        

        private void MainForm_Load(object sender, EventArgs e)
        {
            //初始化图标资源
            DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, "Resources"));
            lock (ilFileIcon)
            {
                foreach (var file in di.GetFiles("*.png"))
                {
                    String key = Path.GetFileNameWithoutExtension(file.Name);
                    Image value = Image.FromFile(file.FullName);
                    if (ilFileIcon.Images.ContainsKey(key))
                        ilFileIcon.Images.RemoveByKey(key);
                    ilFileIcon.Images.Add(key, value);
                }
            }
            //初始化HttpServer
            httpServer = new HttpServer("", IPAddress.Loopback, 80);
            httpServer.BeforeWriteResponse += new EventHandler<HttpServer.BeforeWriteResponseEventArgs>(httpServer_BeforeWriteResponse);
            try
            {
                httpServer.Start();
            }
            catch
            {
                var dr = MessageBox.Show("本地80端口打开失败，是否以随机端口启动？", Application.ProductName, MessageBoxButtons.OKCancel);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    Environment.Exit(0);
                    return;
                }
                httpServer.Port = 0;
                httpServer.Start();
            }
            txtWebRootUrl.Text = httpServer.GetWebRootUrl();
            
            //读取配置文件
            if(File.Exists(configFilePath)){
            	String xml = File.ReadAllText(configFilePath,Encoding.UTF8);
            	XmlTreeNode rootNode = XmlTreeNode.FromXml(xml);
            	String tmpStr = rootNode.GetItemValue("LastFolderPath");
            	if(!String.IsNullOrEmpty(tmpStr))
            		openFolder(tmpStr);
            }
        }

        void httpServer_BeforeWriteResponse(object sender, HttpServer.BeforeWriteResponseEventArgs e)
        {
            if (e.ContentType.StartsWith("text/html"))
            {
                if (e.Path.StartsWith("SHANZHAIF5_"))
                {
                    if (e.Path == "SHANZHAIF5_LASTMODIFYTIME.html")
                    {
                        XmlTreeNode rootNode = new XmlTreeNode("root");
                        rootNode.AddItem("LastModifyTime", lastModifyTime.Ticks.ToString());

                        e.DataStream = getStream(rootNode.ToXml());
                        e.ContentType = "text/xml";
                    }
                }
                else
                {
                    StreamReader reader = new StreamReader(e.DataStream, e.ContentEncoding);
                    String html = reader.ReadToEnd();
                    reader.Close();
                    e.DataStream.Close();

                    StringBuilder sb = new StringBuilder();
                    sb.Append(html);
                    sb.Append(String.Format("<script>var clientLastModifyTime = {0};</script>", lastModifyTime.Ticks));

                    sb.Append(@"
<script>
function SHANZHAIF5_GetXmlHttpObject(){
    var xmlHttp=null;
    try{
        // Firefox, Opera 8.0+, Safari
        xmlHttp=new XMLHttpRequest();
    }catch (e){
        // Internet Explorer
        try{
            xmlHttp=new ActiveXObject('Msxml2.XMLHTTP');
        }catch (e){
            xmlHttp=new ActiveXObject('Microsoft.XMLHTTP');
        }
    }
    return xmlHttp;
}
function SHANZHAIF5_CheckPageModifyTime(){
    var xmlHttp = SHANZHAIF5_GetXmlHttpObject();
    xmlHttp.onreadystatechange = function(){
        if (xmlHttp.readyState == 4){
            if (xmlHttp.status == 200){
                var datas = xmlHttp.responseXML.getElementsByTagName('LastModifyTime');
                var serverLastModifyTime = datas[0].childNodes[0].nodeValue;
                if(clientLastModifyTime != serverLastModifyTime){
                    location.reload();
                }
            }
        }
    }
    xmlHttp.open('GET','/SHANZHAIF5_LASTMODIFYTIME.html',true);
    xmlHttp.setRequestHeader('If-Modified-Since','0');
    xmlHttp.send(null);
}
window.setInterval(SHANZHAIF5_CheckPageModifyTime,1000);
</script>");
                    e.DataStream = getStream(sb.ToString(), e.ContentEncoding);
                }
            }
        }

        private Stream getStream(String text)
        {
            return getStream(text, Encoding.UTF8);
        }
        private Stream getStream(String text, Encoding encoding)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms, encoding);
            writer.Write(text);
            writer.Flush();
            ms.Position = 0;
            return ms;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            httpServer.Stop();
            //保存配置文件
            XmlTreeNode rootNode = new XmlTreeNode("root");
            rootNode.AddItem("LastFolderPath",lastFolderPath);
            Encoding utf8Encoding = new UTF8Encoding(false);
            String xml = rootNode.ToXml(utf8Encoding);
            IoHelper.CreateMultiFolder(Path.GetDirectoryName(configFilePath));
            File.WriteAllText(configFilePath,xml,Encoding.UTF8);
            
            Environment.Exit(0);
        }


        private void lblTip_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择WEB根目录：";
            var dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
                return;
            openFolder(fbd.SelectedPath);
        }

        private void tvFile_DragEnter(object sender, DragEventArgs e)
        {
            String[] PathArray = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (PathArray == null || PathArray.Length != 1) return;
            String path = PathArray[0];
            if (System.IO.File.Exists(path) || System.IO.Directory.Exists(path))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void tvFile_DragDrop(object sender, DragEventArgs e)
        {
            String[] PathArray = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (PathArray == null || PathArray.Length != 1) return;
            String path = PathArray[0];

            String webRootPath = null;
            if (System.IO.File.Exists(path))
            {
                webRootPath = new FileInfo(path).DirectoryName;
            }
            else if (System.IO.Directory.Exists(path))
            {
                webRootPath = path;
            }
            openFolder(webRootPath);
        }

        private void openFolder(String webRootPath)
        {
        	lastFolderPath = webRootPath;
        	lblCurrentFolderPath.Text = "当前路径：" + webRootPath;
            httpServer.RootFolderPath = webRootPath;
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(webRootPath);
            tvFile.Nodes.Clear();
            addFolder(dir, tvFile.Nodes);
            //更改监控目录
            fswWebFolder.EnableRaisingEvents = false;
            fswWebFolder.Path = webRootPath;
            fswWebFolder.EnableRaisingEvents = true; ;
            //刷新时间
            refreshLashModifyTime();

            lblTip.Visible = false;
            tvFile.Visible = true;
            lblCurrentFolderPath.Visible = true;
            btnChangeFolder.Visible  = true;
        }

        private void addFolder(System.IO.DirectoryInfo directoryInfo, TreeNodeCollection nodes)
        {
            foreach (var directory in directoryInfo.GetDirectories())
            {
                String imageKey = "folder";
                TreeNode node = nodes.Add(directory.FullName, directory.Name, imageKey, imageKey);
                addFolder(directory, node.Nodes);
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                String imageKey = "file";
                String key = file.Extension;
                if (!String.IsNullOrEmpty(key) && key.Length > 0)
                {
                    key = key.Substring(1);
                    if (ilFileIcon.Images.ContainsKey(key))
                        imageKey = key;
                }
                nodes.Add(file.FullName, file.Name, imageKey, imageKey);
            }
        }

        private void tvFile_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (tvFile.SelectedNode == null)
                    return;
                string[] files = new String[] { tvFile.SelectedNode.Name };
                DataObject data = new DataObject(DataFormats.FileDrop, files);
                data.SetData(DataFormats.StringFormat, files[0]);
                DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        private void tvFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tvFile.SelectedNode != null)
                在浏览器中访问ToolStripMenuItem_Click(sender, e);
        }

        //刷新最后更改时间
        private void refreshLashModifyTime()
        {
            lastModifyTime = DateTime.Now;
        }

        private void fswWebFolder_Changed(object sender, FileSystemEventArgs e)
        {
            //刷新最后更改时间
            refreshLashModifyTime();
            
            
            //更新列表
            tvFile.Nodes.Clear();
            DirectoryInfo dir = new DirectoryInfo(lastFolderPath);
            addFolder(dir, tvFile.Nodes);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            Process.Start(httpServer.GetWebRootUrl());
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此程序是根据F5(http://getf5.com)的视频编写的，其创意归原作者所有。本程序仅作学习交流之用途。如有疑问请给我发Email:scbeta@qq.com。2011-9-14 12:04", "关于 山寨F5");
        }

        private void tvFile_MouseDown(object sender, MouseEventArgs e)
        {
            tvFile.SelectedNode = tvFile.GetNodeAt(e.Location);
        }

        private void cmsFile_Opening(object sender, CancelEventArgs e)
        {
            if (tvFile.SelectedNode == null)
                e.Cancel = true;
        }

        private void 用记事本打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", tvFile.SelectedNode.Name);
        }

        private void 在浏览器中访问ToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            Process.Start(httpServer.GetWebRootUrl() + tvFile.SelectedNode.FullPath);
        }
        
        void BtnChangeFolderClick(object sender, EventArgs e)
        {
        	lblTip_Click(sender,e);
        }
    }
}
