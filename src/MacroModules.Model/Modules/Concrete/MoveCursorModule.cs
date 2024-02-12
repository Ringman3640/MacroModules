using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules.Responses;
using System.Diagnostics;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that moves the mouse cursor.
    /// </summary>
    public class MoveCursorModule : Module
    {
        /// <summary>
        /// Indicates the screen <see cref="Position"/> to move the cursor.
        /// </summary>
        public Position TargetPosition { get; set; }

        /// <summary>
        /// Indicates the transition time of the cursor to <see cref="TargetPosition"/>.
        /// </summary>
        /// <remarks>
        /// If the given <see cref="TimeSpan"/> is equivalent to <see cref="TimeSpan.Zero"/>, the
        /// mouse cursor will instantly teleport to <see cref="TargetPosition"/>.
        /// </remarks>
        public TimeSpan TransitionTime { get; set; }

        public override ModuleType Type { get; } = ModuleType.MoveCursor;

        public override bool IsConnectable { get; } = true;

        public override void Initialize(out object? processData)
        {
            processData = new MoveCursorModuleData();
        }

        public override IResponse Execute(ref object? processData)
        {
            var data = (MoveCursorModuleData)processData!;
            TimeSpan elapsedTime = data.Watch.Elapsed;

            if (elapsedTime >= TransitionTime)
            {
                MouseControl.MoveCursor(TargetPosition);
                return new ContinueResponse();
            }

            double transitionPercent = elapsedTime / TransitionTime;
            double nextPosX = data.InitialPosition.X;
            double nextPosY = data.InitialPosition.Y;
            nextPosX += (TargetPosition.X - data.InitialPosition.X) * transitionPercent;
            nextPosY += (TargetPosition.Y - data.InitialPosition.Y) * transitionPercent;

            MouseControl.MoveCursor(new Position((int)nextPosX, (int)nextPosY));
            return new WaitRepeatResponse(TimeSpan.FromMilliseconds(1));
        }

        private class MoveCursorModuleData
        {
            /// <summary>
            /// Indicates the initial position of the cursor when the <see cref="Module"/> started.
            /// </summary>
            public Position InitialPosition { get; } = MouseControl.GetCursorPosition();

            /// <summary>
            /// Gets the <see cref="Stopwatch"/> instance that was created and started when the
            /// <see cref="MoveCursorModuleData"/> was created.
            /// </summary>
            public Stopwatch Watch { get; } = new();

            public MoveCursorModuleData()
            {
                Watch.Start();
            }
        }
    }
}
