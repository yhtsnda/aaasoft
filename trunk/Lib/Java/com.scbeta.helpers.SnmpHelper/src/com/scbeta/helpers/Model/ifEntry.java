/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers.Model;

import com.scbeta.helpers.MapDataTableHelper;
import com.scbeta.helpers.StringHelper;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

/**
 *
 * @author aaa
 */
public class ifEntry {
    
    public enum ifTypes {
        
        other(1),
        regular1822(2),
        hdh1822(3),
        ddnX25(4),
        rfc877x25(5),
        ethernetCsmacd(6),
        iso88023Csmacd(7),
        iso88024TokenBus(8),
        iso88025TokenRing(9),
        atm(37),
        miox25(38),
        sonet(39),
        x25ple(40),
        iso88022llc(41),
        localTalk(42),
        smdsDxi(43),
        frameRelayService(44),
        v35(45),
        hssi(46),
        hippi(47),
        modem(48),
        aal5(49),
        sonetPath(50),
        sonetVT(51),
        smdsIcip(52),
        propVirtual(53),
        propMultiplexor(54),
        ieee80212(55),
        fibreChannel(56),
        hippiInterface(57),
        frameRelayInterconnect(58),
        aflane8023(59),
        aflane8025(60),
        cctEmul(61),
        fastEther(62),
        isdn(63),
        v11(64),
        v36(65),
        g703at64k(66),
        g703at2mb(67),
        qllc(68),
        fastEtherFX(69),
        channel(70),
        ieee80211(71),
        ibm370parChan(72),
        escon(73),
        dlsw(74),
        isdns(75),
        isdnu(76),
        lapd(77),
        ipSwitch(78),
        rsrb(79),
        voiceEM(100),
        voiceFXO(101),
        voiceFXS(102),
        voiceEncap(103),
        voiceOverIp(104),
        atmDxi(105),
        atmFuni(106),
        atmIma(107),
        pppMultilinkBundle(108),
        ipOverCdlc(109),
        dtm(140),
        dcn(141),
        ipForward(142),
        msdsl(143),
        ieee1394(144),
        if_gsn(145),
        dvbRccMacLayer(146),
        dvbRccDownstream(147),
        dvbRccUpstream(148),
        atmVirtual(149),
        mplsTunnel(150),
        srp(151),
        voiceOverAtm(152),
        voiceOverFrameRelay(153),
        idsl(154),
        compositeLink(155),
        ss7SigLink(156),
        propWirelessP2P(157),
        frForward(158),
        rfc1483(159),
        usb(160),
        ieee8023adLag(161),
        bgppolicyaccounting(162),
        frf16MfrBundle(163),
        h323Gatekeeper(164),
        h323Proxy(165),
        mpls(166),
        mfSigLink(167),
        hdsl2(168),
        shdsl(169),
        ds1FDL(170),
        pos(171),
        dvbAsiIn(172),
        dvbAsiOut(173),
        plc(174),
        nfas(175),
        tr008(176),
        gr303RDT(177),
        gr303IDT(178),
        isup(179),
        propDocsWirelessMaclayer(180),
        propDocsWirelessDownstream(181),
        ifType182(182),
        linegroup(210),
        voiceEMFGD(211),
        voiceFGDEANA(212),
        voiceDID(213),
        mpegTransport(214),
        sixToFour(215),
        gtp(216),
        pdnEtherLoop1(217),
        pdnEtherLoop2(218),
        opticalChannelGroup(219),
        homepna(220),
        gfp(221),
        ciscoISLvlan(222),
        actelisMetaLOOP(223),
        fcipLink(224),
        rpr(225),
        qam(226),
        lmp(227),
        cblVectaStar(228),
        docsCableMCmtsDownstream(229),
        adsl2(230),
        macSecControlledIF(231),
        macSecUncontrolledIF(232),
        aviciOpticalEther(233),
        atmbond(234);
        private int value;
        
        private ifTypes(int value) {
            this.value = value;
        }
        
        public int toValue() {
            return value;
        }
        
        public static ifTypes fromValue(int value) {
            for (ifTypes tmp : ifTypes.values()) {
                if (tmp.toValue() == value) {
                    return tmp;
                }
            }
            return null;
        }
    }

    //ifAdminStatusEnum枚举
    public enum ifAdminStatusEnum {
        
        up(1),
        down(2),
        testing(3);
        private int value;
        
        private ifAdminStatusEnum(int value) {
            this.value = value;
        }
        
        public int toValue() {
            return value;
        }
        
        public static ifAdminStatusEnum fromValue(int value) {
            for (ifAdminStatusEnum tmp : ifAdminStatusEnum.values()) {
                if (tmp.toValue() == value) {
                    return tmp;
                }
            }
            return null;
        }
    }

    //ifOperStatusEnum枚举
    public enum ifOperStatusEnum {
        
        up(1),
        down(2),
        testing(3),
        unknown(4),
        dormant(5),
        notPresent(6),
        lowerLayerDown(7);
        private int value;
        
        private ifOperStatusEnum(int value) {
            this.value = value;
        }
        
