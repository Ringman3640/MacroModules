using MacroModules.Model.Variable.Events;
using System.Windows.Markup;

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
