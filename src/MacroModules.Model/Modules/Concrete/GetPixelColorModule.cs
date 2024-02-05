using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;
using System.Drawing;

namespace MacroModules.Model.Modules.Concrete
{
    public class GetPixelColorModule : ValuedModule
    {
        public Position PixelPosition { get; set; } = new();

        public override ValueDataType ReturnValueType { get; } = ValueDataType.Color;

        public override ModuleType Type { get; } = ModuleType.GetPixelColor;

        public override IResponse Execute(ref object? processData)
        {
            Color pixelColor = ScreenCapture.GetPixelColor(PixelPosition);

            Value returnValue = new ColorValue(pixelColor);
            SetStoreVariable(returnValue);
            return new ContinueResponse(returnValue);
        }
    }
}
