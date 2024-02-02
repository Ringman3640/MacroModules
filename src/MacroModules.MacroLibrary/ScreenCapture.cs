using System.Drawing;
using System.Drawing.Imaging;
using MacroModules.MacroLibrary.Types;
using static MacroModules.MacroLibrary.DisplayPositionTranslator;
using static MacroModules.MacroLibrary.WinApi.ScreenCaptureApi;

namespace MacroModules.MacroLibrary
{
    public static class ScreenCapture
    {
        public static Bitmap? GetScreenshot(Position topLeft, Position bottomRight)
        {
            Translate(ref topLeft);
            Translate(ref bottomRight);
            int width = bottomRight.X - topLeft.X + 1;
            int height = bottomRight.Y - topLeft.Y + 1;
            if (width <= 0 || height <= 0)
            {
                return null;
            }

            Bitmap screenshot = new(width, height, PixelFormat.Format32bppArgb);
            Graphics screenGraphic = Graphics.FromImage(screenshot);
            screenGraphic.CopyFromScreen(
                sourceX: topLeft.X,
                sourceY: topLeft.Y,
                destinationX: 0,
                destinationY: 0,
                blockRegionSize: new Size(width, height),
                copyPixelOperation: CopyPixelOperation.SourceCopy);

            return screenshot;
        }

        public static Color GetPixelColor(Position pixelPos)
        {
            Translate(ref pixelPos);
            IntPtr screenContext = GetDC(IntPtr.Zero);
            uint rawColor = GetPixel(screenContext, pixelPos.X, pixelPos.Y);

            // The return value of GetPixel() is the BGR color of the pixel. The high two hex values
            // and low two hex values need to be swapped for the RGB format.
            uint formattedColor = (rawColor & 0x0000FF) << 16;  // Red
            formattedColor += rawColor & 0x00FF00;              // Green
            formattedColor += (rawColor & 0xFF0000) >>> 16;     // Blue

            ReleaseDC(IntPtr.Zero, screenContext);
            return Color.FromArgb((int)formattedColor);
        }
    }
}
