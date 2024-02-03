using MacroModules.Model.Modules;

namespace MacroModules.Model.Values
{
    /// <summary>
    /// Represents a piece of data that is returned or used by a <see cref="Module"/>.
    /// </summary>
    public abstract class Value : IDisposable, IEquatable<Value>
    {
        /// <summary>
        /// Gets the type of data this <see cref="Value"/> contians.
        /// </summary>
        public abstract ValueDataType Type { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="Value"/> instance that is a deep clone of the current
        /// <see cref="Value"/>.
        /// </summary>
        /// <returns></returns>
        public abstract Value Clone();

        public virtual bool Equals(Value? other)
        {
            return other != null && other.Type == Type;
        }

        public override bool Equals(object? obj)
        {
            return obj is Value valueObj && Equals(valueObj);
        }

        public override int GetHashCode()
        {
            // This is an abysmal hash code if the derived Values don't override it
            // It's fine for now since Values aren't intended to be put in a hashable collection
            // Also I'm really eepy rn and don't want to go through the trouble
            return base.GetHashCode() + (int)Type;
        }

        public virtual void Dispose()
        {
            Type = ValueDataType.Invalid;
        }
    }
}
