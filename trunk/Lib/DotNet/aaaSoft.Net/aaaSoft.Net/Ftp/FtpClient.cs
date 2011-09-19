using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Net.Sockets;
using aaaSoft.Helpers;
using aaaSoft.Net.Ftp.ListAnalyzer;
using aaaSoft.Net.Ftp.Core;
using System.Threading;

namespace aaaSoft.Net.Ftp
{
    /// <summary>
    /// FTP客户端类
    /// </summary>
    [Serializable()]
    public class FtpClient : IDisposable
    {
        #region 属性与字段

        /// <summary>
        /// 主机地址
        /// </summary>
        public String HostName;
        /// <summary>
        /// 端口
        /// </summary>
        public Int32 Port = 21;
        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName;
        /// <summary>
        /// 密码
        /// </summary>
        public String Password;

        #region 设置部分
        /// <summary>
        /// 是否显示隐藏文件 (LIST -al)
        /// </summary>
        public Boolean IsShowHidenFile = true;
        /// <summary>
        /// 使用 MLSD 列目录 *
        /// </summary>
        public Boolean IsUseMlsdToListFolder = true;
        /// <summary>
        /// 是否不支持FEAT命令
        /// </summary>
        public Boolean IsNotSupportFEAT = false;

        private Int32 _BufferSize = 4 * 1024;
        /// <summary>
        /// 缓存大小
        /// </summary>
        public Int32 BufferSize
        {
            get { return _BufferSize; }
            set
            {
                if (value <= 0) throw new IOException("缓存大小必须大于0");
                _BufferSize = value;
            }
        }

        /// <summary>
        /// 服务器字符编码
        /// </summary>
        public String StringEncoding = "gb2312";
        /// <summary>
        /// 报告进度的时间间隔(单位为毫秒，默认值为1000)
        /// </summary>
        public Int32 ProgressUpdateTimeInterval = 1000;
        #endregion

        #region 扩展命令支持部分
        /// <summary>
        /// 是否支持CLNT命令(向服务器汇报客户端版本)
        /// </summary>
        public bool IsSupportCLNT = false;
        /// <summary>
        /// 是否支持MDTM命令
        /// </summary>
        public bool IsSupportMDTM = false;
        /// <summary>
        /// 是否支持SIZE命令
        /// </summary>
        public bool IsSupportSIZE = false;
        /// <summary>
        /// 是否支持SITE命令
        /// </summary>
        public bool IsSupportSITE = false;
        /// <summary>
        /// 是否支持REST STREAM命令
        /// </summary>
        public bool IsSupportREST_STREAM = false;
        /// <summary>
        /// 是否支持XCRC命令
        /// </summary>
        public bool IsSupportXCRC = false;
        /// <summary>
        /// 是否支持MODE Z命令
        /// </summary>
        public bool IsSupportMODE_Z = false;
        /// <summary>
        /// 是否支持MLSD/MLST命令
        /// </summary>
        public bool IsSupportMLSD = false;
        #endregion

        #region 运行数据部分
        /// <summary>
        /// 数据的总长度
        /// </summary>
        [NonSerialized()]
        public Int64 TotalDataLength;
        /// <summary>
        /// 已传输的数据的长度
        /// </summary>
        [NonSerialized()]
        public Int64 TransferedDataLength;
        /// <summary>
        /// 传输开始时间
        /// </summary>
        [NonSerialized()]
        public DateTime TransferStartTime;
        /// <summary>
        /// 传输结束时间
        /// </summary>
        [NonSerialized()]
        public DateTime TransferEndTime;
        /// <summary>
        /// 即时传输速度(单位：字节每秒[byte/s])
        /// </summary>
        [NonSerialized()]
        public Int64 ImmediateTransferSpeed;

        /// <summary>
        /// 传输所用的时间
        /// </summary>
        public TimeSpan TransferUsedTime
        {
            get
            {
                DateTime start, end;
                start = TransferStartTime;
                if (TransferedDataLength == TotalDataLength)
                    end = TransferEndTime;
                else
                    end = DateTime.Now;
                return end - start;
            }
        }
        /// <summary>
        /// 平均传输速度(单位：字节每秒[byte/s])
        /// </summary>
        public Int64 AverageTransferSpeed
        {
            get
            {
                try
                {
                    Double rtnData = TransferedDataLength / TransferUsedTime.TotalSeconds;
                    if (Double.IsInfinity(rtnData) || Double.IsNaN(rtnData))
                        rtnData = 0;
                    return Convert.ToInt64(rtnData);
                }
                catch { return 0; }
            }
        }
        /// <summary>
        /// 传输进度
        /// </summary>
        public Double TransferProgress
        {
            get
            {
                if (TotalDataLength <= 0)
                    return 0;
                else
                    return TransferedDataLength * 1D / TotalDataLength;
            }
        }
        /// <summary>
        /// 传输是否已经完成
        /// </summary>
        public bool IsTransferCompleted
        {
            get
            {
                return TransferedDataLength == TotalDataLength;
            }
        }

