using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace aaaSoft.Helpers
{
    public class ThreadHelper
    {
        /// <summary>
        /// 未命名委托类型
        /// </summary>
        public delegate void UnnamedDelegate();

        #region 判断线程是否为主线程
        /// <summary>
        /// 当前线程是否为主线程
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentThreadMainThread()
        {
            return IsMainThread(Thread.CurrentThread);
        }

        /// <summary>
        /// 判断线程是否为主线程
        /// </summary>
        /// <param name="trd"></param>
        /// <returns></returns>
        public static bool IsMainThread(Thread trd)
        {
            return trd.GetApartmentState() == ApartmentState.STA;
        }
        #endregion
    }
}
