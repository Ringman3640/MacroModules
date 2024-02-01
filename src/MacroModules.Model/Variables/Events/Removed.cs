namespace MacroModules.Model.Variables.Events
{
    /// <summary>
    /// Defines an event to handle variable deletion.
    /// </summary>
    public interface INotifyVariableDeleted
    {
        /// <summary>
        /// The event that occurs when the variable is marked as deleted.
        /// </summary>
        public event DeletedEventHandler? Deleted;
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="INotifyVariableDeleted.Deleted"/> event raised when a
    /// variable is marked as deleted.
    /// </summary>
    /// <param name="sender">The <see cref="Variable"/> that raised the event.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance that contains event data.</param>
    public delegate void DeletedEventHandler(object sender, EventArgs e);
}
