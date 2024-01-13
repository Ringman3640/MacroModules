using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.WinApi
{
    public class ScreenCaptureApi
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int cx, int cy);

        [DllImport("Gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

        [DllImport("Gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, uint rop);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("Gdi32.dll")]
        public static extern bool DeleteObject(IntPtr ho);

        [DllImport("Gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int x, int y);
    }
}
