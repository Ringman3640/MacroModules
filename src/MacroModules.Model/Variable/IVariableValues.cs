namespace MacroModules.Model.Variable
{
    /// <summary>
    /// Defines value properties for macro variables.
    /// </summary>
    /// <typeparam name="T">
    /// The type of value that will be stored by the macro variable.
    /// </typeparam>
    public interface IVariableValues<T>
    {
        /// <summary>
        /// Gets the initial value of the variable that is set before execution.
        /// </summary>
        public T InitialValue { get; set; }

        /// <summary>
        /// Gets the live value of the variable as it is updated during runtime.
        /// </summary>
        public T RuntimeValue { get; set; }
    }
}
