using System;
using System.Collections.Generic;
using System.Text;
using SnmpSharpNet;
using System.Data;
using System.Linq;
using aaaSoft.Helpers.Model;

namespace aaaSoft.Helpers
{
    /// <summary>
    /// SNMP辅助类
    /// </summary>
    public class SnmpHelper
    {
        #region 属性字段
        /// <summary>
        /// 节点主机名
        /// </summary>
        public String PeerName;
        /// <summary>
        /// 端口
        /// </summary>
        public Int32 PeerPort;
        /// <summary>
        /// 社区名
        /// </summary>
        public String Community;
        /// <summary>
        /// SNMP协议版本
        /// </summary>
        public SnmpVersion SnmpVersion = SnmpVersion.Ver2;

        private SimpleSnmp SimpleSnmp;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="peerName">节点主机名</param>
        /// <param name="community">社区名</param>
        /// <param name="SnmpVersion">SNMP协议版本</param>
        public SnmpHelper(String peerName, String community, Int32 SnmpVersion)
            : this(peerName, 161, community, SnmpVersion)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="peerName">节点主机名</param>
        /// <param name="peerPort">端口</param>
        /// <param name="community">社区名</param>
        /// <param name="SnmpVersion">SNMP协议版本</param>
        public SnmpHelper(String peerName, Int32 peerPort, String community, Int32 SnmpVersion)
        {
            this.PeerName = peerName;
            this.PeerPort = peerPort;
            this.Community = community;
            switch (SnmpVersion)
            {
                case 1:
                    this.SnmpVersion = SnmpSharpNet.SnmpVersion.Ver1;
                    break;
                case 2:
                    this.SnmpVersion = SnmpSharpNet.SnmpVersion.Ver2;
                    break;
                case 3:
                    this.SnmpVersion = SnmpSharpNet.SnmpVersion.Ver3;
                    break;
            }

            SimpleSnmp = new SimpleSnmp(peerName, peerPort, community);
        }
        #endregion

