using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

internal static class Native
{
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern IntPtr LoadLibrary(string lpFileName);
}
