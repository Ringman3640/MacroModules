using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MacroModules.Model.Types
{
    public enum ColorDepth
    {
        Color8Bit,
        Color5Bit,
        Color4Bit,
        Color1Bit
    }

    /// <summary>
    /// Represents a set of filter properties to apply to a <see cref="Snapshot"/> instance.
    /// </summary>
    public class SnapshotFilter : IEquatable<SnapshotFilter>
    {
        /// <summary>
        /// Indicates the <see cref="ColorDepth"/> filter value that should be applied.
        /// </summary>
        public ColorDepth ColorDepth
        { 
            get { return colorDepth; }
            set { colorDepth = value; }
        }

        /// <summary>
        /// Indicates a scale to the <see cref="Snapshot"/>'s resolution that should be applied.
        /// </summary>
        /// <remarks>
        /// This value is a decimal from 0.1 to 1, where 1 represents the original resolution of
        /// the snapshot. Any provided value is rounded to two decimal digits and is clamped to
        /// the valid interval.
        /// </remarks>
        public double ResolutionScale
        {
            get { return resolutionScale; }
            set
            {
                // Round to nearest hundredth
                value = double.Round(value, 2);

                // Lock to bounds
                value = Math.Min(value, 1);
                value = Math.Max(value, 0.01);

                resolutionScale = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotFilter"/> class with default filter
        /// values.
        /// </summary>
        public SnapshotFilter() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotFilter"/> class that is a deep
        /// copy of another <see cref="SnapshotFilter"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="SnapshotFilter"/> instance to copy.</param>
        public SnapshotFilter(SnapshotFilter other)
        {
            ColorDepth = other.ColorDepth;
            ResolutionScale = other.ResolutionScale;
        }

        public bool Equals(SnapshotFilter? other)
        {
            return other != null
                && colorDepth == other.colorDepth
                && resolutionScale == other.resolutionScale;
        }

        public override bool Equals(object? obj)
        {
            return obj is SnapshotFilter filterObj && Equals(filterObj);
        }

        public override int GetHashCode()
        {
            return (int)colorDepth + (int)(resolutionScale * 1000);
        }

        private ColorDepth colorDepth = ColorDepth.Color8Bit;
        private double resolutionScale = 1;
    }
}
