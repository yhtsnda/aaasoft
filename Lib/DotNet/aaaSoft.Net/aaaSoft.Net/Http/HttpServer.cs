using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using aaaSoft.Net.Base;
using System.Threading;
using aaaSoft.Helpers;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Reflection;

namespace aaaSoft.Net.Http
{
    /// <summary>
    /// 简易HTTP服务器
    /// </summary>
    public class HttpServer
    {
        private IPAddress ipAddress = IPAddress.Any;
        private Int32 port = 80;
        private TcpIpServer tcpIpServer;
        private IDictionary<String, String> resourceMimeDict;
        private String folderPngBase64String = "iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAADdgAAA3YBfdWCzAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAHiSURBVDiNtZU/a1RREMXPzNy597234pIYcQVhkQVNY2dlZWGlreUWloKloJ/B2jaVjSiIH0KQVFqpsDHRwGpCiO6fmPiyb98bi2UXxAirNznVXLj8OHfmMJfMDCchPhEqADct7q10PnzfL5an5zRI7+xSevPR7ebq/4Bp2or24/eHt66e80Enj1jfPsCb9b4ZMHevTgXXX7l7+cxvjoUZzDS71GpkaDUyAkB/Io7Wi9dbi9N6BlYVKkpDzDC997OartxfrcxsblfziAjmiEDt6xcHpzNfl8iMlBWwuzfqPX/1ecGpSPV2syjrNQemuEyXRvjxs6xUpHJetSBiSYIHzR+AI2Ug7Ocj8erGLgmaE4HTIFGDm4hAZJIEzV0adI+oWgxKqKo4MDOBAM6CDl0WfJ+JltJAGJdx4XBCIDLOgu+5Wqo7MLuUKKPgOMcqDCJwluqOqwXpHuQFBbXo4Xk1MIyT4Lqulsin/HDEygUgVZxjNjAZ1xLZcMqjj8LGXsbgSMdOKjCDg9qaG3TfbUh9Gd+GI5RlnGMRhjAwHn7tUOvajYU7D5/uNhv1Y1n6m9uD6uWTB3UCIADaTrWJf1iRf5GNi6ID4BmZGYjoAoDzsW4x+RS+mNnWFMwAkmMC52ZmvwCPdapgffx8mwAAAABJRU5ErkJggg==";
        private String filePngBase64String = "iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAADdgAAA3YBfdWCzAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAN2SURBVDiNtZU9b9tWFIYfildURH3VVC0Jtgw4bop4adQhS/5Fg85dOxXZ8hM8+X8U8NKmXYKicxBkTeAArlPINqw4JhRRokXx+54ulSF/KEDR9gAvQIIXD9/z8vBeQ0T4P0ot3hiG8QXw9b9kvgH+QEQuBXybZZmEUSRRFEsc/60kkeS60lSSNJV0QXmey6NHj34ACur666I4ZjQaMRgMmM1mDIdDWq0WSim63S5pkpDnOaPRCMdxWF9fR4ugtebi4uKSU7gONgyDYrFIt9ulUqmwsbFBtVIhmE6pVioUCgWMQoGvHjygVqvheR5JkjA4PaXT6dyeMYA3GtE/OkIpxSwIKJgmY8+jVqtxcHBAlmWYSvFZo4FSiiAImIUh1VqNs/fvlzs2TRPbtplMJrTabRBhxXEQYH19HcuysG0bwzCYTqcUCgWKSpFn2RXHN8BFy8L3fYrFIv5kQtm2icIQZZrMwhCAkmVxfn6O53lkWcb+/j7T6ZThcLgcfOH7DAYD/MmEJE1Jk4QwDKlWq4w+fiSO40tFUcRkMsFxHFzXRWu9PGOlFFt375JrTcmyKJgmjUaDOI6xbZtSqQSAiFCv1+l0OriuS6PRoNlsLnccxzGmaTI4PcXzPPIsYxoETKdTjo6O8H2fUqlEGIbEcczZ2RkAzWYTwzCWO15bW6NcLtNsNllxHPIsu2y51+vh+z7lcplGo0EURSilsCyL4XBItVr9dBRhGPLy5Us+X12lXqtxfHxMr9fjxYsXlMtlVldXsSyLwWDAnTt3cF2XVqtFt9tdDhYRWq0W3zx+jM5zoiji4cOHGIZBp9OhXq9fjprjOIgI29vb2LaNUmo5eP/tW/589444jtna2iIIAgzDYDabEYYhzWYT0zQxTZNKpYJlWbiuy+bmJisrK/GHDx/GgNwAf3nvHtv375MkyeWPMP8o8+v5nHuex8nJCe12myAIZG9v76d+v/+bLO5s890tTVOZzWYSRZEkSSJpmkqWZZLnuWitRWstURTJeDyW16/fiNZaDg8PZXd39xegPWfdGLdFZ7eV1po8z8myDMOAfr/P8+fPf3/69On3InI+X3cr+FM1d6S1RkTk52fPfn3y5Ml3i9ArC+dR5Hl+pf3FCEREtNaSJImMx+N4Z2fnx8X2F2Usnnn/8GhKgFci4t728Ar4v6y/AGs3Q8YQBn2SAAAAAElFTkSuQmCC";

