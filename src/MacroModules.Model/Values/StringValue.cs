namespace MacroModules.Model.Values
{
    public class StringValue : Value, IValueData<string>
    {
        public string Data { get; set; } = "";

        public override ValueDataType Type { get; protected set; } = ValueDataType.String;

        public StringValue() { }

        public StringValue(string stringData)
        {
            Data = stringData;
        }

        public override Value Clone()
        {
            return new StringValue(Data);
        }

        public override bool Equals(Value? other)
        {
            return base.Equals(other)
                && other is StringValue otherString
                && Data.Equals(otherString.Data);
        }
    }
}
