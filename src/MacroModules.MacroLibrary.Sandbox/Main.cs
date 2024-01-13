// See https://aka.ms/new-console-template for more information
using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static MacroModules.MacroLibrary.WinApi.ScreenCaptureApi;
//using static MacroModules.MacroLibrary.MessageLoopApi;
using System.Drawing;
using static MacroModules.MacroLibrary.ScreenCapture;

internal class MainProgram()
{
    private static void Main(string[] args)
    {
        const int iterations = 100;

        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            Bitmap? bmp = GetScreenshot(new Position(0, 0), new Position(1919, 1079));
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);

        stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            Bitmap? bmp = TakeScreenshot();
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    private static Bitmap? TakeScreenshot()
    {
        Bitmap bitmap = new(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(0, 0, 0, 0, new Size(1920, 1080), CopyPixelOperation.SourceCopy);
        return bitmap;
    }
}
