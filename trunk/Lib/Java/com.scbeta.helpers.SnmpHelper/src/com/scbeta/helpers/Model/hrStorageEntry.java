/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers.Model;

import com.scbeta.helpers.ConsoleHelper;
import com.scbeta.helpers.MapDataTableHelper;
import com.scbeta.helpers.StringHelper;
import java.util.AbstractMap.SimpleEntry;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

/**
 *
 * @author aaa
 */
public class hrStorageEntry {

    public enum hrStorageTypes {

        hrStorageOther("1.3.6.1.2.1.25.2.1.1"), hrStorageRam("1.3.6.1.2.1.25.2.1.2"), hrStorageVirtualMemory("1.3.6.1.2.1.25.2.1.3"), hrStorageFixedDisk("1.3.6.1.2.1.25.2.1.4"), hrStorageRemovableDisk("1.3.6.1.2.1.25.2.1.5"), hrStorageFloppyDisk("1.3.6.1.2.1.25.2.1.6"), hrStorageCompactDisc("1.3.6.1.2.1.25.2.1.7"), hrStorageRamDisk("1.3.6.1.2.1.25.2.1.8"), hrStorageFlashMemory("1.3.6.1.2.1.25.2.1.9"), hrStorageNetworkDisk("1.3.6.1.2.1.25.2.1.10");
        private String hrStorageTypeString;

        private hrStorageTypes(String hrStorageTypeString) {
            this.hrStorageTypeString = hrStorageTypeString;
        }

        public static hrStorageTypes fromValue(String value) {
            for (hrStorageTypes tmp : hrStorageTypes.values()) {
                if (tmp.toValue().equals(value)) {
                    return tmp;
                }
            }
            return null;
        }

        public String toValue() {
            return hrStorageTypeString;
        }
    }
    // 驱动器序号
    public int hrStorageIndex;
    // 驱动器类型
    public hrStorageTypes hrStorageType;
    // 驱动器描述
    public String hrStorageDescr;
    // 驱动器分配单元大小
    public int hrStorageAllocationUnits;
    // 驱动器大小(单位：分配单元数量)
    public int hrStorageSize;
    // 驱动器已使用大小(单位：分配单元数量)
    public int hrStorageUsed;

    // 获取驱动器大小(单位：字节)
    public long getStorageSize() {
        return 1L * hrStorageSize * hrStorageAllocationUnits;
    }
    // 获取驱动器已使用大小(单位：字节)

    public long getStorageUsed() {
        return 1L * hrStorageUsed * hrStorageAllocationUnits;
    }
    // 获取驱动器使用率

    public int getStorageUsage() {
        return hrStorageUsed * 100 / hrStorageSize;
    }

    // 从Map对象得到实体数组
    public static hrStorageEntry[] fromMap(Map<String, List<String>> map) {
        int rowCount = MapDataTableHelper.GetRowCount(map);

        List<hrStorageEntry> EntryList = new LinkedList<hrStorageEntry>();
        for (int i = 0; i <= rowCount - 1; i++) {
            try {
                int hrStorageIndex = Integer.parseInt(map.get("hrStorageIndex").get(i));
                hrStorageTypes hrStorageType = hrStorageTypes.fromValue(map.get("hrStorageType").get(i));
                String hrStorageDescr = StringHelper.GetStringFromHexString(map.get("hrStorageDescr").get(i));
                hrStorageDescr = StringHelper.GetLeftString(hrStorageDescr," ");
                int hrStorageAllocationUnits = Integer.parseInt(map.get("hrStorageAllocationUnits").get(i));
                int hrStorageSize = Integer.parseInt(map.get("hrStorageSize").get(i));
                int hrStorageUsed = Integer.parseInt(map.get("hrStorageUsed").get(i));

                hrStorageEntry se = new hrStorageEntry();
                se.hrStorageIndex = hrStorageIndex;
                se.hrStorageType = hrStorageType;
                se.hrStorageDescr = hrStorageDescr;
                se.hrStorageAllocationUnits = hrStorageAllocationUnits;
                se.hrStorageSize = hrStorageSize;
                se.hrStorageUsed = hrStorageUsed;
                EntryList.add(se);
            } catch (Exception ex) {
            }
        }
        return EntryList.toArray(new hrStorageEntry[0]);
    }
}
