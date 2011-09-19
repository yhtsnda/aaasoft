/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import com.scbeta.helpers.Model.hrStorageEntry;
import com.scbeta.helpers.Model.ifEntry;
import java.io.IOException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.snmp4j.*;
import org.snmp4j.event.*;
import org.snmp4j.mp.*;
import org.snmp4j.security.AuthMD5;
import org.snmp4j.security.AuthSHA;
import org.snmp4j.security.PrivDES;
import org.snmp4j.security.SecurityModels;
import org.snmp4j.security.SecurityProtocols;
import org.snmp4j.security.USM;
import org.snmp4j.security.UsmUser;
import org.snmp4j.smi.*;
import org.snmp4j.transport.*;

/**
 *
 * @author aaa
 */
public class SnmpHelper {

    // 节点主机名
    public String PeerName;
    // 端口
    public int PeerPort;
    // 社区名
    public String Community;
    // SNMP协议版本
    public int SnmpVersion;
    private String securityName;
    private String passPhrase;
    private String authProtocol;
    //超时时间
    public int timeout = 10 * 1000;
    private long lastGetStorageInfoDataTime = 0;
    private hrStorageEntry[] AllStorageEntryArray;
    private long lastGetInterfaceInfoDataTime = 0;
    private ifEntry[] AllIfEntryArray;

    // 构造函数(用于SNMPv1,2)
    public SnmpHelper(String peerName, String community, int SnmpVersion) {
        Init(peerName, 161, community, SnmpVersion);
    }

    // 构造函数(用于SNMPv1,2)
    public SnmpHelper(String peerName, int peerPort, String community,
            int SnmpVersion) {
        Init(peerName, peerPort, community, SnmpVersion);
    }

    /**
     * 构造函数(用于SNMPv3)
     * @param peerName
     * @param peerPort
     * @param securityName
     * @param PassPhrase
     * @param AuthProtocol 
     */
    public SnmpHelper(String peerName, int peerPort, String securityName, String passPhrase, String authProtocol) {
    }

    // 初始化函数
    private void Init(String peerName, int peerPort, String community,
            int SnmpVersion) {
        this.PeerName = peerName;
        this.PeerPort = peerPort;
        this.Community = community;
        switch (SnmpVersion) {
            case 1:
                this.SnmpVersion = SnmpConstants.version1;
                break;
            case 2:
                this.SnmpVersion = SnmpConstants.version2c;
                break;
            case 3:
                this.SnmpVersion = SnmpConstants.version3;
                break;
        }
    }

    private void InitV3(String peerName, int peerPort, String securityName, String passPhrase, String authProtocol) {
        this.PeerName = peerName;
        this.PeerPort = peerPort;
        this.SnmpVersion = SnmpConstants.version3;
        this.securityName = securityName;
        this.passPhrase = passPhrase;
        this.authProtocol = authProtocol;
    }

    // 从VB中取出值
    private String GetValueFromVB(VariableBinding vb) {
        if (vb == null) {
            return null;
        } else if (vb.getOid() == null) {
            return null;
        } else if (Null.isExceptionSyntax(vb.getVariable().getSyntax())) {
            return null;
        } else {
            Variable var = vb.getVariable();

            if (var instanceof OctetString) {
                OctetString os = (OctetString) var;
                return os.toHexString();
            } else if (var instanceof TimeTicks) {
                TimeTicks varObj = (TimeTicks) var;
                long milliseconds = varObj.toMilliseconds();
                return Long.toString(milliseconds);
            }
            return vb.toValueString();
        }
    }

    private CommunityTarget getTarget() {
        Address targetAddress = new UdpAddress(String.format("%s/%s", PeerName,
                PeerPort));
        CommunityTarget target = new CommunityTarget();
        target.setCommunity(new OctetString(Community));
        target.setAddress(targetAddress);
        target.setVersion(SnmpVersion);
        target.setRetries(1);
        target.setTimeout(timeout);

        return target;
    }

