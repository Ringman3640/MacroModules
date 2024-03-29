﻿using MacroModules.Model.BoardElements;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Variables;
using System.Text.Json.Serialization;

namespace MacroModules.Model.Modules
{
    /// <summary>
    /// Represents a single executable process in a macro.
    /// </summary>
    public abstract class Module : BoardElement
    {
        /// <summary>
        /// Gets the unique identifier for this <see cref="Module"/>.
        /// </summary>
        [JsonInclude]
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the type of the <see cref="Module"/> as a <see cref="ModuleType"/> enum.
        /// </summary>
        public abstract ModuleType Type { get; }

        /// <summary>
        /// Gets whether or not other <see cref="Module"/> objects can connect to this
        /// <see cref="Module"/>.
        /// </summary>
        /// <remarks>
        /// If this value is <c>false</c>, other modules should not connect to this module from an
        /// <see cref="ExitPort"/>.
        /// </remarks>
        public abstract bool IsConnectable { get; }

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
        [JsonInclude]
        public List<ExitPort> ExitPorts { get; protected set; }

        /// <summary>
        /// The <see cref="VariableStore"/> object that is used as a context to get all necessary
        /// <see cref="Variable"/> instances.
        /// </summary>
        public VariableStore? VariableContext { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class that contains a single
        /// default <see cref="ExitPort"/>.
        /// </summary>
        /// <remarks>
        /// This constructor should only be called if the concrete <see cref="Module"/> being
        /// created will use the default <see cref="ExitPort"/>. Do not call this constructor if
        /// the concrete instance will define its own port or if it has no ports.
        /// </remarks>
        public Module()
        {
            ExitPorts =
            [
                new ExitPort("Next")
            ];
        }

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
        /// A <see cref="IResponse"/> that indicates what the caller should do next.
        /// </returns>
        /// <seealso cref="Initialize(out object?)"/>
        /// <seealso cref="IResponse"/>
        public abstract IResponse Execute(ref object? processData);

        /// <summary>
        /// Revert any system state changes that were set during <see cref="Execute(ref object?)"/>
        /// or <see cref="Initialize(out object?)"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Restore(ref object?)"/> should be called if the caller must stop executing
        /// the <see cref="Module"/> before <see cref="Execute(ref object?)"/> indicates that the
        /// execution is done. 
        /// </remarks>
        /// <param name="processData">
        /// The execution state data object created from <see cref="Initialize(out object?)"/>. This
        /// value will be set to <c>null</c>.
        /// </param>
        /// <seealso cref="Initialize(out object?)"/>
        /// <seealso cref="Execute(ref object?)"/>
        public virtual void Restore(ref object? processData)
        {
            processData = null;
        }

        /// <summary>
        /// Replaces the current Id of this <see cref="Module"/> with a new Id.
        /// </summary>
        /// <remarks>
        /// This method should only be called if the current module is not referenced by any other
        /// modules. Any previous object references to this module may result in broken state.
        /// </remarks>
        public virtual void GenerateNewId()
        {
            Id = Guid.NewGuid();
        }
    }
}
