using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Win32;

namespace aaaSoft.SkinEngine
{
    class MyMessageFilter : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            NativeConsts.WindowMessage WindowsMessage = (NativeConsts.WindowMessage)m.Msg;
            if (WindowsMessage  == NativeConsts.WindowMessage.WM_PAINT)
            {
                Control ctl = Control.FromHandle(m.HWnd);
                if (ctl != null)
                {
                    if (ctl is ProgressBar)
                    {
                        Graphics g = ctl.CreateGraphics();
                        g.FillRectangle(Brushes.Red, new Rectangle(0, 0, 20, 20));
                    }
                }
            }
            Control ctll = Control.FromHandle(m.HWnd);
            if (ctll != null)
            {
                if (ctll is Form)
                {
                    Debug.Print(ctll.Name);
                    Debug.Print(m.Msg.ToString());
                }
            }
            if (WindowsMessage == NativeConsts.WindowMessage.WM_CREATE)
            {
                MessageBox.Show(m.HWnd.ToString());
            }
            return false;
        }
    }
}
