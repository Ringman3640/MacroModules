namespace MacroModules.Model.Variables.Events
{
    /// <summary>
    /// Defines an event to handle variable name changes.
    /// </summary>
    public interface INotifyNameChanged
    {
        /// <summary>
        /// The event that occurs when a variable's name is changed.
        /// </summary>
        public event NameChangedEventHandler? NameChanged;
    }

    /// <summary>
    /// Provides data for the <see cref="INotifyNameChanged.NameChanged"/> event.
    /// </summary>
    public class NameChangedArgs : EventArgs
    {
        /// <summary>
        /// The previous name of the <see cref="Variable"/> before it was modified.
        /// </summary>
        public string PreviousName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameChangedArgs"/> class.
        /// </summary>
        /// <param name="previousName">The name of the </param>
        public NameChangedArgs(string previousName)
        {
            PreviousName = previousName;
        }
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="INotifyNameChanged.NameChanged"/>
    /// event raised when a variable name is modified.
    /// </summary>
    /// <param name="sender">The <see cref="Variable"/> that raised the event.</param>
    /// <param name="e">An <see cref="NameChangedArgs"/> instance that contains event data.</param>
    public delegate void NameChangedEventHandler(object sender, NameChangedArgs e);
}
