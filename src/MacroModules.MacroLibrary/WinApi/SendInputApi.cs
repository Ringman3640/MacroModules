using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.WinApi
{
    /// <summary>
    /// Exposes the Windows API methods required for sending inputs.
    /// </summary>
    internal class SendInputApi
    {
        [DllImport("user32.dll")]
        public static extern uint SendInput(uint cInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
        }

        public struct Input
        {
            public uint type;
            public InputUnion inputData;
        }

        public enum InputType
        {
            Mouse = 0,
            Keyboard = 1
        }

        [Flags]
        public enum MouseInputFlags
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            HorizontalWheel = 0x1000,
            MoveNoCoalesce = 0x2000,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        [Flags]
        public enum KeyboardInputFlags
        {
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            ScanCode = 0x0008,
            Unicode = 0x0004
        }
    }
}
