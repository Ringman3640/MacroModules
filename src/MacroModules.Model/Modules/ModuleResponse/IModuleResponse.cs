namespace MacroModules.Model.Modules.ModuleResponse
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates what the caller
    /// should do next.
    /// </summary>
    public interface IModuleResponse
    {
        /// <summary>
        /// Indicates the type of response that represents this object.
        /// </summary>
        /// <remarks>
        /// Each <see cref="ModuleResponseType"/> corresponds to a specific
        /// <see cref="IModuleResponse"/> implementation class.
        /// </remarks>
        ModuleResponseType Type { get; }
    }
}
