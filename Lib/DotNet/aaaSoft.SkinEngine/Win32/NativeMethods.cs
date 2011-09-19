using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
    [SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        // Methods
        //Dll:user32.dll
        [DllImport("user32")]
        public static extern long DrawIcon(long hdc, long x, long y, long hIcon);
        [DllImport("user32")]
        public static extern long DestroyIcon(long hIcon);
        [DllImport("user32.dll")]
        public static extern IntPtr mouse_event(Microsoft.Win32.NativeConsts.MouseEventFlags dwflags, Int32 dx, Int32 dy, Int32 dwData, Int32 dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, Int64 wParam, Int64 lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SetActiveWindow(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(IntPtr hMenu, int uPosition, int uFlags);
        [DllImport("user32.dll")]
        public static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uFlags);
        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int SetCapture(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool RemoveMenu(IntPtr hMenu, int uPosition, int uFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr nNewLong);
        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);
        [DllImport("user32.dll")]
        public static extern Boolean FlashWindow(IntPtr hwnd, Boolean bInvert);
        [DllImport("user32.dll")]
        public static extern bool TrackMouseEvent(ref NativeStructs.LPTRACKMOUSEEVENT lpEventTrack);
        [DllImport("user32.dll")]
        public static extern bool TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetWindowsHookEx(
            NativeConsts.HookType idHook,
            NativeCallbacks.HookProc lpfn,
            IntPtr hMod,
            int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(
            int idHook,
            int nCode,
            IntPtr wParam,
            ref NativeStructs.CWPSTRUCT lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, int ID);
        //Dll:shell32.dll
        [DllImport("shell32", EntryPoint = "ExtractIconEx")]
        public static extern long ExtractIcon(string lpszFile, int niconIndex, ref IntPtr phiconLarge, ref IntPtr phiconSmall, int nIcons);
        //Dll:kernel32.dll
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool RedrawWindow(IntPtr hwnd, IntPtr rcUpdate, IntPtr hrgnUpdate, int flags);
    }
}
