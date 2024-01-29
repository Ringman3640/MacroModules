namespace MacroModules.Model.Modules.ModuleResponse
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should immediately repeat execution of the current <see cref="Module"/>.
    /// </summary>
    public class ModuleRepeat : IModuleResponse
    {
        /// <inheritdoc/>
        public ModuleResponseType Type { get; } = ModuleResponseType.Repeat;
    }
}
