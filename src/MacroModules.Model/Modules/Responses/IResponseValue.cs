using MacroModules.Model.Values;

namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that provides a <see cref="Value"/>.
    /// </summary>
    public interface IResponseValue
    {
        /// <summary>
        /// Indicates the <see cref="Value"/> returned by the <see cref="Module"/>.
        /// </summary>
        public Value? ReturnValue { get; set; }
    }
}
