namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should execute the next indicated <see cref="Module"/>.
    /// </summary>
    public class ModuleNext : IModuleResponse
    {
        /// <inheritdoc/>
        public ModuleResponseType Type { get; } = ModuleResponseType.Next;

        /// <summary>
        /// Indicates the next MacroProcess the caller should execute.
        /// </summary>
        public Module NextProcess { get; private set; }

        public ModuleNext(Module nextProcess)
        {
            NextProcess = nextProcess;
        }
    }
}
