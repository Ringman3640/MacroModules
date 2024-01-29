namespace MacroModules.Model.Variable
{
    internal class StringVariable : VariableBase, IVariableValues<string>
    {
        public override VariableType Type { get; } = VariableType.String;

        public string InitialValue { get; set; } = "";

        public string RuntimeValue { get; set; } = "";

        public override void InitializeRuntimeValue()
        {
            RuntimeValue = InitialValue;
        }
    }
}
