using MacroModules.Model.Modules;

namespace MacroModules.Model.Execution
{
    /// <summary>
    /// Represents the execution information about a macro.
    /// </summary>
    public class MacroExecutionInfo
    {
        /// <summary>
        /// Gets the <see cref="InputTrigger"/> that triggers the macro.
        /// </summary>
        public InputTrigger Trigger { get; private set; }

        /// <summary>
        /// Gets the first <see cref="Module"/> of the macro.
        /// </summary>
        public Module EntryModule { get; private set; }

        /// <summary>
        /// Gets the <see cref="MacroExecutionType"/> that defines how the macro behaves when the
        /// <see cref="Trigger"/> input is clicked.
        /// </summary>
        public MacroExecutionType ExecutionType { get; private set; }

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
        /// Indicates if the <see cref="Trigger"/> input should be suppressed when clicked.
        /// </summary>
        public bool SuppressInput {  get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroExecutionInfo"/> class that is defined
        /// with an <see cref="InputTrigger"/>, an entry <see cref="Module"/>, and a
        /// <see cref="MacroExecutionType"/> type.
        /// </summary>
        /// <param name="trigger">An <see cref="InputTrigger"/> that triggers the macro.</param>
        /// <param name="entryModule">
        /// A <see cref="Module"/> that references the start of the macro.
        /// </param>
        /// <param name="executionType">
        /// A <see cref="MacroExecutionType"/> that defines the behavior of the macro when the
        /// trigger is clicked.
        /// </param>
        public MacroExecutionInfo(InputTrigger trigger, Module entryModule, MacroExecutionType executionType)
        {
            Trigger = trigger;
            EntryModule = entryModule;
            ExecutionType = executionType;
        }
    }
}
