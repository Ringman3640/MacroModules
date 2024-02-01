using MacroModules.Model.Values;

namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that provides a return
    /// <see cref="Value"/> and indicates that the caller should execute the next indicated
    /// <see cref="Module"/>.
    /// </summary>
    public class ValuedContinueResponse : ContinueResponse, IValuedResponse
    {
        /// <inheritdoc/>
        public Value? ReturnValue { get; set; } = null;

        public ValuedContinueResponse(Module? nextModule) : base(nextModule) { }
    }
}