    private Snmp getSnmp() throws IOException {
        TransportMapping transport = new DefaultUdpTransportMapping();
        Snmp snmp = new Snmp(transport);
        //如果是第三版协议
        if (SnmpVersion == SnmpConstants.version3) {
            USM usm = new USM(SecurityProtocols.getInstance(), new OctetString(MPv3.createLocalEngineID()), 0);
            SecurityModels.getInstance().addSecurityModel(usm);
            OID authenticationProtocol = null;
            if (this.authProtocol.equals("md5")) {
                authenticationProtocol = AuthMD5.ID;
            } else if (this.authProtocol.equals("sha")) {
                authenticationProtocol = AuthSHA.ID;
            }

            // 添加用户
            snmp.getUSM().addUser(
                    new OctetString(this.securityName), new UsmUser(
                    new OctetString(this.securityName), authenticationProtocol, new OctetString(this.passPhrase), PrivDES.ID, new OctetString(this.passPhrase)));
        }
        transport.listen();
        return snmp;
    }

    // 根据oid调用get方法取第一个值
    public String Get(String oid) {
        String rtnString = null;

        //准备相关数据
        CommunityTarget target = getTarget();


        OID targetOID = new OID(oid);

        PDU requestPDU = new PDU();
        requestPDU.add(new VariableBinding(targetOID));
        requestPDU.setType(PDU.GET);

        VariableBinding vb = null;

        try {
            Snmp SimpleSnmp = this.getSnmp();

            //发送请求
            PDU responsePDU = null;
            ResponseEvent responseEvent = SimpleSnmp.send(requestPDU, target);
            responsePDU = responseEvent.getResponse();

            if (responsePDU != null) {
                vb = responsePDU.get(0);
            }

            rtnString = GetValueFromVB(vb);
            SimpleSnmp.close();
        } catch (IOException e) {
            e.printStackTrace();
        }

        if (rtnString == null) {
            return null;
        } else {
            return rtnString.trim();
        }
    }

    // 根据oid调用walk方法得到结果
    public Map<String, String> Walk(String oid) {
        Map<String, String> mapWalkResult = new LinkedHashMap<String, String>();

        CommunityTarget target = this.getTarget();

        OID targetOID = new OID(oid);
        PDU requestPDU = new PDU();
        requestPDU.add(new VariableBinding(targetOID));
        requestPDU.setType(PDU.GETNEXT);

        try {
            Snmp snmp = this.getSnmp();

            boolean finished = false;

            while (!finished) {
                VariableBinding vb = null;

                PDU responsePDU = snmp.sendPDU(requestPDU, target);
                if (responsePDU
                        != null) {
                    vb = responsePDU.get(0);
                }

                if (responsePDU == null) {
                    finished = true;
                } else if (responsePDU.getErrorStatus() != 0) {
                    finished =
                            true;
                } else if (vb.getOid() == null) {
                    finished = true;
                } else if (vb.getOid().size() < targetOID.size()) {
                    finished = true;
                } else if (targetOID.leftMostCompare(targetOID.size(),
                        vb.getOid()) != 0) {
                    finished =
                            true;
                } else if (Null.isExceptionSyntax(vb.getVariable().getSyntax())) {
                    finished =
                            true;
                } else if (vb.getOid().compareTo(targetOID) <= 0) {
                    finished =
                            true;

                } else {
                    mapWalkResult.put(vb.getOid().toString(), this.GetValueFromVB(vb));

                    requestPDU.setRequestID(new Integer32(0));
                    requestPDU.set(0, vb);
                }
            }
            snmp.close();
        } catch (IOException e) {
            System.out.println("IOException: " + e);
        }
        return mapWalkResult;
    }

    // 根据oid调用walk方法得到一个List对象
    public Map<String, List<String>> WalkMap(String oid) {
        return WalkMap(oid, null);
    }

    // 根据oid调用walk方法得到一个Map对象
    public Map<String, List<String>> WalkMap(String oid, Map<String, String> mapOidDesc) {
        Map<String, String> result = Walk(oid);
        return ConvertSnmpWalkResultToMap(result, mapOidDesc);
    }

