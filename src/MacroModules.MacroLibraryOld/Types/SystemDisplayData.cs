using MacroModules.MacroLibrary.WinApi;
using static MacroModules.MacroLibrary.WinApi.DisplayInfoApi;
using static MacroModules.MacroLibrary.WinApi.SystemMetricsApi;

namespace MacroModules.MacroLibrary.Types
{
    /// <summary>
    /// Represents the display information for a specific system. Saves the dimensions and position
    /// of all visible displays. 
    /// </summary>
    public class SystemDisplayData : IEquatable<SystemDisplayData>
    {
        /// <summary>
        /// Width in pixels of the primary display screen.w
        /// </summary>
        public int PrimaryScreenWidth { get; private set; }

        /// <summary>
        /// Height in pixels of the primary display screen.
        /// </summary>
        public int PrimaryScreenHeight { get; private set; }

        /// <summary>
        /// The origin of the virtual bounding rectangle. This is the coordinate position of the top
        /// left corner.
        /// </summary>
        public Position VirtualScreenOrigin { get; private set; }

        /// <summary>
        /// Width in pixels of the virtual bounding rectangle containing all displays.
        /// </summary>
        public int VirtualScreenWidth { get; private set; }

        /// <summary>
        /// Height in pixels of the virtual bounding rectangle containing all displays.
        /// </summary>
        public int VirtualScreenHeight { get; private set; }

        /// <summary>
        /// The number of visible screen displays on the system.
        /// </summary>
        public int DisplayCount { get; private set; }

        /// <summary>
        /// The set of all Display data that is contained in the system.
        /// </summary>
        public HashSet<DisplayData> Displays { get; private set; }

        /// <summary>
        /// Default constructor. Populates the instance properties with metrics of the current
        /// system.
        /// </summary>
        public SystemDisplayData()
        {
            PrimaryScreenWidth = GetSystemMetrics((int)Metric.PrimaryScreenWidth);
            PrimaryScreenHeight = GetSystemMetrics((int)Metric.PrimaryScreenHeight);
            VirtualScreenOrigin = new(
                xPos: GetSystemMetrics((int)Metric.VirtualScreenPosX),
                yPos: GetSystemMetrics((int)Metric.VirtualScreenPosY));
            VirtualScreenWidth = GetSystemMetrics((int)Metric.VirtualScreenWidth);
            VirtualScreenHeight = GetSystemMetrics((int)Metric.VirtualScreenHeight);

            Displays = new();
            object? temp = null; // Create temp null object since dwData param is unused
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumHandler, ref temp);

            DisplayCount = Displays.Count;
        }

        public static bool operator ==(SystemDisplayData? left, SystemDisplayData? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            // Check if either operand is null
            // The first check above will confirm if both operands are null
            if (left is null || right is null)
            {
                return false;
            }

            // Quick false checks
            if (left.DisplayCount != right.DisplayCount
                || left.VirtualScreenOrigin != right.VirtualScreenOrigin
                || left.VirtualScreenWidth != right.VirtualScreenWidth)
            {
                return false;
            }

            // Compare displays
            foreach (DisplayData display in left.Displays) {
                if (!right.Displays.Contains(display))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(SystemDisplayData? left, SystemDisplayData? right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SystemDisplayData? other)
        {
            return other != null && this == other;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is SystemDisplayData other && this == other;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return DisplayCount
                + PrimaryScreenWidth
                + PrimaryScreenHeight << 4
                + VirtualScreenWidth << 8
                + VirtualScreenHeight << 12
                + VirtualScreenOrigin.X << 16
                + VirtualScreenOrigin.Y << 20;
        }

        /// <summary>
        /// Custom constructor. Create a SystemDisplayData object using custom defined values.
        /// </summary>
        /// <remarks>
        /// This constructor is only for testing purposes. DO NOT USE FOR LIBRARY IMPLEMENTATION.
        /// </remarks>
        internal SystemDisplayData(
            int primaryScreenWidth,
            int primaryScreenHeight,
            Position virtualScreenOrigin,
            int virtualScreenWidth,
            int virtualScreenHeight,
            HashSet<DisplayData> displays)
        {
            PrimaryScreenWidth = primaryScreenWidth;
            PrimaryScreenHeight = primaryScreenHeight;
            VirtualScreenOrigin = virtualScreenOrigin;
            VirtualScreenWidth = virtualScreenWidth;
            VirtualScreenHeight = virtualScreenHeight;
            Displays = displays;
            DisplayCount = displays.Count;
        }

        /// <summary>
        /// Handler proceedure for the EnumDisplayMonitors WinApi call. 
        /// </summary>
        private bool MonitorEnumHandler(IntPtr hdm, IntPtr hdc, ref Rect clipArea, ref object? dwData)
        {
            int displayWidth = clipArea.right - clipArea.left - 1;
            int displayHeight = clipArea.bottom - clipArea.top - 1;

            if (displayWidth > 0 && displayHeight > 0)
            {
                Displays.Add(new(
                    origin: new Position(clipArea.left, clipArea.top),
                    width: displayWidth,
                    height: displayHeight));
            }

            return true;
        }
    }
}
