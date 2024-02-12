using MacroModules.MacroLibrary;
using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Modules.Concrete
{
    public enum SendInputAction
    {
        Click,
        Hold,
        Release
    }

    /// <summary>
    /// Represents a <see cref="Module"/> that sends keyboard and mouse inputs to the system.
    /// </summary>
    public class SendInputModule : Module
    {
        /// <summary>
        /// Indicates the virtual key code of the input to send.
        /// </summary>
        /// <remarks>
        /// This module uses the defined Windows virtual key codes to send inputs. A defined set of
        /// most key code can be selected from <see cref="MacroLibrary.Types.InputCode"/>.
        /// </remarks>
        public ushort InputCode { get; set; } = 0;

        /// <summary>
        /// Indicates the input action to perform. This may be a click, hold, or release action.
        /// </summary>
        public SendInputAction Action { get; set; } = SendInputAction.Click;

        public override ModuleType Type { get; } = ModuleType.SendInput;

        public override bool IsConnectable { get; } = true;

        public override IResponse Execute(ref object? processData)
        {
            switch (Action)
            {
                case SendInputAction.Click:
                    InputControl.Click(InputCode);
                    break;

                case SendInputAction.Hold:
                    InputControl.Hold(InputCode);
                    break;

                case SendInputAction.Release:
                    InputControl.Release(InputCode);
                    break;
            }

            return new ContinueResponse();
        }
    }
}
