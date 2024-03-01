using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class CloseProgramModuleVM : ValuedModuleVM
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

    public bool CloseAllMatches
    {
        get { return castedModuleData.CloseAllMatches; }
        set
        {
            if (castedModuleData.CloseAllMatches != value)
            {
                FullCommitPropertyChange(castedModuleData.CloseAllMatches, value);
                castedModuleData.CloseAllMatches = value;
                OnPropertyChanged();
            }
        }
    }

    public bool CloseChildren
    {
        get { return castedModuleData.CloseChildren; }
        set
        {
            if (castedModuleData.CloseChildren != value)
            {
                FullCommitPropertyChange(castedModuleData.CloseChildren, value);
                castedModuleData.CloseChildren = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.CloseProgram;

    public override string ElementTitle { get; } = "Close Program";

    public CloseProgramModuleVM() : base()
    {
        castedModuleData = (CloseProgramModule)ModuleData;
    }

    public CloseProgramModuleVM(CloseProgramModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly CloseProgramModule castedModuleData;
}
