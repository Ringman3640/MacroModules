using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MacroModules.App.ViewModels;

public partial class ModuleBoardVM : MouseAwareVM
{
    public ModuleBoardView ViewRef { get; private set; }

    [ObservableProperty]
    private ObservableCollection<BoardElementVM> _elements = new();

    [ObservableProperty]
    private ScaleTransform _boardTransform = new();

    [ObservableProperty]
    private Visibility _selectBoxVisibility = Visibility.Hidden;

    [ObservableProperty]
    private Rect _selectBoxRegion;

    public double BoardScale
    {
        get { return BoardTransform.ScaleX; }
        set
        {
            if (value >= 0.1 && value <= 5)
            {
                BoardTransform.ScaleX = value;
                BoardTransform.ScaleY = value;
                OnPropertyChanged(nameof(BoardTransform));
            }
        }
    }

    public Point BoardPosition
    {
        get { return _boardPosition; }
        set { SetProperty(ref _boardPosition, value - (Vector)boardOffsetFromMouse); }
    }
    private Point _boardPosition;

    public double MousePosX
    {
        get { return mousePosX; }
        set { mousePosX = value; }
    }
    private double mousePosX;

    public double MousePosY
    {
        get { return mousePosY; }
        set { mousePosY = value; }
    }
    private double mousePosY;

    public double ModuleBoardPosX
    {
        get { return _moduleBoardPosX; }
        set
        {
            _moduleBoardPosX = value - boardOffsetFromMouse.X * BoardScale;
            OnPropertyChanged();
        }
    }
    private double _moduleBoardPosX = 0;

    public double ModuleBoardPosY
    {
        get { return _moduleBoardPosY; }
        set
        {
            _moduleBoardPosY = value - boardOffsetFromMouse.Y * BoardScale;
            OnPropertyChanged();
        }
    }
    private double _moduleBoardPosY = 0;

    public ModuleBoardVM(ModuleBoardView viewRef)
    {
        ViewRef = viewRef;
        MouseInteractionManager.Instance.RegisterModuleBoard(this);
    }

    public void CaptureMouse()
    {
        Mouse.Capture(null);
        Mouse.Capture(ViewRef.GetContainerCanvas(), CaptureMode.SubTree);
    }

    public void UncaptureMouse()
    {
        Mouse.Capture(null);
    }

    public void AddElement(BoardElementVM element)
    {
        if (Elements.Contains(element))
        {
            return;
        }

        Elements.Add(element);
    }

    public void RemoveElement(BoardElementVM element)
    {
        Elements.Remove(element);
    }

    public bool ContainsElement(BoardElementVM element)
    {
        return Elements.Contains(element);
    }

    public void ZoomIn()
    {
        PerformZoom(1.5);
    }

    public void ZoomOut()
    {
        PerformZoom(0.75);
    }

    private void PerformZoom(double zoomFactor)
    {
        LockCanvasToMouse();
        BoardScale *= zoomFactor;
        MoveCanvasWithMouse();
    }

    public void ResetZoom()
    {
        LockCanvasToMouse();
        BoardScale = 1;
        MoveCanvasWithMouse();
    }

    public void Select(BoardElementVM element)
    {
        selectedElements.Add(element);
    }

    public void Unselect(BoardElementVM element)
    {
        selectedElements.Remove(element);
    }

    public void UnselectAll()
    {
        selectedElements.Clear();
    }

    public bool IsSelected(BoardElementVM element)
    {
        return selectedElements.Contains(element);
    }

    public void LockSelectedToMouse()
    {
        foreach (var element in selectedElements)
        {
            element.LockMouseOffset();
        }
    }

    public void MoveSelectedWithMouse()
    {
        foreach (var element in selectedElements)
        {
            element.MoveWithMouse();
        }
    }

    public void LockCanvasToMouse()
    {
        boardOffsetFromMouse = MousePosition - (Vector)BoardPosition;
    }

    public void MoveCanvasWithMouse()
    {
        BoardPosition = MousePosition;
    }

    public void LockSelectBoxPivotToMouse()
    {
        selectBoxPivot = MousePosition;
    }

    public void StartSelectBox()
    {
        SelectBoxVisibility = Visibility.Visible;
        MoveSelectBoxWithMouse();
    }

    public void MoveSelectBoxWithMouse()
    {
        SelectBoxRegion = new(MousePosition, selectBoxPivot);
    }

    public void ConfirmSelectBox()
    {
        foreach (var element in Elements)
        {
            double absolutePosX = (element.Position.X * BoardScale) + BoardPosition.X;
            double absolutePosY = (element.Position.Y * BoardScale) + BoardPosition.Y;
            double scaledWidth = element.Dimensions.Width * BoardScale;
            double scaledHeight = element.Dimensions.Height * BoardScale;

            Rect elementBounds = new(absolutePosX, absolutePosY, scaledWidth, scaledHeight);
            if (elementBounds.IntersectsWith(SelectBoxRegion))
            {
                selectedElements.Add(element);
            }
        }

        SelectBoxVisibility = Visibility.Hidden;
    }

    private HashSet<BoardElementVM> selectedElements = new();

    private Point boardOffsetFromMouse;
    private Point selectBoxPivot;

    [RelayCommand]
    private void Board_LeftMouseDown(MouseButtonEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseLeftDown(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_RightMouseDown(MouseButtonEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseRightDown(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_MouseMove(MouseEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseMove(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_LeftMouseUp(MouseButtonEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseLeftUp(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_RightMouseUp(MouseButtonEventArgs e)
    {
        MouseInteractionManager.Instance.ProcessMouseRightUp(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_MouseWheel(MouseWheelEventArgs e)
    {
        if (!Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            return;
        }

        if (e.Delta > 0)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
        }
    }
}
