using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace aaaSoft.SkinEngine.SkinHelpers
{
    /// <summary>
    /// 消息传送器
    /// </summary>
    public class MessagePortal:NativeWindow
    {
        IntPtr srcHwnd;
        IntPtr desHwnd;

        public MessagePortal(IntPtr srcHwnd, IntPtr desHwnd)
        {
            this.srcHwnd = srcHwnd;
            this.desHwnd = desHwnd;
        }

        public void OpenPortal()
        {
            AssignHandle(srcHwnd);
        }

        public void ClosePortal()
        {
            ReleaseHandle();
        }

        protected override void WndProc(ref Message m)
        {
            NativeMethods.SendMessage(desHwnd, m.Msg, m.WParam, m.LParam);
            base.WndProc(ref m);
        }
    }
}
