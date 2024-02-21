using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.Behaviors;
using MacroModules.App.Managers;
using MacroModules.App.Managers.Commits;
using MacroModules.App.ViewModels.Events;
using MacroModules.Model.BoardElements;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MacroModules.App.ViewModels;

public abstract partial class BoardElementVM : MouseAwareVM, IDimensionsAware, INotifyElementMoved, INotifyElementRemoved, ICommittable
{
    public WorkspaceVM? Workspace { get; private set; } = null;

    public abstract BoardElement ElementData { get; protected set; }

    [ObservableProperty]
    private bool _selected = false;

    [ObservableProperty]
    private bool _hovered = false;

    public Point VisualPosition
    {
        get { return _visualPosition; }
        set
        {
            Point nextPos = value - offsetFromMouse;
            if (nextPos != _visualPosition)
            {
                _visualPosition = nextPos;
                OnPropertyChanged();
                ElementMoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    private Point _visualPosition;

    public Point CenterVisualPosition
    {
        get { return _visualPosition + new Vector(Dimensions.Width / 2, Dimensions.Height / 2); }
    }

    public Point ActualPosition
    {
        get
        {
            _actualPosition = new(ElementData.PositionX, ElementData.PositionY);
            return _actualPosition;
        }
        set
        {
            if (SetAndCommitProperty(ref _actualPosition, value))
            {
                ElementData.PositionX = value.X;
                ElementData.PositionY = value.Y;
                _visualPosition = _actualPosition;
                OnPropertyChanged(nameof(VisualPosition));
                ElementMoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public Point _actualPosition;

    public bool PerformingCommitAction { get; set; }

    [ObservableProperty]
    private Size _dimensions = new(80, 80);

    public event ElementMovedHandler? ElementMoved;
    public event ElementRemovedHandler? ElementRemoved;

    public BoardElementVM() { }

    public BoardElementVM(BoardElement element)
    {
        ElementData = element;
    }

    public virtual void Initialize(WorkspaceVM workspace)
    {
        Workspace = workspace;
    }

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
        VisualPosition += (Vector)MousePosition;
    }

    public void CenterToPoint(Point point)
    {
        _visualPosition = point - new Vector(Dimensions.Width / 2, Dimensions.Height / 2);
        OnPropertyChanged(nameof(VisualPosition));
    }

    public void CenterToMouse()
    {
        _visualPosition += (Vector)MousePosition - new Vector(Dimensions.Width / 2, Dimensions.Height / 2);
        OnPropertyChanged(nameof(VisualPosition));
    }

    public void SetStartingPosition(Point position)
    {
        _visualPosition = position;
        _actualPosition = position;
        OnPropertyChanged(nameof(VisualPosition));
    }

    public void SetCenterStartingPosition(Point position)
    {
        position -= (Vector)Dimensions / 2;

        _visualPosition = position;
        _actualPosition = position;
        OnPropertyChanged(nameof(VisualPosition));
    }

    public void IndicateRemoved()
    {
        ElementRemoved?.Invoke(this, EventArgs.Empty);
    }

    protected Vector offsetFromMouse;

    protected bool SetAndCommitProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue))
        {
            return false;
        }

        OnPropertyChanging(propertyName);

        if (!PerformingCommitAction)
        {
            Workspace?.CommitManager.PushToSeries(new PropertyCommit(this, propertyName!, field, newValue));
        }

        field = newValue;

        OnPropertyChanged(propertyName);

        return true;
    }

    protected void CommitPropertyChange<T>(T initialValue, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!PerformingCommitAction)
        {
            Workspace?.CommitManager.PushToSeries(new PropertyCommit(this, propertyName!, initialValue, newValue));
        }
    }

    protected void FullCommitPropertyChange<T>(T initialValue, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!PerformingCommitAction)
        {
            Workspace?.CommitManager.PushToSeries(new PropertyCommit(this, propertyName!, initialValue, newValue));
            Workspace?.CommitManager.CommitSeries();
        }
    }
}
