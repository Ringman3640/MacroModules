using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Windows;

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
    private void Body_LeftMouseDown(RoutedEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_LeftMouseUp(RoutedEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_MouseMove(RoutedEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }
}
