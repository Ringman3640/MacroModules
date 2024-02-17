namespace MacroModules.MacroLibrary.Types
{
    /// <summary>
    /// The set of Windows virtual key codes for modifier keys mapped to their corresponding key
    /// name.
    /// </summary>
    /// <remarks>
    /// This enum set is a subset of <see cref="InputCode"/>.
    /// </remarks>
    public enum ModifierInputCode
    {
        Shift = 0x10,
        Ctrl = 0x11,
        Alt = 0x12,
        ShiftLeft = 0xA0,
        ShiftRight = 0xA1,
        CtrlLeft = 0xA2,
        CtrlRight = 0xA3,
        AltLeft = 0xA4,
        AltRight = 0xA5
    }
}
