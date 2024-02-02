using MacroModules.MacroLibrary.Types;

namespace MacroModules.Model.Values
{
    public class PositionValue : Value, IValueData<Position>
    {
        public override ValueDataType Type { get; } = ValueDataType.Position;

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
    }
}
