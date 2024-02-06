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
        public string? Name { get; private set; }

        /// <summary>
        /// Brief description of the exit port.
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// The <see cref="Module"/> that this port leads to.
        /// </summary>
        public Module? Destination { get; set; } = null;

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
