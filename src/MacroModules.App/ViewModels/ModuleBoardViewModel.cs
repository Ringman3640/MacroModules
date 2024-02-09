using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.ViewModels.Modules;
using MacroModules.App.Views;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MacroModules.App.ViewModels;

public partial class ModuleBoardViewModel : ObservableObject
{
    public ModuleBoardView ViewRef { get; private set; }

    [ObservableProperty]
    private Visibility _selectBoxVisibility = Visibility.Hidden;

    [ObservableProperty]
    private Rect _selectBoxRegion;

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
            _moduleBoardPosX = value - canvasOffsetFromMouse.X * canvasScaleTransform.ScaleX;
            OnPropertyChanged();
        }
    }
    private double _moduleBoardPosX = 0;

    public double ModuleBoardPosY
    {
        get { return _moduleBoardPosY; }
        set
        {
            _moduleBoardPosY = value - canvasOffsetFromMouse.Y * canvasScaleTransform.ScaleX;
            OnPropertyChanged();
        }
    }
    private double _moduleBoardPosY = 0;

    public ModuleBoardViewModel(ModuleBoardView viewRef)
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

    public void AddElement(BoardElementViewModel element)
    {
        if (containedElements.Contains(element))
        {
            return;
        }

        containedElements.Add(element);
        ViewRef.GetBoardCanvas().Children.Add(element.ViewRef);
    }

    public void RemoveElement(BoardElementViewModel element)
    {
        if (containedElements.Remove(element))
        {
            ViewRef.GetBoardCanvas().Children.Remove(element.ViewRef);
        }
    }

    public bool ContainsElement(BoardElementViewModel element)
    {
        return containedElements.Contains(element);
    }

    public void ZoomIn()
    {
        PerformZoom(1.5);
    }

    public void ZoomOut()
    {
        PerformZoom(0.75);
    }

    private void PerformZoom(double zoomValue)
    {
        LockCanvasToMouse();

        double scaleValue = canvasScaleTransform.ScaleX * zoomValue;
        if (scaleValue < 0.1 || scaleValue > 5)
        {
            return;
        }

        canvasScaleTransform.ScaleX = scaleValue;
        canvasScaleTransform.ScaleY = scaleValue;
        ViewRef.GetBoardCanvas().RenderTransform = canvasScaleTransform;

        MoveCanvasWithMouse();
    }

    public void ResetZoom()
    {
        canvasScaleTransform.ScaleX = 1;
        canvasScaleTransform.ScaleY = 1;
        ViewRef.GetBoardCanvas().RenderTransform = canvasScaleTransform;
    }

    public void Select(BoardElementViewModel element)
    {
        selectedElements.Add(element);
    }

    public void Unselect(BoardElementViewModel element)
    {
        selectedElements.Remove(element);
    }

    public void UnselectAll()
    {
        selectedElements.Clear();
    }

    public bool IsSelected(BoardElementViewModel element)
    {
        return selectedElements.Contains(element);
    }

    public void LockSelectedToMouse()
    {
        foreach (var element in selectedElements)
        {
            element.LockToMouse();
        }
    }

    public void MoveSelectedWithMouse()
    {
        Point mousePos = Mouse.GetPosition(ViewRef.GetBoardCanvas());

        foreach (var element in selectedElements)
        {
            element.PositionX = mousePos.X;
            element.PositionY = mousePos.Y;
        }
    }

    public void LockCanvasToMouse()
    {
        canvasOffsetFromMouse = Mouse.GetPosition(ViewRef.GetBoardCanvas());
    }

    public void MoveCanvasWithMouse()
    {
        Point mousePos = Mouse.GetPosition(ViewRef.GetContainerCanvas());
        ModuleBoardPosX = mousePos.X;
        ModuleBoardPosY = mousePos.Y;
    }

    public void LockSelectBoxPivotToMouse()
    {
        selectBoxPivot = Mouse.GetPosition(ViewRef.GetContainerCanvas());
    }

    public void StartSelectBox()
    {
        SelectBoxVisibility = Visibility.Visible;
        MoveSelectBoxWithMouse();
    }

    public void MoveSelectBoxWithMouse()
    {
        Point mousePos = Mouse.GetPosition(ViewRef.GetContainerCanvas());
        SelectBoxRegion = new(mousePos, selectBoxPivot);
    }

    public void ConfirmSelectBox()
    {
        double boardScale = canvasScaleTransform.ScaleX;
        foreach (var element in containedElements)
        {
            double absolutePosX = (element.PositionX * boardScale) + ModuleBoardPosX;
            double absolutePosY = (element.PositionY * boardScale) + ModuleBoardPosY;
            double scaledWidth = element.Width * boardScale;
            double scaledHeight = element.Height * boardScale;

            Rect elementBounds = new(absolutePosX, absolutePosY, scaledWidth, scaledHeight);
            if (elementBounds.IntersectsWith(SelectBoxRegion))
            {
                selectedElements.Add(element);
            }
        }

        SelectBoxVisibility = Visibility.Hidden;
    }

    private HashSet<BoardElementViewModel> containedElements = new();
    private HashSet<BoardElementViewModel> selectedElements = new();

    private Point canvasOffsetFromMouse;
    private Point selectBoxPivot;
    private readonly ScaleTransform canvasScaleTransform = new();

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
