using MacroModules.Model.Execution;
using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class TriggerEntryModuleVM : ModuleVM
{
    public override ModuleType Type { get; } = ModuleType.TriggerEntry;

    public InputTrigger? Trigger
    {
        get { return ((TriggerEntryModule)ModuleData).Trigger; }
    }
}
