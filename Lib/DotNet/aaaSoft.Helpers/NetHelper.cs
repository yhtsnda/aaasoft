using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers
{
    /// <summary>
    /// 网络辅助类
    /// </summary>
    public static class NetHelper
    {
        #region 获取随机端口(1024~65535)
        /// <summary>
        /// 获取随机端口(1024~65535)
        /// </summary>
        /// <returns></returns>
        public static int GetRandomPort()
        {
            Random rnd = new Random();
            return rnd.Next(1024, 65535);
        }
        #endregion
    }
}