        private Byte[] folderPngByteArray;
        private Byte[] filePngByteArray;

        public String RootFolderPath { get; set; }
        public IPAddress IPAddress { get { return ipAddress; } }
        public Int32 Port { get { return port; } set { port = value; } }
        /// <summary>
        /// 版权信息
        /// </summary>
        public String CopyRight { get; set; }

        public String GetWebRootUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("http://");
            sb.Append(ipAddress.ToString());
            if (port != 80)
            {
                sb.Append(":" + port);
            }
            sb.Append("/");
            return sb.ToString();
        }
        //=======================
        //      事件部分
        //=======================
        public event EventHandler<BeforeWriteResponseEventArgs> BeforeWriteResponse;

        public class BeforeWriteResponseEventArgs : EventArgs
        {
            /// <summary>
            /// 请求路径
            /// </summary>
            public String Path { get; set; }
            /// <summary>
            /// 内容编码
            /// </summary>
            public Encoding ContentEncoding { get; set; }
            /// <summary>
            /// 内容类型
            /// </summary>
            public String ContentType { get; set; }
            /// <summary>
            /// 内容流
            /// </summary>
            public Stream DataStream { get; set; }
        }

        /// <summary>
        /// 设置MIME
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="type"></param>
        public void SetMIME(String extension, String type)
        {
            lock (resourceMimeDict)
            {
                if (resourceMimeDict.ContainsKey(extension))
                {
                    resourceMimeDict.Remove(extension);
                }
                resourceMimeDict.Add(extension, type);
            }
        }

        public HttpServer(String rootFolderPath)
        {
            init(rootFolderPath, IPAddress.Any, 80);
        }

        public HttpServer(String rootFolderPath, Int32 port)
        {
            init(rootFolderPath, IPAddress.Any, port);
        }

        public HttpServer(String rootFolderPath, String ipAddress, Int32 port)
        {
            init(rootFolderPath, IPAddress.Parse(ipAddress), port);
        }
        public HttpServer(String rootFolderPath, IPAddress ipAddress, Int32 port)
        {
            init(rootFolderPath, ipAddress, port);
        }
        private void init(String rootFolderPath, IPAddress ipAddress, Int32 port)
        {
            this.RootFolderPath = rootFolderPath;
            this.ipAddress = ipAddress;
            tcpIpServer = new TcpIpServer();
            tcpIpServer.IPAddress = IPAddress;
            tcpIpServer.TcpListenPort = port;
            tcpIpServer.NewTcpConnected += new EventHandler<Base.EventArgs.NewTcpConnectedArgs>(tcpIpServer_NewTcpConnected);

            Assembly assembly = Assembly.GetExecutingAssembly();
            CopyRight = String.Format("<p style=\"color:#C0C0C0;font-family:黑体;font-size:8;text-align:center;\">Powered by {0}/{1} ,<a href=\"http://www.scbeta.com\">作者网站</a>&nbsp;<a href=\"mailto:scbeta@qq.com\">作者邮箱</a></p>", this.GetType().FullName, assembly.GetName().Version);
            folderPngByteArray = Convert.FromBase64String(folderPngBase64String);
            filePngByteArray = Convert.FromBase64String(filePngBase64String);
            init_mime();
        }

