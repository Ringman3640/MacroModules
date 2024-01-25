using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroModules.MacroLibrary.WinApi
{
    internal static class WindowFocusApi
    {
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, object lParam);

        [DllImport("User32.dll", CharSet = CharSet.Ansi)]
        public static extern int GetWindowTextA(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public delegate bool EnumWindowsProc(IntPtr hwnd, object lParam);
    }
}