        #region 根据oid调用get方法取第一个值
        /// <summary>
        /// 根据oid调用get方法取第一个值
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public AsnType Get(String oid)
        {
            var result = SimpleSnmp.Get(SnmpVersion, new String[] { oid });
            if (result != null)
            {
                foreach (var key in result.Keys)
                {
                    var value = result[key];
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据oid调用get方法取第一个值的ToString()的值
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public String GetString(String oid)
        {
            var result = Get(oid);
            if (result == null)
                return null;
            return result.ToString();
        }
        #endregion

        #region 根据oid调用walk方法
        /// <summary>
        /// 根据oid调用walk方法得到结果
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public Dictionary<Oid, AsnType> Walk(String oid)
        {
            return SimpleSnmp.Walk(SnmpVersion, oid);
        }

        /// <summary>
        /// 根据oid调用walk方法得到一个DataTable对象
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public DataTable WalkDataTable(String oid)
        {
            return WalkDataTable(oid, null);
        }

        /// <summary>
        /// 根据oid调用walk方法得到一个DataTable对象
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="dictOidDesc"></param>
        /// <returns></returns>
        public DataTable WalkDataTable(String oid, Dictionary<String, String> dictOidDesc)
        {
            var result = Walk(oid);
            var dt = ConvertSnmpWalkResultToDataTable(result, dictOidDesc);
            return dt;
        }
        #endregion

        #region 将Snmp的Walk方法得到的结果转换为DataTable对象
        /// <summary>
        /// 将Snmp的Walk方法得到的结果转换为DataTable对象
        /// </summary>
        /// <param name="dictResult">结果字典</param>
        /// <returns></returns>
        public static DataTable ConvertSnmpWalkResultToDataTable(Dictionary<Oid, AsnType> dictResult)
        {
            return ConvertSnmpWalkResultToDataTable(dictResult, null);
        }

        /// <summary>
        /// 将Snmp的Walk方法得到的结果转换为DataTable对象
        /// </summary>
        /// <param name="dictResult">结果字典</param>
        /// <param name="dictOidDesc">OID与字符串对照字典</param>
        /// <returns></returns>
        public static DataTable ConvertSnmpWalkResultToDataTable(Dictionary<Oid, AsnType> dictResult, Dictionary<String, String> dictOidDesc)
        {
            if (dictResult == null || dictResult.Count == 0)
                return null;

            List<KeyValuePair<String, List<AsnType>>> lstDataTable = new List<KeyValuePair<String, List<AsnType>>>();

            String CurrentOid = String.Empty;
            List<AsnType> CurrentList = null;
            foreach (var key in dictResult.Keys)
            {
                var value = dictResult[key];
                var totalKeyString = key.ToString();
                var totalKeyArray = totalKeyString.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var lastKey = totalKeyArray[totalKeyArray.Length - 1];
                var parentKeyString = totalKeyString.Substring(0, totalKeyString.Length - lastKey.Length - 1);

                if (parentKeyString != CurrentOid)
                {
                    CurrentOid = parentKeyString;
                    CurrentList = new List<AsnType>();

                    //列名称
                    var columnName = CurrentOid;
                    if (dictOidDesc != null)
                    {
                        if (dictOidDesc.ContainsKey(columnName))
                            columnName = dictOidDesc[columnName];
                    }
                    lstDataTable.Add(new KeyValuePair<String, List<AsnType>>(columnName, CurrentList));
                }
                CurrentList.Add(value);
            }

            var dt = new DataTable();
            //添加列名称
            foreach (var pair in lstDataTable)
            {
                dt.Columns.Add(pair.Key.ToString());
            }

            //列数量
            var columnCount = lstDataTable.Count;
            //行数量
            var rowCount = lstDataTable[0].Value.Count;

            //添加行
            for (int i = 0; i <= rowCount - 1; i++)
            {
                List<String> lstRowItem = new List<String>();
                for (int j = 0; j <= columnCount - 1; j++)
                {
                    var cellString = lstDataTable[j].Value[i].ToString();
                    if (dictOidDesc != null)
                    {
                        if (dictOidDesc.ContainsKey(cellString))
                            cellString = dictOidDesc[cellString];
                    }
                    lstRowItem.Add(cellString);
                }
                dt.Rows.Add(lstRowItem.ToArray());
            }

            return dt;
        }
        #endregion


        #region 得到存储信息数据
        private DateTime lastGetStorageInfoDataTime = DateTime.MinValue;
        private hrStorageEntry[] AllStorageEntryArray;
        public DataTable GetStorageTable()
        {
            var oid = "1.3.6.1.2.1.25.2.3";
            Dictionary<String, String> dictOidDesc = new Dictionary<string, string>();
            //hrStorageTable表的列
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.1", "hrStorageIndex");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.2", "hrStorageType");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.3", "hrStorageDescr");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.4", "hrStorageAllocationUnits");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.5", "hrStorageSize");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.6", "hrStorageUsed");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.3.1.7", "hrStorageAllocationFailures");
            //hrStorageTypes
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.1", "hrStorageOther");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.2", "hrStorageRam");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.3", "hrStorageVirtualMemory");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.4", "hrStorageFixedDisk");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.5", "hrStorageRemovableDisk");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.6", "hrStorageFloppyDisk");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.7", "hrStorageCompactDisc");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.8", "hrStorageRamDisk");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.9", "hrStorageFlashMemory");
            dictOidDesc.Add("1.3.6.1.2.1.25.2.1.10", "hrStorageNetworkDisk");

            var dt = WalkDataTable(oid, dictOidDesc);
            return dt;
        }

        /// <summary>
        /// 得到存储信息数据
        /// </summary>
        /// <returns></returns>
        public hrStorageEntry[] GetStorageInfo()
        {
            //如果是还未获取过或者在60秒内已经获取过
            if (AllStorageEntryArray == null || (DateTime.Now - lastGetStorageInfoDataTime).TotalSeconds > 60)
            {
                var dt = GetStorageTable();
                if (dt == null)
                    return null;
                AllStorageEntryArray = new hrStorageEntry[dt.Rows.Count];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    AllStorageEntryArray[i] = hrStorageEntry.FromDataRow(dt.Rows[i]);
                }
            }
            return AllStorageEntryArray;
        }
        #endregion

        #region 得到网卡信息数据
        private DateTime lastGetInterfaceInfoDataTime = DateTime.MinValue;
        private ifEntry[] AllIfEntryArray;

        /// <summary>
        /// 得到网卡信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetInterfaceTable()
        {
            var oid = "1.3.6.1.2.1.2.2";
            Dictionary<String, String> dictOidDesc = new Dictionary<string, string>();
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.1", "ifIndex");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.2", "ifDescr");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.3", "ifType");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.4", "ifMtu");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.5", "ifSpeed");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.6", "ifPhysAddress");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.7", "ifAdminStatus");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.8", "ifOperStatus");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.9", "ifLastChange");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.10", "ifInOctets");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.11", "ifInUcastPkts");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.12", "ifInNUcastPkts");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.13", "ifInDiscards");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.14", "ifInErrors");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.15", "ifInUnknownProtos");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.16", "ifOutOctets");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.17", "ifOutUcastPkts");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.18", "ifOutNUcastPkts");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.19", "ifOutDiscards");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.20", "ifOutErrors");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.21", "ifOutQLen");
            dictOidDesc.Add("1.3.6.1.2.1.2.2.1.22", "ifSpecific");
            var dt = WalkDataTable(oid, dictOidDesc);
            return dt;
        }

        /// <summary>
        /// 得到网卡信息数组
        /// </summary>
        /// <returns></returns>
        public ifEntry[] GetInterfaceInfo()
        {
            //如果是还未获取过或者在60秒内已经获取过
            if (AllIfEntryArray == null || (DateTime.Now - lastGetStorageInfoDataTime).TotalSeconds > 60)
            {
                var dt = GetInterfaceTable();
                if (dt == null)
                    return null;
                AllIfEntryArray = new ifEntry[dt.Rows.Count];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    AllIfEntryArray[i] = ifEntry.FromDataRow(dt.Rows[i]);
                }
            }
            return AllIfEntryArray;
        }

        #endregion



        #region 获取系统描述
        /// <summary>
        /// 获取系统描述
        /// </summary>
        /// <returns></returns>
        public String GetSystemDescription()
        {
            var oid = "1.3.6.1.2.1.1.1.0";
            return GetString(oid);
        }
        #endregion

        #region 获取系统名称
        /// <summary>
        /// 获取系统名称
        /// </summary>
        /// <returns></returns>
        public String GetSystemName()
        {
            var oid = "1.3.6.1.2.1.1.5.0";
            return GetString(oid);
        }
        #endregion

        #region 获取系统启动时间
        /// <summary>
        /// 获取系统启动时间
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetSystemUpTime()
        {
            //SNMP服务启动时间 1.3.6.1.2.1.1.3.0
            //操作系统启动时间 1.3.6.1.2.1.25.1.1.0
            var oid = "1.3.6.1.2.1.25.1.1.0";
            var TimeSpanString = GetString(oid);

            var ReturnTimeSpan = new TimeSpan(0);
            if (!String.IsNullOrEmpty(TimeSpanString))
            {
                try
                {
                    var TmpList = TimeSpanString.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    TmpList.ForEach(p =>
                        {
                            if (p.EndsWith("d"))
                            {
                                var days = Convert.ToInt32(p.Substring(0, p.Length - "d".Length));
                                ReturnTimeSpan = ReturnTimeSpan.Add(new TimeSpan(days, 0, 0, 0));
                            }
                            else if (p.EndsWith("h"))
                            {
                                var hours = Convert.ToInt32(p.Substring(0, p.Length - "h".Length));
                                ReturnTimeSpan = ReturnTimeSpan.Add(new TimeSpan(hours, 0, 0));
                            }
                            else if (p.EndsWith("m"))
                            {
                                var minutes = Convert.ToInt32(p.Substring(0, p.Length - "m".Length));
                                ReturnTimeSpan = ReturnTimeSpan.Add(new TimeSpan(0, minutes, 0));
                            }
                            else if (p.EndsWith("ms"))
                            {
                                var milleseconds = Convert.ToInt32(p.Substring(0, p.Length - "ms".Length));
                                ReturnTimeSpan = ReturnTimeSpan.Add(new TimeSpan(0, 00, 0, 0, milleseconds));
                            }
                            else if (p.EndsWith("s"))
                            {
                                var seconds = Convert.ToInt32(p.Substring(0, p.Length - "s".Length));
                                ReturnTimeSpan = ReturnTimeSpan.Add(new TimeSpan(0, 0, seconds));
                            }
                        });
                }
                catch { }
            }
            return ReturnTimeSpan;
        }
        #endregion

        #region 获取系统进程数
        /// <summary>
        /// 获取系统进程数
        /// </summary>
        /// <returns></returns>
        public Int32 GetSystemProcessCount()
        {
            var oid = ".1.3.6.1.2.1.25.1.6.0";
            var processCountString = GetString(oid);
            var processCount = -1;
            if (!String.IsNullOrEmpty(processCountString))
                Int32.TryParse(processCountString, out processCount);
            return processCount;
        }
        #endregion

        #region 获取系统CPU占用率
        /// <summary>
        /// 获取系统CPU占用率
        /// </summary>
        /// <returns></returns>
        public Int32 GetSystemCpuUsage()
        {
            var oid = "1.3.6.1.2.1.25.3.3.1.2";
            var dt = WalkDataTable(oid);

            if (dt == null)
                return -1;
            Int32 cpuCount = dt.Rows.Count;
            Int32 totalPercent = 0;
            foreach (DataRow row in dt.Rows)
            {
                totalPercent += Convert.ToInt32(row[0]);
            }
            return totalPercent / cpuCount;
        }
        #endregion

        #region 获取物理内存信息
        /// <summary>
        /// 获取物理内存信息
        /// </summary>
        /// <returns></returns>
        public hrStorageEntry GetPhysicalMemoryInfo()
        {
            var storageEntryArray = GetStorageInfo();
            if (storageEntryArray == null)
                return null;
            return storageEntryArray.Single(item =>
                {
                    return item.hrStorageType == hrStorageEntry.hrStorageTypes.hrStorageRam;
                });
        }
        #endregion

        #region 获取虚拟内存信息
        /// <summary>
        /// 获取虚拟内存信息
        /// </summary>
        /// <returns></returns>
        public hrStorageEntry GetVirtualMemoryInfo()
        {
            //如果是Windows服务器，这个值为物理内存与虚拟内存之和
            var storageEntryArray = GetStorageInfo();
            if (storageEntryArray == null)
                return null;
            return storageEntryArray.Single(item =>
            {
                return item.hrStorageType == hrStorageEntry.hrStorageTypes.hrStorageVirtualMemory;
            });
        }
        #endregion

        #region 获取硬盘各分区信息
        /// <summary>
        /// 获取硬盘各分区信息
        /// </summary>
        /// <returns></returns>
        public hrStorageEntry[] GetFixedDiskStorageInfo()
        {
            var storageEntryArray = GetStorageInfo();
            if (storageEntryArray == null)
                return null;
            return storageEntryArray.Where(item =>
                {
                    return item.hrStorageType == hrStorageEntry.hrStorageTypes.hrStorageFixedDisk;
                }).ToArray();
        }
        #endregion

        #region 获取物理网卡信息
        /// <summary>
        /// 获取物理网卡信息
        /// </summary>
        /// <returns></returns>
        public ifEntry[] GetPhysicalInterfaceInfo()
        {
            var interfaceInfoArray = GetInterfaceInfo();
            if (interfaceInfoArray == null)
                return null;
            return interfaceInfoArray.Where(item =>
                {
                    return
                        //接口类型为以太网卡
                        item.ifType == ifEntry.ifTypes.ethernetCsmacd
                        //MTU值不能为0
                        && item.ifMtu != 0
                        //网卡速率为10的整数倍
                        && item.ifSpeed % 10 == 0
                        //MAC地址不能为空
                        && !String.IsNullOrEmpty(item.ifPhysAddress)
                        //MAC地址不能为“FE FF FF FF FF FF”
                        && item.ifPhysAddress != "FE FF FF FF FF FF";
                }).ToArray();
        }
        #endregion
    }
}
