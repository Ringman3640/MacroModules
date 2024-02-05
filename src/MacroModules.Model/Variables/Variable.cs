using MacroModules.Model.Values;
using MacroModules.Model.Variables.Events;

namespace MacroModules.Model.Variables
{
    /// <summary>
    /// Represents a named and persistent <see cref="Value"/> during runtime.
    /// </summary>
    public class Variable : INotifyVariableDeleted, INotifyNameChanged
    {
        /// <summary>
        /// Gets the unique identifier of the <see cref="Variable"/> instance.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets the user-defined name of the <see cref="Variable"/>.
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
                lock(eventLock)
                {
                    string prevName = internalName;
                    internalName = value;
                    NameChanged?.Invoke(this, new NameChangedArgs(prevName));
                }
            }
        }

        /// <summary>
        /// Gets the type of the contained <see cref="Value"/> instances as a
        /// <see cref="ValueDataType"/> enum.
        /// </summary>
        public ValueDataType Type { get; private set; }

        /// <summary>
        /// Indicates the initial <see cref="Value"/> that this <see cref="Variable"/> contains
        /// before macro runtime. This value should only be modified when editing.
        /// </summary>
        public Value InitialValue { get; set; }

        /// <summary>
        /// Indicates the runtime <see cref="Value"/> that this <see cref="Variable"/> contains.
        /// This value will change during macro runtime as the variable is modified.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     When getting <see cref="RuntimeValue"/>, a clone of the current runtime value is
        ///     returned. This is done to prevent race conditions between multiple reading and
        ///     writing threads.
        /// </para>
        /// <para>
        ///     When setting <see cref="RuntimeValue"/>, a clone of the provided value will be
        ///     stored. This is to prevent any modifications to the passed value affecting the
        ///     stored runtime value.
        /// </para>
        /// </remarks>
        public Value RuntimeValue
        {
            get
            {
                lock (runtimeAccessLock)
                {
                    return (runtimeValue != null) ? runtimeValue.Clone() : InitialValue.Clone();
                }
            }
            set
            {
                lock (runtimeAccessLock)
                {
                    if (value.Type != this.Type)
                    {
                        return;
                    }
                    if (runtimeValue != null)
                    {
                        runtimeValue.Dispose();
                    }
                    runtimeValue = value.Clone();
                }
            }
        }

        /// <inheritdoc/>
        public event DeletedEventHandler? Deleted;

        /// <inheritdoc/>
        public event NameChangedEventHandler? NameChanged;

        public Variable(ValueDataType type)
        {
            InitialValue = ValueFactory.Create(type);
        }

        /// <summary>
        /// Initializes the <see cref="Variable"/> values such that the runtime value becomes a
        /// clone the initial value.
        /// </summary>
        public void InitializeRuntimeValue()
        {
            runtimeValue = InitialValue.Clone();
        }

        /// <summary>
        /// Indicate to the <see cref="Variable"/> and all listeners that the variable is deleted.
        /// </summary>
        public void IndicateDeleted()
        {
            lock (eventLock)
            {
                Deleted?.Invoke(this, EventArgs.Empty);
                Deleted = null;
                NameChanged = null;
            }
        }

        private object runtimeAccessLock = new();
        private Value? runtimeValue;
        private string internalName = String.Empty;
        private readonly object eventLock = new();
    }
}
