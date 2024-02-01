using MacroModules.Model.Modules;

namespace MacroModules.Model.Values
{
    /// <summary>
    /// Represents a piece of data that is returned or used by a <see cref="Module"/>.
    /// </summary>
    public abstract class Value
    {
        /// <summary>
        /// Gets the type of data this <see cref="Value"/> contians.
        /// </summary>
        public abstract ValueDataType Type { get; }

        /// <summary>
        /// Creates a new <see cref="Value"/> instance that is a deep clone of the current
        /// <see cref="Value"/>.
        /// </summary>
        /// <returns></returns>
        public abstract Value Clone();
    }
}
