using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace aaaSoft.Net.Http
{
    /// <summary>
    /// HTTP客户端类
    /// </summary>
    public class HttpClient
    {
        /// <summary>
        /// 主机名称
        /// </summary>
        public string HostName;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port;
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public Int32 RequestTimeout = 10000;
        /// <summary>
        /// HTTP代理
        /// </summary>
        public IWebProxy WebProxy;
        /// <summary>
        /// 引用URL
        /// </summary>
        public string RefererUrl;

        //保存Cookie的列表
        private List<Cookie> lstCookies;

        #region 构造函数
        public HttpClient(String Url)
        {
            Init(Url, null);
        }

        public HttpClient(string Url, IWebProxy WebProxy)
        {
            Init(Url, WebProxy);
        }

        private void Init(string Url, IWebProxy WebProxy)
        {
            Uri uri = new Uri(Url);

            this.RefererUrl = string.Empty;
            this.lstCookies = new List<Cookie>();
            this.HostName = uri.Host;
            this.Port = uri.Port;
            this.WebProxy = WebProxy;
        }
        #endregion


        //得到端口字符串
        private String GetPortString()
        {
            if (Port == 80)
            {
                return String.Empty;
            }
            else
            {
                return ":" + 80;
            }
        }

        public Stream GetStream(string url, out HttpWebResponse response)
        {
            Stream responseStream = null;
            String FullUrl = url;
            if (url.ToLower().StartsWith("http://") || url.ToLower().StartsWith("https://"))
            { }
            else
            {
                FullUrl = string.Format("http://{0}{1}{2}", this.HostName, GetPortString(), url);
            }

            Uri requestUri = new Uri(FullUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Proxy = WebProxy;
            request.Method = "GET";
            request.Timeout = RequestTimeout;
            request.Referer = this.RefererUrl;
            this.RefererUrl = requestUri.ToString();
            //插入Cookie
            InsertCookieIntoRequest(request);

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                SaveCookieFromResponse(response);
                responseStream = response.GetResponseStream();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                response = null;
            }
            return responseStream;
        }

        public string GetString(string url)
        {
            HttpWebResponse response;
            Stream stream = this.GetStream(url, out response);
            var rtnString = GetString(stream, response);
            return rtnString;
        }

        public string GetString(Stream stream, HttpWebResponse response)
        {
            if (stream == null)
            {
                return string.Empty;
            }
            Encoding responseEncoding = GetEncodingFromCharacterSet(response.CharacterSet);

            string s = new StreamReader(stream, responseEncoding).ReadToEnd();
            response.Close();
            response = null;
            return s;
        }

        /// <summary>
        /// 根据返回的HTTP头的CharactorSet参数，得到对应的Encoding对象
        /// </summary>
        /// <param name="CharacterSet"></param>
        /// <returns></returns>
        public static Encoding GetEncodingFromCharacterSet(String CharacterSet)
        {
            Encoding responseEncoding = null;
            switch (CharacterSet.ToLower())
            {
                case "unicode":
                    responseEncoding = Encoding.Unicode;
                    break;
                case "utf8":
                    responseEncoding = Encoding.UTF8;
                    break;
                default:
                    responseEncoding = Encoding.Default;
                    break;
            }
            return responseEncoding;
        }


        public string Post(string url, string postData)
        {
            return Post(url, postData, Encoding.UTF8);
        }
        public string Post(string url, string postData, Encoding postDataEncoding)
        {
            return Post(url, postDataEncoding.GetBytes(postData));
        }

        #region 将Cookie插入到HttpWebRequest对象中
        private void InsertCookieIntoRequest(HttpWebRequest request)
        {   
            if (this.lstCookies.Count > 0)
            {
                request.CookieContainer = new CookieContainer();
                foreach (Cookie cookie in this.lstCookies)
                {
                    request.CookieContainer.Add(cookie);
                }
            }
        }
        #endregion

        #region 从HttpWebResponse对象中保存Cookie
        private void SaveCookieFromResponse(HttpWebResponse response)
        {
            if (response.Cookies == null || response.Cookies.Count == 0)
            {
            }
            else
            {
                //清除之前的Cookie
                this.lstCookies.Clear();
                //更新Cookie
                foreach (Cookie cookie in response.Cookies)
                {
                    this.lstCookies.Add(cookie);
                }
            }
        }
        #endregion

        public string Post(string url, byte[] postData)
        {
            HttpWebResponse response;
            string s = string.Empty;

            String FullUrl = url;
            if (!url.ToUpper().StartsWith("HTTP://"))
            {
                FullUrl = string.Format("http://{0}{1}{2}", this.HostName, GetPortString(), url);
            }
            Uri requestUri = new Uri(FullUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Proxy = WebProxy;
            request.Method = "POST";
            request.Timeout = RequestTimeout;
            request.Referer = this.RefererUrl;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            this.RefererUrl = requestUri.ToString();

            request.ContentLength = postData.Length;

            InsertCookieIntoRequest(request);

            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                this.lstCookies.Clear();

                SaveCookieFromResponse(response);

                s = new StreamReader(response.GetResponseStream(), GetEncodingFromCharacterSet(response.CharacterSet)).ReadToEnd();
                response.Close();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            finally
            {
                response = null;
            }
            return s;
        }
    }
}
