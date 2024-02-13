using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.Model.Execution
{
    /// <summary>
    /// Represents the execution information about a macro.
    /// </summary>
    public class MacroExecutionInfo
    {
        /// <summary>
        /// Indicates the <see cref="TriggerEntryModule"/> that defines macro.
        /// </summary>
        public TriggerEntryModule EntryModule { get; set; }

        /// <summary>
        /// The <see cref="MacroExecutor"/> that is currently executing the macro.
        /// </summary>
        /// <remarks>
        /// If this value is <c>null</c>, assume that the macro is not executing.
        /// </remarks>
        public MacroExecutor? Executor { get; set; } = null;

        /// <summary>
        /// Indicates if the macro is currently toggled on. This value should only be set if
        /// <see cref="ExecutionType"/> is <see cref="MacroExecutionType.ToggleLoop"/>.
        /// </summary>
        public bool ToggledOn { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroExecutionInfo"/> class that is
        /// defined by a given <see cref="TriggerEntryModule"/>.
        /// </summary>
        /// <param name="entryModule">
        /// The <see cref="TriggerEntryModule"/> that is used to define the macro.
        /// </param>
        public MacroExecutionInfo(TriggerEntryModule entryModule)
        {
            EntryModule = entryModule;
        }
    }
}
