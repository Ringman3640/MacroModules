using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class SendInputModuleVM : ModuleVM
{
    public override ModuleType Type { get; } = ModuleType.SendInput;

    public ushort InputCode
    {
        get { return castedModuleData.InputCode; }
        set
        {
            if (castedModuleData.InputCode != value)
            {
                FullCommitPropertyChange(castedModuleData.InputCode, value);
                castedModuleData.InputCode = value;
                OnPropertyChanged();
            }
        }
    }

    public SendInputAction Action
    {
        get { return castedModuleData.Action; }
        set
        {
            if (castedModuleData.Action != value)
            {
                FullCommitPropertyChange(castedModuleData.Action, value);
                castedModuleData.Action = value;
                OnPropertyChanged();
            }
        }
    }

    public SendInputModuleVM() : base()
    {
        castedModuleData = (SendInputModule)ModuleData;
    }

    public SendInputModuleVM(SendInputModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly SendInputModule castedModuleData;
}
