namespace MacroModules.Model.Variable
{
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
