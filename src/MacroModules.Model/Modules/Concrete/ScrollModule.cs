using MacroModules.MacroLibrary;
using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Modules.Concrete
{
    public enum ScrollDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Represents a <see cref="Module"/> that sends scroll inputs.
    /// </summary>
    public class ScrollModule : Module
    {
        /// <summary>
        /// Indicates the amount to ticks to stroll.
        /// </summary>
        public int ScrollTicks { get; set; } = 1;

        /// <summary>
        /// Indicates the direction to scroll.
        /// </summary>
        public ScrollDirection Direction { get; set; } = ScrollDirection.Down;

        public override ModuleType Type { get; } = ModuleType.Scroll;

        public override IResponse Execute(ref object? processData)
        {
            int scrollValue = Math.Max(ScrollTicks, 0) * scrollValuePerTick;

            switch (Direction)
            {
                case ScrollDirection.Up:
                    MouseControl.Scroll(scrollValue);
                    break;

                case ScrollDirection.Down:
                    MouseControl.Scroll(-scrollValue);
                    break;

                case ScrollDirection.Left:
                    MouseControl.HorizontalScroll(-scrollValue);
                    break;

                case ScrollDirection.Right:
                    MouseControl.HorizontalScroll(scrollValue);
                    break;
            }

            return new ContinueResponse(ExitPorts[0].Destination);
        }

        /// <summary>
        /// This is the default scroll units per tick in Windows (WHEEL_DELTA).
        /// </summary>
        private static readonly int scrollValuePerTick = 120;
    }
}
