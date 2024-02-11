using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.Views;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.ViewModels;

public partial class WorkspaceVM : ObservableObject
{
    public WorkspaceView ViewReference { get; private set; }

    public ModuleBoardVM ModuleBoard { get; private set; }

    public WorkspaceVM(WorkspaceView viewRef)
    {
        ViewReference = viewRef;
        ModuleBoard = new(this);
    }

    public void CaptureMouse()
    {
        Mouse.Capture(null);
        Mouse.Capture(ViewReference, CaptureMode.SubTree);
    }

    public void UncaptureMouse()
    {
        Mouse.Capture(null);
    }

    [RelayCommand]
    private void Workspace_RightMouseUp(RoutedEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseRightUp(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_LeftMouseUp(RoutedEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseLeftUp(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_MouseMove(RoutedEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseMove(this, MouseInteractionItemType.None);
        e.Handled = true;
    }
}
