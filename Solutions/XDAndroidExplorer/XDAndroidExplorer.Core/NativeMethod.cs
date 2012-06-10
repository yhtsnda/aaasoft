using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace XDAndroidExplorer.Core
{
    public class NativeMethod
    {
        public static void InitAdb()
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "AndroidInterface/adb.exe";
            cmd.StartInfo.Arguments = "start-server";
            cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.Start();
        }

        public static String PushFile(String local, String remote)
        {
            return ExecuteCommand(String.Format("push \"{0}\" \"{1}\"", local, remote));
        }

        /*暂时不用
        public static void PushFolder(String local, String remote)
        {
            System.IO.DirectoryInfo ldi = new System.IO.DirectoryInfo(local);
            XDAndroidExplorer.Core.IO.Folder rdi = new XDAndroidExplorer.Core.IO.Folder(remote);
            if (!ldi.Exists)
            {
                return;
            }

            //在手机上创建文件夹
            if (!rdi.CreateFolder(ldi.Name))
            {
                throw new System.IO.IOException(String.Format("在手机中的[{0}]下创建文件夹[{1}]失败！", remote));
            }


            String CurrentRemoteFolderPath = remote + "/" + ldi.Name;

            //传输文件
            foreach (var fileInfo in ldi.GetFiles())
            {
                PushFile(fileInfo.FullName, CurrentRemoteFolderPath);
            }

            //传输文件夹
            foreach (var folderInfo in ldi.GetDirectories())
            {
                PushFolder(folderInfo.FullName, CurrentRemoteFolderPath);
            }
        }
        */

        public static String PullFile(String remote, String local)
        {
            return ExecuteCommand(String.Format("pull \"{0}\" \"{1}\"", remote, local));
        }

        public static String Move(String srcPath, String desPath)
        {
            return ExecuteShellCommand(String.Format("mv \"{0}\" \"{1}\"", srcPath, desPath));
        }

        public static String ExecuteCommand(String cmdStr)
        {
            return ExecuteCommand(cmdStr, Encoding.UTF8, Encoding.UTF8);
        }
        public static String ExecuteCommand(String cmdStr, Encoding srcEncoding, Encoding desEncoding)
        {
            //For Debug
            Debug.Print(">" + cmdStr);

            cmdStr = desEncoding.GetString(srcEncoding.GetBytes(cmdStr));

            //实例化一个进程类
            Process cmd = new Process();

            cmd.StartInfo.FileName = "AndroidInterface/adb.exe";
            cmd.StartInfo.Arguments = cmdStr;
            cmd.StartInfo.StandardOutputEncoding = srcEncoding;

            cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常

            cmd.StartInfo.RedirectStandardInput = true; //标准输入
            cmd.StartInfo.RedirectStandardOutput = true; //标准输出
            cmd.StartInfo.RedirectStandardError = true; //错误输出

            //不显示命令行窗口界面
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            cmd.Start(); //启动进程

            String rtnStr = cmd.StandardOutput.ReadToEnd();
            //String rtnStr = cmd.StandardError.ReadToEnd();
            

            //For Debug
#warning For Debug
            Debug.Print("<" + rtnStr);

            return rtnStr;
        }

        public static String ExecuteShellCommand(String cmdStr)
        {
            return ExecuteCommand("shell " + cmdStr, Encoding.UTF8, Encoding.Default);
        }

        public static DateTime ConvertLinuxTimeStringToDateTime(String str)
        {
        	String[] strs = str.Split(' ');
            String weekDay = strs[0];
            String month = strs[1];
            String day = strs[2];
            String time = strs[3];
            String year = strs[4];
            String netString = String.Format("{0}, {1} {2} {3} {4}", weekDay, day, month, year, time);

            DateTime returnValue = DateTime.MinValue;
            DateTime.TryParse(netString, out returnValue);
            return returnValue;
        }
    }
}
