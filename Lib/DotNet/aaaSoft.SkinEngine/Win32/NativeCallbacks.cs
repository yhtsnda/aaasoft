using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Win32
{
    public class NativeCallbacks
    {
        public delegate int HookProc(int nCode, IntPtr wParam, ref NativeStructs.CWPSTRUCT lParam);
    }
}
