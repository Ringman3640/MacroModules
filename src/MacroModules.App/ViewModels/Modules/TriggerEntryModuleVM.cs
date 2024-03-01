using MacroModules.Model.Execution;
using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class TriggerEntryModuleVM : ModuleVM
{
    public InputTrigger? Trigger
    {
        get { return castedModuleData.Trigger; }
        set
        {
            FullCommitPropertyChange(castedModuleData.Trigger, value);
            castedModuleData.Trigger = value;
            OnPropertyChanged();
        }
    }

    public MacroExecutionType ExecutionType
    {
        get { return castedModuleData.ExecutionType; }
        set
        {
            if (castedModuleData.ExecutionType != value)
            {
                FullCommitPropertyChange(castedModuleData.ExecutionType, value);
                castedModuleData.ExecutionType = value;
                OnPropertyChanged();
            }
        }
    }

    public bool SuppressInput
    {
        get { return castedModuleData.SuppressInput; }
        set
        {
            if (castedModuleData.SuppressInput != value)
            {
                FullCommitPropertyChange(castedModuleData.SuppressInput, value);
                castedModuleData.SuppressInput = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.TriggerEntry;

    public override string ElementTitle { get; } = "Trigger Entry";

    public TriggerEntryModuleVM() : base()
    {
        castedModuleData = (TriggerEntryModule)ModuleData;
    }

    public TriggerEntryModuleVM(TriggerEntryModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly TriggerEntryModule castedModuleData;
}
