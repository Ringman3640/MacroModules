namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should execute the next indicated <see cref="Module"/>.
    /// </summary>
    public class ContinueResponse : IModuleResponse
    {
        /// <inheritdoc/>
        public ModuleResponseType Type { get; } = ModuleResponseType.Continue;

        /// <summary>
        /// Indicates the next <see cref="Module"/> the caller should execute.
        /// </summary>
        public Module NextModule { get; private set; }

        public ContinueResponse(Module nextModule)
        {
            NextModule = nextModule;
        }
    }
}
