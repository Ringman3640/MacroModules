using MacroModules.Model.Values;

namespace MacroModules.Model.Variables
{
    public class VariableStore
    {
        /// <summary>
        /// Creates a new <see cref="Variable"/> instance and adds it to the
        /// <see cref="VariableStore"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Variable"/> to create.</param>
        /// <returns>
        /// The <see cref="Variable"/> that was created and added to the
        /// <see cref="VariableStore"/>.
        /// </returns>
        public Variable CreateVariable(ValueDataType type)
        {

            Variable variable = new(type);
            lock (store)
            {
                store.Add(variable.Id, variable);
            }
            return variable;
        }

        /// <summary>
        /// Gets a stored <see cref="Variable"/> from the <see cref="VariableStore"/> given the
        /// variable's <see cref="Variable.Id"/>.
        /// </summary>
        /// <param name="variableId">
        /// The <see cref="Variable.Id"/> of the variable to get.
        /// </param>
        /// <returns>
        /// <c>null</c> if <paramref name="variableId"/> was not found in the store. Otherwise
        /// returns the corresponding <see cref="Variable"/> instance.
        /// </returns>
        public Variable? GetVariable(Guid variableId)
        {
            lock (store)
            {
                store.TryGetValue(variableId, out Variable? variable);
                return variable;
            }
        }

        /// <summary>
        /// Initializes all <see cref="Variable"/> instances in the store by calling
        /// <see cref="Variable.InitializeRuntimeValue"/> on each instance.
        /// </summary>
        /// <remarks>
        /// This method should be called right when macro runtime execution is started.
        /// </remarks>
        public void InitializeRuntimeVariables()
        {
            lock (store)
            {
                foreach (Variable variable in store.Values)
                {
                    variable.InitializeRuntimeValue();
                }
            }
        }

        /// <summary>
        /// Removes a stored <see cref="Variable"/> from the <see cref="VariableStore"/> given
        /// the variable's <see cref="Variable.Id"/>.
        /// </summary>
        /// <remarks>
        /// Removing a variable from the store will mark the variable as deleted by calling
        /// <see cref="Variable.IndicateDeleted"/>. This will notify all listeners of the
        /// <see cref="Variable.Deleted"/> event.
        /// </remarks>
        /// <param name="variableId">
        /// The <see cref="Variable.Id"/> of the variable to remove.
        /// </param>
        /// <returns>
        /// <c>true</c> if the variable is successfully removed. Returns <c>false</c> if
        /// <paramref name="variableId"/> was not found in the store.
        /// </returns>
        public bool RemoveVariable(Guid variableId)
        {
            lock (store)
            {
                if (!store.TryGetValue(variableId, out Variable? variable))
                {
                    return false;
                }
                variable.IndicateDeleted();
                return store.Remove(variableId);
            }
        }

        private readonly Dictionary<Guid, Variable> store = [];
    }
}
