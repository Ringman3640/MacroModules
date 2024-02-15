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
    public struct Duration
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
        /// Initializes a new instance of the <see cref="Duration"/> structure that represents a
        /// default duration of zero seconds.
        /// </summary>
        public Duration() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration"/> structure to a specified number
        /// of time units defined by a specified <see cref="DurationGranularity"/>.
        /// </summary>
        /// <param name="timeUnits">The time units of the duration.</param>
        /// <param name="granularity">
        /// The <see cref="DurationGranularity"/> that defines the exact units of
        /// <paramref name="timeUnits"/>.
        /// </param>
        public Duration(double timeUnits, DurationGranularity granularity)
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
    }
}
