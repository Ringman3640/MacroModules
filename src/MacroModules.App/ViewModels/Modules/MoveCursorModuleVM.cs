using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class MoveCursorModuleVM : ModuleVM
{
    public Position TargetPosition
    {
        get { return castedModuleData.TargetPosition; }
        set
        {
            if (castedModuleData.TargetPosition != value)
            {
                CommitPropertyChange(castedModuleData.TargetPosition, value);
                castedModuleData.TargetPosition = value;
                OnPropertyChanged();
            }
        }
    }

    public TimeSpan TransitionTime
    {
        get { return castedModuleData.TransitionTime; }
        set
        {
            if (castedModuleData.TransitionTime != value)
            {
                CommitPropertyChange(castedModuleData.TransitionTime, value);
                castedModuleData.TransitionTime = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.MoveCursor;

    public MoveCursorModuleVM() : base()
    {
        castedModuleData = (MoveCursorModule)ModuleData;
    }

    public MoveCursorModuleVM(MoveCursorModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly MoveCursorModule castedModuleData;
}
