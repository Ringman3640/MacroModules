using System.Drawing;
using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.WinApi
{
    /// <summary>
    /// Exposes the Windows API methods required for system hooking.
    /// </summary>
    internal class HookingApi
    {
        [DllImport("User32.dll")]
        public static extern IntPtr SetWindowsHookExA(int idHook, KeyboardHookProc lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("User32.dll")]
        public static extern IntPtr SetWindowsHookExA(int idHook, MouseHookProc lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("User32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("User32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        [DllImport("User32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref MouseHookStruct lParam);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibraryA(string lpFileName);

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardHookStruct
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            public Point pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public delegate int KeyboardHookProc(int nCode, int wParam, ref KeyboardHookStruct lParam);
        public delegate int MouseHookProc(int nCode, int wParam, ref MouseHookStruct lParam);
    }
}
