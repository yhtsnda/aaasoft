using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using aaaSoft.Helpers;
using System.Xml.Serialization;

namespace aaaSoft.Net.Http
{
    public class HttpSpider
    {
        /*
        #region 属性部分
        /// <summary>
        /// 开始URL
        /// </summary>
        public String StartUrl { get; set; }
        /// <summary>
        /// 开始URL信息
        /// </summary>
        private UrlInfo StartUrlInfo;
        /// <summary>
        /// 线程数
        /// </summary>
        public Int32 ThreadCount { get; set; }

        private CrawlTypeEnum _CrawlType = CrawlTypeEnum.JustStartDomain;
        /// <summary>
        /// 爬起类型
        /// </summary>
        public CrawlTypeEnum CrawlType
        {
            get { return _CrawlType; }
            set { _CrawlType = value; }
        }

        public enum CrawlTypeEnum
        {
            /// <summary>
            /// 只是起始域名
            /// </summary>
            JustStartDomain = 0
            ,
            /// <summary>
            /// 包括起始域名的所有子域名
            /// </summary>
            IncludeAllSubDomainFromStartDomain = 1
                ,
            /// <summary>
            /// 包括根域名的所有子域名
            /// </summary>
            IncludeAllSubDomainFromRootDomain = 2
                ,
            /// <summary>
            /// 类似起始域名
            /// </summary>
            LikeStartDomain = 3
                ,
            /// <summary>
            /// 类似根域名
            /// </summary>
            LikeRootDomain = 4
                ,
            /// <summary>
            /// 所有域名
            /// </summary>
            All = 5
        }

        private Int32 _HttpErrorRetryTime = 3;
        /// <summary>
        /// HTTP错误重试次数
        /// </summary>
        public Int32 HttpErrorRetryTime
        {
            get { return _HttpErrorRetryTime; }
            set { _HttpErrorRetryTime = value; }
        }

        private Int32 _ThreadInterval = 10;
        /// <summary>
        /// 线程的休息时间
        /// </summary>
        public Int32 ThreadInterval
        {
            get { return _ThreadInterval; }
            set { _ThreadInterval = value; }
        }

        private DateTime _StartCrawlTime;
        /// <summary>
        /// 开始爬行时间
        /// </summary>
        public DateTime StartCrawlTime
        { get { return _StartCrawlTime; } }

        private DateTime _StopCrawlTime;
        /// <summary>
        /// 结束爬行时间
        /// </summary>
        public DateTime StopCrawlTime
        { get { return _StopCrawlTime; } }

        //已经发现的URL列表(防止重复爬行)
        private List<String> AlreadyFoundUrlList;
        //等待爬行的URL列表
        private List<UrlInfo> WaitCrawlUrlInfoList;
        /// <summary>
        /// 可访问URL信息列表
        /// </summary>
        public List<UrlInfo> UrlInfoList;

        #endregion

        #region 事件部分
        /// <summary>
        /// 爬行线程已开启事件
        /// </summary>
        public event EventHandler<CrawlThreadStateChangedEventArgs> CrawlThreadStarted;
        /// <summary>
        /// 爬行线程已停止事件
        /// </summary>
        public event EventHandler<CrawlThreadStateChangedEventArgs> CrawlThreadStoped;
        /// <summary>
        /// 爬行线程获得新任务事件
        /// </summary>
        public event EventHandler<CrawThreadGotNewTaskEventArgs> CrawThreadGotNewTask;
        /// <summary>
        /// 爬行线程已空闲事件
        /// </summary>
        public event EventHandler<CrawThreadIdledEventArgs> CrawThreadIdled;
        /// <summary>
        /// 爬行已开始事件
        /// </summary>
        public event EventHandler CrawlStarted;
        /// <summary>
        /// 爬行已中止事件
        /// </summary>
        public event EventHandler CrawlStoped;
        /// <summary>
        /// 爬行已完成事件
        /// </summary>
        public event EventHandler CrawlFinished;
        /// <summary>
        /// 发现新的URL事件(从网页代码中分析出的URL)
        /// </summary>
        public event EventHandler<NewUrlFoundEventArgs> NewUrlFound;
        /// <summary>
        /// 发现新的可访问URL事件(可以访问的URL)
        /// </summary>
        public event EventHandler<NewUrlFoundEventArgs> NewVisitableUrlFound;

        #endregion

        #region 事件参数部分
        /// <summary>
        /// 爬行线程获得新任务事件参数
        /// </summary>
        public class CrawThreadGotNewTaskEventArgs : EventArgs
        {
            /// <summary>
            /// 爬行线程序号
            /// </summary>
            public Int32 CrawlThreadIndex { get; set; }
            /// <summary>
            /// 获取到的UrlInfo对象任务
            /// </summary>
            public UrlInfo UrlInfo { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="CrawlThreadIndex">爬行线程序号</param>
            /// <param name="UrlInfo">UrlInfo对象</param>
            public CrawThreadGotNewTaskEventArgs(Int32 CrawlThreadIndex, UrlInfo UrlInfo)
            {
                this.CrawlThreadIndex = CrawlThreadIndex;
                this.UrlInfo = UrlInfo;
            }
        }

        /// <summary>
        /// 爬行线程已空闲事件参数
        /// </summary>
        public class CrawThreadIdledEventArgs : EventArgs
        {
            /// <summary>
            /// 爬行线程序号
            /// </summary>
            public Int32 CrawlThreadIndex { get; set; }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="CrawlThreadIndex">爬行线程序号</param>
            public CrawThreadIdledEventArgs(Int32 CrawlThreadIndex)
            {
                this.CrawlThreadIndex = CrawlThreadIndex;
            }
        }
        /// <summary>
        /// 爬行线程状态改变事件参数
        /// </summary>
        public class CrawlThreadStateChangedEventArgs : EventArgs
        {
            /// <summary>
            /// 爬行线程序号
            /// </summary>
            public Int32 CrawlThreadIndex { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="CrawlThreadIndex">爬行线程序号</param>
            public CrawlThreadStateChangedEventArgs(Int32 CrawlThreadIndex)
            {
                this.CrawlThreadIndex = CrawlThreadIndex;
            }
        }

        /// <summary>
        /// 发现新的URL事件参数
        /// </summary>
        public class NewUrlFoundEventArgs : EventArgs
        {
            /// <summary>
            /// URL信息对象
            /// </summary>
            public UrlInfo UrlInfo { get; set; }
            /// <summary>
            /// 来源URL信息对象
            /// </summary>
            public UrlInfo SourceUrlInfo { get; set; }
            /// <summary>
            /// 爬行线程序号
            /// </summary>
            public Int32 CrawlThreadIndex { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="UrlInfo">UrlInfo对象</param>
            public NewUrlFoundEventArgs(UrlInfo UrlInfo)
            {
                this.UrlInfo = UrlInfo;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="StartUrl">起始URL</param>
        public HttpSpider(String StartUrl)
            : this(StartUrl, 20)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="StartUrl">起始URL</param>
        /// <param name="ThreadCount">爬行线程数</param>
        public HttpSpider(String StartUrl, Int32 ThreadCount)
        {
            var lowerStartUrl = StartUrl.ToLower();
            if (lowerStartUrl.StartsWith("http://") || lowerStartUrl.StartsWith("https://"))
            { }
            else
            {
                StartUrl = "http://" + StartUrl;
            }

            this.StartUrl = StartUrl;
            this.StartUrlInfo = new UrlInfo(StartUrl);

            this.ThreadCount = ThreadCount;
            WaitCrawlUrlInfoList = new List<UrlInfo>();
            UrlInfoList = new List<UrlInfo>();
            AlreadyFoundUrlList = new List<string>();
        }
        #endregion

        #region Url信息类
        /// <summary>
        /// Url信息类
        /// </summary>
        [Serializable()]
        public class UrlInfo
        {
            /// <summary>
            /// 不包含参数的URL部分
            /// </summary>
            public String Url;
            /// <summary>
            /// 此URL的参数名称列表
            /// </summary>
            public List<String> UrlArgumentNameList;
            /// <summary>
            /// 此URL的参数值列表
            /// </summary>
            public List<String> UrlArgumentValueList;
            /// <summary>
            /// 含参数的URL
            /// </summary>
            public String UrlWithArgments;
            /// <summary>
            /// 内容类型
            /// </summary>
            public String ContentType;
            /// <summary>
            /// 爬行状态(0:未爬行；1：爬行中；2：已爬行)
            /// </summary>
            [XmlIgnore()]
            public Int32 CrawlState;

            /// <summary>
            /// 获取此URL对应的主机部分
            /// </summary>
            public String Host
            {
                get
                {
                    Uri uri = new Uri(UrlWithArgments);
                    return uri.Host;
                }
            }
            /// <summary>
            /// 获取此URL对应的根域名
            /// </summary>
            public String RootDomain
            {
                get
                {
                    var HostSegmentArray = Host.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    var rtnString = HostSegmentArray[HostSegmentArray.Length - 2] + "." + HostSegmentArray[HostSegmentArray.Length - 1];
                    return rtnString;
                }
            }
            public UrlInfo()
            {

            }
            public UrlInfo(String UrlWithArgments)
            {
                this.UrlWithArgments = UrlWithArgments;
                UrlArgumentNameList = new List<string>();
                UrlArgumentValueList = new List<string>();

                CrawlState = 0;

                String tmpUrl = UrlWithArgments;
                if (tmpUrl.Contains("#"))
                {
                    tmpUrl = StringHelper.GetLeftString(tmpUrl, "#");
                }
                if (tmpUrl.Contains("?"))
                {
                    Url = StringHelper.GetLeftString(tmpUrl, "?");
                    var argsString = StringHelper.GetRightString(tmpUrl, "?");
                    var argsArray = argsString.Split(new Char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var args in argsArray)
                    {
                        if (args.Contains("="))
                        {
                            var tmpArray = args.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            UrlArgumentNameList.Add(tmpArray[0].Trim());
                            if (tmpArray.Length >= 2)
                                UrlArgumentValueList.Add(tmpArray[1].Trim());
                            else
                                UrlArgumentValueList.Add(null);
                        }
                    }
                }
                else
                {
                    Url = tmpUrl;
                }
            }
        }
        #endregion

        #region 开始
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            //触发爬行已开始事件
            if (CrawlStarted != null)
                CrawlStarted(this, null);

            _StartCrawlTime = DateTime.Now;
            WaitCrawlUrlInfoList.Add(new UrlInfo(StartUrl));


            List<Thread> CrawlThreadList = new List<Thread>();
            //开启爬行线程
            for (Int32 i = 0; i <= ThreadCount - 1; i++)
            {
                var CrawlThreadIndex = i + 1;
                var newThread = new Thread(CrawlTheadFunction);
                newThread.Name = String.Format("HttpSpider爬行{0}号线程", CrawlThreadIndex);
                newThread.Start(CrawlThreadIndex);
                CrawlThreadList.Add(newThread);
            }

            //开启爬行管理线程
            var manageThread = new Thread(CrawlManageThreadFunction);
            manageThread.Name = "HttpSpider爬行管理线程";
            manageThread.Start(CrawlThreadList);
        }
        #endregion

        #region 暂停
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 继续
        /// <summary>
        /// 继续
        /// </summary>
        public void Resume()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 停止
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _Stop();
        }

        private void _Stop()
        {
            _StartCrawlTime = StartCrawlTime + new TimeSpan(1);
            _StopCrawlTime = DateTime.Now;

            Console.WriteLine("Spider Stoped.");
            Console.WriteLine("ThreadsCount:" + ThreadCount);
            Console.WriteLine("Finally Find Urls:" + UrlInfoList.Count);
            Console.WriteLine();
            Console.WriteLine("StartCrawlTime:" + StartCrawlTime);
            Console.WriteLine("StopCrawlTime:" + StopCrawlTime);
            Console.WriteLine("Crawl Used Time:" + (StopCrawlTime - StartCrawlTime));
        }
        #endregion

        #region 爬行管理线程函数
        private void CrawlManageThreadFunction(Object obj)
        {
            List<Thread> CrawlThreadList = obj as List<Thread>;

            DateTime threadStartTime = StartCrawlTime;
            do
            {
                if (threadStartTime != StartCrawlTime)
                    break;

                Thread.Sleep(ThreadInterval);

                //如果所有URL都已爬行过
                lock (WaitCrawlUrlInfoList)
                {
                    if (WaitCrawlUrlInfoList.Count == 0)
                    {
                        _Stop();
                        if (CrawlFinished != null)
                            CrawlFinished(this, null);
                        return;
                    }
                }
            } while (true);

            while (CrawlThreadList.Any(p => p.IsAlive))
            {
                Thread.Sleep(ThreadInterval);
            }
            if (CrawlStoped != null)
                CrawlStoped(this, null);
        }
        #endregion

        #region 是否应该保存这个URL
        private Boolean IsUrlShouldStore(UrlInfo UrlInfo)
        {
            switch (CrawlType)
            {
                case CrawlTypeEnum.JustStartDomain:
                    if (UrlInfo.Host == StartUrlInfo.Host)
                        return true;
                    break;
                case CrawlTypeEnum.IncludeAllSubDomainFromStartDomain:
                    if (UrlInfo.Host.EndsWith(StartUrlInfo.Host))
                        return true;
                    break;
                case CrawlTypeEnum.IncludeAllSubDomainFromRootDomain:
                    if (UrlInfo.Host.EndsWith(StartUrlInfo.RootDomain))
                        return true;
                    break;
                case CrawlTypeEnum.LikeStartDomain:
                    if (UrlInfo.Host.Contains(StartUrlInfo.Host))
                        return true;
                    break;
                case CrawlTypeEnum.LikeRootDomain:
                    if (UrlInfo.Host.Contains(StartUrlInfo.RootDomain))
                        return true;
                    break;
                case CrawlTypeEnum.All:
                    break;
            }
            return false;
        }
        #endregion

        #region 爬行线程函数
        private void CrawlTheadFunction(Object obj)
        {
            //爬行线程序号(从1开始)
            Int32 CrawlThreadIndex = (Int32)obj;
            DateTime threadStartTime = StartCrawlTime;
            Boolean IsIdleBefore = false;

            //触发爬行线程已开启事件
            if (CrawlThreadStarted != null)
                CrawlThreadStarted(this, new CrawlThreadStateChangedEventArgs(CrawlThreadIndex));

            do
            {
                //如果爬行线程启动时间不等于开始爬行时间，则退出线程
                if (threadStartTime != StartCrawlTime)
                    break;

                Thread.Sleep(ThreadInterval);

                //获取爬行任务
                UrlInfo ToCrawlUrlInfo = GetCrawlTask();
                //如果没有任务
                if (ToCrawlUrlInfo == null)
                {
                    if (!IsIdleBefore)
                    {
                        IsIdleBefore = !IsIdleBefore;

                        //触发爬行线程已空闲事件
                        if (CrawThreadIdled != null)
                            CrawThreadIdled(this, new CrawThreadIdledEventArgs(CrawlThreadIndex));
                    }
                    continue;
                }
                else
                {
                    //触发爬行线程已获得新任务事件
                    if (CrawThreadGotNewTask != null)
                        CrawThreadGotNewTask(this, new CrawThreadGotNewTaskEventArgs(CrawlThreadIndex, ToCrawlUrlInfo));
                }

                //当前任务
                var CurrentCrawlUrlInfo = ToCrawlUrlInfo;

                //执行任务
                try
                {
                    HttpClient httpClient = new HttpClient(ToCrawlUrlInfo.Url);
                    String html = null;
                    //要爬行的URL是否可访问
                    Boolean IsToCrawlUrlVisitable = false;

                    var CurrentRetryTime = HttpErrorRetryTime;

                    while (CurrentRetryTime > 0)
                    {
                        CurrentRetryTime = CurrentRetryTime - 1;

                        try
                        {
                            System.Net.HttpWebResponse response;
                            var stream = httpClient.GetStream(CurrentCrawlUrlInfo.Url, out response);
                            if (stream == null || response == null)
                                continue;

                            if (CurrentCrawlUrlInfo.Url.ToLower() != response.ResponseUri.ToString().ToLower())
                            {
                                //保存之前的URL信息对象
                                AddUrlToUrlInfoList(CrawlThreadIndex, CurrentCrawlUrlInfo);

                                CurrentCrawlUrlInfo = new UrlInfo(response.ResponseUri.ToString());
                                if (!IsUrlShouldStore(CurrentCrawlUrlInfo))
                                    break;
                            }

                            IsToCrawlUrlVisitable = true;

                            CurrentCrawlUrlInfo.ContentType = response.ContentType;

                            //如果不是html网页内容，则不爬行此URL
                            if (!response.ContentType.StartsWith("text/html"))
                            {
                                stream.Close();
                                response.Close();
                                response = null;
                                break;
                            }

                            //获取HTML字符串
                            html = httpClient.GetString(stream, response);
                            break;
                        }
                        catch
                        { }
                    }

                    //如果要爬行的URL可访问
                    if (IsToCrawlUrlVisitable)
                    {
                        //提交执行结果
                        if (!String.IsNullOrEmpty(html))
                            SubmitCrawlTaskResult(CrawlThreadIndex, CurrentCrawlUrlInfo, html);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error From " + ex);
                }
                finally
                {
                    ToCrawlUrlInfo.CrawlState = 2;
                    lock (WaitCrawlUrlInfoList)
                    {
                        WaitCrawlUrlInfoList.Remove(ToCrawlUrlInfo);
                    }
                }
            } while (true);

            if (CrawlThreadStoped != null)
                CrawlThreadStoped(this, new CrawlThreadStateChangedEventArgs(CrawlThreadIndex));
        }
        #endregion

        #region 将URL添加到列表中
        private void AddUrlToAlreadyFoundUrlList(String Url)
        {
            lock (AlreadyFoundUrlList)
            {
                AlreadyFoundUrlList.Add(Url.ToLower());
            }
        }

        private void AddUrlToUrlInfoList(Int32 CrawlThreadIndex, UrlInfo UrlInfo)
        {
            lock (UrlInfoList)
            {
                UrlInfoList.Add(UrlInfo);
            }
            //触发找到可访问URL事件
            if (NewVisitableUrlFound != null)
                NewVisitableUrlFound(this, new NewUrlFoundEventArgs(UrlInfo) { CrawlThreadIndex = CrawlThreadIndex });
        }

        private void AddUrlToWaitCrawlUrlInfoList(Int32 CrawlThreadIndex, UrlInfo tmpUrlInfo, UrlInfo SourceUrlInfo)
        {
            lock (WaitCrawlUrlInfoList)
            {
                WaitCrawlUrlInfoList.Add(tmpUrlInfo);
                if (NewUrlFound != null)
                    NewUrlFound(this, new NewUrlFoundEventArgs(tmpUrlInfo) { CrawlThreadIndex = CrawlThreadIndex, SourceUrlInfo = SourceUrlInfo });
            }
        }
        #endregion

        #region 获取爬行任务
        private UrlInfo GetCrawlTask()
        {
            UrlInfo ToCrawlUrlInfo = null;
            lock (WaitCrawlUrlInfoList)
            {
                if (WaitCrawlUrlInfoList.Count(p => p.CrawlState == 0) != 0)
                {
                    ToCrawlUrlInfo = WaitCrawlUrlInfoList.Where(p => p.CrawlState == 0).First();
                    ToCrawlUrlInfo.CrawlState = 1;
                }
            }
            return ToCrawlUrlInfo;
        }
        #endregion

        #region 提交爬行任务执行结果
        private void SubmitCrawlTaskResult(Int32 CrawlThreadIndex, UrlInfo SourceUrlInfo, String html)
        {
            if (String.IsNullOrEmpty(html))
                return;

            string rgxString = @"(?<=(href|src|url)\s*=)(?:[ \s""']*)(?!#|mailto|location.|javascript|.*css|.*this\.)[^""']*(?:[ \s>""'])";

            Regex rgx = new Regex(rgxString);
            MatchCollection mc = rgx.Matches(html);
            if (mc.Count == 0)
                return;

            AddUrlToUrlInfoList(CrawlThreadIndex, SourceUrlInfo);

            foreach (Match m in mc)
            {
                String newUrl = String.Empty;

                newUrl = m.Value.Replace("\"", String.Empty).Replace("'", String.Empty).Trim();
                if (String.IsNullOrEmpty(newUrl))
                    continue;

                if (newUrl.ToLower().StartsWith("http://")
                    || newUrl.ToLower().StartsWith("https://"))
                { }
                else
                {
                    newUrl = UrlHelper.Combin(SourceUrlInfo.Url, newUrl);
                }

                if (!Uri.IsWellFormedUriString(newUrl, UriKind.Absolute))
                    continue;
                var tmpUrlInfo = new UrlInfo(newUrl);

                //此URL是否已找到过
                lock (AlreadyFoundUrlList)
                {
                    if (AlreadyFoundUrlList.Any(p => p == tmpUrlInfo.Url.ToLower()))
                        continue;
                    AddUrlToAlreadyFoundUrlList(tmpUrlInfo.Url);
                }

                if (!IsUrlShouldStore(tmpUrlInfo))
                    continue;

                AddUrlToWaitCrawlUrlInfoList(CrawlThreadIndex, tmpUrlInfo, SourceUrlInfo);
            }
        }
        #endregion
         */
    }
}
