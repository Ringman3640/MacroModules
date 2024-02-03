using MacroModules.MacroLibrary.Types;

namespace MacroModules.Model.Values
{
    public class PositionValue : Value, IValueData<Position>
    {
        public override ValueDataType Type { get; protected set; } = ValueDataType.Position;

        public Position Data { get; set; }

        public PositionValue() { }

        public PositionValue(Position positionData)
        {
            Data = positionData;
        }

        public override Value Clone()
        {
            return new PositionValue(Data);
        }

        public override bool Equals(Value? other)
        {
            return base.Equals(other)
                && other is PositionValue otherPosition
                && Data.Equals(otherPosition.Data);
        }
    }
}
