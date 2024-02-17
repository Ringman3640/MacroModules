using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Execution;

namespace MacroModules.Model.GolbalSystems
{
    /// <summary>
    /// Represents a retrieval utitlity class for obtaining an <see cref="InputTrigger"/> from the
    /// user's input.
    /// </summary>
    public static class TriggerInputObtainer
    {
        /// <summary>
        /// Gets if the <see cref="TriggerInputObtainer"/> is currently running.
        /// </summary>
        public static bool Running { get; private set; } = false;

        /// <summary>
        /// Represents the handler that is called when the user input trigger is received.
        /// </summary>
        public static Action<InputTrigger>? InputHandler
        {
            get { return _inputHandler; }
            set
            {
                if (!Running)
                {
                    _inputHandler = value;
                }
            }
        }
        private static Action<InputTrigger>? _inputHandler = null;

        /// <summary>
        /// Starts the <see cref="TriggerInputObtainer"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="TriggerInputObtainer"/> must not yet be running and
        /// <see cref="InputHandler"/> must be set.
        /// </remarks>
        public static void Start()
        {
            if (Running || InputHandler == null)
            {
                return;
            }

            InputMonitor.SetInputHandler(InputMonitor_HandleInput);
            InputMonitor.CollectInput = true;
            InputMonitor.FilterMouseMovements = true;
            InputMonitor.FilterInjectedInputs = true;
            InputMonitor.Install();
            Running = true;
        }

        /// <summary>
        /// Stops the <see cref="TriggerInputObtainer"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     When this method is called, the supplied <see cref="InputHandler"/> will be cleared
        ///     and must be set again before calling <see cref="Start"/>.
        /// </para>
        /// <para>
        ///     This method is called after the first input trigger from the user is received.
        /// </para>
        /// </remarks>
        public static void Stop()
        {
            if (!Running)
            {
                return;
            }

            InputMonitor.CollectInput = false;
            InputMonitor.Uninstall();
            Running = false;
            InputHandler = null;
        }

        private static bool InputMonitor_HandleInput(InputData input)
        {
            if (input.Type != InputType.MouseDown && input.Type != InputType.KeyDown)
            {
                return true;
            }
            if (Enum.IsDefined(typeof(ModifierInputCode), input.InputKeyCode))
            {
                return false;
            }

            InputHandler?.Invoke(InputTrigger.CreateFrom(input)!);
            Stop();
            return false;
        }
    }
}
