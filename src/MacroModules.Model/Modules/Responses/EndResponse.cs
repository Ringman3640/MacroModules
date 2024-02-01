namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that this is the
    /// last <see cref="Module"/> in the current execution thread.
    /// </summary>
    public class EndResponse : IResponse
    {
        /// <inheritdoc/>
        public ResponseType Type { get; } = ResponseType.End;
    }
}
