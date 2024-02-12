namespace MacroModules.Model.BoardElements
{
    /// <summary>
    /// Represents and element on a board that has a coordinate position relative to the board.
    /// </summary>
    public abstract class BoardElement
    {
        /// <summary>
        /// Indicates the horizontal position of the element.
        /// </summary>
        /// <remarks>
        /// A value of 0 represents a <see cref="BoardElement"/> that's positioned to the left edge
        /// of the board. Positive values indicate a position to the right while negative values
        /// indicate a position to the left.
        /// </remarks>
        public double PositionX { get; set; }

        /// <summary>
        /// Indicates the vertical position of the element.
        /// </summary>
        /// <remarks>
        /// A value of 0 represents a <see cref="BoardElement"/> that's positioned to the top edge
        /// of the board. Positive values indicate a position downwards while negative values
        /// indicate a position upwards.
        /// </remarks>
        public double PositionY { get; set; }
    }
}
