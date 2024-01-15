using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroModules.MacroLibrary.WinApi
{
    internal static class SystemMetricsApi
    {
        [DllImport("User32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
    }
}
