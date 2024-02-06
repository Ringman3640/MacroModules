using MacroModules.Model.Values;
using MacroModules.Model.Values.Comparison;

namespace MacroModules.Model.Modules.Concrete
{
    public enum ComparisonType
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan
    }

    /// <summary>
    /// Represents a condition statement used by <see cref="BranchModule"/> to indicate a new
    /// execution branch and its requirement.
    /// </summary>
    public class BranchCondition
    {
        /// <summary>
        /// Indicates the descriptive name of the condition.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Indicates the <see cref="Value"/> used for comparison.
        /// </summary>
        public Value? ComparisonValue { get; set; } = null;

        /// <summary>
        /// Indicates the comparison operation to use when comparing the current <see cref="Value"/>
        /// with <see cref="ComparisonValue"/>.
        /// </summary>
        public ComparisonType? Operation { get; set; } = null;

        /// <summary>
        /// Gets the result of the comparison given the current <see cref="Value"/>.
        /// </summary>
        /// <param name="mainValue">A <see cref="Value"/> instance that is used to compare to
        /// <see cref="ComparisonValue"/></param>
        /// <returns>
        /// <c>true</c> if the operation is valid and if it is logically correct. Otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool GetResult(Value mainValue)
        {
            if (ComparisonValue == null || Operation == null)
            {
                return false;
            }
            if (mainValue.Type != ComparisonValue.Type)
            {
                return false;
            }

            switch (Operation)
            {
                case ComparisonType.Equals:
                    return mainValue.Equals(ComparisonValue);

                case ComparisonType.NotEquals:
                    return !mainValue.Equals(ComparisonValue);

                case ComparisonType.GreaterThan:
                {
                    IValueComparable? currentComparable = mainValue as IValueComparable;
                    if (currentComparable == null)
                    {
                        return false;
                    }
                    return currentComparable.GreaterThan(ComparisonValue);
                }

                case ComparisonType.LessThan:
                {
                    IValueComparable? currentComparable = mainValue as IValueComparable;
                    if (currentComparable == null)
                    {
                        return false;
                    }
                    return currentComparable.LessThan(ComparisonValue);
                }
            }

            return false;
        }
    }
}
