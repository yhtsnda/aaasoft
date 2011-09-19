using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace aaaSoft.SkinEngine.SkinHelpers
{
    /// <summary>
    /// 消息钩子类
    /// </summary>
    public class MessageHook
    {
        ~MessageHook()
        {
            Stop(false);
        }

        public event MessageCallback MessageOccurred;
        public delegate void MessageCallback(ref Message msg);


        private int hMessageHook = 0;

        private static Microsoft.Win32.NativeCallbacks.HookProc MessageHookProcedure;

        public void Start()
        {
            if (hMessageHook == 0)
            {
                MessageHookProcedure = new Microsoft.Win32.NativeCallbacks.HookProc(MessageHookProc);
                hMessageHook = Microsoft.Win32.NativeMethods.SetWindowsHookEx(
                    Microsoft.Win32.NativeConsts.HookType.WH_CALLWNDPROC,
                    MessageHookProcedure,

                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                     Microsoft.Win32.NativeMethods.GetCurrentThreadId()
                    );
                if (hMessageHook == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Stop(false);
                    try
                    {
                        throw new Win32Exception(errorCode); 
                    }
                    catch(Exception ex)
                    {
                        Debug.Print("Error From MessageHook.Start.Reason:" + ex.Message);
                    }
                }
            }
        }

        public void Stop()
        {
            this.Stop(true);
        }

        public void Stop(bool ThrowExceptions)
        {
            if (hMessageHook != 0)
            {
                int retMessage = Microsoft.Win32.NativeMethods.UnhookWindowsHookEx(hMessageHook);
                hMessageHook = 0;
                if (retMessage == 0 && ThrowExceptions)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private int MessageHookProc(int nCode, IntPtr wParam, ref Microsoft.Win32.NativeStructs.CWPSTRUCT lParam)
        {
            if ((nCode >= 0))
            {
                Message msg = Message.Create(lParam.hWnd, (int)lParam.Message, lParam.wParam, lParam.lParam);
                if (MessageOccurred != null)
                {
                    MessageOccurred(ref msg);
                }
                if (!msg.Result.Equals(IntPtr.Zero))
                {
                    nCode = -1;
                    lParam.Message = NativeConsts.WindowMessage.WM_NULL;
                }
            }
            return Microsoft.Win32.NativeMethods.CallNextHookEx(hMessageHook, nCode, wParam, ref lParam);
        }
    }
}