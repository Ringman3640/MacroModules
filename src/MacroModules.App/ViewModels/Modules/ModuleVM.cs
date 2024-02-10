using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MacroModules.App.ViewModels.Modules;

public partial class ModuleVM : BoardElementVM
{
    public ModuleType Type { get; }

    //public Module ModuleModel { get; protected set; }

    public ObservableCollection<ExitPortVM> ExitPorts { get; private set; } = new();

    public ModuleVM()
    {
        //ModuleModel = ModuleFactory.Create(Type);
    }

    [RelayCommand]
    private void Body_LeftMouseDown(MouseButtonEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseLeftDown(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_LeftMouseUp(MouseButtonEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseLeftUp(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_MouseMove(MouseEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseMove(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }
}
