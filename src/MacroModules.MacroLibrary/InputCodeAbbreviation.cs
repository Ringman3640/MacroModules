using MacroModules.MacroLibrary.Types;

namespace MacroModules.MacroLibrary
{
    /// <summary>
    /// Provides a method for converting an <see cref="InputCode"/> to a corresponding string
    /// abbreviation representation.
    /// </summary>
    public static class InputCodeAbbreviation
    {
        /// <summary>
        /// Gets a string abbreviation representation of an <see cref="InputCode"/>.
        /// </summary>
        /// <param name="inputCode">The <see cref="InputCode"/> to abbreviate.</param>
        /// <returns>
        /// The string abbreviation of <paramref name="inputCode"/>. If there is no corresponding
        /// abbreviation for the input code, the string representation of the input code enum is
        /// returned.
        /// </returns>
        public static string GetAbbreviation(InputCode inputCode)
        {
            abbreviationMap.TryGetValue(inputCode, out string? abbreviation);
            return abbreviation ?? inputCode.ToString();
        }

        /// <param name="inputCode">
        /// The <see cref="ushort"/> representation of the <see cref="InputCode"/> to abbreviate.
        /// </param>
        /// <returns>
        /// The string abbreviation of <paramref name="inputCode"/>. If there is no corresponding
        /// abbreviation for the input code, the string representation of the input code enum is
        /// returned. If <paramref name="inputCode"/> does not correspond to a valid
        /// <see cref="InputCode"/> enum, <paramref name="inputCode"/> is returned prepended with
        /// "Vk_".
        /// </returns>
        /// <inheritdoc cref="GetAbbreviation(InputCode)"/>
        public static string GetAbbreviation(ushort inputCode)
        {
            if (Enum.IsDefined(typeof(InputCode), inputCode))
            {
                InputCode temp = (InputCode)inputCode;
                abbreviationMap.TryGetValue((InputCode)inputCode, out string? abbreviation);
                return abbreviation ?? ((InputCode)inputCode).ToString();
            }
            return "Vk_" + inputCode;
        }

