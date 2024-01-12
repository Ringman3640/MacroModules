using MacroModules.MacroLibrary.Types;
using static MacroModules.MacroLibrary.WinApi.ScreenCaptureApi;

namespace MacroModules.MacroLibrary
{
    public class ScreenCapture : IDisposable
    {
        ~ScreenCapture()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _ = ReleaseDC(IntPtr.Zero, screenContext);
        }

        public void GetScreenshot()
        {
            // stub
            // todo
        }

        public uint GetPixelColor(Position pixelPos)
        {
            uint rawColor = GetPixel(screenContext, pixelPos.X, pixelPos.Y);

            // The return value of GetPixel() is the BGR color of the pixel. The high two hex values
            // and low two hex values need to be swapped for the RGB format.
            uint formattedColor = (rawColor & 0x0000FF) << 16;  // Red
            formattedColor += rawColor & 0x00FF00;              // Green
            formattedColor += (rawColor & 0xFF0000) >>> 16;     // Blue
            return formattedColor;
        }

        private readonly IntPtr screenContext = GetDC(IntPtr.Zero);
    }
}
