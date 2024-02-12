namespace MacroModules.Model.BoardElements
{
    /// <summary>
    /// Represents a <see cref="BoardElement"/> that defines test on the board.
    /// </summary>
    public class Label : BoardElement
    {
        /// <summary>
        /// Indicates the string test of the <see cref="Label"/>.
        /// </summary>
        public string Text { get; set; } = "";
    }
}
