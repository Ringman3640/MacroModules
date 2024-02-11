﻿using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.Behaviors;
using MacroModules.App.ViewModels.Events;
using System.Windows;

namespace MacroModules.App.ViewModels;

public abstract partial class BoardElementVM : MouseAwareVM, IDimensionsAware, INotifyElementMoved
{
    public Point Position
    {
        get { return _position; }
        set
        {
            Point nextPos = value - offsetFromMouse;
            if (nextPos != _position)
            {
                _position = nextPos;
                OnPropertyChanged();
                ElementMoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    private Point _position;

    public Point CenterPosition
    {
        get { return _position + new Vector(Dimensions.Width / 2, Dimensions.Height / 2); }
    }

    [ObservableProperty]
    private Size _dimensions;

    public event ElementMovedHandler? ElementMoved;

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
