namespace MacroModules.Model.Variable
{
    public class VariableTest : VariableBase, IVariableValues<string>
    {
        public override VariableType Type { get; } = VariableType.String;

        public string InitialValue { get; set; } = "";

        public string RuntimeValue { get; set; } = "";
    }
}
