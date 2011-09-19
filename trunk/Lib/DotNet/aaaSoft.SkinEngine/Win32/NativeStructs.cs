using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
    public class NativeStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTSTRUCT
        {
            public int x;
            public int y;
            public POINTSTRUCT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LPTRACKMOUSEEVENT
        {
            public uint cbSize;
            public uint dwFlags;
            public IntPtr hwndTrack;
            public uint dwHoverTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public Microsoft.Win32.NativeConsts.WindowMessage Message;
            public IntPtr hWnd;
        }


        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct tagNCCALCSIZE_PARAMS
        {

            /// RECT[3]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public tagRECT[] rgrc;

            /// PWINDOWPOS->tagWINDOWPOS*
            public System.IntPtr lppos;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct tagRECT
        {

            /// LONG->int
            public int left;

            /// LONG->int
            public int top;

            /// LONG->int
            public int right;

            /// LONG->int
            public int bottom;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct tagWINDOWPOS
        {

            /// HWND->HWND__*
            public System.IntPtr hwnd;

            /// HWND->HWND__*
            public System.IntPtr hwndInsertAfter;

            /// int
            public int x;

            /// int
            public int y;

            /// int
            public int cx;

            /// int
            public int cy;

            /// UINT->unsigned int
            public uint flags;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct HWND__
        {

            /// int
            public int unused;
        }
    }
}
