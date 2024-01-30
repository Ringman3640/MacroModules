namespace MacroModules.Model.Execution.Events
{
    /// <summary>
    /// Defines an event to handle completion of a <see cref="MacroExecutor"/>.
    /// </summary>
    public interface INotifyExecutionFinished
    {
        /// <summary>
        /// The event that occurs when a <see cref="MacroExecutor"/> has finished executing a macro.
        /// </summary>
        public event ExecutionFinishedEventHandler? ExecutionFinished;
    }

    /// <summary>
    /// Provides data for the <see cref="INotifyExecutionFinished.ExecutionFinished"/> event.
    /// </summary>
    public class ExecutionFinishedArgs : EventArgs
    {
        /// <summary>
        /// Indicates if the <see cref="MacroExecutor"/> should restart execution of the macro.
        /// </summary>
        public bool RestartExecution { get; set; } = false;
    }

    /// <summary>
    /// Represents the method that will handle the
    /// <see cref="INotifyExecutionFinished.ExecutionFinished"/> event raised when a
    /// <see cref="MacroExecutor"/> is finished executing a macro.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExecutionFinishedEventHandler(object sender, ExecutionFinishedArgs e);
}
