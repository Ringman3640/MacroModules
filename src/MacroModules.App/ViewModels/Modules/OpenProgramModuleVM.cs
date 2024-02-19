using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class OpenProgramModuleVM : ValuedModuleVM
{
    public string ProgramPath
    {
        get { return castedModuleData.ProgramPath; }
        set
        {
            if (castedModuleData.ProgramPath != value)
            {
                FullCommitPropertyChange(castedModuleData.ProgramPath, value);
                castedModuleData.ProgramPath = value;
                OnPropertyChanged();
            }
        }
    }

    public string Arguments
    {
        get { return castedModuleData.Arguments; }
        set
        {
            if (castedModuleData.Arguments != value)
            {
                FullCommitPropertyChange(castedModuleData.Arguments, value);
                castedModuleData.Arguments = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.OpenProgram;

    public OpenProgramModuleVM() : base()
    {
        castedModuleData = (OpenProgramModule)ModuleData;
    }

    public OpenProgramModuleVM(OpenProgramModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly OpenProgramModule castedModuleData;
}
