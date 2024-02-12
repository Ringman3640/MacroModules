using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;
using MacroModules.Model.Variables;
using MacroModules.Model.Variables.Events;

namespace MacroModules.Model.Modules
{
    /// <summary>
    /// Represents a specialized <see cref="Module"/> that returns a <see cref="Value"/> from its
    /// execution <see cref="IResponse"/>.
    /// </summary>
    public abstract class ValuedModule : Module
    {
        /// <inheritdoc/>
        public override bool IsConnectable { get; } = true;

        /// <summary>
        /// Gets the <see cref="ValueType"/> of the <see cref="Value"/> returned by this object from
        /// <see cref="Execute(ref object?)"/>.
        /// </summary>
        public abstract ValueDataType ReturnValueType { get; }

        /// <summary>
        /// Indicates the <see cref="Variable"/> used to store the <see cref="Value"/> created from
        /// the <see cref="ValuedModule"/> during runtime.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     If this value is <c>null</c>, it is assumed that the <see cref="ValuedModule"/>
        ///     should not store its value.
        /// </para>
        /// <para>
        ///     Attempting to set a <see cref="Variable"/> whose type is different from
        ///     <see cref="ReturnValueType"/> will result in the set doing nothing.
        /// </para>
        /// <para>
        ///     The <see cref="Variable.Deleted"/> event is automatically handled by the property.
        ///     If the stored <see cref="Variable"/> invokes its deleted event, the property will
        ///     automatically set itself to <c>null</c>.
        /// </para>
        /// </remarks>
        public Variable? StoreVariable
        {
            protected get { return storeVariable; }
            set
            {
                if (value?.Type != ReturnValueType)
                {
                    return;
                }
                if (storeVariable != null)
                {
                    storeVariable.Deleted -= StoreVariable_Deleted;
                }
                if (value != null)
                {
                    value.Deleted += StoreVariable_Deleted;
                }
                storeVariable = value;
            }
        }

        /// <inheritdoc/>
        public ValuedModule() : base() { }

        /// <inheritdoc/>
        public abstract override IResponse Execute(ref object? processData);

        /// <summary>
        /// Sets the <see cref="Variable.RuntimeValue"/> of <see cref="StoreVariable"/> to the
        /// provided <see cref="Value"/>.
        /// </summary>
        /// <remarks>
        /// This is a general helper method for <see cref="ValuedModule"/> derived types.
        /// </remarks>
        /// <param name="value">
        /// The <see cref="Value"/> to store in <see cref="StoreVariable"/>.
        /// </param>
        protected virtual void SetStoreVariable(Value value)
        {
            if (storeVariable != null)
            {
                storeVariable.RuntimeValue = value;
            }
        }

        private Variable? storeVariable = null;

        /// <inheritdoc cref="DeletedEventHandler"/>
        private void StoreVariable_Deleted(object sender, EventArgs e)
        {
            storeVariable = null;
        }
    }
}
