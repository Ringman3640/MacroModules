namespace MacroModules.Model.Values
{
    public class BoolValue : Value, IValueData<bool>
    {
        public override ValueDataType Type { get; protected set; } = ValueDataType.Bool;

        public bool Data { get; set; }

        public BoolValue() { }

        public BoolValue(bool boolData)
        {
            Data = boolData;
        }

        public override Value Clone()
        {
            return new BoolValue(Data);
        }
    }
}
