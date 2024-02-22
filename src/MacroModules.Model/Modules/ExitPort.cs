using System.Text.Json.Serialization;

namespace MacroModules.Model.Modules
{
    /// <summary>
    /// Defines a single exit port of a <see cref="Module"/>.
    /// </summary>
    public class ExitPort
    {
        /// <summary>
        /// Descriptive name of the exit port.
        /// </summary>
        [JsonInclude]
        public string? Name { get; private set; }

        /// <summary>
        /// Brief description of the exit port.
        /// </summary>
        [JsonInclude]
        public string? Description { get; private set; }

        /// <summary>
        /// The Id of the <see cref="Module"/> that is port leads to.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This property should only ever be set privately from the Destination property
        ///     setter.
        /// </para>
        /// <para>
        ///     The purpose of this property is to indicate what <see cref="Module"/> this
        ///     <see cref="ExitPort"/> leads to without needing to know the exact object reference.
        ///     This is needed when a predefined module is loaded from JSON. Object references are
        ///     lost during serialization, so this property allows a logical reference to be
        ///     maintained.
        /// </para>
        /// </remarks>
        [JsonInclude]
        public Guid DestinationId { get; private set; } = Guid.Empty;

        /// <summary>
        /// The <see cref="Module"/> that this port leads to.
        /// </summary>
        [JsonIgnore]
        public Module? Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                DestinationId = (value == null) ? Guid.Empty : value.Id;
            }
        }
        private Module? _destination = null;

        /// <summary>
        /// Initializes a new <see cref="ExitPort"/> instance that has no name or description.
        /// </summary>
        public ExitPort() { }

        /// <summary>
        /// Initializes a new <see cref="ExitPort"/> instance with the given name value.
        /// </summary>
        /// <inheritdoc cref="ExitPort(string, string)" path="/param"/>
        public ExitPort(string portName)
        {
            Name = portName;
            Description = null;
        }

        /// <summary>
        /// Initializes a new <see cref="ExitPort"/> instance with the given name and description
        /// values.
        /// </summary>
        /// <param name="portName">The descriptive name of the port.</param>
        /// <param name="description">A brief description of the port.</param>
        public ExitPort(string portName, string description)
        {
            Name = portName;
            Description = description;
        }
    }
}
