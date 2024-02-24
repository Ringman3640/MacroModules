using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;

namespace MacroModules.App.ViewModels;

public class ModuleBarVM
{
    public WorkspaceVM Workspace { get; private set; }

    public List<ModuleBarItemVM> Items { get; } = [];

    public ModuleBarVM(WorkspaceVM workspace)
    {
        Workspace = workspace;
        Array modulesArray = typeof(ModuleType).GetEnumValues();
        foreach (ModuleType moduleType in modulesArray)
        {
            try
            {
                ModuleVM module = ModuleVMFactory.Create(moduleType);
                Items.Add(new ModuleBarItemVM(module));
            }
            catch { }
        }
    }
}
