using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace aaaSoft.Update.Helpers
{
    class ShellHelper
    {
        #region 创建快捷方式,返回结果表示是否创建成功
        ///<summary>创建快捷方式,返回结果表示是否创建成功</summary>
        ///<param name="SaveName">将快捷方式保存到的位置</param>
        ///<param name="TargetName">快捷方式指向的目标名</param>
        ///<param name="Arguments">执行参数,默认为空</param>
        ///<param name="WorkingDirectory">起始位置</param>
        ///<param name="WindowStyle">运行方式,1,常规窗口;3,最大化;7最小化.默认为1</param>
        ///<param name="Description">备注</param>
        ///<param name="IconLocation">图标</param>
        public static bool CreateShortCut(string SaveName, string TargetName, string Arguments, string WorkingDirectory, int WindowStyle, String Description, String IconLocation)
        {
            try
            {
                if (!SaveName.ToLower().EndsWith(".lnk"))
                {
                    SaveName += ".lnk";
                }

                Assembly ass = Assembly.Load(CompressHelper.DecompressBytes(aaaSoft.Update.Properties.Resources.Interop_IWshRuntimeLibrary_dll));
                Type WshShellClass = ass.GetType("IWshRuntimeLibrary.WshShellClass");
                Type IWshShortcut = ass.GetType("IWshRuntimeLibrary.IWshShortcut");

                Object obj = Activator.CreateInstance(WshShellClass);

                //WshShell的CreateShortcut方法
                MethodInfo WshShellClass_CreateShortcut = WshShellClass.GetMethod("CreateShortcut");
                Object WshShortcutObj = WshShellClass_CreateShortcut.Invoke(obj, new Object[] { SaveName });

                //目标
                IWshShortcut.GetProperty("TargetPath").SetValue(WshShortcutObj, TargetName, null);
                //命令行
                IWshShortcut.GetProperty("Arguments").SetValue(WshShortcutObj, Arguments, null);
                //运行方式
                IWshShortcut.GetProperty("WindowStyle").SetValue(WshShortcutObj, WindowStyle, null);
                //备注
                IWshShortcut.GetProperty("Description").SetValue(WshShortcutObj, Description, null);
                //起始位置
                IWshShortcut.GetProperty("WorkingDirectory").SetValue(WshShortcutObj, WorkingDirectory, null);
                //图标
                if (string.IsNullOrEmpty(IconLocation))
                {
                    IWshShortcut.GetProperty("IconLocation").SetValue(WshShortcutObj, TargetName, null);
                }
                else
                {
                    IWshShortcut.GetProperty("IconLocation").SetValue(WshShortcutObj, IconLocation, null);
                }

                //IWshShortcut_ClassClass的Save方法
                MethodInfo IWshShortcut_Save = IWshShortcut.GetMethod("Save");
                IWshShortcut_Save.Invoke(WshShortcutObj, new Object[] { });
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}