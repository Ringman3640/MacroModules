using System.Drawing;
using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.WinApi
{
    /// <summary>
    /// Exposes the Windows API methods required for creating a message loop for hook funtionality.
    /// </summary>
    public class MessageLoopApi
    {
        [DllImport("User32.dll")]
        public static extern int GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("User32.dll")]
        public static extern bool TranslateMessage([In] ref Message lpMsg);

        [DllImport("User32.dll")]
        public static extern IntPtr DispatchMessage([In] ref Message lpMsg);

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point pt;
            public uint lPrivate;
        }
    }
}
