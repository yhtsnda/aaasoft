using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MsSqlServerKeeperProvider
{
    public class Provider : DatabaseKeeperCore.IDatabaseKeeperProvider
    {
        public const String ConnectionStringTemplate = "Data Source = {0},{1}; Initial Catalog = master; User Id = {2}; Password = {3};";

        public const String SQL_GetDatabaseName = "SELECT name FROM master.sys.databases ORDER BY name";
        public const String SQL_BackupDatabase = @"
EXEC master.sys.sp_addumpdevice 'disk', '{2}', '{1}'
BACKUP DATABASE {0} TO {2} WITH INIT
EXEC master.sys.sp_dropdevice '{2}'";

        public const String SQL_RestoreDatabase = @"
use master
declare @dbname varchar(20)
set @dbname='{0}'
declare @sql nvarchar(500)
declare @spid int--SPID 值是当用户进行连接时指派给该连接的一个唯一的整数
set @sql='declare getspid cursor for
select spid from sysprocesses where dbid=db_id('''+@dbname+''')'
exec (@sql)
open getspid
fetch next from getspid into @spid
while @@fetch_status<>-1--如果FETCH 语句没有执行失败或此行不在结果集中。
begin
    exec('kill '+@spid)--终止正常连接
    fetch next from getspid into @spid
end
close getspid
deallocate getspid

RESTORE DATABASE {0}
FROM DISK = '{1}'
";


        public string GetDatabaseType()
        {
            return "Micrrosoft SQL Server";
        }

        public string GetProviderName()
        {
            return "";
        }

        public string GenerateConnectionString(DatabaseKeeperCore.DatabaseConnectionInfo databaseConnectionInfo)
        {
            if (databaseConnectionInfo.Port <= 0)
            {
                databaseConnectionInfo.Port = 1433;
            }
            return String.Format(ConnectionStringTemplate, databaseConnectionInfo.Host, databaseConnectionInfo.Port, databaseConnectionInfo.UserName, databaseConnectionInfo.Password);
        }

        public string[] GetDatabaseNameArray(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SQL_GetDatabaseName, connection);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adp.Fill(ds);

                DataTable dt = ds.Tables[0];
                List<String> nameList = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    nameList.Add(row[0].ToString());
                }
                return nameList.ToArray();
            }
        }

        public bool BackupDatabase(string connectionString, string databaseName, string backupFileName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String backupSQL = String.Format(SQL_BackupDatabase, databaseName, backupFileName, "KEEPER" + Guid.NewGuid().ToString().Replace("-",""));
                SqlCommand cmd = new SqlCommand(backupSQL, connection);
                cmd.CommandTimeout = 0;
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool RestoreDatabase(string connectionString, string databaseName, string backupFileName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String backupSQL = String.Format(SQL_RestoreDatabase, databaseName, backupFileName);
                SqlCommand cmd = new SqlCommand(backupSQL, connection);
                cmd.CommandTimeout = 0;
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