        private static Dictionary<InputCode, string?> abbreviationMap = new()
        {
            { InputCode.MouseLeft, "Left Mouse" },
            { InputCode.MouseRight, "Right Mouse" },
            { InputCode.Cancel, "Cancel" },
            { InputCode.MouseMiddle, "Middle Mouse" },
            { InputCode.MouseX1, "X1" },
            { InputCode.MouseX2, "X2" },
            { InputCode.Backspace, "Back" },
            { InputCode.Tab, null },
            { InputCode.Clear, null },
            { InputCode.Enter, null },
            { InputCode.Shift, null },
            { InputCode.Ctrl, null },
            { InputCode.Alt, null },
            { InputCode.Pause, "Pb" },
            { InputCode.CapsLock, "Caps" },
            { InputCode.Escape, "Esc" },
            { InputCode.Space, null },
            { InputCode.PageUp, "Pgup" },
            { InputCode.PageDown, "pgdn" },
            { InputCode.End, null },
            { InputCode.Home, null },
            { InputCode.LeftArrow, "Left" },
            { InputCode.UpArrow, "Up" },
            { InputCode.RightArrow, "Right" },
            { InputCode.DownArrow, "Down" },
            { InputCode.Select, null },
            { InputCode.Print, null },
            { InputCode.Execute, "Exec" },
            { InputCode.PrintScreen, "Ps" },
            { InputCode.Insert, "Ins" },
            { InputCode.Delete, "Del" },
            { InputCode.Help, "Help" },
            { InputCode.Key0, "0" },
            { InputCode.Key1, "1" },
            { InputCode.Key2, "2" },
            { InputCode.Key3, "3" },
            { InputCode.Key4, "4" },
            { InputCode.Key5, "5" },
            { InputCode.Key6, "6" },
            { InputCode.Key7, "7" },
            { InputCode.Key8, "8" },
            { InputCode.Key9, "9" },
            { InputCode.A, null },
            { InputCode.B, null },
            { InputCode.C, null },
            { InputCode.D, null },
            { InputCode.E, null },
            { InputCode.F, null },
            { InputCode.G, null },
            { InputCode.H, null },
            { InputCode.I, null },
            { InputCode.J, null },
            { InputCode.K, null },
            { InputCode.L, null },
            { InputCode.M, null },
            { InputCode.N, null },
            { InputCode.O, null },
            { InputCode.P, null },
            { InputCode.Q, null },
            { InputCode.R, null },
            { InputCode.S, null },
            { InputCode.T, null },
            { InputCode.U, null },
            { InputCode.V, null },
            { InputCode.W, null },
            { InputCode.X, null },
            { InputCode.Y, "" },
            { InputCode.Z, null },
            { InputCode.WinLeft, "Left Win" },
            { InputCode.WinRight, "Right Win" },
            { InputCode.Apps, null },
            { InputCode.Sleep, null },
            { InputCode.Num0, null },
            { InputCode.Num1, null },
            { InputCode.Num2, null },
            { InputCode.Num3, null },
            { InputCode.Num4, null },
            { InputCode.Num5, null },
            { InputCode.Num6, null },
            { InputCode.Num7, null },
            { InputCode.Num8, null },
            { InputCode.Num9, null },
            { InputCode.Multiply, null },
            { InputCode.Add, null },
            { InputCode.Separator, null },
            { InputCode.Subtract, null },
            { InputCode.Decimal, null },
            { InputCode.Divide, null },
            { InputCode.F1, null },
            { InputCode.F2, null },
            { InputCode.F3, null },
            { InputCode.F4, null },
            { InputCode.F5, null },
            { InputCode.F6, null },
            { InputCode.F7, null },
            { InputCode.F8, null },
            { InputCode.F9, null },
            { InputCode.F10, null },
            { InputCode.F11, null },
            { InputCode.F12, null },
            { InputCode.F13, null },
            { InputCode.F14, null },
            { InputCode.F15, null },
            { InputCode.F16, null },
            { InputCode.F17, null },
            { InputCode.F18, null },
            { InputCode.F19, null },
            { InputCode.F20, null },
            { InputCode.F21, null },
            { InputCode.F22, null },
            { InputCode.F23, null },
            { InputCode.F24, null },
            { InputCode.NumLock, null },
            { InputCode.ScrollLock, "Sl" },
            { InputCode.ShiftLeft, "Left Shift" },
            { InputCode.ShiftRight, "Right Shift" },
            { InputCode.CtrlLeft, "Left Ctrl" },
            { InputCode.CtrlRight, "Right Ctrl" },
            { InputCode.AltLeft, "Left Alt" },
            { InputCode.AltRight, "Right Alt" },
            { InputCode.BrowserBack, null },
            { InputCode.BrowserForward, null },
            { InputCode.BrowserRefresh, null },
            { InputCode.BrowserStop, null },
            { InputCode.BrowserSearch, null },
            { InputCode.BrowserFavorites, null },
            { InputCode.BrowserHome, null },
            { InputCode.VolumeMute, null },
            { InputCode.VolumeDown, null },
            { InputCode.VolumeUp, null },
            { InputCode.MediaNext, null },
            { InputCode.MediaPrev, null },
            { InputCode.MediaStop, null },
            { InputCode.MediaPlayPause, null },
            { InputCode.Mail, null },
            { InputCode.SelectMedia, null },
            { InputCode.App1, null },
            { InputCode.App2, null },
            { InputCode.SemiColon, ";" },
            { InputCode.Plus, "+" },
            { InputCode.Comma, "," },
            { InputCode.Minus, "-" },
            { InputCode.Period, "." },
            { InputCode.Slash, "/" },
            { InputCode.Backtick, "`" },
            { InputCode.OpenBracket, "[" },
            { InputCode.BackSlash, "\\" },
            { InputCode.CloseBracket, "]" },
            { InputCode.Quote, "\"" },
        };
    }
}
