using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Modules
{
    /// <summary>
    /// Represents a specialized <see cref="Module"/> that indicates the start of a macro.
    /// </summary>
    public abstract class EntryModule : Module
    {
        /// <inheritdoc/>
        public override bool IsConnectable { get; } = false;

        /// <returns>
        /// A <see cref="ContinueResponse"/> that points to the <see cref="ExitPort"/> at index 0.
        /// </returns>
        /// <inheritdoc/>
        public override sealed IResponse Execute(ref object? processData)
        {
            return new ContinueResponse();
        }
    }
}
