namespace MacroModules.Model.Values.Comparison
{
    /// <summary>
    /// Defines a generalized method for <see cref="Value"/> instances to implement to allow
    /// comparison checks.
    /// </summary>
    public interface IValueComparable
    {
        /// <summary>
        /// Indicates whether the current <see cref="Value"/> is greater than another
        /// <see cref="Value"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Value"/> to compare with this <see cref="Value"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the comparison is valid and if the current <see cref="Value"/> is greater
        /// than <paramref name="other"/>. Otherwise, <c>false</c> if the two values cannot be
        /// compared or if the current value is not greater than the other value.
        /// </returns>
        public bool GreaterThan(Value other);

        /// <summary>
        /// Indicates whether the current <see cref="Value"/> is less than another
        /// <see cref="Value"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Value"/> to compare with this <see cref="Value"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the comparison is valid and if the current <see cref="Value"/> is less
        /// than <paramref name="other"/>. Otherwise, <c>false</c> if the two values cannot be
        /// compared or if the current value is not less than the other value.
        /// </returns>
        public bool LessThan(Value other);
    }
}
