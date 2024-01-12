﻿using MacroModules.MacroLibrary.Types;
using MacroModules.MacroLibrary.WinApi;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static MacroModules.MacroLibrary.WinApi.HookingApi;
using static MacroModules.MacroLibrary.WinApi.MessageLoopApi;

namespace MacroModules.MacroLibrary
{
    public class InputMonitor
    {
        /// <summary>
        /// Indicates if the input monitor should filter out mouse movement inputs.
        /// </summary>
        public static bool FilterMouseMovements
        {
            get { return filterMouseMovements; }
            set { filterMouseMovements = value; }
        }

        /// <summary>
        /// Indicates if the input monitor should filter out injected inputs.
        /// </summary>
        /// <remarks>
        /// Injected inputs are inputs created by other processes rather than inputs generated by
        /// the physical mouse or keyboard. Turning off filtering will result in inputs generated
        /// using <c>InputControl</c> to be collected by the input monitor. 
        /// </remarks>
        /// <seealso cref="InputControl"/>
        public static bool FilterInjectedInputs
        {
            get { return filterInjectedInputs; }
            set { filterInjectedInputs = value; }
        }

        public InputMonitor()
        {

        }

        ~InputMonitor()
        {
            Uninstall();
        }

        /// <summary>
        /// Check if a specific input is being held down.
        /// </summary>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <returns>True if the input is being held down, otherwise false.</returns>
        public static bool InputHeld(ushort inputCode)
        {
            // GetKeyState returns a short with the high-order-bit set if the key is being pressed.
            // To check if the key is pressed, the return value is logically bit shift right until
            // the high-order-bit is now at the position of the low-order-bit. Then just check if
            // the value is 1.
            // The return value is bit shift 31 bits since there is not bi shift operator for short
            // types. The short gets automatically elevated to an int, which is 32 bits.
            return (short)(GetKeyState(inputCode) >>> 31) == 1;
        }

        /// <summary>
        /// Check if a toggleable input is toggled on.
        /// </summary>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <returns>True if the input is toggled on, otherwise false.</returns>
        public static bool InputToggled(ushort inputCode)
        {
            // Same idea as in InputHeld, but check low-order bit.
            // The value is only shift by 15 since the additional bits will be lost when casting
            // back to a short.
            return (short)(GetKeyState(inputCode) << 15) != 0;
        }

        /// <summary>
        /// Starts collection of inputs from the input monitor.
        /// </summary>
        /// <remarks>
        /// This method will activate the input monitor. When activated, the input monitor will
        /// dispatch input events to the user-defined input handler. 
        /// </remarks>
        /// <seealso cref="Stop"/>
        public static void Start()
        {
            active = true;
        }

        /// <summary>
        /// Stops collection of inputs from the input monitor.
        /// </summary>
        /// <seealso cref="Stop"/>
        public static void Stop()
        {
            active = false;
        }

        /// <summary>
        /// Set the input handler that receives input events from the input monitor.
        /// </summary>
        /// <param name="handler">The input handler that receives input events.</param>
        public void SetInputHandler(Func<InputData, bool> handler)
        {
            inputHandler = handler;
        }

        /// <summary>
        /// Install the required hooks needed to start input monitoring in the current thread.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Calling <c>Install</c> will set two global low-level hooks into the system, which
        ///     takes up system resources. It is important to call <c>Uninstall</c> to free these
        ///     resources. <c>Uninstall</c> will be automatically called when the object is garbage
        ///     collected.
        /// </para>
        /// <para>
        ///     The thread calling this method is responsible for handling system messages collected
        ///     from the hooks. Therefore, the installing thread must have a message pump.
        /// </para>
        /// </remarks>
        /// <seealso cref="Uninstall"/>
        public void Install()
        {
            keyboardHookHandle = SetWindowsHookExA(WH_KEYBOARD_LL, KeyboardHookProc, IntPtr.Zero, 0);
            mouseHookHandle = SetWindowsHookExA(WH_MOUSE_LL, MouseHookProc, IntPtr.Zero, 0);
        }

        /// <summary>
        /// Uninstall the input monitoring hooks that were set from <c>Install</c>.
        /// </summary>
        /// <seealso cref="Install"/>
        public void Uninstall()
        {
            if (keyboardHookHandle != IntPtr.Zero)
            {
                _ = UnhookWindowsHookEx(keyboardHookHandle);
                keyboardHookHandle = IntPtr.Zero;
            }
            if (mouseHookHandle != IntPtr.Zero)
            {
                _ = UnhookWindowsHookEx(mouseHookHandle);
                mouseHookHandle = IntPtr.Zero;
            }
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;

        private static volatile bool active = false;
        private static volatile bool filterMouseMovements = false;
        private static volatile bool filterInjectedInputs = true;

        /// <summary>
        /// User-provided handler that is called when a mouse or keyboard input is received from the
        /// monitor.
        /// </summary>
        private Func<InputData, bool>? inputHandler = null;

        private IntPtr keyboardHookHandle = IntPtr.Zero;
        private IntPtr mouseHookHandle = IntPtr.Zero;

        private int KeyboardHookProc(int nCode, int wParam, ref KeyboardHookStruct lParam)
        {
            // Default ignores
            if (!active || nCode < 0 || inputHandler == null)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
            }

            // Check 4th flag bit to see if event is injected
            if (filterInjectedInputs && (lParam.flags & 8) != 0)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
            }

            if (!inputHandler(new InputData((KeyboardMessage)wParam, lParam)))
            {
                return 1;
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
        }

        private int MouseHookProc(int nCode, int wParam, ref MouseHookStruct lParam)
        {
            // Default ignores
            if (!active || nCode < 0 || inputHandler == null)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
            }

            // Ignore mouse movements if filter is on
            if (filterMouseMovements && wParam == (int)MouseMessage.Move)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
            }

            // Check 4th flag bit to see if event is injected
            if (filterInjectedInputs && (lParam.flags & 8) != 0)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
            }

            if (!inputHandler(new InputData((MouseMessage)wParam, lParam)))
            {
                return 1;
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, ref lParam);
        }

        [DllImport("User32.dll")]
        private static extern short GetKeyState(int nVirtKey);
    }
}
