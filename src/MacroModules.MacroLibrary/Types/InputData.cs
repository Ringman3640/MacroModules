﻿using MacroModules.MacroLibrary.WinApi;
using static MacroModules.MacroLibrary.WinApi.HookingApi;
using static MacroModules.MacroLibrary.WinApi.KeyStateApi;
using static MacroModules.MacroLibrary.WinApi.SendInputApi;

namespace MacroModules.MacroLibrary.Types
{
    public enum InputType
    {
        Invalid,
        KeyDown,
        KeyUp,
        MouseDown,
        MouseUp,
        MouseMove,
        VerticalScroll,
        HorizontalScroll
    }

    [Flags]
    public enum InputModifiers
    {
        ShiftHeld = 1,
        CtrlHeld = 1 << 1,
        AltHeld = 1 << 2
    }

    /// <summary>
    /// Represents the data associated with an input event.
    /// </summary>
    public class InputData
    {
        /// <summary>
        /// The type of the input event. 
        /// </summary>
        public InputType Type { get; private set; }

        /// <summary>
        /// Flags that indicate if any modifier keys were being held during the input.
        /// </summary>
        public InputModifiers Modifiers { get; private set; }

        /// <summary>
        /// The time the input occurred in milliseconds since the system started.
        /// </summary>
        public uint Timestamp { get; private set; }

        /// <summary>
        /// The Windows virtual key code associated with the input.
        /// </summary>
        /// <remarks>
        /// This value is only set if the <c>Type</c> is <c>InputDown</c> or <c>InputUp</c>.
        /// </remarks>
        /// <seealso cref="InputType"/>
        public ushort InputKeyCode { get; private set; }

        /// <summary>
        /// The directional scroll amount that the input sent.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This value is only set if the <c>Type</c> is <c>VerticalScroll</c> or
        ///     <c>HorizontalScroll</c>.
        /// </para>
        /// <para>
        ///     If set, the value represents different inputs based on if the scroll was horizontal
        ///     or vertical. A positive value correlates to up and right scrolls. A negative value
        ///     correlates to down and left scrolls.
        /// </para>
        /// </remarks>
        public int ScrollValue { get; private set; }

        internal InputData(KeyboardMessage messageType, KeyboardHookStruct data)
        {
            Timestamp = data.time;
            InputKeyCode = (ushort)data.vkCode;
            SetModifierFlags();

            if (messageType == KeyboardMessage.KeyDown || messageType == KeyboardMessage.SysKeyDown)
            {
                Type = InputType.KeyDown;
                return;
            }
            Type = InputType.KeyUp;
        }

        internal InputData(MouseMessage messageType, MouseHookStruct data)
        {
            Timestamp = data.time;
            SetModifierFlags();

            switch (messageType)
            {
                case MouseMessage.Move:
                    Type = InputType.MouseMove;
                    return;
                case MouseMessage.LeftButtonDown:
                    Type = InputType.MouseDown;
                    InputKeyCode = (ushort)InputCode.MouseLeft;
                    return;
                case MouseMessage.LeftButtonUp:
                    Type = InputType.MouseUp;
                    InputKeyCode = (ushort)InputCode.MouseLeft;
                    return;
                case MouseMessage.RightButtonDown:
                    Type = InputType.MouseDown;
                    InputKeyCode = (ushort)InputCode.MouseRight;
                    return;
                case MouseMessage.RightButtonUp:
                    Type = InputType.MouseUp;
                    InputKeyCode = (ushort)InputCode.MouseRight;
                    return;
                case MouseMessage.MiddleButtonDown:
                    Type = InputType.MouseDown;
                    InputKeyCode = (ushort)InputCode.MouseMiddle;
                    return;
                case MouseMessage.MiddleButtonUp:
                    Type = InputType.MouseUp;
                    InputKeyCode = (ushort)InputCode.MouseMiddle;
                    return;
                case MouseMessage.VerticalWheel:
                    Type = InputType.VerticalScroll;
                    // Bit shift and narrow cast to only get the high-order word (16 bits).
                    ScrollValue = (short)(data.mouseData >>> 16);
                    return;
                case MouseMessage.XButtonDown:
                    Type = InputType.MouseDown;
                    InputKeyCode = (ushort)((short)(data.mouseData >>> 16) == 1 ? InputCode.MouseX1 : InputCode.MouseX2);
                    return;
                case MouseMessage.XButtonUp:
                    Type = InputType.MouseUp;
                    InputKeyCode = (ushort)((short)(data.mouseData >>> 16) == 1 ? InputCode.MouseX1 : InputCode.MouseX2);
                    return;
                case MouseMessage.HorizontalWheel:
                    Type = InputType.HorizontalScroll;
                    ScrollValue = (short)(data.mouseData >>> 16);
                    return;
                default:
                    Type = InputType.Invalid;
                    return;
            }
        }

        private void SetModifierFlags()
        {
            // To check if an input is being held down, the high-order bit needs to be checked.
            // This is done by unsigned shifting the bit right by 31 bits so that the high order
            // bit is at the low-order position. Then just check if the value is 1.

            // The return value of GetKeyState is a short, which is 16 bits. But we need to shift
            // by 31 bits since the short gets casted to an int (32 bits) when doing bit shifts.

            if (GetKeyState((int)InputCode.Shift) >>> 31 == 1)
            {
                Modifiers |= InputModifiers.ShiftHeld;
            }
            if (GetKeyState((int)InputCode.Ctrl) >>> 31 == 1)
            {
                Modifiers |= InputModifiers.CtrlHeld;
            }
            if (GetKeyState((int)InputCode.Alt) >>> 31 == 1)
            {
                Modifiers |= InputModifiers.AltHeld;
            }
        }
    }
}
