namespace MacroModules.Model.Module.ModuleResponse
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that this is the
    /// last <see cref="Module"/> in the current execution thread.
    /// </summary>
    public class ModuleEnd : IModuleResponse
    {
        /// <inheritdoc/>
        public ModuleResponseType Type { get; } = ModuleResponseType.End;
    }
}
