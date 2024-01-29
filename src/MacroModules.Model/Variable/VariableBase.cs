using MacroModules.Model.Variable.Events;

namespace MacroModules.Model.Variable
{
    public abstract class VariableBase : INotifyVariableDeleted, INotifyNameChanged
    {
        /// <summary>
        /// Gets the unique identifier of the <see cref="VariableBase"/> instance.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets the user-defined name of the <see cref="VariableBase"/>.
        /// </summary>
        /// <remarks>
        /// Setting the name value will invoke all listeners of the <see cref="NameChanged"/> event.
        /// </remarks>
        public string Name
        {
            get { return internalName; }
            set
            {
                if (value == internalName)
                {
                    return;
                }
                lock(eventMutex)
                {
                    string prevName = internalName;
                    internalName = value;
                    NameChanged?.Invoke(this, new NameChangedArgs(prevName));
                }
            }
        }

        /// <summary>
        /// Gets the type of the <see cref="VariableBase"/> as a <see cref="VariableType"/> enum.
        /// </summary>
        public abstract VariableType Type { get; }

        /// <inheritdoc/>
        public event DeletedEventHandler? Deleted;

        /// <inheritdoc/>
        public event NameChangedEventHandler? NameChanged;

        /// <summary>
        /// Initializes the <see cref="VariableBase"/> values such that the runtime value becomes a
        /// deep copy of the initial value.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This method must be implemented in a derived class that also implements the
        ///     <see cref="IVariableValues{T}"/> interface. The purpose of this method is to
        ///     initialize <see cref="IVariableValues{T}.RuntimeValue"/> with a copy of
        ///     <see cref="IVariableValues{T}.InitialValue"/> so that any modifications to the
        ///     runtime value do not affect the initial value.
        /// </para>
        /// <para>
        ///     This method needs to be separated from <see cref="IVariableValues{T}"/> so that it
        ///     can be called while iterating over a collection of <see cref="VariableBase"/>
        ///     instances. Otherwise, each instance would need to be casted to a corresponding
        ///     derived type since you cannot abstract down to a generic base class/interface
        ///     without specifying the generic type. Sad :(
        /// </para>
        /// </remarks>
        public abstract void InitializeRuntimeValue();

        /// <summary>
        /// Indicate to the <see cref="VariableBase"/> and all listeners that the variable is
        /// deleted.
        /// </summary>
        public void IndicateDeleted()
        {
            lock (eventMutex)
            {
                Deleted?.Invoke(this, EventArgs.Empty);
                Deleted = null;
                NameChanged = null;
            }
        }

        private string internalName = String.Empty;
        private readonly object eventMutex = new();
    }
}
