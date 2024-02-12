namespace MacroModules.Model.Modules
{
    // Commented types are not implemented yet

    public enum ModuleType
    {
        StartupEntry,
        TriggerEntry,
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
        Branch,
        Wait,
        //WaitUntil,
        PlaySound
    }
}
