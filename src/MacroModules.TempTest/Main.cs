// See https://aka.ms/new-console-template for more information
using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using System.Runtime.InteropServices;
using static MacroModules.MacroLibrary.WindowControl;
//using static MacroModules.MacroLibrary.MessageLoopApi;

internal class MainProgram()
{
    private static void Main(string[] args)
    {
        ScreenCapture screen = new();

        Thread.Sleep(3000);
        Position pos = MouseControl.CursorPosition;
        Console.WriteLine(screen.GetPixelColor(pos));
    }
}
