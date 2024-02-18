using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class FocusWindowModuleVM : ModuleVM
{
    public string SearchTerm
    {
        get { return castedModuleData.SearchTerm; }
        set
        {
            if (castedModuleData.SearchTerm != value)
            {
                FullCommitPropertyChange(castedModuleData.SearchTerm, value);
                castedModuleData.SearchTerm = value;
                OnPropertyChanged();
            }
        }
    }

    public ProgramSearchTarget SearchComponent
    {
        get { return castedModuleData.SearchComponent; }
        set
        {
            if (castedModuleData.SearchComponent != value)
            {
                FullCommitPropertyChange(castedModuleData.SearchComponent, value);
                castedModuleData.SearchComponent = value;
                OnPropertyChanged();
            }
        }
    }

    public bool FocusAllMatches
    {
        get { return castedModuleData.FocusAllMatches; }
        set
        {
            if (castedModuleData.FocusAllMatches != value)
            {
                FullCommitPropertyChange(castedModuleData.FocusAllMatches, value);
                castedModuleData.FocusAllMatches = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.FocusWindow;

    public FocusWindowModuleVM() : base()
    {
        castedModuleData = (FocusWindowModule)ModuleData;
    }

    public FocusWindowModuleVM(FocusWindowModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly FocusWindowModule castedModuleData;
}
