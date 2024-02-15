using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;
using MacroModules.Model.Types;

namespace MacroModules.App.ViewModels.Modules;

public class WaitModuleVM : ModuleVM
{
    public TimeDuration Time
    {
        get { return castedModuleData.Time; }
        set
        {
            FullCommitPropertyChange(castedModuleData.Time, value);
            castedModuleData.Time = value;
            OnPropertyChanged();
        }
    }

    public override ModuleType Type { get; } = ModuleType.Wait;

    public WaitModuleVM() : base()
    {
        castedModuleData = (WaitModule)ModuleData;
    }

    public WaitModuleVM(WaitModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly WaitModule castedModuleData;
}
