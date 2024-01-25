using System.Runtime.InteropServices;
using  MacroModules.MacroLibrary.WinApi;
using static MacroModules.MacroLibrary.WinApi.SendInputApi;

namespace MacroModules.MacroLibrary
{
    /// <summary>
    /// Provides methods for sending mouse and keyboard inputs to the system.
    /// </summary>
    public static class InputControl
    {
        /// <summary>
        /// Click a specific input given its Windows virtual key code.
        /// </summary>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <returns>
        /// True if inputCode is valid and the input is successfully sent. Otherwise false.
        /// </returns>
        public static bool Click(ushort inputCode)
        {
            Input[] inputs =
            [
                CreateInput(inputCode, true),
                CreateInput(inputCode, false)
            ];

            uint sent = SendInput(2, inputs, Marshal.SizeOf(typeof(Input)));
            return sent == 2;
        }

        /// <summary>
        /// Hold down a specific input given its Windows virtual key code.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Calling <c>Hold</c> for a specific input will make that input appear to be held down
        ///     from the view of the system. The input will remain in the held position until
        ///     <c>Release</c> or <c>ReleaseAll</c> is called. It is important to call these other 
        ///     methods during process cleanup to ensure that the state of the machine is reverted.
        /// </para>
        /// <para>
        ///     A held input may become unheld if the user physically clicks the specific input. In
        ///     this case, the input will still be considered held by the class and will allow for
        ///     <c>Release</c> and <c>ReleaseAll</c> to apply to the input. This should not cause
        ///     any major side effects in most situations. 
        /// </para>
        /// <para>
        ///     Using <c>Hold</c> and immediately calling <c>Release</c> or <c>ReleaseAll</c> is
        ///     roughly equivalent to the behavior of <c>Click</c>. If the delay between holding the
        ///     input and releasing the input is irrelevant, <c>Click</c> is favorable as it is more
        ///     performant. This is because <c>Hold</c>, <c>Release</c>, and <c>ReleaseAll</c> must
        ///     obtain a lock on each call.
        /// </para>
        /// </remarks>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <returns>
        /// True if inputCode is valid and the input is successfully send. Otherwise false.
        /// </returns>
        /// <seealso cref="Release(ushort)"/>
        /// <seealso cref="ReleaseAll"/>
        public static bool Hold(ushort inputCode)
        {
            Input[] input = [CreateInput(inputCode, true)];

            lock (heldInputs)
            {
                uint sent = SendInput(1, input, Marshal.SizeOf(typeof(Input)));
                if (sent == 1)
                {
                    heldInputs.Add(inputCode);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Release a specific input that was held previously given its Windows virtual key code.
        /// </summary>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <returns>
        /// A bool indicating if the release was a success. The release will fail if the
        /// provided inputCode is invalid, the specified input is not currently being held down,
        /// or if the input fails to send.
        /// </returns>
        public static bool Release(ushort inputCode)
        {
            lock (heldInputs)
            {
                if (!heldInputs.Contains(inputCode))
                {
                    return false;
                }
                Input[] input = [CreateInput(inputCode, false)];
                uint sent = SendInput(1, input, Marshal.SizeOf(typeof(Input)));
                if (sent == 1)
                {
                    heldInputs.Remove(inputCode);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Release all inputs that are currently being held.
        /// </summary>
        /// <remarks>
        /// Calling ReleaseAll() will release all keys that were previously held by calling Hold().
        /// If there are no keys currently being held, the method will do nothing.
        /// </remarks>
        public static void ReleaseAll()
        {
            lock (heldInputs)
            {
                if (heldInputs.Count == 0)
                {
                    return;
                }

                Input[] inputs = new Input[heldInputs.Count];
                ushort inputsIdx = 0;
                foreach (ushort inputCode in heldInputs)
                {
                    inputs[inputsIdx++] = CreateInput(inputCode, false);
                }

                _ = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
                heldInputs.Clear();
            }
        }

        const ushort VkLeftMouse = 1;
        const ushort VkRightMouse = 2;
        const ushort VkCancel = 3;
        const ushort VkMiddleMouse = 4;
        const ushort VkXMouse1 = 5;
        const ushort VkXMouse2 = 6;

        private static readonly HashSet<ushort> heldInputs = [];

        /// <summary>
        /// Creates and returns a new Input struct for a specific key in the up or down state.
        /// </summary>
        /// <remarks>
        /// Helper method for <c>Click</c>, <c>Hold</c>, <c>Release</c>, and <c>ReleaseAll</c>.
        /// </remarks>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <param name="sendKeyDown">
        /// Indicates if the input is being pressed. Otherwise, the input is assumed to be released.
        /// </param>
        /// <returns>The constructed Input struct.</returns>
        /// <seealso cref="Click(ushort)"/>
        /// <seealso cref="Hold(ushort)"/>
        /// <seealso cref="Release(ushort)"/>
        /// <seealso cref="ReleaseAll"/>
        private static Input CreateInput(ushort inputCode, bool sendKeyDown)
        {
            // Check if input is a mouse input
            if (inputCode <= VkXMouse2 && inputCode != VkCancel)
            {
                MouseInputFlags mouseFlags = GetMouseInputFlag(inputCode, sendKeyDown);
                uint mouseData = 0;
                if (inputCode == VkXMouse1)
                {
                    mouseData = 1;
                }
                else if (inputCode == VkXMouse2)
                {
                    mouseData = 2;
                }

                return new Input
                {
                    type = (int)SendInputApi.InputType.Mouse,
                    inputData = new InputUnion
                    {
                        mi = new MouseInput
                        {
                            mouseData = mouseData,
                            dwFlags = (uint)mouseFlags,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };
            }

            KeyboardInputFlags keyboardFlags = 0;
            if (!sendKeyDown)
            {
                keyboardFlags = KeyboardInputFlags.KeyUp;
            }

            return new Input
            {
                type = (int)SendInputApi.InputType.Keyboard,
                inputData = new InputUnion
                {
                    ki = new KeyboardInput
                    {
                        wVk = inputCode,
                        dwFlags = (uint)keyboardFlags,
                        dwExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
        }

        /// <summary>
        /// Translates a given mouse virtual key code and input state into a corresponding
        /// MouseInputFlags enum.
        /// </summary>
        /// <remarks>
        /// Helper method for <c>CreateInput</c>
        /// </remarks>
        /// <param name="inputCode">Windows virtual key code of the input.</param>
        /// <param name="sendKeyDown">
        /// Indicates if the input is being pressed. Otherwise, the input is assumed to be released.
        /// </param>
        /// <returns>
        /// 0 if the inputCode is not a mouse input. Otherwise returns the translated
        /// MouseInputFlags enum.
        /// </returns>
        private static MouseInputFlags GetMouseInputFlag(ushort inputCode, bool sendKeyDown)
        {
            switch (inputCode)
            {
                case VkLeftMouse:
                    return sendKeyDown ? MouseInputFlags.LeftDown : MouseInputFlags.LeftUp;
                case VkRightMouse:
                    return sendKeyDown ? MouseInputFlags.RightDown : MouseInputFlags.RightUp;
                case VkMiddleMouse:
                    return sendKeyDown ? MouseInputFlags.MiddleDown : MouseInputFlags.MiddleUp;
                case VkXMouse1:
                case VkXMouse2:
                    return sendKeyDown ? MouseInputFlags.XDown : MouseInputFlags.XUp;
                default:
                    return 0;
            }
        }
    }
}
