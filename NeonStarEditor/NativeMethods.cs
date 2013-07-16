using System;
using System.Runtime.InteropServices;

namespace NeonStar
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();
    }
}
