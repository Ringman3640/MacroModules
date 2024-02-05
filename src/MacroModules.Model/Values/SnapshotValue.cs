using MacroModules.Model.Types;

namespace MacroModules.Model.Values
{
    public class SnapshotValue : Value, IValueData<Snapshot>
    {
        public override ValueDataType Type { get; protected set; } = ValueDataType.Snapshot;

        public Snapshot Data { get; set; }

        public SnapshotValue()
        {
            Data = new();
        }

        public SnapshotValue(Snapshot snapshotData)
        {
            Data = snapshotData;
        }

        public override Value Clone()
        {
            return new SnapshotValue(new Snapshot(Data));
        }

        public override bool Equals(Value? other)
        {
            return base.Equals(other)
                && other is SnapshotValue otherSnapshot
                && Data.Equals(otherSnapshot.Data);
        }

        public override void Dispose()
        {
            base.Dispose();
            Data.Dispose();
        }
    }
}