    //将Snmp的Walk方法得到的结果转换为List对象
    public static Map<String, List<String>> ConvertSnmpWalkResultToMap(Map<String, String> mapResult, Map<String, String> mapOidDesc) {
        if (mapResult == null || mapResult.isEmpty()) {
            return null;
        }
        Map<String, List<String>> mapDataTable = new LinkedHashMap<String, List<String>>();

        String CurrentOid = "";
        List<String> CurrentList = null;

        for (String key : mapResult.keySet()) {
            String value = mapResult.get(key);

            String totalKeyString = key;
            String[] totalKeyArray = totalKeyString.split("\\.");
            String lastKey = totalKeyArray[totalKeyArray.length - 1];
            String parentKeyString = totalKeyString.substring(0, totalKeyString.length() - lastKey.length() - 1);

            if (!parentKeyString.equals(CurrentOid)) {
                CurrentOid = parentKeyString;
                CurrentList = new ArrayList<String>();

                //列名称
                String columnName = CurrentOid;
                if (mapOidDesc != null) {
                    if (mapOidDesc.containsKey(columnName)) {
                        columnName = mapOidDesc.get(columnName);
                    }
                }
                mapDataTable.put(columnName, CurrentList);
            }
            CurrentList.add(value);
        }
        return mapDataTable;
    }

    //获取系统描述
    public String GetSystemDescription() {
        try {
            String oid = "1.3.6.1.2.1.1.1.0";
            return StringHelper.GetStringFromHexString(Get(oid));
        } catch (Exception ex) {
            return null;
        }
    }

    //获取系统名称
    public String GetSystemName() {
        try {
            String oid = "1.3.6.1.2.1.1.5.0";
            return StringHelper.GetStringFromHexString(Get(oid));
        } catch (Exception ex) {
            return null;
        }
    }

    //获取系统启动时间(单位：微秒)
    public String GetSystemUpTime() {
        //SNMP服务启动时间 1.3.6.1.2.1.1.3.0
        //操作系统启动时间 1.3.6.1.2.1.25.1.1.0
        String oid = "1.3.6.1.2.1.25.1.1.0";
        String TimeSpanString = Get(oid);
        return TimeSpanString;
    }

    //获取系统CPU占用率
    public int GetSystemCpuUsage() {
        String oid = "1.3.6.1.2.1.25.3.3.1.2";
        Map<String, List<String>> map = WalkMap(oid);

        if (map == null) {
            return -1;
        }
        int cpuCount = MapDataTableHelper.GetRowCount(map);
        int totalPercent = 0;

        for (String columnName : map.keySet()) {
            List<String> columnData = map.get(columnName);
            for (String cpuUsageString : columnData) {
                totalPercent += Integer.parseInt(cpuUsageString);
            }
        }
        return totalPercent / cpuCount;
    }

    //获取系统进程数
    public int GetSystemProcessCount() {
        String oid = ".1.3.6.1.2.1.25.1.6.0";
        String processCountString = Get(oid);
        int processCount = -1;
        if (null == processCountString || processCountString.equals("")) {
        } else {
            processCount = Integer.parseInt(processCountString);
        }
        return processCount;
    }

    public Map<String, List<String>> GetStorageMap() {
        String oid = "1.3.6.1.2.1.25.2.3";
        Map<String, String> dictOidDesc = new LinkedHashMap<String, String>();
        //hrStorageTable表的列
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.1", "hrStorageIndex");
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.2", "hrStorageType");
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.3", "hrStorageDescr");
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.4", "hrStorageAllocationUnits");
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.5", "hrStorageSize");
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.6", "hrStorageUsed");
        dictOidDesc.put("1.3.6.1.2.1.25.2.3.1.7", "hrStorageAllocationFailures");


        Map<String, List<String>> list = WalkMap(oid, dictOidDesc);
        return list;
    }

    // 得到存储信息数据
    public hrStorageEntry[] GetStorageInfo() {
        long timeSp = (System.currentTimeMillis() - lastGetStorageInfoDataTime) / 1000;
        //如果是还未获取过或者在60秒内已经获取过
        if (AllStorageEntryArray == null || timeSp > 60) {
            lastGetStorageInfoDataTime = System.currentTimeMillis();
            Map<String, List<String>> map = GetStorageMap();
            if (map == null) {
                return null;
            }
            AllStorageEntryArray = hrStorageEntry.fromMap(map);
        }
        return AllStorageEntryArray;
    }

