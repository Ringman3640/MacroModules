using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroModules.MacroLibrary.WinApi
{
    internal static class DisplayInfoApi
    {
        [DllImport("User32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, ref object? dwData);

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public delegate bool MonitorEnumProc(IntPtr hdm, IntPtr hdc, ref Rect clipArea, ref object? dwData);
    }
}
