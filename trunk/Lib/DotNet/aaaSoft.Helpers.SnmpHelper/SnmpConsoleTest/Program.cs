using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using aaaSoft.Helpers;
using SnmpSharpNet;

namespace SnmpConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SnmpHelper sh = new SnmpHelper("192.168.0.107", 161, "public", 2);

            Single abc = 0;
            while (false)
            {
                var currentAbc = sh.GetSystemCpuUsage();
                if (currentAbc != abc)
                {
                    Console.WriteLine(DateTime.Now.ToString() + ":  CPU   " + currentAbc);
                    abc = currentAbc;
                }
                Thread.Sleep(100);
            }

            while (true)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("系统描述：" + sh.GetSystemDescription());
                Console.WriteLine("系统名称：" + sh.GetSystemName());
                //Console.WriteLine("系统启动时间：" + aaaSoft.Helpers.DateTimeHelper.ConvertTimespanToString(sh.GetSystemUpTime()));
                Console.WriteLine("系统进程数：" + sh.GetSystemProcessCount());
                Console.WriteLine("      -----内存部分----");
                var ramInfo = sh.GetPhysicalMemoryInfo();
                //Console.WriteLine("物理内存大小：" + aaaSoft.Helpers.IoHelper.GetFileLengthLevelString(ramInfo.StorageSize, 1));
                Console.WriteLine("物理内存占用率：" + ramInfo.StorageUsage + "%");
                var vramInfo = sh.GetVirtualMemoryInfo();
                //Console.WriteLine("虚拟内存大小：" + aaaSoft.Helpers.IoHelper.GetFileLengthLevelString(vramInfo.StorageSize, 1));
                Console.WriteLine("虚拟内存占用率：" + vramInfo.StorageUsage + "%");

                Console.WriteLine("      -----磁盘部分----");
                var fixedDiskInfos = sh.GetFixedDiskStorageInfo();

                Console.WriteLine("分区描述  分区大小  已使用大小  使用率");
                foreach (var info in fixedDiskInfos)
                {
                    /*
                    Console.WriteLine(String.Format(
                        "{0}        {1}     {2}     {3}%"
                        , info.hrStorageDescr
                        , aaaSoft.Helpers.IoHelper.GetFileLengthLevelString(info.StorageSize,1)
                        , aaaSoft.Helpers.IoHelper.GetFileLengthLevelString(info.StorageUsed,1)
                        , info.StorageUsage));
                     */
                }
                Console.WriteLine("      -----网卡部分----");
                var interfaceInfoArray = sh.GetPhysicalInterfaceInfo();
                Console.WriteLine("接口描述\t速率\t物理地址\t接收数据\t发送数据");
                foreach (var item in interfaceInfoArray)
                {
                    Console.WriteLine(String.Format(
                        "{0}\t{1}\t{2}\t{3}\t{4}"
                        , item.ifDescr
                        , item.ifSpeed
                        , item.ifPhysAddress
                        , item.ifInOctets
                        , item.ifOutOctets
                        ));
                }

                Console.WriteLine("      -----其他----");
                Console.WriteLine("系统CPU占用率：" + sh.GetSystemCpuUsage() + "%");


                Console.ReadLine();
            }
        }
    }
}
