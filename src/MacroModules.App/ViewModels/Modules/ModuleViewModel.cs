using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.Views.Modules;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.ViewModels.Modules;

public partial class ModuleViewModel : ObservableObject
{
    public ModuleView ViewRef { get; private set; }

    public double PosX
    {
        get { return _posX; }
        set
        {
            _posX = value - offsetFromMouse.X;
            OnPropertyChanged();
        }
    }
    private double _posX = 0;

    public double PosY
    {
        get { return _posY; }
        set
        {
            _posY = value - offsetFromMouse.Y;
            OnPropertyChanged();
        }
    }
    private double _posY = 0;

    public ModuleViewModel(ModuleView viewRef)
    {
        ViewRef = viewRef;
    }

    public void LockToMouse()
    {
        offsetFromMouse = Mouse.GetPosition(ViewRef);
    }

    public void UnlockFromMouse()
    {
        offsetFromMouse = new(0, 0);
    }

    private Point offsetFromMouse;

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
