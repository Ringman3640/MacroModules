namespace MacroModules.Model.Types
{
    public enum DurationGranularity
    {
        Ms,
        Sec,
        Min,
        Hr
    }

    /// <summary>
    /// Represents a duration of time.
    /// </summary>
    public struct TimeDuration : IEquatable<TimeDuration>
    {
        /// <summary>
        /// Indicates the numeric value the duration lasts in generic units.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="TimeUnits"/> is only meaningful when paired with the value of
        /// <see cref="Granularity"/>, which defines the exact units of the duration.
        /// </remarks>
        public double TimeUnits { get; set; } = 0;

        /// <summary>
        /// Indicates the unit of time to apply to the value of <see cref="TimeUnits"/>.
        /// </summary>
        public DurationGranularity Granularity { get; set; } = DurationGranularity.Sec;

        /// <summary>
        /// Gets the duration represented as a <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return GetTimeSpan(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDuration"/> structure that represents a
        /// default duration of zero seconds.
        /// </summary>
        public TimeDuration() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDuration"/> structure to a specified
        /// number of time units defined by a specified <see cref="DurationGranularity"/>.
        /// </summary>
        /// <param name="timeUnits">The time units of the duration.</param>
        /// <param name="granularity">
        /// The <see cref="DurationGranularity"/> that defines the exact units of
        /// <paramref name="timeUnits"/>.
        /// </param>
        public TimeDuration(double timeUnits, DurationGranularity granularity)
        {
            TimeUnits = timeUnits;
            Granularity = granularity;
        }

        /// <summary>
        /// Gets the duration representation as a <see cref="TimeSpan"/>.
        /// </summary>
        /// <returns>
        /// The duration represented as a <see cref="TimeSpan"/>. The return value is clamped
        /// between <see cref="TimeSpan.MinValue"/> and <see cref="TimeSpan.MaxValue"/> in response
        /// to an overflow or if <see cref="TimeUnits"/> was <see cref="double.PositiveInfinity"/>
        /// or <see cref="double.NegativeInfinity"/>.
        /// </returns>
        public readonly TimeSpan GetTimeSpan()
        {
            try
            {
                switch (Granularity)
                {
                    case DurationGranularity.Ms:
                        return TimeSpan.FromMilliseconds(TimeUnits);

                    case DurationGranularity.Sec:
                        return TimeSpan.FromSeconds(TimeUnits);

                    case DurationGranularity.Min:
                        return TimeSpan.FromMinutes(TimeUnits);

                    case DurationGranularity.Hr:
                        return TimeSpan.FromHours(TimeUnits);
                }
            }
            catch
            {
                if (double.IsNaN(TimeUnits))
                {
                    return TimeSpan.Zero;
                }
                if (double.IsPositiveInfinity(TimeUnits))
                {
                    return TimeSpan.MaxValue;
                }
                if (double.IsNegativeInfinity(TimeUnits))
                {
                    return TimeSpan.MinValue;
                }
                return (TimeUnits < 0) ? TimeSpan.MinValue : TimeSpan.MaxValue;
            }

            return TimeSpan.Zero;
        }

        /// <inheritdoc/>
        public readonly bool Equals(TimeDuration other)
        {
            return (TimeUnits == other.TimeUnits) && (Granularity == other.Granularity);
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj)
        {
            return obj is TimeDuration otherDuration && Equals(otherDuration);
        }

        public static bool operator == (TimeDuration left, TimeDuration right)
        {
            return left.Equals(right);
        }

        public static bool operator != (TimeDuration left, TimeDuration right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return TimeUnits.GetHashCode() + ((int)Granularity + 1) * 7;
        }
    }
}
