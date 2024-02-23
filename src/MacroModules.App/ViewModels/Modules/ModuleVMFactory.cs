using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules
{
    public static class ModuleVMFactory
    {
        public static ModuleVM Create(ModuleType type)
        {
            if (!defaultModuleVmFactories.TryGetValue(type, out var moduleFactory))
            {
                throw new Exception($"Could not create moduleVM of type {type}");
            }
            return moduleFactory();
        }

        public static ModuleVM Create(Module data)
        {
            if (!parameterizedModuleVmFactories.TryGetValue(data.Type, out var moduleFactory))
            {
                throw new Exception($"Could not create parameterized moduleVM of type {data.Type}");
            }
            return moduleFactory(data);
        }

        private static Dictionary<ModuleType, Func<ModuleVM>> defaultModuleVmFactories = new()
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
            { ModuleType.CloseProgram, () => new CloseProgramModuleVM() },
            { ModuleType.FocusWindow, () => new FocusWindowModuleVM() },
            // TODO: Add GetSnapshot factory
            // TODO: Add GetPixelColor factory
            // TODO: Add Branch factory
            { ModuleType.Wait, () => new WaitModuleVM() },
            // TODO: Add WaitUntil factory
            { ModuleType.PlaySound, () => new PlaySoundModuleVM() },
        };

        private static Dictionary<ModuleType, Func<Module, ModuleVM>> parameterizedModuleVmFactories = new()
        {
            { ModuleType.StartupEntry, (data) => new StartupEntryModuleVM((StartupEntryModule)data) },
            { ModuleType.TriggerEntry, (data) => new TriggerEntryModuleVM((TriggerEntryModule)data) },
            { ModuleType.SendInput, (data) => new SendInputModuleVM((SendInputModule)data) },
            // TODO: Add GetInputState factory
            { ModuleType.MoveCursor, (data) => new MoveCursorModuleVM((MoveCursorModule)data) },
            // TODO: Add PathCursor factory
            { ModuleType.Scroll, (data) => new ScrollModuleVM((ScrollModule)data) },
            // TODO: Add GetCursorPosition factory
            { ModuleType.OpenProgram, (data) => new OpenProgramModuleVM((OpenProgramModule)data) },
            { ModuleType.CloseProgram, (data) => new CloseProgramModuleVM((CloseProgramModule)data) },
            { ModuleType.FocusWindow, (data) => new FocusWindowModuleVM((FocusWindowModule)data) },
            // TODO: Add GetSnapshot factory
            // TODO: Add GetPixelColor factory
            // TODO: Add Branch factory
            { ModuleType.Wait, (data) => new WaitModuleVM((WaitModule)data) },
            // TODO: Add WaitUntil factory
            { ModuleType.PlaySound, (data) => new PlaySoundModuleVM((PlaySoundModule)data) },
        };
    }
}
