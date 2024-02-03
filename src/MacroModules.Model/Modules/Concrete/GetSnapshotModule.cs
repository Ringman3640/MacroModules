using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Types;
using MacroModules.Model.Values;
using System.Drawing;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that takes a snapshot of a region on the screen.
    /// </summary>
    public class GetSnapshotModule : ValuedModule
    {
        /// <summary>
        /// Indicates the screen region to take a snapshot from.
        /// </summary>
        public Rectangle SnapshotRegion { get; set; }

        /// <summary>
        /// Indicates the <see cref="SnapshotFilter"/> that will be applied to the resulting
        /// <see cref="Snapshot"/>.
        /// </summary>
        public SnapshotFilter Filter { get; } = new();

        public override ValueDataType ReturnValueType { get; } = ValueDataType.Snapshot;

        public override ModuleType Type { get; } = ModuleType.GetSnapshot;

        public override void Initialize(out object? processData)
        {
            processData = new GetSnapshotModuleData(SnapshotRegion);
        }

        public override IResponse Execute(ref object? processData)
        {
            var data = (GetSnapshotModuleData)processData!;
            if (!data.Completed)
            {
                return new WaitRepeatResponse(TimeSpan.FromMilliseconds(1));
            }

            Value returnValue;
            if (data.Result == null)
            {
                returnValue = new InvalidValue();
            }
            else
            {
                returnValue = new SnapshotValue(new Snapshot(data.Result)
                {
                    Filter = this.Filter
                });
            }
            return new ContinueResponse(returnValue);
        }

        public class GetSnapshotModuleData
        {
            /// <summary>
            /// Gets if the parallel task is finished obtaining a value.
            /// </summary>
            public bool Completed { get; private set; } = false;

            /// <summary>
            /// Gets the result obtained from the parallel task.
            /// </summary>
            public Bitmap? Result { get; private set; } = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetSnapshotModuleData"/> class that
            /// begins a separate, parallel task that take a screenshot.
            /// </summary>
            /// <param name="captureRegion">The region of the screen to snapshot.</param>
            public GetSnapshotModuleData(Rectangle captureRegion)
            {
                Parallel.Invoke(() =>
                {
                    Position topLeft = new(captureRegion.X, captureRegion.Y);
                    Position bottomRight = new(captureRegion.Right, captureRegion.Bottom);
                    Result = ScreenCapture.GetScreenshot(topLeft, bottomRight);
                    Completed = true;
                });
            }
        }
    }
}
