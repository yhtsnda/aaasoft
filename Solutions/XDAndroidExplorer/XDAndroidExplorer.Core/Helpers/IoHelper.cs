using System;
using System.Collections.Generic;
using System.Text;

namespace XDAndroidExplorer.Core.Helpers
{
    public class IoHelper
    {
        #region 得到父路径
        /// <summary>
        /// 得到父路径
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="spStr">路径分隔符</param>
        /// <returns></returns>
        public static String GetParentPath(String Path, Char spStr)
        {
            var strs = Path.Split(spStr);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= strs.Length - 1 - 1; i++)
            {
                if (String.IsNullOrEmpty(strs[i])) continue;
                sb.Append(spStr);
                sb.Append(strs[i]);
            }
            return sb.ToString();
        }
        #endregion
    }
}
