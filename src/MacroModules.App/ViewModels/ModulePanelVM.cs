using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;

namespace MacroModules.App.ViewModels;

public class ModulePanelVM
{
    public WorkspaceVM Workspace { get; private set; }

    public List<ModulePanelItemVM> Items { get; } = [];

    public ModulePanelVM(WorkspaceVM workspace)
    {
        Workspace = workspace;
        Array modulesArray = typeof(ModuleType).GetEnumValues();
        foreach (ModuleType moduleType in modulesArray)
        {
            try
            {
                ModuleVM module = ModuleVMFactory.Create(moduleType);
                Items.Add(new ModulePanelItemVM(module));
            }
            catch { }
        }
    }
}
