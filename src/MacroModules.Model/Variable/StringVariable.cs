namespace MacroModules.Model.Variable
{
    /// <summary>
    /// Represents a string macro variable.
    /// </summary>
    public class StringVariable : VariableBase, IVariableValues<string>
    {
        public override VariableType Type { get; } = VariableType.String;

        public string InitialValue { get; set; } = string.Empty;

        public string RuntimeValue { get; set; } = string.Empty;

        public override void InitializeRuntimeValue()
        {
            RuntimeValue = InitialValue;
        }
    }
}
