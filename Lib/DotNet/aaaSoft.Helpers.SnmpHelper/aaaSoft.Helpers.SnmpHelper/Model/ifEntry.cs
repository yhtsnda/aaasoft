using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers.Model
{
    public class ifEntry
    {
        #region ifTypes枚举
        public enum ifTypes
        {
            other = 1,
            regular1822 = 2,
            hdh1822 = 3,
            ddnX25 = 4,
            rfc877x25 = 5,
            ethernetCsmacd = 6,
            iso88023Csmacd = 7,
            iso88024TokenBus = 8,
            iso88025TokenRing = 9,
            atm = 37,
            miox25 = 38,
            sonet = 39,
            x25ple = 40,
            iso88022llc = 41,
            localTalk = 42,
            smdsDxi = 43,
            frameRelayService = 44,
            v35 = 45,
            hssi = 46,
            hippi = 47,
            modem = 48,
            aal5 = 49,
            sonetPath = 50,
            sonetVT = 51,
            smdsIcip = 52,
            propVirtual = 53,
            propMultiplexor = 54,
            ieee80212 = 55,
            fibreChannel = 56,
            hippiInterface = 57,
            frameRelayInterconnect = 58,
            aflane8023 = 59,
            aflane8025 = 60,
            cctEmul = 61,
            fastEther = 62,
            isdn = 63,
            v11 = 64,
            v36 = 65,
            g703at64k = 66,
            g703at2mb = 67,
            qllc = 68,
            fastEtherFX = 69,
            channel = 70,
            ieee80211 = 71,
            ibm370parChan = 72,
            escon = 73,
            dlsw = 74,
            isdns = 75,
            isdnu = 76,
            lapd = 77,
            ipSwitch = 78,
            rsrb = 79,
            voiceEM = 100,
            voiceFXO = 101,
            voiceFXS = 102,
            voiceEncap = 103,
            voiceOverIp = 104,
            atmDxi = 105,
            atmFuni = 106,
            atmIma = 107,
            pppMultilinkBundle = 108,
            ipOverCdlc = 109,
            dtm = 140,
            dcn = 141,
            ipForward = 142,
            msdsl = 143,
            ieee1394 = 144,
            if_gsn = 145,
            dvbRccMacLayer = 146,
            dvbRccDownstream = 147,
            dvbRccUpstream = 148,
            atmVirtual = 149,
            mplsTunnel = 150,
            srp = 151,
            voiceOverAtm = 152,
            voiceOverFrameRelay = 153,
            idsl = 154,
            compositeLink = 155,
            ss7SigLink = 156,
            propWirelessP2P = 157,
            frForward = 158,
            rfc1483 = 159,
            usb = 160,
            ieee8023adLag = 161,
            bgppolicyaccounting = 162,
            frf16MfrBundle = 163,
            h323Gatekeeper = 164,
            h323Proxy = 165,
            mpls = 166,
            mfSigLink = 167,
            hdsl2 = 168,
            shdsl = 169,
            ds1FDL = 170,
            pos = 171,
            dvbAsiIn = 172,
            dvbAsiOut = 173,
            plc = 174,
            nfas = 175,
            tr008 = 176,
            gr303RDT = 177,
            gr303IDT = 178,
            isup = 179,
            propDocsWirelessMaclayer = 180,
            propDocsWirelessDownstream = 181,
            ifType182 = 182,
            linegroup = 210,
            voiceEMFGD = 211,
            voiceFGDEANA = 212,
            voiceDID = 213,
            mpegTransport = 214,
            sixToFour = 215,
            gtp = 216,
            pdnEtherLoop1 = 217,
            pdnEtherLoop2 = 218,
            opticalChannelGroup = 219,
            homepna = 220,
            gfp = 221,
            ciscoISLvlan = 222,
            actelisMetaLOOP = 223,
            fcipLink = 224,
            rpr = 225,
            qam = 226,
            lmp = 227,
            cblVectaStar = 228,
            docsCableMCmtsDownstream = 229,
            adsl2 = 230,
            macSecControlledIF = 231,
            macSecUncontrolledIF = 232,
            aviciOpticalEther = 233,
            atmbond = 234
        }
        #endregion

        #region ifAdminStatusEnum枚举
        public enum ifAdminStatusEnum
        {
            up = 1,
            down = 2,
            testing = 3
        }
        #endregion

        #region ifOperStatusEnum枚举
        public enum ifOperStatusEnum
        {
            up = 1,
            down = 2,
            testing = 3,
            unknown = 4,
            dormant = 5,
            notPresent = 6,
            lowerLayerDown = 7,
        }
        #endregion

        public Int32 ifIndex;
        /// <summary>
        /// 网络设备描述
        /// </summary>
        public String ifDescr;
        /// <summary>
        /// 网络设备类型
        /// </summary>
        public ifTypes ifType;
        /// <summary>
        /// MTU(数据包最大字节数)
        /// </summary>
        public Int32 ifMtu;
        /// <summary>
        /// 速度
        /// </summary>
        public UInt32 ifSpeed;
        /// <summary>
        /// 物理地址(MAC地址)
        /// </summary>
        public String ifPhysAddress;
        public ifAdminStatusEnum ifAdminStatus;
        public ifOperStatusEnum ifOperStatus;
        public String ifLastChange;
        /// <summary>
        /// 已接收数据字节数
        /// </summary>
        public UInt32 ifInOctets;
        public UInt32 ifInUcastPkts;
        public UInt32 ifInNUcastPkts;
        public UInt32 ifInDiscards;
        public UInt32 ifInErrors;
        public UInt32 ifInUnknownProtos;
        /// <summary>
        /// 已发送数据字节数
        /// </summary>
        public UInt32 ifOutOctets;
        public UInt32 ifOutUcastPkts;
        public UInt32 ifOutNUcastPkts;
        public UInt32 ifOutDiscards;
        public UInt32 ifOutErrors;
        public UInt32 ifOutQLen;
        public String ifSpecific;


        public static ifEntry FromDataRow(System.Data.DataRow dataRow)
        {
            var ifIndex = Convert.ToInt32(dataRow["ifIndex"]);
            var ifDescr = Convert.ToString(dataRow["ifDescr"]).Replace("\0", "");
            var ifType = (ifTypes)Convert.ToInt32(dataRow["ifType"]);
            var ifMtu = Convert.ToInt32(dataRow["ifMtu"]);
            var ifSpeed = Convert.ToUInt32(dataRow["ifSpeed"]);
            var ifPhysAddress = Convert.ToString(dataRow["ifPhysAddress"]);
            var ifAdminStatus = (ifAdminStatusEnum)Convert.ToUInt32(dataRow["ifAdminStatus"]);
            var ifOperStatus = (ifOperStatusEnum)Convert.ToUInt32(dataRow["ifOperStatus"]);
            var ifLastChange = Convert.ToString(dataRow["ifLastChange"]);
            var ifInOctets = Convert.ToUInt32(dataRow["ifInOctets"]);
            var ifInUcastPkts = Convert.ToUInt32(dataRow["ifInUcastPkts"]);
            var ifInNUcastPkts = Convert.ToUInt32(dataRow["ifInNUcastPkts"]);
            var ifInDiscards = Convert.ToUInt32(dataRow["ifInDiscards"]);
            var ifInErrors = Convert.ToUInt32(dataRow["ifInErrors"]);
            var ifInUnknownProtos = Convert.ToUInt32(dataRow["ifInUnknownProtos"]);
            var ifOutOctets = Convert.ToUInt32(dataRow["ifOutOctets"]);
            var ifOutUcastPkts = Convert.ToUInt32(dataRow["ifOutUcastPkts"]);
            var ifOutNUcastPkts = Convert.ToUInt32(dataRow["ifOutNUcastPkts"]);
            var ifOutDiscards = Convert.ToUInt32(dataRow["ifOutDiscards"]);
            var ifOutErrors = Convert.ToUInt32(dataRow["ifOutErrors"]);
            var ifOutQLen = Convert.ToUInt32(dataRow["ifOutQLen"]);
            var ifSpecific = Convert.ToString(dataRow["ifSpecific"]);

            var ie = new ifEntry()
            {
                ifIndex = ifIndex,
                ifDescr = ifDescr,
                ifType = ifType,
                ifMtu = ifMtu,
                ifSpeed = ifSpeed,
                ifPhysAddress = ifPhysAddress,
                ifAdminStatus = ifAdminStatus,
                ifOperStatus = ifOperStatus,
                ifLastChange = ifLastChange,
                ifInOctets = ifInOctets,
                ifInUcastPkts = ifInUcastPkts,
                ifInNUcastPkts = ifInNUcastPkts,
                ifInDiscards = ifInDiscards,
                ifInErrors = ifInErrors,
                ifInUnknownProtos = ifInUnknownProtos,
                ifOutOctets = ifOutOctets,
                ifOutUcastPkts = ifOutUcastPkts,
                ifOutNUcastPkts = ifOutNUcastPkts,
                ifOutDiscards = ifOutDiscards,
                ifOutErrors = ifOutErrors,
                ifOutQLen = ifOutQLen,
                ifSpecific = ifSpecific
            };
            return ie;
        }
    }
}
