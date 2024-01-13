using System.Drawing;
using MacroModules.MacroLibrary.Types;
using static MacroModules.MacroLibrary.WinApi.ScreenCaptureApi;

namespace MacroModules.MacroLibrary
{
    public static class ScreenCapture
    {
        public static Bitmap? GetScreenshot(Position topLeft, Position bottomRight)
        {
            int width = bottomRight.X - topLeft.X + 1;
            int height = bottomRight.Y - topLeft.Y + 1;
            if (width <= 0 || height <= 0)
            {
                return null;
            }

            IntPtr screenContext = GetDC(IntPtr.Zero);
            IntPtr targetContext = CreateCompatibleDC(screenContext);
            IntPtr screenBmp = CreateCompatibleBitmap(screenContext, width, height);
            IntPtr oldBmp = SelectObject(targetContext, screenBmp);
            BitBlt(targetContext, 0, 0, width, height, screenContext, topLeft.X, topLeft.Y, SRCCOPY);
            SelectObject(targetContext, oldBmp);
            DeleteDC(targetContext);
            ReleaseDC(IntPtr.Zero, screenContext);
            Bitmap bitmap = Image.FromHbitmap(screenBmp);
            DeleteObject(screenBmp);

            return bitmap;
        }

        public static uint GetPixelColor(Position pixelPos)
        {
            IntPtr screenContext = GetDC(IntPtr.Zero);
            uint rawColor = GetPixel(screenContext, pixelPos.X, pixelPos.Y);

            // The return value of GetPixel() is the BGR color of the pixel. The high two hex values
            // and low two hex values need to be swapped for the RGB format.
            uint formattedColor = (rawColor & 0x0000FF) << 16;  // Red
            formattedColor += rawColor & 0x00FF00;              // Green
            formattedColor += (rawColor & 0xFF0000) >>> 16;     // Blue

            ReleaseDC(IntPtr.Zero, screenContext);
            return formattedColor;
        }

        private static readonly uint SRCCOPY = 0xCC0020;
    }
}
