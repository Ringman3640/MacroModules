using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Modules
{
    public class ModuleTest : Module
    {
        public override void Initialize(out object? processData)
        {
            processData = new ModuleTestData();
            MouseControl.MoveCursor(((ModuleTestData)processData).CurrentPos);
        }

        public override IModuleResponse Execute(ref object? processData)
        {
            var data = (ModuleTestData)processData!;
            if (data.StepsRemaining <= 0)
            {
                return new ModuleEnd();
            }
            --data.StepsRemaining;

            Position nextPos = data.CurrentPos;
            ++nextPos.X;
            ++nextPos.Y;
            MouseControl.MoveCursor(nextPos);
            data.CurrentPos = nextPos;
            return new ModuleWaitRepeat(new TimeSpan(0, 0, 0, 0, 10));
        }

        private class ModuleTestData
        {
            public Position CurrentPos { get; set; } = new(10, 10);

            public int StepsRemaining { get; set; } = 1000;
        }
    }
}
