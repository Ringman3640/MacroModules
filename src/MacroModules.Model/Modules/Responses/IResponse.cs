namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates what the caller
    /// should do next.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Indicates the type of response that represents this object.
        /// </summary>
        /// <remarks>
        /// Each <see cref="ResponseType"/> corresponds to a specific
        /// <see cref="IResponse"/> implementation class.
        /// </remarks>
        ResponseType Type { get; }
    }
}
