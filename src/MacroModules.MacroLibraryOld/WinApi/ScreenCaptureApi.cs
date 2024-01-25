using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.WinApi
{
    internal class ScreenCaptureApi
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int x, int y);
    }
}
