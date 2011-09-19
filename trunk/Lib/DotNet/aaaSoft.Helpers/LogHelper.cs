using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace aaaSoft.Helpers
{
    /// <summary>
    /// 日志辅助类
    /// </summary>
    public class LogHelper
    {
        private static Object objLock = new object();
        public class LogEventArgs : EventArgs
        {
            /// <summary>
            /// 日志文本
            /// </summary>
            public String LogText { get; set; }
            /// <summary>
            /// 日志颜色
            /// </summary>
            public Color LogColor { get; set; }
        }
        public delegate void LogEventHandler(LogEventArgs e);

        /// <summary>
        /// 日志推送事件
        /// </summary>
        public static event LogEventHandler LogPushed;

        /// <summary>
        /// 推送日志
        /// </summary>
        /// <param name="LogText">日志文本</param>
        public static void PushLog(String LogText)
        {
            PushLog(LogText, Color.Black);
        }
        /// <summary>
        /// 推送日志
        /// </summary>
        /// <param name="LogText">日志文本</param>
        /// <param name="LogColor">日志颜色</param>
        public static void PushLog(String LogText, Color LogColor)
        {
            lock (objLock)
            {
                if (LogPushed != null)
                {
                    LogEventArgs e = new LogEventArgs();
                    e.LogText = LogText;
                    e.LogColor = LogColor;
                    LogPushed(e);
                }
            }
        }
    }
}
