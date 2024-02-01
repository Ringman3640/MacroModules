namespace MacroModules.Model.Values
{
    public class StringValue : Value, IValueData<string>
    {
        public string Data { get; set; } = "";

        public override ValueDataType Type { get; } = ValueDataType.String;

        public StringValue() { }

        public StringValue(string stringData)
        {
            Data = stringData;
        }

        public override Value Clone()
        {
            return new StringValue(Data);
        }
    }
}
