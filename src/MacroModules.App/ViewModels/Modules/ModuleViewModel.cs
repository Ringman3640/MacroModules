using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.ViewModels.Modules;

public abstract partial class ModuleViewModel : BoardElementViewModel
{
    public override abstract FrameworkElement ViewRef { get; }

    public abstract ModuleType Type { get; }

    public Module ModuleModel { get; protected set; }

    public ObservableCollection<ExitPortViewModel> ExitPorts { get; private set; } = new();

    public ModuleViewModel()
    {
        ModuleModel = ModuleFactory.Create(Type);
        ViewRef.DataContext = this;
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
