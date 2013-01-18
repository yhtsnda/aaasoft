using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseKeeperCore
{
    public interface IDatabaseKeeperProvider
    {
        /// <summary>
        /// 返回数据库类型
        /// </summary>
        /// <returns></returns>
        String GetDatabaseType();

        /// <summary>
        /// 返回Provider名称
        /// </summary>
        /// <returns></returns>
        String GetProviderName();

        /// <summary>
        /// 生成连接字符串
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        String GenerateConnectionString(DatabaseConnectionInfo databaseConnectionInfo);

        /// <summary>
        /// 得到数据库名称数组
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        String[] GetDatabaseNameArray(String connectionString);

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="backupFileName"></param>
        /// <returns></returns>
        Boolean BackupDatabase(String connectionString, String databaseName, String backupFileName);

        /// <summary>
        /// 还原数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="backupFileName"></param>
        /// <returns></returns>
        Boolean RestoreDatabase(String connectionString, String databaseName, String backupFileName);
    }
}
