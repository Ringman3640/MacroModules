using MacroModules.Model.Modules;

namespace MacroModules.App.ViewModels.Modules;

public class StartupEntryModuleVM : ModuleVM
{
    public override ModuleType Type { get; } = ModuleType.StartupEntry;
}
