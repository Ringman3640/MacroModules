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

    public MouseInteractionManager MouseInteraction { get; private set; }

    public ModuleBoardVM ModuleBoard { get; private set; }

    public WorkspaceVM(WorkspaceView viewRef)
    {
        ViewReference = viewRef;
        MouseInteraction = new(this);
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
        MouseInteraction.ProcessMouseRightUp(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_LeftMouseUp(RoutedEventArgs e)
    {
        MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_MouseMove(RoutedEventArgs e)
    {
        MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.None);
        e.Handled = true;
    }
}
