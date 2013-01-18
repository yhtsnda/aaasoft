using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using aaaSoft.Helpers;

namespace MultiserverFtpClient.Helpers
{
    public class UpdateHelper
    {
        static String UpdaterFileName = "aaaSoft.Update.exe";

        public static void StartUpdate
            (
                String UpdateApiUrl,
                String SoftwareName,
                String SoftwarePath,
                WebProxy Proxy,
                Int32 ProcessId
            )
        {
            StartUpdate(UpdateApiUrl, SoftwareName, SoftwarePath, Proxy, ProcessId, "", "", "");
        }

        public static void StartUpdate
            (
                String UpdateApiUrl,
                String SoftwareName,
                String SoftwarePath,
                WebProxy Proxy,
                Int32 ProcessId,
                String AboutUsText,
                String AboutUsName,
                String AboutUsUrl
            )
        {
            //生成XML文件
            String xml = MakeUpdateXml(UpdateApiUrl, "Update", SoftwarePath, Proxy, new String[] { SoftwareName }, ProcessId, AboutUsText, AboutUsName, AboutUsUrl);
            String tmpXmlFileName = Path.GetTempFileName();
            File.WriteAllText(tmpXmlFileName, xml, new UTF8Encoding(false));

            //释放更新程序
            File.WriteAllBytes(UpdaterFileName, Properties.Resources.aaaSoft_Update);
            Process prc = Process.Start(UpdaterFileName, String.Format("\"{0}\"", tmpXmlFileName));
        }

        public static String MakeUpdateXml
            (
                String UpdateApiUrl,
                String Action,
                String SoftwarePath,
                WebProxy Proxy,
                String[] SoftwareNameList,
                Int32 ProcessId,

                String AboutUsText,
                String AboutUsName,
                String AboutUsUrl
            )
        {
            XmlTreeNode root = new XmlTreeNode("aaaSoft.Updater");
            root.AddItem("UpdateApiUrl", UpdateApiUrl);
            root.AddItem("Action", Action);
            root.AddItem("Folder", SoftwarePath);
            root.AddItem("AboutUsText", AboutUsText);
            root.AddItem("AboutUsName", AboutUsName);
            root.AddItem("AboutUsUrl", AboutUsUrl);
            //代理
            if (Proxy != null)
            {
                XmlTreeNode tnProxy = root.AddItem("ProxySetting");
                tnProxy.AddItem("ProxyHost", Proxy.Address.Host);
                tnProxy.AddItem("ProxyPort", Proxy.Address.Port.ToString());
                if (Proxy.Credentials != null)
                {
                    NetworkCredential nc = (NetworkCredential)Proxy.Credentials;
                    tnProxy.AddItem("ProxyUserName", nc.UserName);
                    tnProxy.AddItem("ProxyPassword", nc.Password);
                    tnProxy.AddItem("ProxyDomain", nc.Domain);
                }
            }
            XmlTreeNode tnSoftwareList = root.AddItem("SoftwareList");
            foreach (var SoftwareName in SoftwareNameList)
            {
                var tnSoftware = tnSoftwareList.AddItem("Software");
                tnSoftware.AddItem("Name", SoftwareName);
            }
            if (ProcessId > 0)
                root.AddItem("ProcessId", ProcessId.ToString());
            return XmlTreeNode.GenerateXml(root, Encoding.UTF8);
        }

        /*
        delegate void UnnamedDelegate();
        public static void CheckUpdate(System.Windows.Forms.Form faForm, WebProxy proxy, String SoftwareMainExeFileName, String SoftwareName, String SoftwarePath, int ProcessId)
        {
            string html = HttpHelper.GetUrlData("http://www.scbeta.com/update/?Action=GetFileVersion&FileName=" + SoftwareMainExeFileName, Encoding.UTF8, proxy, 5);
            if (String.IsNullOrEmpty(html)) return;
            try
            {
                Version serverVersion = new Version(html);
                Version clientVersion = new Version(System.Windows.Forms.Application.ProductVersion);
                //不用下面语句的原因：Application.ExcutablePath获取形如%e8%34路径时会转换而导致找不到文件
                //FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion

                if (serverVersion > clientVersion)
                {
                    DialogResult dr = DialogResult.Cancel;
                    faForm.Invoke(new UnnamedDelegate(delegate()
                    {
                        dr = System.Windows.Forms.MessageBox.Show(String.Format("发现更新服务器上有更新版本({0})，现在开始在线升级？", serverVersion.ToString()), SoftwareName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    }));
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        UpdateHelper.StartUpdate(SoftwareName, SoftwarePath, proxy, ProcessId);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Error From CheckUpdateThread.Reason:" + ex.Message);
            }
        }
         */
    }
}