        /// <summary>
        /// 执行命令的数据通道
        /// </summary>
        [NonSerialized()]
        public TcpClient ControlTcpClient;
        //命令发送器
        [NonSerialized()]
        public StreamWriter ControlWriter;
        //命令返回结果Reader
        [NonSerialized()]
        public StreamReader ControlReader;

        /// <summary>
        /// 传输数据的数据通道
        /// </summary>
        public TcpClient DataTcpClient;
        /// <summary>
        /// 传输数据的流
        /// </summary>
        public Stream DataStream;
        /// <summary>
        /// 最后一次收到的数据包
        /// </summary>
        public FtpCommandData LastReturnData;
        /// <summary>
        /// 错误消息(最后一次)
        /// </summary>
        [NonSerialized()]
        public String ErrMsg;
        /// <summary>
        /// 错误异常对象(最后一次)
        /// </summary>
        [NonSerialized()]
        public Exception ErrException;
        /// <summary>
        /// 根目录路径(登录时的默认路径)
        /// </summary>
        [NonSerialized()]
        public String BaseDirectoryPath;
        /// <summary>
        /// 当前目录路径
        /// </summary>
        [NonSerialized()]
        public String CurrentDirectoryPath;
        /// <summary>
        /// FTP服务器欢迎信息
        /// </summary>
        [NonSerialized()]
        public String FtpServerWelcomeText;
        /// <summary>
        /// FTP服务器信息
        /// </summary>
        [NonSerialized()]
        public String FtpServerSystemInfo;
        /// <summary>
        /// FTP服务器返回的FEAT字符串
        /// </summary>
        [NonSerialized()]
        public String FtpServerFeatString;
        /// <summary>
        /// 传输类型(A或I)
        /// </summary>
        [NonSerialized()]
        public String TransferType = String.Empty;
        //获取传输速度线程
        Thread trdGetTransferSpeed = null;
        #endregion

        #region 是否已经连接到服务器
        private bool _IsConnected = false;
        /// <summary>
        /// 是否已经连接到服务器
        /// </summary>
        private bool IsConnected
        {
            get { return _IsConnected; }
            set
            {
                _IsConnected = value;
                if (!value)
                {
                    WaitGetDataQueue.Clear();
                }

                if (value)
                {
                    trdGetTransferSpeed = new Thread(GetTransferSpeedThread);
                    trdGetTransferSpeed.Start();
                }
                else
                    trdGetTransferSpeed = null;
            }
        }
        #endregion

        #region 是否已经成功登陆到服务器
        private bool _IsLogin = false;
        /// <summary>
        /// 是否已经成功登陆到服务器
        /// </summary>
        public bool IsLogin
        {
            get { return _IsLogin; }
            set { _IsLogin = value; }
        }
        #endregion


        //等待取数据队列(用于Execute函数)
        private Queue<FtpCommandData> WaitGetDataQueue = new Queue<FtpCommandData>();
        //等待触发通知事件队列(用于触发ResponseEvent事件)
        //private Queue<FtpCommandData> WaitHandleDataQueue = new Queue<FtpCommandData>();

        /// <summary>
        /// 获取当前的服务器返回命令对象(会阻塞直到获取到对象)
        /// </summary>
        /// <returns></returns>
        private FtpCommandData GetCurrentReturnData()
        {
            FtpCommandData rtnData = null;
            while (true)
            {
                if (!IsConnected)
                    break;
                if (WaitGetDataQueue.Count > 0)
                    break;
                if (ThreadBlocked != null)
                    ThreadBlocked(this, new FtpSiteEventArgs());
                Thread.Sleep(100);
            }
            if (WaitGetDataQueue.Count > 0)
                rtnData = WaitGetDataQueue.Dequeue();
            return rtnData;
        }

        /// <summary>
        /// FTP函数的锁对象
        /// </summary>
        private Object FtpFunctionLockObj = new Object();
        /// <summary>
        /// Execute函数的锁对象
        /// </summary>
        private Object ExecuteLockObj = new Object();
        #endregion

        #region 事件

