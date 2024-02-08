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

    public void AddModule(ModuleViewModel module)
    {
        if (containedModules.Contains(module))
        {
            return;
        }

        containedModules.Add(module);
        ViewRef.GetBoardCanvas().Children.Add(module.ViewRef);
    }

    public void RemoveModule(ModuleViewModel module)
    {
        if (containedModules.Remove(module))
        {
            ViewRef.GetBoardCanvas().Children.Remove(module.ViewRef);
        }
    }

    public bool ContainsModule(ModuleViewModel module)
    {
        return containedModules.Contains(module);
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

    public void Select(ModuleViewModel module)
    {
        selectedModules.Add(module);
    }

    public void Unselect(ModuleViewModel module)
    {
        selectedModules.Remove(module);
    }

    public void UnselectAll()
    {
        selectedModules.Clear();
    }

    public bool IsSelected(ModuleViewModel module)
    {
        return selectedModules.Contains(module);
    }

    public void LockSelectedToMouse()
    {
        foreach (var moduleVM in selectedModules)
        {
            moduleVM.LockToMouse();
        }
    }

    public void MoveSelectedWithMouse()
    {
        Point mousePos = Mouse.GetPosition(ViewRef.GetBoardCanvas());

        foreach (var moduleVM in selectedModules)
        {
            moduleVM.PosX = mousePos.X;
            moduleVM.PosY = mousePos.Y;
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
        foreach (var module in containedModules)
        {
            double absolutePosX = (module.PosX * boardScale) + ModuleBoardPosX;
            double absolutePosY = (module.PosY * boardScale) + ModuleBoardPosY;
            double scaledWidth = module.ViewRef.ActualWidth * boardScale;
            double scaledHeight = module.ViewRef.ActualHeight * boardScale;

            Rect moduleBounds = new(absolutePosX, absolutePosY, scaledWidth, scaledHeight);
            if (moduleBounds.IntersectsWith(SelectBoxRegion))
            {
                selectedModules.Add(module);
            }
        }

        SelectBoxVisibility = Visibility.Hidden;
    }

    private HashSet<ModuleViewModel> containedModules = new();
    private HashSet<ModuleViewModel> selectedModules = new();

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
