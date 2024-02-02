using MacroModules.MacroLibrary;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that checks the current state of an input.
    /// </summary>
    public class GetInputStateModule : ValuedModule
    {
        /// <summary>
        /// Indicates the input to check as a Windows virtual key code.
        /// </summary>
        public ushort InputCode { get; set; } = 0;

        /// <summary>
        /// Indicates if the toggle state of the key should be accounted for.
        /// </summary>
        public bool TestToggled { get; set; } = false;

        public override ValueDataType ReturnValueType { get; } = ValueDataType.Bool;

        public override ModuleType Type { get; } = ModuleType.GetInputState;

        public override IResponse Execute(ref object? processData)
        {
            InputMonitor.Uninstall();
            bool result = InputMonitor.InputHeld(InputCode);
            if (!result && TestToggled)
            {
                result = InputMonitor.InputToggled(InputCode);
            }

            return new ValuedContinueResponse(ExitPorts[0].Destination)
            {
                ReturnValue = new BoolValue(result)
            };
        }
    }
}