        public delegate void FtpSiteEventHandler(Object sender, FtpSiteEventArgs e);
        public class FtpSiteEventArgs : EventArgs
        {
            /// <summary>
            /// 是否应该列目录
            /// </summary>
            public Boolean ShouldListFolder { get; set; }
            /// <summary>
            /// 文件列表
            /// </summary>
            public List<FtpBaseFileInfo> BaseFileList { get; set; }
        }
        /// <summary>
        /// 列目录完成事件
        /// </summary>
        public event FtpSiteEventHandler ListFolderComplete;
        /// <summary>
        /// 开始登录
        /// </summary>
        public event FtpSiteEventHandler LoginBegin;
        /// <summary>
        /// 登录完成事件
        /// </summary>
        public event FtpSiteEventHandler LoginComplete;
        /// <summary>
        /// 线程阻塞时
        /// </summary>
        public event FtpSiteEventHandler ThreadBlocked;

        public delegate void FtpSiteCommandEventHandler(Object sender, FtpCommandData e);
        /// <summary>
        /// 请求事件(客户端向服务器端发送请求)
        /// </summary>
        public event FtpSiteCommandEventHandler RequestEvent;
        /// <summary>
        /// 响应事件(服务器端向客户端发送响应)
        /// </summary>
        public event FtpSiteCommandEventHandler ResponseEvent;
        /// <summary>
        /// 进度变化时间(如果在传输状态，则1秒钟触发一次)
        /// </summary>
        public event EventHandler ProgressUpdatedEvent;
        /// <summary>
        /// 连接断开事件
        /// </summary>
        public event EventHandler ConnectionClosedEvent;
        #endregion

        #region 清除事件绑定
        /// <summary>
        /// 清除事件绑定
        /// </summary>
        public void ClearEventBinding()
        {
            RequestEvent = null;
            ResponseEvent = null;
            ProgressUpdatedEvent = null;
            ConnectionClosedEvent = null;
            LoginBegin = null;
            LoginComplete = null;
            ThreadBlocked = null;
        }
        #endregion

        #region 构造函数
        public FtpClient() { }
        public FtpClient(String HostName, Int32 Port)
            : this(HostName, Port, "anonymous", "anonymous@anonymous.com")
        { }

