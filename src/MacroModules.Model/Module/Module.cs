using MacroModules.Model.Module.ModuleResponse;
using MacroModules.Model.Variable;

namespace MacroModules.Model.Module
{
    /// <summary>
    /// Represents a single executable process in a macro.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// Get the unique identifier for this <see cref="Module"/>.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// A list of all <see cref="ExitPort"/> objects that exist on this <see cref="Module"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Each available <see cref="ExitPort"/> in the list represents an output port that
        ///     exists on the current <see cref="Module"/>. By default, each port will have no
        ///     destination set.
        /// </para>
        /// <para>
        ///     The order of ExitPorts in the list corresponds to their priority level. Ports at a
        ///     lower index position should be visibly displayed above ports with a lower index
        ///     position.
        /// </para>
        /// </remarks>
        public List<ExitPort> ExitPorts { get; protected set; } = [];

        /// <summary>
        /// The <see cref="VariableStore"/> object that is used as a context to get all necessary
        /// <see cref="VariableBase"/> instances.
        /// </summary>
        public VariableStore? VariableContext { get; set; } = null;

        /// <summary>
        /// Initialize the <see cref="Module"/> object by generating a data object that may
        /// hold running state data of the execution.
        /// </summary>
        /// <remarks>
        /// Must be called before the first call to <see cref="Execute(ref object?)"/>.
        /// </remarks>
        /// <param name="processData">
        /// Object that will hold the <see cref="Module"/>'s execution state data after
        /// initialization.
        /// </param>
        /// <seealso cref="Execute(ref object?)"/>
        public virtual void Initialize(out object? processData)
        {
            processData = null;
        }

        /// <summary>
        /// Executes the <see cref="Module"/> action and returns a response.
        /// </summary>
        /// <param name="processData">
        /// The execution state data object created from <see cref="Initialize(out object?)"/>
        /// </param>
        /// <returns>
        /// A <see cref="IModuleResponse"/> that indicates what the caller should do next.
        /// </returns>
        /// <seealso cref="Initialize(out object?)"/>
        /// <seealso cref="IModuleResponse"/>
        public abstract IModuleResponse Execute(ref object? processData);

        /// <summary>
        /// Revert any system state changes that were set during <see cref="Execute(ref object?)"/>
        /// or <see cref="Initialize(out object?)"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Restore(ref object?)"/> should only be called if the caller must stop
        /// executing the process before <see cref="Execute(ref object?)"/> indicates that the
        /// execution is done.
        /// </remarks>
        /// <param name="processData">
        /// The execution state data object created from <see cref="Initialize(out object?)"/>.
        /// </param>
        /// <seealso cref="Initialize(out object?)"/>
        /// <seealso cref="Execute(ref object?)"/>
        public virtual void Restore(ref object? processData) { }
    }
}
