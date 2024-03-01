using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class StartupEntryModuleVM : ModuleVM
{
    public override ModuleType Type { get; } = ModuleType.StartupEntry;

    public override string ElementTitle { get; } = "Startup Entry";

    public StartupEntryModuleVM() : base() { }

    public StartupEntryModuleVM(StartupEntryModule moduleData) : base(moduleData) { }
}
