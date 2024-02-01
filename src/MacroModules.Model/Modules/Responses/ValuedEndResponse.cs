using MacroModules.Model.Values;

namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that provides a return
    /// <see cref="Value"/> and indicates that this is the last <see cref="Module"/> in the current
    /// execution thread.
    /// </summary>
    public class ValuedEndResponse : EndResponse, IValuedResponse
    {
        public Value? ReturnValue { get; set; } = null;
    }
}
