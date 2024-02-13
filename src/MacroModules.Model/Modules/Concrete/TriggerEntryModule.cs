using MacroModules.Model.Execution;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents an <see cref="EntryModule"/> that indicates a macro should be executed when the
    /// corresponding <see cref="InputTrigger"/> is pressed.
    /// </summary>
    public class TriggerEntryModule : EntryModule
    {
        /// <inheritdoc/>
        public override ModuleType Type { get; } = ModuleType.TriggerEntry;

        /// <summary>
        /// Indicates the <see cref="InputTrigger"/> that should activate this macro.
        /// </summary>
        public InputTrigger? Trigger { get; set; } = null;

        /// <summary>
        /// Indicates the execution behavior of the connected macro as a
        /// <see cref="MacroExecutionType"/> enum.
        /// </summary>
        public MacroExecutionType ExecutionType { get; set; } = MacroExecutionType.IgnoreOnReclick;

        /// <summary>
        /// Indicates if the input event corresponding to <see cref="Trigger"/> should be suppressed
        /// when pressed.
        /// </summary>
        public bool SuppressInput { get; set; } = true;
    }
}
