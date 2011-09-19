using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Win32;

namespace aaaSoft.SkinEngine.SkinHelpers
{
    class FormHelper
    {
        public static void PopupSysMenu(IntPtr hWnd, Point pt)
        {
            NativeMethods.ReleaseCapture();
            IntPtr systemMenu = NativeMethods.GetSystemMenu(hWnd, false);
            NativeMethods.EnableMenuItem(systemMenu, 1, 0x401);
            NativeMethods.EnableMenuItem(systemMenu, 2, 0x401);
            NativeMethods.TrackPopupMenu(systemMenu, 2, pt.X, pt.Y, 0, hWnd, IntPtr.Zero);
        }
    }
}
