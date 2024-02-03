using MacroModules.Model.Types;

namespace MacroModules.Model.Values
{
    public class SnapshotValue : Value, IValueData<Snapshot>
    {
        public override ValueDataType Type { get; } = ValueDataType.Snapshot;
        public Snapshot Data { get; set; }

        public SnapshotValue(Snapshot snapshotData)
        {
            Data = snapshotData;
        }

        public override Value Clone()
        {
            return new SnapshotValue(new Snapshot(Data));
        }

        public override void Dispose()
        {
            base.Dispose();
            Data.Dispose();
        }
    }
}
