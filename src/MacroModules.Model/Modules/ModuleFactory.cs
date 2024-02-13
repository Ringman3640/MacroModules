using MacroModules.Model.Modules.Concrete;

namespace MacroModules.Model.Modules
{
    /// <summary>
    /// Provides a staic method for creating <see cref="Module"/> instances.
    /// </summary>
    public static class ModuleFactory
    {
        /// <summary>
        /// Creates a new instance of a <see cref="Module"/> that corresponds to a specific
        /// <see cref="ModuleType"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Module"/> to create.</param>
        /// <returns>
        /// The created Module concrete instance as a <see cref="Module"/> object.
        /// </returns>
        /// <exception cref="Exception">
        /// <paramref name="type"/> does not correspond to a specific <see cref="Module"/>.
        /// </exception>
        public static Module Create(ModuleType type)
        {
            if (!moduleFactories.TryGetValue(type, out var moduleFactory))
            {
                throw new Exception($"Could not create module of type {type}");
            }
            return moduleFactory();
        }

        /// <summary>
        /// A mapping of <see cref="ModuleType"/> values to individual <see cref="Module"/>
        /// factories.
        /// </summary>
        private static Dictionary<ModuleType, Func<Module>> moduleFactories = new()
        {
            { ModuleType.StartupEntry, () => new StartupEntryModule() },
            { ModuleType.TriggerEntry, () => new TriggerEntryModule() },
            { ModuleType.SendInput, () => new SendInputModule() },
            { ModuleType.GetInputState, () => new GetInputStateModule() },
            { ModuleType.MoveCursor, () => new MoveCursorModule() },
            // TODO: Add PathCursor factory
            { ModuleType.Scroll, () => new ScrollModule() },
            { ModuleType.GetCursorPosition, () => new GetCursorPositionModule() },
            { ModuleType.OpenProgram, () => new OpenProgramModule() },
            { ModuleType.CloseProgram, () => new CloseProgramModule() },
            { ModuleType.FocusWindow, () => new FocusWindowModule() },
            { ModuleType.GetSnapshot, () => new GetSnapshotModule() },
            { ModuleType.GetPixelColor, () => new GetPixelColorModule() },
            { ModuleType.Branch, () => new BranchModule() },
            { ModuleType.Wait, () => new WaitModule() },
            // TODO: Add WaitUntil factory
            { ModuleType.PlaySound, () => new PlaySoundModule() },
        };
    }
}
