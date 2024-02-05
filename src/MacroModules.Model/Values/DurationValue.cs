namespace MacroModules.Model.Values
{
    public class DurationValue : Value, IValueData<TimeSpan>
    {
        public override ValueDataType Type { get; protected set; } = ValueDataType.Duration;

        public TimeSpan Data { get; set; }

        public DurationValue()
        {
            Data = TimeSpan.FromSeconds(1);
        }

        public DurationValue(TimeSpan timeSpanData)
        {
            Data = timeSpanData;
        }

        public override Value Clone()
        {
            return new DurationValue(Data);
        }

        public override bool Equals(Value? other)
        {
            return base.Equals(other)
                && other is DurationValue otherDuration
                && Data.Equals(otherDuration.Data);
        }
    }
}