        public int toValue() {
            return value;
        }
        
        public static ifOperStatusEnum fromValue(int value) {
            for (ifOperStatusEnum tmp : ifOperStatusEnum.values()) {
                if (tmp.toValue() == value) {
                    return tmp;
                }
            }
            return null;
        }
    }
    public int ifIndex;
    // 网络设备描述
    public String ifDescr;
    // 网络设备类型
    public ifTypes ifType;
    // MTU(数据包最大字节数)
    public int ifMtu;
    // 速度
    public long ifSpeed;
    // 物理地址(MAC地址)
    public String ifPhysAddress;
    public ifAdminStatusEnum ifAdminStatus;
    public ifOperStatusEnum ifOperStatus;
    public String ifLastChange;
    // 已接收数据字节数
    public long ifInOctets;
    public long ifInUcastPkts;
    public long ifInNUcastPkts;
    public long ifInDiscards;
    public long ifInErrors;
    public long ifInUnknownProtos;
    // 已发送数据字节数
    public long ifOutOctets;
    public long ifOutUcastPkts;
    public long ifOutNUcastPkts;
    public long ifOutDiscards;
    public long ifOutErrors;
    public long ifOutQLen;
    public String ifSpecific;

    // 从Map对象得到实体数组
    public static ifEntry[] fromMap(Map<String, List<String>> map) {
        int rowCount = MapDataTableHelper.GetRowCount(map);
        
        List<ifEntry> EntryList = new LinkedList<ifEntry>();
        for (int i = 0; i <= rowCount - 1; i++) {
            try {
                int ifIndex = Integer.parseInt(map.get("ifIndex").get(i));
                String ifDescr = StringHelper.GetStringFromHexString(map.get("ifDescr").get(i));
                ifTypes ifType = ifTypes.fromValue(Integer.parseInt(map.get("ifType").get(i)));
                int ifMtu = Integer.parseInt(map.get("ifMtu").get(i));
                long ifSpeed = Long.parseLong(map.get("ifSpeed").get(i));
                String ifPhysAddress = map.get("ifPhysAddress").get(i);
                ifAdminStatusEnum ifAdminStatus = ifAdminStatusEnum.fromValue(Integer.parseInt(map.get("ifAdminStatus").get(i)));
                ifOperStatusEnum ifOperStatus = ifOperStatusEnum.fromValue(Integer.parseInt(map.get("ifOperStatus").get(i)));
                String ifLastChange = map.get("ifLastChange").get(i);
                Long ifInOctets = Long.parseLong(map.get("ifInOctets").get(i));
                Long ifInUcastPkts = Long.parseLong(map.get("ifInUcastPkts").get(i));
                Long ifInNUcastPkts = Long.parseLong(map.get("ifInNUcastPkts").get(i));
                Long ifInDiscards = Long.parseLong(map.get("ifInDiscards").get(i));
                Long ifInErrors = Long.parseLong(map.get("ifInErrors").get(i));
                Long ifInUnknownProtos = Long.parseLong(map.get("ifInUnknownProtos").get(i));
                Long ifOutOctets = Long.parseLong(map.get("ifOutOctets").get(i));
                Long ifOutUcastPkts = Long.parseLong(map.get("ifOutUcastPkts").get(i));
                Long ifOutNUcastPkts = Long.parseLong(map.get("ifOutNUcastPkts").get(i));
                Long ifOutDiscards = Long.parseLong(map.get("ifOutDiscards").get(i));
                Long ifOutErrors = Long.parseLong(map.get("ifOutErrors").get(i));
                Long ifOutQLen = Long.parseLong(map.get("ifOutQLen").get(i));
                String ifSpecific = map.get("ifSpecific").get(i);
                
                ifEntry ie = new ifEntry();
                ie.ifIndex = ifIndex;
                ie.ifDescr = ifDescr;
                ie.ifType = ifType;
                ie.ifMtu = ifMtu;
                ie.ifSpeed = ifSpeed;
                ie.ifPhysAddress = ifPhysAddress;
                ie.ifAdminStatus = ifAdminStatus;
                ie.ifOperStatus = ifOperStatus;
                ie.ifLastChange = ifLastChange;
                ie.ifInOctets = ifInOctets;
                ie.ifInUcastPkts = ifInUcastPkts;
                ie.ifInNUcastPkts = ifInNUcastPkts;
                ie.ifInDiscards = ifInDiscards;
                ie.ifInErrors = ifInErrors;
                ie.ifInUnknownProtos = ifInUnknownProtos;
                ie.ifOutOctets = ifOutOctets;
                ie.ifOutUcastPkts = ifOutUcastPkts;
                ie.ifOutNUcastPkts = ifOutNUcastPkts;
                ie.ifOutDiscards = ifOutDiscards;
                ie.ifOutErrors = ifOutErrors;
                ie.ifOutQLen = ifOutQLen;
                ie.ifSpecific = ifSpecific;
                
                EntryList.add(ie);
            } catch (Exception ex) {
            }
        }
        return EntryList.toArray(new ifEntry[0]);
    }
}
