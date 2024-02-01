namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should execute the next indicated <see cref="Module"/>. If the given <see cref="Module"/>
    /// is null, the caller should end its macro execution.
    /// </summary>
    public class ContinueResponse : IResponse
    {
        /// <inheritdoc/>
        public ResponseType Type { get; } = ResponseType.Continue;

        /// <summary>
        /// Indicates the next <see cref="Module"/> the caller should execute.
        /// </summary>
        public Module? NextModule { get; private set; }

        public ContinueResponse(Module? nextModule)
        {
            NextModule = nextModule;
        }
    }
}
