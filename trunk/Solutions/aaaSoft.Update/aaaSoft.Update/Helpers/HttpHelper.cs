namespace aaaSoft.Update.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;

    public class HttpHelper
    {
        /*
        public static int DownloadSong(SongInfo si , WebProxy proxy)
        {
            return DownloadSong(si, proxy, 5);
        }

        public static int DownloadSong(SongInfo si, WebProxy proxy, int retryTimes)
        {
            HttpWebResponse response;
            List<Cookie> list = new List<Cookie>();
            Uri requestUri = new Uri(si.DownloadUrl);
            int num = 0;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            if (proxy != null)
            {
                request.Proxy = proxy;
            }
            request.Method = "GET";
            request.Timeout = 0x1388;
            request.Referer = "http://www.google.cn";
            if (list.Count > 0)
            {
                request.CookieContainer = new CookieContainer();
                for (int i = 0; i <= (list.Count - 1); i++)
                {
                    request.CookieContainer.Add(list[i]);
                }
            }
            FileStream stream = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (System.IO.File.Exists(si.FullFileName))
                {
                    FileInfo info = new FileInfo(si.FullFileName);
                    if (info.Length == response.ContentLength)
                    {
                        si.DownloadPercent = 1.0;
                        si.Length = info.Length;
                        response.Close();
                        return 1;
                    }
                    System.IO.File.Delete(si.FullFileName);
                }
                stream = new FileStream(si.FullFileName + ".tmp", FileMode.OpenOrCreate);
                stream.SetLength(0L);
                int count = 0;
                byte[] buffer = new byte[0x400];
                long num4 = 0L;
                long contentLength = response.ContentLength;
                si.Length = contentLength;
                do
                {
                    count = responseStream.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, count);
                    num4 += count;
                    si.DownloadPercent = Convert.ToDouble(num4) / ((double)contentLength);
                }
                while (num4 != contentLength);
                response.Close();
                stream.Close();
                System.IO.File.Move(si.FullFileName + ".tmp", si.FullFileName);
                num = 1;
            }
            catch
            {
                try
                {
                    stream.Close();
                    System.IO.File.Delete(si.FullFileName + ".tmp");
                }
                catch
                {
                }
                if (retryTimes > 0)
                {
                    num = DownloadSong(si, proxy, retryTimes - 1);
                }
            }
            finally
            {
                response = null;
            }
            return num;
        }
        */
        public static string GetUrlData(string URL)
        {
            return GetUrlData(URL, Encoding.Default, null, 5);
        }

        public static string GetUrlData(string URL,WebProxy proxy)
        {
            return GetUrlData(URL, Encoding.Default, proxy, 5);
        }

        public static string GetUrlData(string URL, Encoding strEncoding,WebProxy proxy, int retryTimes)
        {
            HttpWebResponse response;
            List<Cookie> list = new List<Cookie>();
            Uri requestUri = new Uri(URL);
            string s = "";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);

            if (proxy != null)
            {
                request.Proxy = proxy;
            }
            request.Method = "GET";
            request.Timeout = 0x1388;
            request.Referer = "http://www.google.cn";
            if (list.Count > 0)
            {
                request.CookieContainer = new CookieContainer();
                for (int i = 0; i <= (list.Count - 1); i++)
                {
                    request.CookieContainer.Add(list[i]);
                }
            }
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                s = new StreamReader(response.GetResponseStream(), strEncoding).ReadToEnd();
                response.Close();
            }
            catch
            {
                if (retryTimes > 0)
                {
                    s = GetUrlData(URL, strEncoding, proxy, retryTimes - 1);
                }
            }
            finally
            {
                response = null;
            }
            return HttpUtility.HtmlDecode(s);
        }

        //从URL中获取流
        public static Stream GetUrlStream(string URL, WebProxy proxy)
        {
            return GetUrlStream(URL, proxy, 5);
        }
        public static Stream GetUrlStream(string URL, WebProxy proxy, int retryTimes)
        {
            HttpWebResponse response;
            List<Cookie> list = new List<Cookie>();
            Uri requestUri = new Uri(URL);

            Stream s = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);

            if (proxy != null)
            {
                request.Proxy = proxy;
            }
            request.Method = "GET";
            request.Timeout = 0x1388;
            request.Referer = "http://www.google.cn";
            if (list.Count > 0)
            {
                request.CookieContainer = new CookieContainer();
                for (int i = 0; i <= (list.Count - 1); i++)
                {
                    request.CookieContainer.Add(list[i]);
                }
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                s = response.GetResponseStream();                
            }
            catch
            {
                if (retryTimes > 0)
                {
                    s = GetUrlStream(URL, proxy, retryTimes - 1);
                }
            }
            finally
            {
                response = null;
            }
            return s;
        }
    }
}

