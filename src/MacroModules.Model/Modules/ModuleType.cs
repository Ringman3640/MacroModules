namespace MacroModules.Model.Modules
{
    // Commented types are not implemented yet

    public enum ModuleType
    {
        StartupEntry,
        TriggerEntry,
        Branch,
        Wait,
        //WaitUntil,
        SendInput,
        // SendRawText,
        GetInputState,
        MoveCursor,
        //PathCursor,
        Scroll,
        GetCursorPosition,
        OpenProgram,
        CloseProgram,
        FocusWindow,
        GetSnapshot,
        GetPixelColor,
        PlaySound
    }
}
