using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that gets the current cursor position on the screen.
    /// </summary>
    public class GetCursorPositionModule : ValuedModule
    {
        public override ValueDataType ReturnValueType { get; } = ValueDataType.Position;

        public override ModuleType Type { get; } = ModuleType.GetCursorPosition;

        public override IResponse Execute(ref object? processData)
        {
            InputMonitor.Uninstall();
            Position cursorPos = MouseControl.GetCursorPosition();

            Value returnValue = new PositionValue(cursorPos);
            SetStoreVariable(returnValue);
            return new ContinueResponse(returnValue);
        }
    }
}