        private void init_mime()
        {
            resourceMimeDict = new Dictionary<String, String>();

            resourceMimeDict.Add(".323", "text/h323");
            resourceMimeDict.Add(".acx", "application/internet-property-stream");
            resourceMimeDict.Add(".ai", "application/postscript");
            resourceMimeDict.Add(".aif", "audio/x-aiff");
            resourceMimeDict.Add(".aifc", "audio/x-aiff");
            resourceMimeDict.Add(".aiff", "audio/x-aiff");
            resourceMimeDict.Add(".asf", "video/x-ms-asf");
            resourceMimeDict.Add(".asr", "video/x-ms-asf");
            resourceMimeDict.Add(".asx", "video/x-ms-asf");
            resourceMimeDict.Add(".au", "audio/basic");
            resourceMimeDict.Add(".avi", "video/x-msvideo");
            resourceMimeDict.Add(".axs", "application/olescript");
            resourceMimeDict.Add(".bas", "text/plain");
            resourceMimeDict.Add(".bcpio", "application/x-bcpio");
            resourceMimeDict.Add(".bin", "application/octet-stream");
            resourceMimeDict.Add(".bmp", "image/bmp");
            resourceMimeDict.Add(".c", "text/plain");
            resourceMimeDict.Add(".cat", "application/vnd.ms-pkiseccat");
            resourceMimeDict.Add(".cdf", "application/x-cdf");
            resourceMimeDict.Add(".cer", "application/x-x509-ca-cert");
            resourceMimeDict.Add(".class", "application/octet-stream");
            resourceMimeDict.Add(".clp", "application/x-msclip");
            resourceMimeDict.Add(".cmx", "image/x-cmx");
            resourceMimeDict.Add(".cod", "image/cis-cod");
            resourceMimeDict.Add(".cpio", "application/x-cpio");
            resourceMimeDict.Add(".crd", "application/x-mscardfile");
            resourceMimeDict.Add(".crl", "application/pkix-crl");
            resourceMimeDict.Add(".crt", "application/x-x509-ca-cert");
            resourceMimeDict.Add(".csh", "application/x-csh");
            resourceMimeDict.Add(".css", "text/css");
            resourceMimeDict.Add(".dcr", "application/x-director");
            resourceMimeDict.Add(".der", "application/x-x509-ca-cert");
            resourceMimeDict.Add(".dir", "application/x-director");
            resourceMimeDict.Add(".dll", "application/x-msdownload");
            resourceMimeDict.Add(".dms", "application/octet-stream");
            resourceMimeDict.Add(".doc", "application/msword");
            resourceMimeDict.Add(".dot", "application/msword");
            resourceMimeDict.Add(".dvi", "application/x-dvi");
            resourceMimeDict.Add(".dxr", "application/x-director");
            resourceMimeDict.Add(".eps", "application/postscript");
            resourceMimeDict.Add(".etx", "text/x-setext");
            resourceMimeDict.Add(".evy", "application/envoy");
            resourceMimeDict.Add(".exe", "application/octet-stream");
            resourceMimeDict.Add(".fif", "application/fractals");
            resourceMimeDict.Add(".flr", "x-world/x-vrml");
            resourceMimeDict.Add(".gif", "image/gif");
            resourceMimeDict.Add(".gtar", "application/x-gtar");
            resourceMimeDict.Add(".gz", "application/x-gzip");
            resourceMimeDict.Add(".h", "text/plain");
            resourceMimeDict.Add(".hdf", "application/x-hdf");
            resourceMimeDict.Add(".hlp", "application/winhlp");
            resourceMimeDict.Add(".hqx", "application/mac-binhex40");
            resourceMimeDict.Add(".hta", "application/hta");
            resourceMimeDict.Add(".htc", "text/x-component");
            resourceMimeDict.Add(".htm", "text/html");
            resourceMimeDict.Add(".html", "text/html");
            resourceMimeDict.Add(".htt", "text/webviewhtml");
            resourceMimeDict.Add(".ico", "image/x-icon");
            resourceMimeDict.Add(".ief", "image/ief");
            resourceMimeDict.Add(".iii", "application/x-iphone");
            resourceMimeDict.Add(".ins", "application/x-internet-signup");
            resourceMimeDict.Add(".isp", "application/x-internet-signup");
            resourceMimeDict.Add(".jfif", "image/pipeg");
            resourceMimeDict.Add(".jpe", "image/jpeg");
            resourceMimeDict.Add(".jpeg", "image/jpeg");
            resourceMimeDict.Add(".jpg", "image/jpeg");
            resourceMimeDict.Add(".js", "application/x-javascript");
            resourceMimeDict.Add(".latex", "application/x-latex");
            resourceMimeDict.Add(".lha", "application/octet-stream");
            resourceMimeDict.Add(".lsf", "video/x-la-asf");
            resourceMimeDict.Add(".lsx", "video/x-la-asf");
            resourceMimeDict.Add(".lzh", "application/octet-stream");
            resourceMimeDict.Add(".m13", "application/x-msmediaview");
            resourceMimeDict.Add(".m14", "application/x-msmediaview");
            resourceMimeDict.Add(".m3u", "audio/x-mpegurl");
            resourceMimeDict.Add(".man", "application/x-troff-man");
            resourceMimeDict.Add(".mdb", "application/x-msaccess");
            resourceMimeDict.Add(".me", "application/x-troff-me");
            resourceMimeDict.Add(".mht", "message/rfc822");
            resourceMimeDict.Add(".mhtml", "message/rfc822");
            resourceMimeDict.Add(".mid", "audio/mid");
            resourceMimeDict.Add(".mny", "application/x-msmoney");
            resourceMimeDict.Add(".mov", "video/quicktime");
            resourceMimeDict.Add(".movie", "video/x-sgi-movie");
            resourceMimeDict.Add(".mp2", "video/mpeg");
            resourceMimeDict.Add(".mp3", "audio/mpeg");
            resourceMimeDict.Add(".mpa", "video/mpeg");
            resourceMimeDict.Add(".mpe", "video/mpeg");
            resourceMimeDict.Add(".mpeg", "video/mpeg");
            resourceMimeDict.Add(".mpg", "video/mpeg");
            resourceMimeDict.Add(".mpp", "application/vnd.ms-project");
            resourceMimeDict.Add(".mpv2", "video/mpeg");
            resourceMimeDict.Add(".ms", "application/x-troff-ms");
            resourceMimeDict.Add(".mvb", "application/x-msmediaview");
            resourceMimeDict.Add(".nws", "message/rfc822");
            resourceMimeDict.Add(".oda", "application/oda");
            resourceMimeDict.Add(".p10", "application/pkcs10");
            resourceMimeDict.Add(".p12", "application/x-pkcs12");
            resourceMimeDict.Add(".p7b", "application/x-pkcs7-certificates");
            resourceMimeDict.Add(".p7c", "application/x-pkcs7-mime");
            resourceMimeDict.Add(".p7m", "application/x-pkcs7-mime");
            resourceMimeDict.Add(".p7r", "application/x-pkcs7-certreqresp");
            resourceMimeDict.Add(".p7s", "application/x-pkcs7-signature");
            resourceMimeDict.Add(".pbm", "image/x-portable-bitmap");
            resourceMimeDict.Add(".pdf", "application/pdf");
            resourceMimeDict.Add(".pfx", "application/x-pkcs12");
            resourceMimeDict.Add(".pgm", "image/x-portable-graymap");
            resourceMimeDict.Add(".pko", "application/ynd.ms-pkipko");
            resourceMimeDict.Add(".pma", "application/x-perfmon");
            resourceMimeDict.Add(".pmc", "application/x-perfmon");
            resourceMimeDict.Add(".pml", "application/x-perfmon");
            resourceMimeDict.Add(".pmr", "application/x-perfmon");
            resourceMimeDict.Add(".pmw", "application/x-perfmon");
            resourceMimeDict.Add(".png", "image/png");
            resourceMimeDict.Add(".pnm", "image/x-portable-anymap");
            resourceMimeDict.Add(".pot", "application/vnd.ms-powerpoint");
            resourceMimeDict.Add(".ppm", "image/x-portable-pixmap");
            resourceMimeDict.Add(".pps", "application/vnd.ms-powerpoint");
            resourceMimeDict.Add(".ppt", "application/vnd.ms-powerpoint");
            resourceMimeDict.Add(".prf", "application/pics-rules");
            resourceMimeDict.Add(".ps", "application/postscript");
            resourceMimeDict.Add(".pub", "application/x-mspublisher");
            resourceMimeDict.Add(".qt", "video/quicktime");
            resourceMimeDict.Add(".ra", "audio/x-pn-realaudio");
            resourceMimeDict.Add(".ram", "audio/x-pn-realaudio");
            resourceMimeDict.Add(".ras", "image/x-cmu-raster");
            resourceMimeDict.Add(".rgb", "image/x-rgb");
            resourceMimeDict.Add(".rmi", "audio/mid");
            resourceMimeDict.Add(".roff", "application/x-troff");
            resourceMimeDict.Add(".rtf", "application/rtf");
            resourceMimeDict.Add(".rtx", "text/richtext");
            resourceMimeDict.Add(".scd", "application/x-msschedule");
            resourceMimeDict.Add(".sct", "text/scriptlet");
            resourceMimeDict.Add(".setpay", "application/set-payment-initiation");
            resourceMimeDict.Add(".setreg", "application/set-registration-initiation");
            resourceMimeDict.Add(".sh", "application/x-sh");
            resourceMimeDict.Add(".shar", "application/x-shar");
            resourceMimeDict.Add(".sit", "application/x-stuffit");
            resourceMimeDict.Add(".snd", "audio/basic");
            resourceMimeDict.Add(".spc", "application/x-pkcs7-certificates");
            resourceMimeDict.Add(".spl", "application/futuresplash");
            resourceMimeDict.Add(".src", "application/x-wais-source");
            resourceMimeDict.Add(".sst", "application/vnd.ms-pkicertstore");
            resourceMimeDict.Add(".stl", "application/vnd.ms-pkistl");
            resourceMimeDict.Add(".stm", "text/html");
            resourceMimeDict.Add(".svg", "image/svg+xml");
            resourceMimeDict.Add(".sv4cpio", "application/x-sv4cpio");
            resourceMimeDict.Add(".sv4crc", "application/x-sv4crc");
            resourceMimeDict.Add(".swf", "application/x-shockwave-flash");
            resourceMimeDict.Add(".t", "application/x-troff");
            resourceMimeDict.Add(".tar", "application/x-tar");
            resourceMimeDict.Add(".tcl", "application/x-tcl");
            resourceMimeDict.Add(".tex", "application/x-tex");
            resourceMimeDict.Add(".texi", "application/x-texinfo");
            resourceMimeDict.Add(".texinfo", "application/x-texinfo");
            resourceMimeDict.Add(".tgz", "application/x-compressed");
            resourceMimeDict.Add(".tif", "image/tiff");
            resourceMimeDict.Add(".tiff", "image/tiff");
            resourceMimeDict.Add(".tr", "application/x-troff");
            resourceMimeDict.Add(".trm", "application/x-msterminal");
            resourceMimeDict.Add(".tsv", "text/tab-separated-values");
            resourceMimeDict.Add(".txt", "text/plain");
            resourceMimeDict.Add(".uls", "text/iuls");
            resourceMimeDict.Add(".ustar", "application/x-ustar");
            resourceMimeDict.Add(".vcf", "text/x-vcard");
            resourceMimeDict.Add(".vrml", "x-world/x-vrml");
            resourceMimeDict.Add(".wav", "audio/x-wav");
            resourceMimeDict.Add(".wcm", "application/vnd.ms-works");
            resourceMimeDict.Add(".wdb", "application/vnd.ms-works");
            resourceMimeDict.Add(".wks", "application/vnd.ms-works");
            resourceMimeDict.Add(".wmf", "application/x-msmetafile");
            resourceMimeDict.Add(".wps", "application/vnd.ms-works");
            resourceMimeDict.Add(".wri", "application/x-mswrite");
            resourceMimeDict.Add(".wrl", "x-world/x-vrml");
            resourceMimeDict.Add(".wrz", "x-world/x-vrml");
            resourceMimeDict.Add(".xaf", "x-world/x-vrml");
            resourceMimeDict.Add(".xbm", "image/x-xbitmap");
            resourceMimeDict.Add(".xla", "application/vnd.ms-excel");
            resourceMimeDict.Add(".xlc", "application/vnd.ms-excel");
            resourceMimeDict.Add(".xlm", "application/vnd.ms-excel");
            resourceMimeDict.Add(".xls", "application/vnd.ms-excel");
            resourceMimeDict.Add(".xlt", "application/vnd.ms-excel");
            resourceMimeDict.Add(".xlw", "application/vnd.ms-excel");
            resourceMimeDict.Add(".xof", "x-world/x-vrml");
            resourceMimeDict.Add(".xpm", "image/x-xpixmap");
            resourceMimeDict.Add(".xwd", "image/x-xwindowdump");
            resourceMimeDict.Add(".z", "application/x-compress");
            resourceMimeDict.Add(".zip", "application/zip");
        }

