using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;

namespace MacroModules.Model.Modules
{
    /// <summary>
    /// Represents a specialized <see cref="Module"/> that returns a <see cref="Value"/> from its
    /// execution <see cref="IResponse"/>.
    /// </summary>
    public abstract class ValuedModule : Module
    {
        /// <summary>
        /// Gets the <see cref="ValueType"/> of the <see cref="Value"/> returned by this object from
        /// <see cref="Execute(ref object?)"/>.
        /// </summary>
        public abstract ValueType ReturnValueType { get; }

        /// <inheritdoc/>
        public ValuedModule() : base() { }

        /// <inheritdoc/>
        public abstract override IResponse Execute(ref object? processData);
    }
}
