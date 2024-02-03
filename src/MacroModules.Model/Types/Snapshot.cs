using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MacroModules.Model.Types
{
    /// <summary>
    /// Represents a <see cref="Bitmap"/> of a screen region that can be visually filtered.
    /// </summary>
    public class Snapshot : IDisposable, IEquatable<Snapshot>
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
        /// Gets the <see cref="SnapshotFilter"/> instance that defines how the
        /// <see cref="Snapshot"/> should be filtered.
        /// </summary>
        public SnapshotFilter Filter { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot"/> class that contains the given
        /// snapshot and has default <see cref="Filter"/> values.
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
            Filter = new(other.Filter);
            if (other.filteredSnapshot != null)
            {
                filteredSnapshot = new(other.filteredSnapshot);
            }
            if (other.cachedFilter != null)
            {
                cachedFilter = new(other.cachedFilter);
            }
        }

        /// <summary>
        /// Gets the filtered snapshot of <see cref="OriginalSnapshot"/> that has the custom
        /// <see cref="Filter"/> values applied.
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
                if (filteredSnapshot != null && Filter.Equals(cachedFilter))
                {
                    return filteredSnapshot;
                }

                try
                {
                    PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                    switch (Filter.ColorDepth)
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

                    int scaledWidth = (int)(originalSnapshot.Width * Filter.ResolutionScale);
                    int scaledHeight = (int)(originalSnapshot.Height * Filter.ResolutionScale);

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
                    cachedFilter = null;
                }

                cachedFilter = new(Filter);
                return filteredSnapshot;
            }
        }

        public bool Equals(Snapshot? other)
        {
            if (other is null)
            {
                return false;
            }

            Bitmap snapshot1 = FilteredSnapshot;
            Bitmap snapshot2 = other.FilteredSnapshot;

            if (snapshot1.Size != snapshot2.Size || snapshot1.PixelFormat != snapshot2.PixelFormat)
            {
                return false;
            }

            // The long ass comparison
            // Idea to use ReadOnlySpan<byte> from https://stackoverflow.com/a/48599119

            ReadOnlySpan<byte> snapshot1Bytes;
            ReadOnlySpan<byte> snapshot2Bytes;
            using (var mstream = new MemoryStream())
            {
                // The actual ImageFormat of the Bitmaps are MemoryBmp but Bmp needs to be used
                // because Save() doesn't support MemoryBmp and throws an error if used but doesn't
                // have that behavior documented; the method is literally unfinished what the fuck
                // Microsoft
                snapshot1.Save(mstream, ImageFormat.Bmp);
                snapshot1Bytes = mstream.ToArray();
            }
            using (var mstream = new MemoryStream())
            {
                snapshot2.Save(mstream, ImageFormat.Bmp);
                snapshot2Bytes = mstream.ToArray();
            }

            return snapshot1Bytes.SequenceEqual(snapshot2Bytes);
        }

        public override bool Equals(object? obj)
        {
            return obj is Snapshot snapshotObj && Equals(snapshotObj);
        }

        public override int GetHashCode()
        {
            return Filter.GetHashCode()
                + OriginalSnapshot.Width << 16
                + OriginalSnapshot.Height << 24;
        }

        public void Dispose()
        {
            originalSnapshot.Dispose();
            if (filteredSnapshot != null)
            {
                filteredSnapshot.Dispose();
                filteredSnapshot = null;
            }
        }

        private readonly object snapshotLock = new();
        private Bitmap originalSnapshot;
        private Bitmap? filteredSnapshot = null;
        private SnapshotFilter? cachedFilter = null;
    }
}
