using MacroModules.Model.Values;

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

        /// <summary>
        /// Indicates an optional <see cref="Value"/> that is returned when a
        /// <see cref="ValuedModule"/> completes execution.
        /// </summary>
        public Value? ReturnValue { get; set; } = null;

        public ContinueResponse(Module? nextModule)
        {
            NextModule = nextModule;
        }
    }
}
