using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MacroModules.App.ViewModels.Modules;

public partial class ModuleVM : BoardElementVM
{
    public ModuleType Type { get; }

    public Module ModuleData { get; protected set; }

    public ObservableCollection<ExitPortVM> ExitPorts { get; private set; } = new();

    public ModuleVM()
    {
        //ModuleModel = ModuleFactory.Create(Type);
        ExitPorts.Add(new ExitPortVM(new(), this)); // TODO: remove later, FOR TESTING ONLY
    }

    [RelayCommand]
    private void Body_LeftMouseDown(MouseButtonEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_LeftMouseUp(MouseButtonEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_MouseMove(MouseEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }
}
