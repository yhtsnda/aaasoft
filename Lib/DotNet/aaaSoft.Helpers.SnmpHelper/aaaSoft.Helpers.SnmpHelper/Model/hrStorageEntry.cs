using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers.Model
{
    public class hrStorageEntry
    {
        public enum hrStorageTypes
        {
            hrStorageOther = 1,
            hrStorageRam = 2,
            hrStorageVirtualMemory = 3,
            hrStorageFixedDisk = 4,
            hrStorageRemovableDisk = 5,
            hrStorageFloppyDisk = 6,
            hrStorageCompactDisc = 7,
            hrStorageRamDisk = 8,
            hrStorageFlashMemory = 9,
            hrStorageNetworkDisk = 10
        }
        /// <summary>
        /// 驱动器序号
        /// </summary>
        public Int32 hrStorageIndex;
        /// <summary>
        /// 驱动器类型
        /// </summary>
        public hrStorageTypes hrStorageType;
        /// <summary>
        /// 驱动器描述
        /// </summary>
        public String hrStorageDescr;
        /// <summary>
        /// 驱动器分配单元大小
        /// </summary>
        public Int32 hrStorageAllocationUnits;
        /// <summary>
        /// 驱动器大小(单位：分配单元数量)
        /// </summary>
        public Int32 hrStorageSize;
        /// <summary>
        /// 驱动器已使用大小(单位：分配单元数量)
        /// </summary>
        public Int32 hrStorageUsed;
        /// <summary>
        /// 获取驱动器大小(单位：字节)
        /// </summary>
        public Int64 StorageSize { get { return 1L * hrStorageSize * hrStorageAllocationUnits; } }
        /// <summary>
        /// 获取驱动器已使用大小(单位：字节)
        /// </summary>
        public Int64 StorageUsed { get { return 1L * hrStorageUsed * hrStorageAllocationUnits; } }
        /// <summary>
        /// 获取驱动器使用率
        /// </summary>
        public Int32 StorageUsage { get { return hrStorageUsed * 100 / hrStorageSize; } }

        /// <summary>
        /// 从dataRow得到StorageEntry对象
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static hrStorageEntry FromDataRow(System.Data.DataRow dataRow)
        {
            var hrStorageIndex = Convert.ToInt32(dataRow["hrStorageIndex"]);
            var hrStorageType = (hrStorageTypes)Enum.Parse(typeof(hrStorageTypes), dataRow["hrStorageType"].ToString());
            var hrStorageDescr = aaaSoft.Helpers.StringHelper.GetLeftString(Convert.ToString(dataRow["hrStorageDescr"]), " ").Replace("\0", "");
            var hrStorageAllocationUnits = Convert.ToInt32(dataRow["hrStorageAllocationUnits"]);
            var hrStorageSize = Convert.ToInt32(dataRow["hrStorageSize"]);
            var hrStorageUsed = Convert.ToInt32(dataRow["hrStorageUsed"]);

            var se = new hrStorageEntry()
            {
                hrStorageIndex = hrStorageIndex,
                hrStorageType = hrStorageType,
                hrStorageDescr = hrStorageDescr,
                hrStorageAllocationUnits = hrStorageAllocationUnits,
                hrStorageSize = hrStorageSize,
                hrStorageUsed = hrStorageUsed,
            };
            return se;
        }
    }
}
