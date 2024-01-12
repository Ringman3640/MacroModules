using System.Runtime.InteropServices;

namespace MacroModules.MacroLibrary.Types
{
    /// <summary>
    /// Indicates the exact coordinates of a specific position.
    /// </summary>
    /// <remarks>
    /// Position explicitly declares <c>StructLayout</c> as <c>Sequential</c> so it can be used as
    /// a drop-in struct for the WinAPI <c>POINT</c> structure.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct Position
    {
        public int X;
        public int Y;

        /// <summary>
        /// Creates a new <c>Position</c> struct with a specified coordinate.
        /// </summary>
        /// <param name="xPos">The X component of the position coordinate.</param>
        /// <param name="yPos">The Y component of the position coordinate.</param>
        public Position(int xPos, int yPos)
        {
            X = xPos;
            Y = yPos;
        }

        /// <summary>
        /// Check if two Position structs are equal in value.
        /// </summary>
        /// <param name="left">First Position operand.</param>
        /// <param name="right">Second Position operand.</param>
        /// <returns>True if the Positions are equal, else false.</returns>
        public static bool operator ==(Position left, Position right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        /// <summary>
        /// Check if two Position structs are not equal in value.
        /// </summary>
        /// <param name="left">First Position operand.</param>
        /// <param name="right">Second Position operand.</param>
        /// <returns>True if the Positions are not equal, else false.</returns>
        public static bool operator !=(Position left, Position right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        /// <summary>
        /// Add an <c>Offset</c> to a <c>Position</c>.
        /// </summary>
        /// <param name="pos">Initial Position value.</param>
        /// <param name="offset">Offset value to add.</param>
        /// <returns>A Position struct that is the sum of <c>pos</c> and <c>offset</c>.</returns>
        public static Position operator +(Position pos, Offset offset)
        {
            pos.X += offset.X;
            pos.Y += offset.Y;
            return pos;
        }

        /// <summary>
        /// Subtract an <c>Offset</c> from a <c>Position</c>.
        /// </summary>
        /// <param name="pos">Initial Position value.</param>
        /// <param name="offset">Offset value to subtract.</param>
        /// <returns>
        /// A Position struct that is the difference of <c>pos</c> and <c>offset</c>.
        /// </returns>
        public static Position operator -(Position pos, Offset offset)
        {
            pos.X -= offset.X;
            pos.Y -= offset.Y;
            return pos;
        }

        /// <inheritdoc />
        public override readonly bool Equals(object? other)
        {
            return other is Position otherPos && this == otherPos;
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            return (X << 16) + Y;
        }
    }

    /// <summary>
    /// Indicates an offset vector from a specific position.
    /// </summary>
    public struct Offset
    {
        public int X;
        public int Y;

        /// <summary>
        /// Creates a new <c>Offset</c> struct with a specified coordinate.
        /// </summary>
        /// <param name="xComponent">The X component of the offset.</param>
        /// <param name="yComponent">The Y component of the offset.</param>
        public Offset(int xComponent, int yComponent)
        {
            X = xComponent;
            Y = yComponent;
        }

        /// <summary>
        /// Check if two Offset structs are equal in value.
        /// </summary>
        /// <param name="left">First Offset operand.</param>
        /// <param name="right">Second Offset operand.</param>
        /// <returns>True if the Offsets are equal, else false.</returns>
        public static bool operator ==(Offset left, Offset right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        /// <summary>
        /// Check if two Offset structs are not equal in value.
        /// </summary>
        /// <param name="left">First Offset operand.</param>
        /// <param name="right">Second Offset operand.</param>
        /// <returns>True if the Offsets are not equal, else false.</returns>
        public static bool operator !=(Offset left, Offset right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        /// <summary>
        /// Add two <c>Offset</c> structs together.
        /// </summary>
        /// <param name="left">First Offset operand.</param>
        /// <param name="right">Second Offset operand.</param>
        /// <returns>An Offset struct that is the sum of <c>left</c> and <c>right</c>.</returns>
        public static Offset operator +(Offset left, Offset right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>
        /// Subtract two <c>Offset</c> structs.
        /// </summary>
        /// <param name="left">First Offset operand.</param>
        /// <param name="right">Second Offset operand.</param>
        /// <returns>An Offset struct that is the difference of <c>left</c> and <c>right</c>.</returns>
        public static Offset operator -(Offset left, Offset right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        /// <inheritdoc />
        public override readonly bool Equals(object? other)
        {
            return other is Offset otherOffset && this == otherOffset;
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            return (X << 16) + Y;
        }
    }
}
