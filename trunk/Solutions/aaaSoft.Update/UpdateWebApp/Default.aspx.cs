using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data;
using System.Text;

namespace UpdateWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        SoftwareHelper softHelper;

        public String CurrentPath
        {
            get
            {
                if (Session["CurrentFolder"] == null)
                {
                    return null;
                }
                else
                {
                    return Session["CurrentFolder"].ToString();
                }
            }
            set
            {
                Session["CurrentFolder"] = value;
            }
        }

        public String RootPath
        {
            get
            {
                return Server.MapPath(".");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pFoot.InnerHtml = String.Format("UpdateServer({0}) at {1}"
                                                , System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
                                                , Request.Headers["host"]
                                                );
            if (!IsPostBack)
            {
                softHelper = new SoftwareHelper(RootPath);
                if (CurrentPath == null || !CurrentPath.Contains(RootPath))
                {
                    CurrentPath = RootPath;
                }

                if (CurrentPath.Equals(RootPath))
                {
                    lblCurrentPath.Text = "\\";
                }
                else
                {
                    lblCurrentPath.Text = CurrentPath.ToString().Substring(RootPath.Length);
                }


                if (String.IsNullOrEmpty(ClientQueryString))
                {
                    //如果没有QueryString，则显示文件列表
                    BindFolderInfoData();
                    BindFileInfoData();
                }
                else
                {
                    //动作
                    String Action = Request.QueryString["Action"];
                    if (Action == null) Action = String.Empty;

                    //更新器版本
                    String UpdaterVersion = Request.QueryString["UpdaterVersion"];

                    //写到客户端的字符串
                    String ToWriteString = String.Empty;
                    switch (Action)
                    {

                        #region 获取软件列表 - Action:GetSoftwareList
                        case "GetSoftwareList":
                            {
                                //获取软件列表

                                Response.ContentType = "text/xml";
                                ToWriteString = softHelper.GetSoftwareListXml();
                                break;
                            }
                        #endregion

                        #region 获取软件信息 - Action:GetSoftwareInfo
                        case "GetSoftwareInfo":
                            {
                                //获取软件信息

                                Response.ContentType = "text/xml";
                                String SoftwareName = Request.QueryString["SoftwareName"];
                                if (String.IsNullOrEmpty(SoftwareName))
                                {
                                    ToWriteString = "请输入软件名称";
                                }
                                else
                                {
                                    String ErrMsg;
                                    ToWriteString = softHelper.GetSoftwareInfoXml(SoftwareName, out ErrMsg);
                                    if (ToWriteString == null)
                                    {
                                        String tmpSoftwareName = String.Empty;
                                        /*
                                        SetQueryStringEncoding(Encoding.Default);
                                        tmpSoftwareName = Request.QueryString["SoftwareName"];
                                        ToWriteString = softHelper.GetSoftwareInfoXml(tmpSoftwareName);
                                         */
                                        if (ToWriteString == null)
                                        {
                                            ToWriteString = String.Format("未找到软件 {0}({1})。错误描述：{2}", SoftwareName, tmpSoftwareName, ErrMsg);
                                        }
                                    }
                                }
                                break;
                            }
                        #endregion

                        #region 获取软件LOGO - Action:GetSoftwareLogo
                        case "GetSoftwareLogo":
                            {
                                String SoftwareName = Request.QueryString["SoftwareName"];
                                String LogoSize = Request.QueryString["LogoSize"];

                                if (String.IsNullOrEmpty(SoftwareName))
                                {
                                    ToWriteString = "请输入软件名称";
                                }
                                else
                                {
                                    DirectoryInfo dir = softHelper.GetSoftwareDirectoryInfo(SoftwareName);
                                    if (dir == null)
                                    {
                                        String tmpSoftwareName = String.Empty;
                                        /*
                                        SetQueryStringEncoding(Encoding.Default);
                                        tmpSoftwareName = Request.QueryString["SoftwareName"];
                                        dir = softHelper.GetSoftwareDirectoryInfo(tmpSoftwareName);
                                         */
                                        if (dir == null)
                                        {
                                            ToWriteString = String.Format("未找到软件 {0}({1})", SoftwareName, tmpSoftwareName);
                                            break;
                                        }
                                    }
                                    String logoFileFullPath = Path.Combine(dir.FullName, "Logo.png");

                                    Response.Clear();
                                    if (File.Exists(logoFileFullPath))
                                    {
                                        Bitmap bmp = new Bitmap(logoFileFullPath);
                                        int intLogoSize = 32;
                                        if (!String.IsNullOrEmpty(LogoSize) && Int32.TryParse(LogoSize, out intLogoSize))
                                        {
                                            bmp = new Bitmap(bmp, new Size(intLogoSize, intLogoSize));
                                        }
                                        MemoryStream ms = new MemoryStream();
                                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                        ms.Position = 0;

                                        FileInfo newFileInfo = new FileInfo(logoFileFullPath);
                                        Response.ContentType = "application";
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(newFileInfo.Name, System.Text.Encoding.UTF8));
                                        Response.AddHeader("Content-Length", ms.Length.ToString());
                                        Response.BinaryWrite(ms.ToArray());
                                    }
                                    else
                                    {
                                        Response.AddHeader("Content-Length", "0");
                                    }
                                    Response.Flush();
                                    Response.Close();

                                }
                                break;
                            }
                        #endregion

                        #region 获取文件版本 - Action:GetFileVersion
                        case "GetFileVersion":
                            {
                                //获取文件版本

                                String FileName = Request.QueryString["FileName"];
                                DataTable dtFileInfos = GetFileInfos(FileName, RootPath, SearchOption.AllDirectories);
                                if (dtFileInfos.Rows.Count == 0) return;

                                String fileVersionString = dtFileInfos.Rows[0]["FileVersion"].ToString();
                                ToWriteString = fileVersionString;
                                break;
                            }
                        #endregion

                        #region 下载文件 - Action:DownloadFile
                        case "DownloadFile":
                            {
                                //下载文件
                                String FileName = Request.QueryString["FileName"];
                                String fileFullPath = String.Empty;

                                bool IsChangedEncoding = false;
                                while (true)
                                {
                                    if (!FileName.StartsWith("."))
                                    {
                                        DataTable dtFileInfos = GetFileInfos(FileName, RootPath, SearchOption.AllDirectories);
                                        if (dtFileInfos.Rows.Count != 0)
                                        {
                                            fileFullPath = dtFileInfos.Rows[0]["FileFullPath"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        fileFullPath = RootPath + FileName.Substring(1);
                                    }

                                    if (File.Exists(fileFullPath)) break;
                                    if (IsChangedEncoding) break;

                                    //SetQueryStringEncoding(Encoding.Default);
                                    FileName = Request.QueryString["FileName"];
                                    IsChangedEncoding = true;
                                }

                                if (!File.Exists(fileFullPath))
                                {
                                    ToWriteString = "未找到该文件！";
                                    break;
                                }
                                FileInfo newFileInfo = new FileInfo(fileFullPath);
                                Response.ContentType = "application";
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(newFileInfo.Name, System.Text.Encoding.UTF8));
                                Response.AddHeader("Content-Length", newFileInfo.Length.ToString());
                                Response.WriteFile(newFileInfo.FullName);
                                Response.Flush();
                                Response.Close();
                                break;
                            }
                        #endregion

                        #region 清除Session - Action:ClearSession
                        case "ClearSession":
                            {
                                //清除Session
                                Session.Clear();
                                ToWriteString = "Session已清除！";
                                break;
                            }
                        #endregion

                        default:
                            {
                                ToWriteString = "未定义的命令！";
                                break;
                            }
                    }

                    if (!String.IsNullOrEmpty(ToWriteString))
                    {
                        Response.Charset = "utf-8";
                        Response.AddHeader("Content-Length", Encoding.UTF8.GetByteCount(ToWriteString).ToString());
                        Response.Write(ToWriteString);
                        Response.Flush();
                        Response.Close();
                    }
                }
            }
        }

        protected void lklGoto_Click(Object sender, EventArgs e)
        {
            IButtonControl lb = (IButtonControl)sender;
            if (lb.CommandName.Equals(".."))
            {
                CurrentPath = new DirectoryInfo(CurrentPath).Parent.FullName;
            }
            else
            {
                CurrentPath = CurrentPath + Path.DirectorySeparatorChar + lb.CommandName;
            }
            Response.Redirect(".");
        }

        #region 获取文件夹信息
        private DataTable GetFolderInfo()
        {
            DataTable dtDlls = new DataTable();
            dtDlls.Columns.Add("FileName");
            dtDlls.Columns.Add("FileVersion");
            dtDlls.Columns.Add("LastWriteTime");
            dtDlls.Columns.Add("FileLength");
            dtDlls.Columns.Add("FileFullPath");

            if (!CurrentPath.Equals(RootPath))
            {
                dtDlls.Rows.Add("..",
                                "",
                                "",
                                "",
                                "");
            }

            DirectoryInfo updateDir = new DirectoryInfo(CurrentPath);
            if (!updateDir.Exists)
            {
                return dtDlls;
            }
            DirectoryInfo[] dirs = updateDir.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                dtDlls.Rows.Add(dir.Name,
                    "",
                    dir.LastWriteTime.ToShortDateString() + " " + dir.LastWriteTime.ToShortTimeString(),
                    "",
                    dir.FullName);
            }
            return dtDlls;
        }
        #endregion

        #region 获取文件信息

        private FileInfo GetFileInfo(String searchPattern, String FolderPath, SearchOption searchOption)
        {
            DirectoryInfo updateDir = new DirectoryInfo(FolderPath);
            FileInfo[] files = updateDir.GetFiles(searchPattern, searchOption);
            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="searchPattern">搜索字符串</param>
        /// <returns>包含文件信息的数据表</returns>
        private DataTable GetFileInfos(String searchPattern, String FolderPath, SearchOption searchOption)
        {
            DataTable dtDlls = new DataTable();
            if (searchPattern == null) return dtDlls;

            dtDlls.Columns.Add("FileName");
            dtDlls.Columns.Add("FileVersion");
            dtDlls.Columns.Add("LastWriteTime");
            dtDlls.Columns.Add("FileLength");
            dtDlls.Columns.Add("FileFullPath");
            dtDlls.Columns.Add("RelativeFilePath");

            DirectoryInfo updateDir = new DirectoryInfo(FolderPath);
            if (!updateDir.Exists)
            {
                return dtDlls;
            }
            FileInfo[] files = updateDir.GetFiles(searchPattern, searchOption);

            foreach (FileInfo file in files)
            {
                String version = softHelper.GetFileVersion(file);
                dtDlls.Rows.Add(file.Name,
                                version,
                                file.LastWriteTime.ToShortDateString() + " " + file.LastWriteTime.ToShortTimeString(),
                                file.Length,
                                file.FullName,
                                "." + file.FullName.Substring(RootPath.Length));
            }
            return dtDlls;
        }
        #endregion

        #region 绑定文件夹信息列表
        private void BindFolderInfoData()
        {
            rptFolders.DataSource = GetFolderInfo();
            rptFolders.DataBind();
        }
        #endregion

        #region 绑定文件信息列表
        /// <summary>
        /// 绑定文件信息列表
        /// </summary>
        private void BindFileInfoData()
        {
            rptFiles.DataSource = GetFileInfos("*", CurrentPath, SearchOption.TopDirectoryOnly);
            rptFiles.DataBind();
        }
        #endregion

    }
}