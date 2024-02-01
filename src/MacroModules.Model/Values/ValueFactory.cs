namespace MacroModules.Model.Values
{
    /// <summary>
    /// Provides a staic method for creating <see cref="Value"/> instances.
    /// </summary>
    public static class ValueFactory
    {
        /// <summary>
        /// Creates a new instance of a <see cref="Value"/> that corresponds to a specific
        /// <see cref="ValueDataType"/>.
        /// </summary>
        /// <param name="type">The type of value to create.</param>
        /// <returns>The created value instance as a <see cref="Value"/> object.</returns>
        /// <exception cref="Exception">
        /// <paramref name="type"/> does not correspond to a specific <see cref="Value"/>.
        /// </exception>
        public static Value Create(ValueDataType type)
        {
            if (!valueFactories.TryGetValue(type, out var valueFactory))
            {
                throw new Exception($"Could not create value of type value {type}");
            }
            return valueFactory();
        }

        /// <summary>
        /// A mapping of <see cref="ValueDataType"/> values to individual <see cref="Value"/>
        /// factories.
        /// </summary>
        private static Dictionary<ValueDataType, Func<Value>> valueFactories = new()
        {
            { ValueDataType.String, () => new StringValue() }
        };
    }
}
