namespace MacroModules.Model.Variable
{
    public class VariableStore
    {
        /// <summary>
        /// Creates a new <see cref="VariableBase"/> instance and adds it to the
        /// <see cref="VariableStore"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="VariableBase"/> to create.</param>
        /// <returns>
        /// The <see cref="VariableBase"/> that was created and added to the
        /// <see cref="VariableStore"/>.
        /// </returns>
        public VariableBase CreateVariable(VariableType type)
        {
            
            VariableBase variable = VariableFactory.Create(type);
            lock (store)
            {
                store.Add(variable.Id, variable);
            }
            return variable;
        }

        /// <summary>
        /// Gets a stored <see cref="VariableBase"/> from the <see cref="VariableStore"/> given the
        /// variable's <see cref="VariableBase.Id"/>.
        /// </summary>
        /// <param name="variableId">
        /// The <see cref="VariableBase.Id"/> of the variable to get.
        /// </param>
        /// <returns>
        /// <c>null</c> if <paramref name="variableId"/> was not found in the store. Otherwise
        /// returns the corresponding <see cref="VariableBase"/> instance.
        /// </returns>
        public VariableBase? GetVariable(Guid variableId)
        {
            lock (store)
            {
                store.TryGetValue(variableId, out VariableBase? variable);
                return variable;
            }
        }

        /// <summary>
        /// Removes a stored <see cref="VariableBase"/> from the <see cref="VariableStore"/> given
        /// the variable's <see cref="VariableBase.Id"/>.
        /// </summary>
        /// <remarks>
        /// Removing a variable from the store will mark the variable as deleted by calling
        /// <see cref="VariableBase.IndicateDeleted"/>. This will notify all listeners of the
        /// <see cref="VariableBase.Deleted"/> event.
        /// </remarks>
        /// <param name="variableId">
        /// The <see cref="VariableBase.Id"/> of the variable to remove.
        /// </param>
        /// <returns>
        /// <c>true</c> if the variable is successfully removed. Returns <c>false</c> if
        /// <paramref name="variableId"/> was not found in the store.
        /// </returns>
        public bool RemoveVariable(Guid variableId)
        {
            lock (store)
            {
                if (!store.TryGetValue(variableId, out VariableBase? variable))
                {
                    return false;
                }
                variable.IndicateDeleted();
                return store.Remove(variableId);
            }
        }

        private readonly Dictionary<Guid, VariableBase> store = [];
    }
}