        public FtpClient(String HostName, Int32 Port, String UserName, String Password)
        {
            this.HostName = HostName;
            this.Port = Port;
            if (String.IsNullOrEmpty(UserName))
            {
                this.UserName = "anonymous";
                this.Password = "anonymous@anonymous.com";
            }
            else
            {
                this.UserName = UserName;
                this.Password = Password;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ftp://");
            sb.Append(UserName);
            sb.Append("@");
            sb.Append(HostName);
            if (Port != 21)
            {
                sb.Append(":" + Port);
            }
            return sb.ToString();
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            trdGetTransferSpeed = null;
        }

        #endregion

        #region 得到字符编码对象
        /// <summary>
        /// 得到字符编码对象
        /// </summary>
        /// <returns></returns>
        private Encoding GetEncoding()
        {
            Encoding rtnEncoding = Encoding.Default;
            if (!String.IsNullOrEmpty(StringEncoding))
            {
                try
                {
                    rtnEncoding = Encoding.GetEncoding(StringEncoding);
                }
                catch { }
            }
            //如果是UTF8编码，则特殊处理以免产生BOM头
            if (rtnEncoding is UTF8Encoding)
                rtnEncoding = new UTF8Encoding(false);
            //如果是Unicode编码，则特殊处理以免产生BOM头
            else if (rtnEncoding is UnicodeEncoding)
                rtnEncoding = new UnicodeEncoding(true, false);
            return rtnEncoding;
        }
        #endregion

        #region 检查返回码与给定的返回码是否匹配(同时设置一些变量状态如：是否登陆，是否退出等)
        private bool CheckReturnCode(FtpCommandData rtnData, String CorrectCode)
        {
            return CheckReturnCode(rtnData, new String[] { CorrectCode });
        }

        private bool CheckReturnCode(FtpCommandData rtnData, String[] CorrectCodeArray)
        {
            //预处理返回码
            foreach (var CorrectCode in CorrectCodeArray)
            {
                if (rtnData.FtpCommandName == CorrectCode)
                {
                    return true;
                }
            }
            ErrMsg = rtnData.ToString();
            return false;
        }
        #endregion

        #region 连接到服务器
        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns></returns>
        private bool Connect()
        {
            try
            {
                if (ControlTcpClient != null && ControlTcpClient.Connected)
                    return true;
                IsConnected = false;

                //连接到服务器指定端口
                ControlTcpClient = new TcpClient();
                ControlTcpClient.Connect(HostName, Port);
                IsConnected = true;

                //初始化相关变量
                Stream CommandStream = ControlTcpClient.GetStream();
                Encoding encoding = GetEncoding();
                ControlWriter = new StreamWriter(CommandStream, encoding, BufferSize);
                ControlReader = new StreamReader(CommandStream, encoding, true, BufferSize);


                //初始化相关线程
                Thread trdGetResponse = new Thread(GetServerResponseThread);
                trdGetResponse.Start();

                FtpCommandData fcd = new FtpCommandData("");
                var rtnData = Execute(fcd);
                if (!CheckReturnCode(rtnData, "220")) return false;
                FtpServerWelcomeText = rtnData.FtpCommandArgs;

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                ErrException = ex;

                IsConnected = false;
                return false;
            }
        }
        #endregion

        #region 得到传输即时速度线程
        private void GetTransferSpeedThread()
        {
            DateTime startRecordTime = DateTime.Now;
            //上次传输到的位置
            Int64 lastTransferedDataLength = TransferedDataLength;
            while (trdGetTransferSpeed != null && Thread.CurrentThread.Equals(trdGetTransferSpeed))
            {
                if (IsTransferCompleted || lastTransferedDataLength > TransferedDataLength)
                {
                    ImmediateTransferSpeed = 0;
                }
                else
                {
                    Int64 dataLength = TransferedDataLength - lastTransferedDataLength;
                    DateTime endRecordTime = DateTime.Now;
                    Double seconds = (endRecordTime - startRecordTime).TotalSeconds;
                    Double speed = dataLength / seconds;
                    if (!Double.IsNaN(speed))
                        ImmediateTransferSpeed = Convert.ToInt64(speed);
                }

                lastTransferedDataLength = TransferedDataLength;
                startRecordTime = DateTime.Now;

                if (ProgressUpdatedEvent != null)
                    ProgressUpdatedEvent(this, null);
                Thread.Sleep(ProgressUpdateTimeInterval);
            }
        }
        #endregion

        #region 得到服务器返回码线程
        private void GetServerResponseThread()
        {
            try
            {
                while (IsConnected)
                {
                    StringBuilder sb = new StringBuilder();
                    FtpCommandData rtnData = null;
                    do
                    {
                        String newLine = ControlReader.ReadLine();
                        if (newLine == null) break;
                        //接收响应
                        //Debug.Print(DateTime.Now.ToShortTimeString() + ":" + newLine);
                        String RegexStr = @"^\d\d\d[ |$]";
                        System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(RegexStr);
                        if (rgx.IsMatch(newLine))
                        {
                            rtnData = FtpCommandData.FromCommandText(newLine);
                            break;
                        }
                        sb.AppendLine(newLine);
                    } while (true);
                    if (rtnData == null) continue;
                    rtnData.FtpCommandOther = sb.ToString();

                    //先触发响应事件
                    if (ResponseEvent != null)
                        ResponseEvent(this, rtnData);
                    //然后加入到取队列的队列中
                    LastReturnData = rtnData;
                    WaitGetDataQueue.Enqueue(rtnData);

                    if (rtnData.FtpCommandName == "421")
                    {
                        //连接超时
                        IsLogin = false;
                        CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                ErrException = ex;

                IsConnected = false;
                //触发 连接断开事件
                if (ConnectionClosedEvent != null)
                    ConnectionClosedEvent(this, null);
            }
        }
        #endregion

        #region 登录
        /// <summary>
        /// 异步登录
        /// </summary>
        public void BeginLogin()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                { Login(); }));

        }
        public bool Login()
        {
            return Login(true);
        }
        /// <summary>
        /// 同步登录
        /// </summary>
        /// <returns></returns>
        public bool Login(bool ShouldListFolder)
        {
            lock (FtpFunctionLockObj)
            {
                try
                {
                    if (LoginBegin != null)
                        LoginBegin(this, new FtpSiteEventArgs());

                    if (!Connect())
                        throw new IOException(ErrMsg);

                    FtpCommandData fcd;
                    //输入用户名(USER)
                    fcd = new FtpCommandData("USER", UserName);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "331")) return false;
                    //输入密码(PASS)
                    fcd = new FtpCommandData("PASS", Password);
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "230")) return false;
                    //得到服务器信息
                    fcd = new FtpCommandData("SYST");
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "215")) return false;
                    FtpServerSystemInfo = rtnData.FtpCommandArgs;
                    //如果服务器支持扩展命令
                    if (!IsNotSupportFEAT)
                    {
                        //得到服务器支持的扩展命令
                        fcd = new FtpCommandData("FEAT");
                        rtnData = Execute(fcd);
                        if (!CheckReturnCode(rtnData, "211"))
                        {
                            throw new System.Security.Authentication.AuthenticationException(rtnData.FtpCommandArgs);
                        }
                        FtpServerFeatString = rtnData.FtpCommandOther;
                        if (FtpServerFeatString.Contains("CLNT")) IsSupportCLNT = true;
                        if (FtpServerFeatString.Contains("MDTM")) IsSupportMDTM = true;
                        if (FtpServerFeatString.Contains("SIZE")) IsSupportSIZE = true;
                        if (FtpServerFeatString.Contains("SITE")) IsSupportSITE = true;
                        if (FtpServerFeatString.Contains("REST STREAM")) IsSupportREST_STREAM = true;
                        if (FtpServerFeatString.Contains("XCRC")) IsSupportXCRC = true;
                        if (FtpServerFeatString.Contains("MODE Z")) IsSupportMODE_Z = true;
                        if (FtpServerFeatString.Contains("MLST")) IsSupportMLSD = true;
                    }
                    //得到默认目录
                    fcd = new FtpCommandData("PWD");
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "257")) return false;
                    BaseDirectoryPath = StringHelper.GetMiddleString(rtnData.FtpCommandArgs, "\"", "\"");
                    CurrentDirectoryPath = BaseDirectoryPath;

                    IsLogin = true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    IsLogin = false;
                }
                finally
                {
                    if (LoginComplete != null)
                        LoginComplete(this, new FtpSiteEventArgs() { ShouldListFolder = ShouldListFolder });
                }
                return IsLogin;
            }
        }
        #endregion

        #region 改变传输类型
        /// <summary>
        /// 改变传输类型
        /// </summary>
        /// <param name="type">传输类型(A:ASCII模式；I:二进制模式)</param>
        /// <returns></returns>
        public bool ChangeType(String type)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;

                if (TransferType == type) return true;

                var fcd = new FtpCommandData("TYPE", type);
                var rtnData = Execute(fcd);
                if (!CheckReturnCode(rtnData, "200")) return false;
                TransferType = type;
                return true;
            }
        }
        #endregion

        #region 执行命令
        //回车换行
        public const String CONST_CRLF = "\r\n";
        /// <summary>
        /// 执行FTP命令
        /// </summary>
        /// <returns></returns>
        private FtpCommandData Execute(FtpCommandData FtpCommandData)
        {
            lock (ExecuteLockObj)
            {
                if (!String.IsNullOrEmpty(FtpCommandData.FtpCommandName))
                {
                    //发送命令
                    String CmdText = FtpCommandData.ToString();
                    if (FtpCommandData.FtpCommandName == "PASS")
                    {
                        FtpCommandData.FtpCommandArgs = "******";
                    }
                    if (RequestEvent != null)
                        RequestEvent(this, FtpCommandData);

                    try
                    {
                        ControlWriter.Write(CmdText + CONST_CRLF);
                        ControlWriter.Flush();
                    }
                    catch
                    {
                        IsConnected = false;
                        IsLogin = false;
                        return null;
                    }
                }
                var rtnData = GetCurrentReturnData();
                return rtnData;
            }
        }

        public String ExecuteCommand(String CommandText)
        {
            FtpCommandData fcd = FtpCommandData.FromCommandText(CommandText);
            var rtnData = Execute(fcd);
            if (rtnData == null) return null;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(rtnData.FtpCommandOther);
            sb.Append(rtnData.ToString());
            return sb.ToString();
        }

        #endregion

        #region 改变工作目录
        public bool ChangeWorkDirectory(String WorkDirectory)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;
                try
                {
                    //如果当前的工作目录是要设置的目录，则不改变
                    if (WorkDirectory.Equals(CurrentDirectoryPath))
                        return true;

                    FtpCommandData fcd;

                    //改变工作目录(USER)
                    fcd = new FtpCommandData("CWD", WorkDirectory);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "250")) return false;

                    CurrentDirectoryPath = WorkDirectory;
                    return true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region 移除路径中多余的路径分隔符
        private String RemoveDoublePathSp(String Path)
        {
            String tmpPath = Path;
            while (tmpPath.Contains("//"))
            {
                tmpPath = tmpPath.Replace("//", "/");
            }
            return tmpPath;
        }
        #endregion

        #region 列出当前目录的所有文件和目录
        /// <summary>
        /// 异步列目录(结果会在ListFolderEvent事件中返回)
        /// </summary>
        public void BeginListDirectory()
        {
            BeginListDirectory(null);
        }
        /// <summary>
        /// 异步列目录(结果会在ListFolderEvent事件中返回)
        /// </summary>
        /// <param name="FolderPath"></param>
        public void BeginListDirectory(String FolderPath)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    if (String.IsNullOrEmpty(FolderPath))
                        ListDirectory();
                    else
                        ListDirectory(FolderPath);
                }));
        }
        /// <summary>
        /// 同步列目录
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public List<FtpBaseFileInfo> ListDirectory()
        {
            return ListDirectory("");
        }

        /// <summary>
        /// 同步列目录
        /// </summary>
        /// <returns></returns>
        public List<FtpBaseFileInfo> ListDirectory(String FolderPath)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return null;

                List<FtpBaseFileInfo> BaseFileList = new List<FtpBaseFileInfo>();
                List<FtpBaseFileInfo> FileList = new List<FtpBaseFileInfo>();
                List<FtpBaseFileInfo> FolderList = new List<FtpBaseFileInfo>();

                try
                {
                    if (!String.IsNullOrEmpty(FolderPath))
                    {
                        String tmpPath = RemoveDoublePathSp(FolderPath);
                        if (!ChangeWorkDirectory(tmpPath))
                        {
                            throw new IOException(ErrMsg);
                        }
                    }
                    //改变传输模式为A
                    ChangeType("A");
                    //先执行PASV命令，得到数据通道
                    if (!PASV())
                    {
                        return null;
                    }
                    Encoding encoding = GetEncoding();
                    StreamWriter DataWriter = new StreamWriter(DataStream, encoding, BufferSize);
                    StreamReader DataReader = new StreamReader(DataStream, encoding, true, BufferSize);

                    FtpCommandData fcd;

                    //如果服务器支持MLSD命令，则使用MLSD命令                
                    if (IsUseMlsdToListFolder && IsSupportMLSD)
                    {
                        fcd = new FtpCommandData("MLSD");
                    }
                    //执行LIST(-al参数是为了显示隐藏文件)
                    else
                    {
                        if (IsShowHidenFile)
                            fcd = new FtpCommandData("LIST", "-al");
                        else
                            fcd = new FtpCommandData("LIST");
                    }
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, new String[] { "150", "125" })) return null;
                    //此时数据已开始传输
                    var Analyzer = FtpListAnalyzer.GetFtpListAnalyzer(this);
                    do
                    {
                        String line = DataReader.ReadLine();
                        if (String.IsNullOrEmpty(line))
                            break;

                        var baseFileInfo = Analyzer.AnalyzeLine(line);
                        if (baseFileInfo == null) continue;

                        baseFileInfo.ParentPath = CurrentDirectoryPath;
                        if (baseFileInfo.IsFolder)
                            FolderList.Add(baseFileInfo);
                        else
                            FileList.Add(baseFileInfo);
                    } while (true);
                    fcd = new FtpCommandData("");
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "226")) return BaseFileList;
                    //此时数据已结束传输
                    BaseFileList.AddRange(FolderList);
                    BaseFileList.AddRange(FileList);

                    return BaseFileList;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return null;
                }
                finally
                {
                    //触发LIST命令完成事件
                    if (ListFolderComplete != null)
                        ListFolderComplete(this, new FtpSiteEventArgs() { BaseFileList = BaseFileList });
                }
            }
        }
        #endregion

        #region 检测当前工作目录，如果不是指定的目录，则改变工作目录为指定的目录
        private bool CheckToChangeWorkDirectory(String WorkDirectory)
        {
            if (WorkDirectory.ToUpper() != CurrentDirectoryPath.ToUpper())
            {
                return ChangeWorkDirectory(WorkDirectory);
            }
            return true;
        }
        #endregion

        #region 检测是否已登录，如果未登录，则登录
        private bool CheckToLogin()
        {
            if (IsLogin)
                return true;
            return Login(false);
        }
        #endregion

        #region 创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="DirectoryPath">目录路径</param>
        /// <returns></returns>
        public bool CreateDirectory(String DirectoryPath)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;

                //如果不是绝对路径，则处理成绝对路径
                if (!DirectoryPath.StartsWith("/"))
                {
                    DirectoryPath = CurrentDirectoryPath + "/" + DirectoryPath;
                }

                var directoryName = IoHelper.GetFileOrFolderName(DirectoryPath, '/');
                var parentDirectoryPath = IoHelper.GetParentPath(DirectoryPath, '/');
                //如果当前目录不是要创建目录的父目录
                if (CurrentDirectoryPath.ToUpper() != parentDirectoryPath.ToUpper())
                {
                    if (ChangeWorkDirectory(parentDirectoryPath))
                        return CreateDirectory(directoryName);
                    else
                    {
                        if (!CreateDirectory(parentDirectoryPath)) return false;

                        ChangeWorkDirectory(parentDirectoryPath);
                        DirectoryPath = directoryName;
                    }
                }

                try
                {
                    bool IsCreateSuccess = false;
                    FtpCommandData fcd;
                    //MKD
                    fcd = new FtpCommandData("MKD", DirectoryPath);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "257"))
                        IsCreateSuccess = false;
                    else
                        IsCreateSuccess = true;
                    return IsCreateSuccess;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public bool DeleteFile(String FilePath)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;
                try
                {
                    FtpCommandData fcd;
                    //DELE
                    fcd = new FtpCommandData("DELE", FilePath);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "250")) return false;
                    return true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region 删除目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="DirectoryPath">目录路径</param>
        /// <returns></returns>
        public bool RemoveDirectory(String DirectoryPath)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;
                try
                {
                    FtpCommandData fcd;
                    //RMD
                    fcd = new FtpCommandData("RMD", DirectoryPath);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "250")) return false;
                    return true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region 重命名文件
        public bool Rename(String From, String To)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;
                try
                {
                    String renameBaseDirectoryPath = IoHelper.GetParentPath(From, '/');
                    //改变工作目录
                    if (renameBaseDirectoryPath != CurrentDirectoryPath)
                    {
                        ChangeWorkDirectory(renameBaseDirectoryPath);
                    }

                    FtpCommandData fcd;
                    //RNFR
                    fcd = new FtpCommandData("RNFR", From.Substring(renameBaseDirectoryPath.Length).Replace("/", ""));
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "350")) return false;
                    //RNTO
                    fcd = new FtpCommandData("RNTO", To);
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "250")) return false;

                    return true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region PASV命令
        /// <summary>
        /// 执行PASV命令
        /// </summary>
        /// <returns></returns>
        public bool PASV()
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;
                try
                {
                    FtpCommandData fcd;

                    //PASV
                    fcd = new FtpCommandData("PASV");
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "227")) return false;

                    String IPEPStr = StringHelper.GetMiddleString(rtnData.FtpCommandArgs, "(", ")");
                    String[] IPEPArray = IPEPStr.Split(',');
                    IPAddress remoteIp = IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}", IPEPArray[0], IPEPArray[1], IPEPArray[2], IPEPArray[3]));
                    Int32 remotePort = Convert.ToInt32(IPEPArray[4]) * 256 + Convert.ToInt32(IPEPArray[5]);

                    if (ResponseEvent != null)
                    {
                        ResponseEvent(this, FtpCommandData.FromCommandText(String.Format("正在打开数据连接 IP: {0} 端口: {1}", remoteIp, remotePort)));
                    }
                    DataTcpClient = new TcpClient();
                    //发送和接收数据的超时时间都为20秒
                    DataTcpClient.SendTimeout = 20 * 1000;
                    DataTcpClient.ReceiveTimeout = 20 * 1000;

                    DataTcpClient.Connect(remoteIp, remotePort);
                    DataStream = DataTcpClient.GetStream();

                    TotalDataLength = 0;
                    TransferedDataLength = 0;
                    ImmediateTransferSpeed = 0;
                    return true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region 上传文件
        public bool UploadFile(String RemoteFileName, String LocalFilePath)
        {
            lock (FtpFunctionLockObj)
            {
                String RemoteFolderPath = CurrentDirectoryPath;
                if (!CheckToLogin()) return false;

                Stream srcStream = null;
                Stream desStream = null;
                try
                {
                    //检查当前工作目录
                    if (!CheckToChangeWorkDirectory(RemoteFolderPath)) return false;

                    //改变传输模式为二进制传输模式
                    ChangeType("I");
                    //使用被动模式
                    PASV();



                    var localFileInfo = new FileInfo(LocalFilePath);
                    //最后修改时间(UTC时间)
                    var lastModifyTime = localFileInfo.LastWriteTime.ToUniversalTime();
                    srcStream = localFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                    desStream = DataStream;

                    FtpCommandData fcd;
                    //发送STOR命令
                    fcd = new FtpCommandData("STOR", RemoteFileName);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, new String[] { "125", "150" })) return false;

                    //设置总大小
                    TotalDataLength = srcStream.Length;
                    //设置传输开始时间
                    TransferStartTime = DateTime.Now;
                    TransferEndTime = TransferStartTime;
                    //传输数据
                    IoHelper.CopyStream(srcStream, desStream, srcStream.Length, ref TransferedDataLength);
                    //设置传输结束时间
                    TransferEndTime = DateTime.Now;
                    desStream.Close();
                    desStream = null;
                    srcStream.Close();
                    srcStream = null;

                    fcd = new FtpCommandData("");
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "226")) return false;

                    //修改远端文件的最后一次修改时间
                    fcd = new FtpCommandData("MDTM", String.Format("{0} {1}", lastModifyTime.ToString("yyyyMMddHHmmss"), RemoteFileName));
                    rtnData = Execute(fcd);
                    return true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    ErrException = ex;

                    var fcd = new FtpCommandData("");
                    var rtnData = Execute(fcd);

                    return false;
                }
                finally
                {
                    if(srcStream!= null)
                        try { srcStream.Close(); }
                        catch { }
                    if(desStream!=null)
                        try { desStream.Close(); }
                        catch { }
                }
            }
        }
        #endregion

        #region 下载文件
        public bool DownloadFile(FtpBaseFileInfo RemoteFile, String LocalFilePath)
        {
            lock (FtpFunctionLockObj)
            {
                if (!CheckToLogin()) return false;
                try
                {
                    //检查当前工作目录
                    if (!CheckToChangeWorkDirectory(RemoteFile.ParentPath)) return false;

                    //改变传输模式为二进制传输模式
                    ChangeType("I");
                    //使用被动模式
                    PASV();

                    FtpCommandData fcd;
                    //发送RETR命令
                    fcd = new FtpCommandData("RETR", RemoteFile.Name);
                    var rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, new String[] { "125", "150" })) return false;

                    //检查创建文件的目录
                    IoHelper.CreateMultiFolder(Path.GetDirectoryName(LocalFilePath));

                    var srcStream = DataStream;
                    var desStream = File.Create(LocalFilePath);

                    //设置总大小
                    TotalDataLength = RemoteFile.Length;
                    //设置传输开始时间
                    TransferStartTime = DateTime.Now;
                    TransferEndTime = TransferStartTime;
                    //传输数据
                    try
                    {
                        IoHelper.CopyStream(srcStream, desStream, RemoteFile.Length, ref TransferedDataLength);
                    }
                    catch (Exception ex)
                    {
                        //将流关闭掉，以免被占用
                        desStream.Close();
                        throw ex;
                    }
                    //设置传输结束时间
                    TransferEndTime = DateTime.Now;
                    desStream.Close();
                    srcStream.Close();

                    fcd = new FtpCommandData("");
                    rtnData = Execute(fcd);
                    if (!CheckReturnCode(rtnData, "226")) return false;

                    //设置文件的修改时间
                    DateTime? lastModifyTime = null;
                    //如果服务器是用的MLSD命令列目录，则不发MDTM命令，直接从RemoteFile对象中取时间
                    if (IsUseMlsdToListFolder && IsSupportMLSD)
                    {
                        lastModifyTime = RemoteFile.LastModifyTime;
                    }
                    //发送MDTM命令取时间
                    else
                    {
                        //得到远端文件的最后一次修改时间
                        fcd = new FtpCommandData("MDTM", RemoteFile.Name);
                        rtnData = Execute(fcd);
                        if (CheckReturnCode(rtnData, "213"))
                        {
                            var srcDateTimeString = rtnData.FtpCommandArgs;
                            var DataTimeString = String.Format(
                                "{0}/{1}/{2} {3}:{4}:{5}",
                                srcDateTimeString.Substring(0, 4),
                                srcDateTimeString.Substring(4, 2),
                                srcDateTimeString.Substring(6, 2),
                                srcDateTimeString.Substring(8, 2),
                                srcDateTimeString.Substring(10, 2),
                                srcDateTimeString.Substring(12, 2)
                                );

                            DateTime tmpDateTime;
                            if (DateTime.TryParse(DataTimeString, out tmpDateTime))
                            {
                                tmpDateTime = tmpDateTime.ToLocalTime();
                                lastModifyTime = tmpDateTime;
                            }
                        }
                    }

                    //修改本地文件的最后一次修改时间
                    if (lastModifyTime.HasValue)
                    {
                        try { File.SetLastWriteTime(LocalFilePath, lastModifyTime.Value); }
                        catch { }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.Print("Error From " + ex.ToString());
                    ErrMsg = ex.Message;
                    ErrException = ex;
                    return false;
                }
            }
        }
        #endregion

        #region 退出
        /// <summary>
        /// 退出
        /// </summary>
        public void Quit()
        {
            lock (FtpFunctionLockObj)
            {
                if (IsLogin)
                {
                    FtpCommandData fcd;
                    //发送RETR命令
                    fcd = new FtpCommandData("QUIT");
                    var rtnData = Execute(fcd);
                }
                CloseConnection();
            }
        }
        #endregion

        #region 关闭连接
        private void CloseConnection()
        {
            try
            {
                if (ControlTcpClient == null || !ControlTcpClient.Connected)
                {
                    return;
                }
                ControlTcpClient.Close();
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                ErrException = ex;
            }
            ControlTcpClient = null;
        }
        #endregion

        #region 强制关闭数据传输(DataTcpClient)连接
        /// <summary>
        /// 强制关闭数据传输(DataTcpClient)连接
        /// </summary>
        public void ForceCloseDataConnection()
        {
            try
            {
                if (DataTcpClient != null)
                    DataTcpClient.Client.Close();
            }
            catch { }
        }
        #endregion
    }
}
