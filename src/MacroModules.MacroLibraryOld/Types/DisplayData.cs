
namespace MacroModules.MacroLibrary.Types
{
    /// <summary>
    /// Represents the dimensions and position of a specific display screen within the encompassing
    /// virtual display screen.
    /// </summary>
    public class DisplayData : IEquatable<DisplayData>
    {
        /// <summary>
        /// The origin of the display. This is the coordinate position of the top left corner.
        /// </summary>
        public Position Origin { get; private set; }

        /// <summary>
        /// Width of the display in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of the display in pixels.
        /// </summary>
        public int Height { get; private set; }

        public DisplayData(Position origin, int width, int height)
        {
            Origin = origin;
            Width = width > 0 ? width : 0;
            Height = height > 0 ? height : 0;
        }

        /// <summary>
        /// Determine if a given Position lies within the display bounds.
        /// </summary>
        /// <param name="position">The coordinate point to check.</param>
        /// <returns>
        /// True if the Position coordinate is within the bounds of the display. Otherwise, false.
        /// </returns>
        public bool Contains(Position position)
        {
            return position.X >= Origin.X
                && position.X < Origin.X + Width
                && position.Y >= Origin.Y
                && position.Y < Origin.Y + Height;
        }

        /// <summary>
        /// Determine if two DisplayData objects are equivalent.
        /// </summary>
        /// <param name="left">The left DisplayData operand.</param>
        /// <param name="right">The right DisplayData operand</param>
        /// <returns>True if the DisplayData objects are equivalent. Otherwise, false.</returns>
        public static bool operator ==(DisplayData? left, DisplayData? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Origin == right.Origin
                && left.Width == right.Width
                && left.Height == right.Height;
        }

        /// <summary>
        /// Determine if two DisplayData objects are not equivalent.
        /// </summary>
        /// <param name="left">The left DisplayData operand.</param>
        /// <param name="right">The right DisplayData operand</param>
        /// <returns>True if the DisplayData objects not are equivalent. Otherwise, false.</returns>
        public static bool operator !=(DisplayData left, DisplayData right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(DisplayData? other)
        {
            return this == other;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is DisplayData other && this == other;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Width * Height + Origin.GetHashCode();
        }
    }
}
