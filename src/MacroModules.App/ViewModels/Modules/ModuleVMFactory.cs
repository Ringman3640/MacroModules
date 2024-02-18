using MacroModules.Model.Modules;

namespace MacroModules.App.ViewModels.Modules
{
    public static class ModuleVMFactory
    {
        public static ModuleVM Create(ModuleType type)
        {
            if (!moduleVmFactories.TryGetValue(type, out var moduleFactory))
            {
                throw new Exception($"Could not create moduleVM of type {type}");
            }
            return moduleFactory();
        }

        private static Dictionary<ModuleType, Func<ModuleVM>> moduleVmFactories = new()
        {
            { ModuleType.StartupEntry, () => new StartupEntryModuleVM() },
            { ModuleType.TriggerEntry, () => new TriggerEntryModuleVM() },
            { ModuleType.SendInput, () => new SendInputModuleVM() },
            // TODO: Add GetInputState factory
            { ModuleType.MoveCursor, () => new MoveCursorModuleVM() },
            // TODO: Add PathCursor factory
            { ModuleType.Scroll, () => new ScrollModuleVM() },
            // TODO: Add GetCursorPosition factory
            { ModuleType.OpenProgram, () => new OpenProgramModuleVM() },
            // TODO: Add CloseProgram factory
            // TODO: Add FocusWindow factory
            // TODO: Add GetSnapshot factory
            // TODO: Add GetPixelColor factory
            // TODO: Add Branch factory
            { ModuleType.Wait, () => new WaitModuleVM() },
            // TODO: Add WaitUntil factory
            { ModuleType.PlaySound, () => new PlaySoundModuleVM() },
        };
    }
}
