using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.ViewModels.Modules;

public class ScrollModuleVM : ModuleVM
{
    public int ScrollTicks
    {
        get { return castedModuleData.ScrollTicks; }
        set
        {
            if (castedModuleData.ScrollTicks != value)
            {
                FullCommitPropertyChange(castedModuleData.ScrollTicks, value);
                castedModuleData.ScrollTicks = value;
                OnPropertyChanged();
            }
        }
    }

    public ScrollDirection Direction
    {
        get { return castedModuleData.Direction; }
        set
        {
            if (castedModuleData.Direction != value)
            {
                FullCommitPropertyChange(castedModuleData.Direction, value);
                castedModuleData.Direction = value;
                OnPropertyChanged();
            }
        }
    }

    public override ModuleType Type { get; } = ModuleType.Scroll;

    public ScrollModuleVM() : base()
    {
        castedModuleData = (ScrollModule)ModuleData;
    }

    public ScrollModuleVM(ScrollModule moduleData) : base(moduleData)
    {
        castedModuleData = moduleData;
    }

    private readonly ScrollModule castedModuleData;
}
