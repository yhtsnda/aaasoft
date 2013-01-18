using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseKeeperCore
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DatabaseConnectionInfo
    {
        /// <summary>
        /// 主机
        /// </summary>
        public String Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public Int32 Port { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }
    }
}
