using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class MoveCursorModuleVM : ModuleVM
{
    public override ModuleType Type { get; } = ModuleType.MoveCursor;

    public MoveCursorModuleVM() : base() { }

    public MoveCursorModuleVM(MoveCursorModule moduleData) : base(moduleData) { }
}
