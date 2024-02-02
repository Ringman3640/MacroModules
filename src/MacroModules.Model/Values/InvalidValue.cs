namespace MacroModules.Model.Values
{
    internal class InvalidValue : Value
    {
        public override ValueDataType Type { get; } = ValueDataType.Invalid;

        public override Value Clone()
        {
            return new InvalidValue();
        }
    }
}
