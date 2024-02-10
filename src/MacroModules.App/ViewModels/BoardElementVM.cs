using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace MacroModules.App.ViewModels;

public abstract partial class BoardElementVM : MouseAwareVM
{
    public Point Position
    {
        get { return _position; }
        set { SetProperty(ref _position, value - offsetFromMouse); }
    }
    private Point _position;

    [ObservableProperty]
    private Size _dimensions;

    public void LockMouseOffset()
    {
        offsetFromMouse = (Vector)MousePosition;
    }

    public void UnlockMouseOffset()
    {
        offsetFromMouse = new(0, 0);
    }

    public void MoveWithMouse()
    {
        Position += (Vector)MousePosition;
    }

    protected Vector offsetFromMouse;

}
