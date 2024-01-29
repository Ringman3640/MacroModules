namespace MacroModules.Model.Variable
{
    /// <summary>
    /// Provides a staic method for creating <see cref="VariableBase"/> instances.
    /// </summary>
    public static class VariableFactory
    {
        /// <summary>
        /// Creates a new instance of a <see cref="VariableBase"/> that corresponds to a specific
        /// <see cref="VariableType"/>.
        /// </summary>
        /// <param name="type">The type of variable to create.</param>
        /// <returns>The created variable instance as a <see cref="VariableBase"/> object.</returns>
        /// <exception cref="Exception">
        /// <paramref name="type"/> does not correspond to a specific <see cref="VariableBase"/>.
        /// </exception>
        public static VariableBase Create(VariableType type)
        {
            if (!variableFactories.TryGetValue(type, out var variableFactory))
            {
                throw new Exception($"Could not create variable of type value {type}");
            }
            return variableFactory();
        }

        /// <summary>
        /// A mapping of <see cref="VariableType"/> values to indivisual <see cref="VariableBase"/>
        /// factories.
        /// </summary>
        private static Dictionary<VariableType, Func<VariableBase>> variableFactories = new()
        {
            { VariableType.String, () => new StringVariable() }
        };
    }
}