    // 获取物理内存信息
    public hrStorageEntry GetPhysicalMemoryInfo() {
        hrStorageEntry[] storageEntryArray = GetStorageInfo();
        if (storageEntryArray == null) {
            return null;
        }

        for (hrStorageEntry tmp : storageEntryArray) {
            if (tmp.hrStorageType == hrStorageEntry.hrStorageTypes.hrStorageRam) {
                return tmp;
            }
        }
        return null;
    }

    // 获取虚拟内存信息
    public hrStorageEntry GetVirtualMemoryInfo() {
        //如果是Windows服务器，这个值为物理内存与虚拟内存之和
        hrStorageEntry[] storageEntryArray = GetStorageInfo();
        if (storageEntryArray == null) {
            return null;
        }
        for (hrStorageEntry tmp : storageEntryArray) {
            if (tmp.hrStorageType == hrStorageEntry.hrStorageTypes.hrStorageVirtualMemory) {
                return tmp;
            }
        }
        return null;
    }

    // 获取硬盘各分区信息
    public hrStorageEntry[] GetFixedDiskStorageInfo() {
        hrStorageEntry[] storageEntryArray = GetStorageInfo();
        if (storageEntryArray == null) {
            return null;
        }

        List<hrStorageEntry> tmpList = new ArrayList<hrStorageEntry>();
        for (hrStorageEntry tmp : storageEntryArray) {
            if (tmp.hrStorageType == hrStorageEntry.hrStorageTypes.hrStorageFixedDisk) {
                tmpList.add(tmp);
            }
        }
        return tmpList.toArray(new hrStorageEntry[0]);
    }

    // 得到网卡信息表
    public Map<String, List<String>> GetInterfaceMap() {
        String oid = "1.3.6.1.2.1.2.2";
        Map<String, String> dictOidDesc = new LinkedHashMap<String, String>();
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.1", "ifIndex");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.2", "ifDescr");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.3", "ifType");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.4", "ifMtu");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.5", "ifSpeed");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.6", "ifPhysAddress");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.7", "ifAdminStatus");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.8", "ifOperStatus");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.9", "ifLastChange");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.10", "ifInOctets");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.11", "ifInUcastPkts");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.12", "ifInNUcastPkts");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.13", "ifInDiscards");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.14", "ifInErrors");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.15", "ifInUnknownProtos");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.16", "ifOutOctets");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.17", "ifOutUcastPkts");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.18", "ifOutNUcastPkts");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.19", "ifOutDiscards");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.20", "ifOutErrors");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.21", "ifOutQLen");
        dictOidDesc.put("1.3.6.1.2.1.2.2.1.22", "ifSpecific");
        Map<String, List<String>> map = WalkMap(oid, dictOidDesc);
        return map;
    }

    // 得到网卡信息数组
    public ifEntry[] GetInterfaceInfo() {
        long timeSp = (System.currentTimeMillis() - lastGetInterfaceInfoDataTime) / 1000;
        //如果是还未获取过或者在60秒内已经获取过
        if (AllIfEntryArray == null || timeSp > 60) {
            lastGetInterfaceInfoDataTime = System.currentTimeMillis();
            Map<String, List<String>> map = GetInterfaceMap();
            if (map == null) {
                return null;
            }
            AllIfEntryArray = ifEntry.fromMap(map);
        }
        return AllIfEntryArray;
    }

    // 获取物理网卡信息
    public ifEntry[] GetPhysicalInterfaceInfo() {
        ifEntry[] interfaceInfoArray = GetInterfaceInfo();
        if (interfaceInfoArray == null) {
            return null;
        }

        List<ifEntry> tmpList = new ArrayList<ifEntry>();
        for (ifEntry item : interfaceInfoArray) {
            if (item.ifType == ifEntry.ifTypes.ethernetCsmacd
                    //MTU值不能为0
                    && item.ifMtu != 0
                    //网卡速率为10的整数倍
                    && item.ifSpeed % 10 == 0
                    //MAC地址不能为空
                    && item.ifPhysAddress != null && !item.ifPhysAddress.isEmpty()
                    //MAC地址不能为“fe:ff:ff:ff:ff:ff”
                    && !"fe:ff:ff:ff:ff:ff".equals(item.ifPhysAddress)) {
                tmpList.add(item);
            }
        }
        return tmpList.toArray(new ifEntry[0]);
    }
}
