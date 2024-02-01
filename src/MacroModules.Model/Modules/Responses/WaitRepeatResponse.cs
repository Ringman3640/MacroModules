namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should wait for a specified amount of time and then repeat the current <see cref="Module"/>
    /// execution.
    /// </summary>
    public class WaitRepeatResponse : IResponse
    {
        /// <inheritdoc/>
        public ResponseType Type { get; } = ResponseType.WaitRepeat;

        /// <summary>
        /// Indicates the amount of time the caller should wait for before repeating the current
        /// MacroProcess execution.
        /// </summary>
        public TimeSpan WaitTime { get; private set; }

        public WaitRepeatResponse(TimeSpan waitTime)
        {
            WaitTime = waitTime;
        }
    }
}
