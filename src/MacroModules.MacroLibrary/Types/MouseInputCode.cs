namespace MacroModules.MacroLibrary.Types
{
    /// <summary>
    /// The set of Windows virtual key codes for mouse buttons mapped to their corresponding button
    /// name.
    /// </summary>
    /// <remarks>
    /// This enum set is a subset of <see cref="InputCode"/>.
    /// </remarks>
    public enum MouseInputCode : ushort
    {
        MouseLeft = 0x01,
        MouseRight = 0x02,
        MouseMiddle = 0x04,
        MouseX1 = 0x05,
        MouseX2 = 0x06,
    }
}
