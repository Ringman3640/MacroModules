namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents an <see cref="EntryModule"/> that indicates a macro that should run when the
    /// execution context is started.
    /// </summary>
    public class StartupEntryModule : EntryModule
    {
        /// <inheritdoc/>
        public override ModuleType Type { get; } = ModuleType.StartupEntry;
    }
}
