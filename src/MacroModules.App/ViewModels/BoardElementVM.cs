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

    public Point CenterPosition
    {
        get { return _position + new Vector(Dimensions.Width / 2, Dimensions.Height / 2); }
    }

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

    public void CenterToPoint(Point point)
    {
        _position = point - new Vector(Dimensions.Width / 2, Dimensions.Height / 2);
        OnPropertyChanged(nameof(Position));
    }

    public void CenterToMouse()
    {
        _position += (Vector)MousePosition - new Vector(Dimensions.Width / 2, Dimensions.Height / 2);
        OnPropertyChanged(nameof(Position));
    }

    protected Vector offsetFromMouse;
}
