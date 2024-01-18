using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.WinApi
{
    internal class KeyStateApi
    {
        [DllImport("User32.dll")]
        public static extern short GetKeyState(int nVirtKey);
    }
}
