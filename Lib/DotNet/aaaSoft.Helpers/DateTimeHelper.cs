using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取当前日期字符串
        /// </summary>
        /// <returns></returns>
        public static String GetNowDateString()
        {
            return GetNowDateTimeStringBase("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取当前时间字符串
        /// </summary>
        /// <returns></returns>
        public static String GetNowTimeString()
        {
            return GetNowDateTimeStringBase("HH:mm:ss");
        }

        /// <summary>
        /// 获取当前日期与时间字符串
        /// </summary>
        /// <returns></returns>
        public static String GetNowDateTimeString()
        {
            return GetNowDateTimeStringBase("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取当前日期与时间字符串(仅数字)
        /// </summary>
        /// <returns></returns>
        public static String GetNowDateTimeStringNo()
        {
            return GetNowDateTimeStringBase("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 获取指定格式的当前日期与时间字符串
        /// </summary>
        /// <param name="formateString"></param>
        /// <returns></returns>
        public static String GetNowDateTimeStringBase(String formateString)
        {
            return DateTime.Now.ToString(formateString);
        }
    }
}
