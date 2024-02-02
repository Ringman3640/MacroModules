using System.Drawing;
using System.Drawing.Imaging;

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
    /// Represents a <see cref="Bitmap"/> of a screen region that can be visually filtered.
    /// </summary>
    public class Snapshot
    {
        /// <summary>
        /// Indicates the original <see cref="Bitmap"/> provided before any filtering is applied.
        /// </summary>
        public Bitmap OriginalSnapshot
        {
            get { return originalSnapshot; }
            set
            {
                lock (snapshotLock)
                {
                    originalSnapshot = value;
                    filteredSnapshot = null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Bitmap"/> filtered from <see cref="OriginalSnapshot"/>.
        /// </summary>
        /// <remarks>
        /// Read <see cref="GetFilteredSnapshot"/> for remarks on performance.
        /// </remarks>
        public Bitmap FilteredSnapshot
        {
            get
            {
                Bitmap? snapshot = GetFilteredSnapshot();
                return snapshot ?? originalSnapshot;
            }
        }

        /// <summary>
        /// Indicates the <see cref="ColorDepth"/> filter value that is applied to
        /// <see cref="FilteredSnapshot"/>.
        /// </summary>
        public ColorDepth FilterColorDepth
        {
            get { return filterColorDepth; }
            set
            {
                lock (snapshotLock)
                {
                    if (!value.Equals(filterColorDepth))
                    {
                        filterColorDepth = value;
                        filteredSnapshot = null;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates the resolution scale that is applied to <see cref="FilteredSnapshot"/>.
        /// </summary>
        /// <remarks>
        /// This value is a decimal from 0.1 to 1, where 1 represents the original resolution of
        /// the snapshot. Any provided value is rounded to two decimal digits. 
        /// </remarks>
        public double FilterResolutionScale
        {
            get { return filterResolutionScale; }
            set
            {
                lock (snapshotLock)
                {
                    // Round to nearest hundredth
                    value = Double.Round(value, 2);

                    // Lock to bounds
                    value = Math.Min(value, 1);
                    value = Math.Max(value, 0.01);

                    // Set if not the same
                    if (!value.Equals(filterResolutionScale))
                    {
                        filterResolutionScale = value;
                        filteredSnapshot = null;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot"/> class that contains the given
        /// snapshot and has default filter values (none).
        /// </summary>
        /// <param name="snapshot"></param>
        public Snapshot(Bitmap snapshot)
        {
            originalSnapshot = snapshot;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot"/> class that is a deep copy of
        /// another <see cref="Snapshot"/> instance.
        /// </summary>
        /// <param name="other"></param>
        public Snapshot(Snapshot other)
        {
            originalSnapshot = new(other.originalSnapshot);
            filterColorDepth = other.filterColorDepth;
            filterResolutionScale = other.filterResolutionScale;
            if (other.filteredSnapshot != null)
            {
                filteredSnapshot = new(other.filteredSnapshot);
            }
        }

        /// <summary>
        /// Gets the filtered snapshot of <see cref="OriginalSnapshot"/> that has the custom filter
        /// values applied.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Generating the filtered snapshot is significantly taxing on performance. It ranges
        ///     roughly in the tens of milliseconds. It is recommended to run this method
        ///     asynchronously through a <see cref="Task"/>. If latency is important.
        /// </para>
        /// <para>
        ///     To reduce the performance overhead, this method caches the previously generated
        ///     filtered snapshot until <see cref="OriginalSnapshot"/> is modified or if the filter
        ///     values change.
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public Bitmap? GetFilteredSnapshot()
        {
            lock (snapshotLock)
            {
                if (filteredSnapshot != null)
                {
                    return filteredSnapshot;
                }

                try
                {
                    PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                    switch (FilterColorDepth)
                    {
                        case ColorDepth.Color8Bit:
                            pixelFormat = PixelFormat.Format32bppArgb;
                            break;

                        case ColorDepth.Color5Bit:
                            pixelFormat = PixelFormat.Format16bppArgb1555;
                            break;

                        case ColorDepth.Color4Bit:
                            pixelFormat = PixelFormat.Format4bppIndexed;
                            break;

                        case ColorDepth.Color1Bit:
                            pixelFormat = PixelFormat.Format1bppIndexed;
                            break;
                    }

                    int scaledWidth = (int)(originalSnapshot.Width * filterResolutionScale);
                    int scaledHeight = (int)(originalSnapshot.Height * filterResolutionScale);

                    // Apply resolution scaling
                    using Bitmap scaledBitmap = (Bitmap)originalSnapshot.GetThumbnailImage(
                        thumbWidth: scaledWidth,
                        thumbHeight: scaledHeight,
                        callback: null,
                        callbackData: IntPtr.Zero);

                    // Apply color depth
                    Rectangle cloneRegion = new(0, 0, scaledBitmap.Width, scaledBitmap.Height);
                    filteredSnapshot = scaledBitmap.Clone(cloneRegion, pixelFormat);
                }
                catch
                {
                    filteredSnapshot = null;
                }

                return filteredSnapshot;
            }
        }

        private object snapshotLock = new();
        private Bitmap originalSnapshot;
        private Bitmap? filteredSnapshot = null;
        private ColorDepth filterColorDepth = ColorDepth.Color8Bit;
        private double filterResolutionScale = 1;
    }
}