        void tcpIpServer_NewTcpConnected(object sender, Base.EventArgs.NewTcpConnectedArgs e)
        {
            Thread trdNew = new Thread(newTcpConnectedHandleThreadFunction);
            trdNew.Start(e.getSocket());
        }

        private void newTcpConnectedHandleThreadFunction(Object obj)
        {
            Socket socket = (Socket)obj;
            NetworkStream ns = new NetworkStream(socket);
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            String path = null;
            try
            {
                while (true)
                {
                    path = null;
                    do
                    {
                        String line = reader.ReadLine().Trim();
                        if (String.IsNullOrEmpty(line))
                        {
                            break;
                        }
                        String lineToUpper = line.ToUpper();
                        if (lineToUpper.StartsWith("GET"))
                        {
                            path = StringHelper.GetMiddleString(line, " ", " ");
                            path = System.Web.HttpUtility.UrlDecode(path);
                        }
                    } while (true);

                    if (String.IsNullOrEmpty(path))
                    {
                        try { socket.Close(); }
                        catch { }
                        return;
                    }


                    //去除开始的“/”
                    while (path.StartsWith("/"))
                    {
                        path = path.Substring(1);
                    }

                    Stream dataStream = null;
                    String contentType = "application/octet-stream";
                    Encoding contentEncoding = Encoding.UTF8;
                    String otherContent = null;
                    String localPath = Path.Combine(RootFolderPath, path);
                    if (File.Exists(localPath))
                    {
                        FileInfo fi = new FileInfo(localPath);
                        dataStream = fi.Open(FileMode.Open, FileAccess.Read);
                        String ext = System.Web.VirtualPathUtility.GetExtension(path);
                        if (resourceMimeDict.ContainsKey(ext))
                        {
                            contentType = resourceMimeDict[ext];
                        }

                        if (ext == ".html" || ext == ".htm")
                        {
                            StreamReader sr = new StreamReader(dataStream, Encoding.Default, true);
                            dataStream.Position = 0;
                            contentEncoding = sr.CurrentEncoding;
                        }
                    }
                    else if (Directory.Exists(localPath))
                    {
                        DirectoryInfo di = new DirectoryInfo(localPath);

                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("<html>");
                        sb.AppendLine("<head>");
                        sb.AppendLine("</head>");
                        sb.AppendLine("<body>");
                        sb.AppendLine(String.Format("<h1 style=\"color:#C0C0C0;font-family:黑体;font-weight:bold;\">{0}</h1>", di.FullName));
                        sb.AppendLine("<table width=\"100%\" border=\"2\" cellspacing=\"0px\" rules=\"none\" bordercolor=\"#F2F2F2\">");
                        sb.AppendLine("<tbody style=\"font-size: 12px;line-height: 20px;color: #1648A7;\">");

                        if (di.Parent != null && di.FullName.Length > RootFolderPath.Length)
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td>");
                            sb.AppendLine(String.Format("<a href=\"..\"><img src=\"/AAASOFT_NET_HTTPSERVER_FOLDER.png\" border=\"0\">..</a>", di.Parent.FullName.Substring(RootFolderPath.Length)));
                            sb.AppendLine("</td>");
                            sb.AppendLine("</tr>");
                        }
                        foreach (var folder in di.GetDirectories())
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td>");
                            String tmpPath = folder.FullName.Substring(RootFolderPath.Length);
                            while (tmpPath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                            {
                                tmpPath = tmpPath.Substring(1);
                            }
                            sb.AppendLine(String.Format("<a href=\"/{0}/\"><img src=\"/AAASOFT_NET_HTTPSERVER_FOLDER.png\" border=\"0\">{1}</a>", tmpPath, folder.Name));
                            sb.AppendLine("</td>");
                            sb.AppendLine("</tr>");
                        }
                        foreach (var file in di.GetFiles())
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td>");
                            String tmpPath = file.FullName.Substring(RootFolderPath.Length);
                            while (tmpPath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                            {
                                tmpPath = tmpPath.Substring(1);
                            }
                            sb.AppendLine(String.Format("<a href=\"/{0}\"><img src=\"/AAASOFT_NET_HTTPSERVER_FILE.png\" border=\"0\">{1}</a>", tmpPath, file.Name));
                            sb.AppendLine("</td>");
                            sb.AppendLine("</tr>");
                        }
                        sb.AppendLine("</tbody>");
                        sb.AppendLine("</table>");
                        sb.AppendLine("</body>");
                        sb.AppendLine(CopyRight);
                        sb.AppendLine("</html>");

                        otherContent = sb.ToString();
                    }
                    else if (path.StartsWith("AAASOFT_NET_HTTPSERVER_"))
                    {
                        if (path == "AAASOFT_NET_HTTPSERVER_FILE.png")
                        {
                            dataStream = new MemoryStream(filePngByteArray);
                            dataStream.Position = 0;
                            contentType = "image/png";
                        }
                        else if (path == "AAASOFT_NET_HTTPSERVER_FOLDER.png")
                        {
                            dataStream = new MemoryStream(folderPngByteArray);
                            dataStream.Position = 0;
                            contentType = "image/png";
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(RootFolderPath))
                        {
                            otherContent = "没有设置RootFolderPath!";
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("未找到资源：" + path);
                            sb.Append("<a href=\"/\">点此返回根目录</a>");
                            otherContent = sb.ToString();
                        }
                    }

                    if (dataStream != null)
                        writeResponse(ns, writer, path, contentType, contentEncoding, dataStream);
                    else
                        writeResponse(ns, writer, path, otherContent);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    writeResponse(ns, writer, path, "资源不可用！原因：" + ex.Message);
                    socket.Close();
                }
                catch { }
                return;
            }
        }

        private void writeResponse(NetworkStream ns, StreamWriter writer, String path, String content)
        {
            Stream dataStream = new MemoryStream();
            StreamWriter contentWriter = new StreamWriter(dataStream);
            contentWriter.Write(content);
            contentWriter.Flush();
            dataStream.Position = 0;

            writeResponse(ns, writer, path, "text/html", Encoding.UTF8, dataStream);
        }

        private void writeResponse(NetworkStream ns, StreamWriter writer, String path, String contentType , Encoding contentEncoding, Stream dataStream)
        {
            if (BeforeWriteResponse != null)
            {
                BeforeWriteResponseEventArgs args = new BeforeWriteResponseEventArgs();
                args.Path = path;
                args.ContentType = contentType;
                args.ContentEncoding = contentEncoding;
                args.DataStream = dataStream;

                BeforeWriteResponse(this, args);

                contentType = args.ContentType;
                contentEncoding = args.ContentEncoding;
                dataStream = args.DataStream;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("HTTP/1.1 200 OK");
            sb.AppendLine("Date: " + DateTime.Now.ToUniversalTime().ToString());
            sb.AppendLine("Server: aaaSoft.Net.Http.HttpServer");
            sb.AppendLine("Content-Length: " + dataStream.Length);
            if (contentType.StartsWith("text"))
            {
                sb.AppendLine("Content-Type: " + contentType + "; charset=" + contentEncoding.WebName);
            }
            else
            {
                sb.AppendLine("Content-Type: " + contentType);
            }
            sb.AppendLine("Connection: Keep-Alive");

            //写响应头
            String responseHttpDead = sb.ToString();
            writer.WriteLine(responseHttpDead);
            writer.Flush();

            //写内容
            aaaSoft.Helpers.IoHelper.CopyStream(dataStream, ns, dataStream.Length);
            dataStream.Close();
        }

        public void Start()
        {
            tcpIpServer.TcpListenPort = Port;
            tcpIpServer.Start();
            if (port == 0)
            {
                port = tcpIpServer.GetTcpListenPort();
            }
        }

        public void Stop()
        {
            tcpIpServer.Stop();
        }
    }
}
