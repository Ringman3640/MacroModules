using MacroModules.Model.Values;

namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should execute the next indicated <see cref="Module"/>. If the given <see cref="Module"/>
    /// is null, the caller should end its macro execution.
    /// </summary>
    public class ContinueResponse : IResponse
    {
        /// <inheritdoc/>
        public ResponseType Type { get; } = ResponseType.Continue;

        /// <summary>
        /// Indicates the port the caller should access to get the next <see cref="Module"/> of the
        /// macro. If this value does not correspond to a valid exit port, end execution of the
        /// macro.
        /// </summary>
        public int ContinuePort { get; private set; } = 0;

        /// <summary>
        /// Indicates an optional <see cref="Value"/> that is returned when a
        /// <see cref="ValuedModule"/> completes execution.
        /// </summary>
        public Value? ReturnValue { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueResponse"/> class that points to
        /// the default exit port (index 0) and has no return value.
        /// </summary>
        public ContinueResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueResponse"/> class that indicates an
        /// exit port to continue and has no return value.
        /// </summary>
        public ContinueResponse(int continuePort)
        {
            ContinuePort = continuePort;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueResponse"/> class that points to
        /// the default exit port (index 0) and indicates a return value.
        /// </summary>
        public ContinueResponse(Value returnValue)
        {
            ReturnValue = returnValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueResponse"/> class that indicates an
        /// exit port to continue and indicates a return value.
        /// </summary>
        public ContinueResponse(int continuePort, Value returnValue)
        {
            ContinuePort = continuePort;
            ReturnValue = returnValue;
        }
    }
}
