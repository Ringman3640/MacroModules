using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.ViewModels;

public abstract partial class BoardElementViewModel : ObservableObject
{
    public abstract FrameworkElement ViewRef { get; }

    public double PositionX
    {
        get { return _positionX; }
        set
        {
            _positionX = value - offsetFromMouse.X;
            OnPropertyChanged();
        }
    }
    private double _positionX;

    public double PositionY
    {
        get { return _positionY; }
        set
        {
            _positionY = value - offsetFromMouse.Y;
            OnPropertyChanged();
        }
    }
    private double _positionY;

    public double Width
    {
        get { return ViewRef.ActualWidth; }
    }

    public double Height
    {
        get { return ViewRef.ActualHeight; }
    }

    public void LockToMouse()
    {
        offsetFromMouse = (Vector)Mouse.GetPosition(ViewRef);
    }

    public void UnlockFromMouse()
    {
        offsetFromMouse = new(0, 0);
    }

    protected Vector offsetFromMouse;
}
