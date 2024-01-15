using System.Runtime.InteropServices;
using MacroModules.MacroLibrary.Types;
using MacroModules.MacroLibrary.WinApi;
using static MacroModules.MacroLibrary.DisplayPositionTranslator;
using static MacroModules.MacroLibrary.WinApi.SendInputApi;

namespace MacroModules.MacroLibrary
{
    /// <summary>
    /// Provides methods for sending mouse inputs to the system and controlling the cursor position.
    /// </summary>
    /// <remarks>
    /// This class does not include functionality for sending mouse button inputs. See
    /// <c>InputControl</c> for these actions.
    /// </remarks>
    /// <seealso cref="InputControl"/>
    public static class MouseControl
    {
        /// <summary>
        /// Gets or sets the position of the mouse cursor.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Getting is equivalent to <c>GetCursorPosition</c>.
        /// </para>
        /// <para>
        ///     Setting will perform the actions of <c>MoveCursor(Position)</c> but will will not
        ///     return if the send was sucessfull. Use <c>MoveCursor</c> instead if the success
        ///     state is needed.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetCursorPosition"/>
        /// <seealso cref="MoveCursor(Position)"/>
        public static Position CursorPosition
        {
            get { return GetCursorPosition(); }
            set { SetCursorPos(value.X, value.Y); }
        }

        /// <summary>
        /// Move the cursor's screen position to the specified coordinate.
        /// </summary>
        /// <param name="coordinate">Screen coordinate where to set the cursor position.</param>
        /// <returns>A bool indicating if the move was successful.</returns>
        public static bool MoveCursor(Position coordinate)
        {
            Translate(ref coordinate);
            return SetCursorPos(coordinate.X, coordinate.Y);
        }

        /// <summary>
        /// Move the cursor's screen position relative to its current potition by a specified vector
        /// offset.
        /// </summary>
        /// <param name="moveVector">
        /// Vector offset which defines how the cursor will move.
        /// </param>
        /// <returns>A bool indicating if the move was successful.</returns>
        public static bool MoveCursor(Offset moveVector)
        {
            Position currPos;
            if (!GetCursorPos(out currPos))
            {
                return false;
            }
            currPos.X += moveVector.X;
            currPos.Y += moveVector.Y;
            return SetCursorPos(currPos.X, currPos.Y);
        }

        /// <summary>
        /// Get the current coordinate position of the mouse cursor.
        /// </summary>
        /// <returns>A Position containing the coordinates of the cursor.</returns>
        public static Position GetCursorPosition()
        {
            GetCursorPos(out Position currPos);
            return currPos;
        }

        /// <summary>
        /// Send a vertical scroll input at the current cursor position.
        /// </summary>
        /// <param name="scrollAmount">
        /// Directional amount to scroll. A positive value will scroll up. A negative value will
        /// scroll down.
        /// </param>
        /// <returns>A bool indicating if the scroll input was successfully sent.</returns>
        public static bool Scroll(int scrollAmount)
        {
            return SendScroll(scrollAmount, true);

        }

        /// <summary>
        /// Send a horizontal scroll input at the current cursor position.
        /// </summary>
        /// <param name="scrollAmount">
        /// Directional amount to scroll. A positive value will scroll right. A negative value will
        /// scroll left.
        /// </param>
        /// <returns>A bool indicating if the scroll input was successfully sent.</returns>
        public static bool HorizontalScroll(int scrollAmount)
        {
            return SendScroll(scrollAmount, false);
        }

        /// <summary>
        /// Send a horizontal or scroll input to the system.
        /// </summary>
        /// <remarks>
        /// Helper method for <c>Scroll</c> and <c>HorizontalScroll</c>.
        /// </remarks>
        /// <param name="scrollAmount">Directional amount to scroll.</param>
        /// <param name="vertical">Indicates if the scroll is vertical or horizontal.</param>
        /// <returns>A bool indicating if the scroll input was successfully sent.</returns>
        private static bool SendScroll(int scrollAmount, bool vertical)
        {
            Input[] input =
            [
                new Input
                {
                    type = (uint)SendInputApi.InputType.Mouse,
                    inputData = new InputUnion
                    {
                        mi = new MouseInput
                        {
                            // The unchecked operator is used here since a negative value may need
                            // to be placed into a uint value (DWORD in WinAPI). 
                            // Solution from here: https://stackoverflow.com/a/19184115
                            mouseData = unchecked((uint)scrollAmount),
                            dwFlags = (uint)(vertical ? MouseInputFlags.Wheel : MouseInputFlags.HorizontalWheel),
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            ];

            return SendInput(1, input, Marshal.SizeOf(typeof(Input))) == 1;
        }

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out Position lpPoint);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}
