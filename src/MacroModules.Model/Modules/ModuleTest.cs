using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Modules
{
    public class ModuleTest : Module
    {
        public ModuleTest() { }

        public ModuleTest(Position startingPos)
        {
            this.startingPos = startingPos;
        }

        public override void Initialize(out object? processData)
        {
            processData = new ModuleTestData();

            var castedProcessData = (ModuleTestData)processData;
            castedProcessData.CurrentPos = startingPos;
            MouseControl.MoveCursor(castedProcessData.CurrentPos);
        }

        public override IModuleResponse Execute(ref object? processData)
        {
            var data = (ModuleTestData)processData!;
            if (data.StepsRemaining <= 0)
            {
                return new EndResponse();
            }
            --data.StepsRemaining;

            Position nextPos = data.CurrentPos;
            ++nextPos.X;
            ++nextPos.Y;
            MouseControl.MoveCursor(nextPos);
            data.CurrentPos = nextPos;
            return new WaitRepeatResponse(new TimeSpan(0, 0, 0, 0, 5));
        }

        private Position startingPos = new(10, 10);

        private class ModuleTestData
        {
            public Position CurrentPos { get; set; } = new(10, 10);

            public int StepsRemaining { get; set; } = 100;
        }
    }
}
