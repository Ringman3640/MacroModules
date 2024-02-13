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
        /// Indicates the <see cref="EntryModule"/> that defines the entry point for the module.
        /// </summary>
        public EntryModule MacroEntryModule { get; set; }

        /// <summary>
        /// The <see cref="MacroExecutor"/> that is currently executing the macro.
        /// </summary>
        /// <remarks>
        /// If this value is <c>null</c>, assume that the macro is not executing.
        /// </remarks>
        public MacroExecutor? Executor { get; set; } = null;

        /// <summary>
        /// Indicates if the macro is currently toggled on. This value should only be set if
        /// <see cref="MacroEntryModule"/> is a <see cref="TriggerEntryModule"/> with an execution
        /// type of <see cref="MacroExecutionType.ToggleLoop"/>.
        /// </summary>
        public bool ToggledOn { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroExecutionInfo"/> class that is
        /// defined by a given <see cref="EntryModule"/>.
        /// </summary>
        /// <param name="entryModule">
        /// The <see cref="EntryModule"/> that is used to define the macro.
        /// </param>
        public MacroExecutionInfo(EntryModule entryModule)
        {
            MacroEntryModule = entryModule;
        }
    }
}
