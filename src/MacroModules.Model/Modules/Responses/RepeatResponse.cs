﻿namespace MacroModules.Model.Modules.Responses
{
    /// <summary>
    /// Represents a return message from a <see cref="Module"/> that indicates that the caller
    /// should immediately repeat execution of the current <see cref="Module"/>.
    /// </summary>
    public class RepeatResponse : IResponse
    {
        /// <inheritdoc/>
        public ResponseType Type { get; } = ResponseType.Repeat;
    }
}
