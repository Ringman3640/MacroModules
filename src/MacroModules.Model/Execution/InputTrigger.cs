using MacroModules.MacroLibrary.Types;

namespace MacroModules.Model.Execution
{
    /// <summary>
    /// Represents an input that triggers a macro.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     An <see cref="InputTrigger"/> is effectively a specialization of an
    ///     <see cref="InputData"/> instance. However, rather than representing an input that took
    ///     place, an <see cref="InputTrigger"/> is used to indicate what input is needed for a
    ///     macro to activate. It stores the input key code of the activating input and the required
    ///     state of the modifier keys when the input is clicked.
    /// </para>
    /// </remarks>
    public class InputTrigger : IEquatable<InputTrigger>
    {
        /// <summary>
        /// Gets the virtual key code of the trigger.
        /// </summary>
        public ushort InputKeyCode { get; private set; }

        /// <summary>
        /// Gets the the modifier key states of the trigger.
        /// </summary>
        public InputModifiers Modifiers { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputTrigger"/> that is populated with a
        /// virtual key code input value and no <see cref="InputModifiers"/> flags.
        /// </summary>
        /// <param name="inputKeyCode">The virtual key code of the trigger.</param>
        public InputTrigger(ushort inputKeyCode)
        {
            InputKeyCode = inputKeyCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InputTrigger"/> that is populated with a
        /// virtual key code input value and an <see cref="InputModifiers"/> flag value.
        /// </summary>
        /// <param name="inputKeyCode">The virtual key code of the trigger.</param>
        /// <param name="modifiers">The modifier key states of the trigger.</param>
        public InputTrigger(ushort inputKeyCode, InputModifiers modifiers)
        {
            InputKeyCode = inputKeyCode;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Creates a new <see cref="InputTrigger"/> instance from an input down
        /// <see cref="InputData"/> instance.
        /// </summary>
        /// <param name="inputData">The initialization data.</param>
        /// <returns>
        /// A new <see cref="InputTrigger"/> instance on success. Otherwise <c>null</c>. The method
        /// will fail if the type of <paramref name="inputData"/> is not either
        /// <see cref="InputType.MouseDown"/> or <see cref="InputType.KeyDown"/>.
        /// </returns>
        public static InputTrigger? CreateFrom(InputData inputData)
        {
            if (inputData.Type != InputType.KeyDown && inputData.Type != InputType.MouseDown)
            {
                return null;
            }

            return new InputTrigger(inputData.InputKeyCode, inputData.Modifiers);
        }

        /// <summary>
        /// Gets if the <see cref="InputTrigger"/> originates from a mouse button input.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <see cref="InputTrigger"/> is a mouse button input. Otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool IsMouseInput()
        {
            return Enum.IsDefined(typeof(MouseInputCode), InputKeyCode);
        }

        /// <inheritdoc/>
        public bool Equals(InputTrigger? other)
        {
            if (other is null)
            {
                return false;
            }

            return InputKeyCode == other.InputKeyCode && Modifiers == other.Modifiers;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is InputTrigger other && other.Equals(this);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (InputKeyCode << 16) + (int)Modifiers;
        }
    }
}
