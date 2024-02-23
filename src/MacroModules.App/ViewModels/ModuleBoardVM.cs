using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.Managers.Commits;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.BoardElements;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace MacroModules.App.ViewModels;

public partial class ModuleBoardVM : MouseAwareVM, ICommittable
{
    public WorkspaceVM Workspace { get; private set; }

    [ObservableProperty]
    private FrameworkElement? _containerViewRef;

    [ObservableProperty]
    private ObservableCollection<BoardElementVM> _elements = [];

    [ObservableProperty]
    private ObservableCollection<ExitPortVM> _wires = [];

    [ObservableProperty]
    private ScaleTransform _boardTransform = new();

    public SelectBoxVM SelectBox { get; private set; }

    public StartupEntryModuleVM? StartupEntryModule { get; private set; }

    public List<TriggerEntryModuleVM> TriggerModulesList
    {
        get { return triggerModules.ToList(); }
    }

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

    public Point BoardMousePosition
    {
        get
        {
            Point unscaledPosition = MousePosition - (Vector)BoardPosition;
            return new Point(unscaledPosition.X / BoardScale, unscaledPosition.Y / BoardScale);
        }
    }

    public bool PerformingCommitAction { get; set; }

    public ModuleBoardVM(WorkspaceVM workspace)
    {
        Workspace = workspace;
        SelectBox = new(this);
    }

    public void Focus()
    {
        if (ContainerViewRef is Canvas containerCanvas)
        {
            containerCanvas.Focus();
        }
    }

    public void AddElement(BoardElementVM element)
    {
        if (!AddElementHelper(element))
        {
            return;
        }

        element.Initialize(Workspace);
        if (element is ModuleVM module)
        {
            foreach (var exitPort in module.ExitPorts)
            {
                Wires.Add(exitPort);
            }
        }
        if (!PerformingCommitAction)
        {
            Workspace.CommitManager.PushToSeries(new ElementAddedCommit(this, element));
        }
    }

    public void AddElements(List<BoardElementVM> elements)
    {
        Dictionary<Guid, ModuleVM> moduleIdMap = [];
        foreach (var element in elements.ToList())
        {
            if (!AddElementHelper(element))
            {
                elements.Remove(element);
            }
            else if (element is ModuleVM module)
            {
                moduleIdMap.Add(module.ModuleData.Id, module);

                if (!PerformingCommitAction)
                {
                    Workspace.CommitManager.PushToSeries(new ElementAddedCommit(this, element));
                }
            }
        }
        
        foreach (var element in elements)
        {
            if (element is ModuleVM module)
            {
                module.Initialize(Workspace, moduleIdMap);
                foreach (var exitPort in module.ExitPorts)
                {
                    Wires.Add(exitPort);
                }
            }
            else
            {
                element.Initialize(Workspace);
            }
        }
        if (!PerformingCommitAction)
        {
            Workspace.CommitManager.PushToSeries(new ElementGroupAddedCommit(this, elements));
        }
    }

    public void RemoveElement(BoardElementVM element)
    {
        if (!Elements.Remove(element))
        {
            return;
        }
        if (element is ModuleVM module)
        {
            foreach (var exitPort in module.ExitPorts)
            {
                Wires.Remove(exitPort);
            }
        }
        if (element is StartupEntryModuleVM)
        {
            StartupEntryModule = null;
        }
        if (element is TriggerEntryModuleVM triggerEntryModule)
        {
            triggerModules.Remove(triggerEntryModule);
        }

        if (!PerformingCommitAction)
        {
            Workspace.CommitManager.PushToSeries(new ElementRemovedCommit(this, element));
        }
        element.IndicateRemoved();
    }

    public bool ContainsElement(BoardElementVM element)
    {
        return Elements.Contains(element);
    }

    public void ZoomIn()
    {
        SetZoom(BoardScale * 1.5);
    }

    public void ZoomOut()
    {
        SetZoom(BoardScale / 1.5);
    }

    public void ResetZoom()
    {
        SetZoom(1);
    }

    private void SetZoom(double zoomValue)
    {
        Point startBoardPos = BoardMousePosition;
        BoardScale = zoomValue;
        UnlockCanvasFromMouse();
        Vector movedOffset = ((Vector)startBoardPos - (Vector)BoardMousePosition) * BoardScale;
        BoardPosition -= movedOffset;
    }

    public void LockCanvasToMouse()
    {
        boardOffsetFromMouse = MousePosition - (Vector)BoardPosition;
    }

    public void UnlockCanvasFromMouse()
    {
        boardOffsetFromMouse = new(0, 0);
    }

    public void MoveCanvasWithMouse()
    {
        BoardPosition = MousePosition;
    }

    public Point MapContainerPositionToBoard(Point position)
    {
        Point unscaledPosition = position - (Vector)BoardPosition;
        return new Point(unscaledPosition.X / BoardScale, unscaledPosition.Y / BoardScale);
    }

    private readonly HashSet<TriggerEntryModuleVM> triggerModules = new();
    private Point boardOffsetFromMouse;

    private bool AddElementHelper(BoardElementVM element)
    {
        if (Elements.Contains(element))
        {
            return false;
        }

        if (element is StartupEntryModuleVM startupEntryModule)
        {
            if (StartupEntryModule != null)
            {
                return false;
            }

            StartupEntryModule = startupEntryModule;
        }
        else if (element is TriggerEntryModuleVM triggerEntryModule)
        {
            if (triggerEntryModule.Trigger != null)
            {
                foreach (var triggerModule in triggerModules)
                {
                    if (triggerModule.Trigger == null)
                    {
                        continue;
                    }
                    if (triggerModule.Trigger.Equals(triggerEntryModule.Trigger))
                    {
                        return false;
                    }
                }
            }
            triggerModules.Add(triggerEntryModule);
        }

        Elements.Add(element);
        if (!PerformingCommitAction)
        {
            SelectBox.Select(element);
        }
        return true;
    }

    [RelayCommand]
    private void Board_LeftMouseDown(RoutedEventArgs e)
    {
        ((UIElement)e.OriginalSource).Focus();
        Workspace.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_RightMouseDown(RoutedEventArgs e)
    {
        ((UIElement)e.OriginalSource).Focus();
        Workspace.MouseInteraction.ProcessMouseRightDown(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_MouseMove(RoutedEventArgs e)
    {
        Workspace.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_LeftMouseUp(RoutedEventArgs e)
    {
        Workspace.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Board);
        e.Handled = true;
    }

    [RelayCommand]
    private void Board_RightMouseUp(RoutedEventArgs e)
    {
        Workspace.MouseInteraction.ProcessMouseRightUp(this, MouseInteractionItemType.Board);
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

    [RelayCommand]
    private void Board_RemoveSelected()
    {
        foreach (var element in SelectBox.SelectedElementsList)
        {
            RemoveElement(element);
        }
        SelectBox.UnselectAll();
        Workspace.CommitManager.CommitSeries();
    }

    [RelayCommand]
    private void Board_Cut()
    {
        Board_Copy();
        Board_RemoveSelected();
    }

    [RelayCommand]
    private void Board_Copy()
    {
        if (SelectBox.SelectedElements.Count == 0)
        {
            return;
        }

        List<BoardElement> elements = [];
        foreach (var elementVM in SelectBox.SelectedElements)
        {
            elements.Add(elementVM.ElementData);
        }

        Clipboard.SetData("SerializedBoardElements", JsonSerializer.Serialize(elements));
    }

    [RelayCommand]
    private void Board_Paste()
    {
        if (Clipboard.GetData("SerializedBoardElements") is not string stringData)
        {
            return;
        }

        // Get elements saved in clipboard data
        List<BoardElement>? elements;
        try
        {
            elements = JsonSerializer.Deserialize<List<BoardElement>>(stringData);
        }
        catch
        {
            return;
        }
        if (elements == null || elements.Count == 0)
        {
            return;
        }

        // Generate new IDs for Modules and map old IDs to corresponding Module
        Dictionary<Guid, Module> moduleMap = [];
        foreach (var element in elements)
        {
            if (element is Module module)
            {
                moduleMap.Add(module.Id, module);
                module.GenerateNewId();
            }
        }

        // Remap exit ports for modules using the created module map from above
        foreach (var element in elements)
        {
            if (element is not Module module)
            {
                continue;
            }

            foreach (var exitPort in module.ExitPorts)
            {
                if (moduleMap.TryGetValue(exitPort.DestinationId, out Module? newDestination))
                {
                    exitPort.Destination = newDestination;
                }
            }
        }

        // Wrap each element in a corresponding viewmodel
        List<BoardElementVM> elementVMs = [];
        foreach (var elementData in elements)
        {
            if (elementData is Module module)
            {
                ModuleVM moduleVM = ModuleVMFactory.Create(module);
                moduleVM.SetStartingPosition(moduleVM.ActualPosition);
                elementVMs.Add(moduleVM);
            }
            else
            {
                // TODO: create label element VM
            }
        }

        // Add elements to the board
        SelectBox.UnselectAll();
        AddElements(elementVMs);
        Workspace.CommitManager.CommitSeries();

        // Get the group center offset from the mouse
        Point startingPoint = elementVMs[0].CenterVisualPosition;
        Rect boundingRect = new(startingPoint, Size.Empty);
        foreach (var elementVM in elementVMs)
        {
            boundingRect.Union(elementVM.CenterVisualPosition);
        }
        Point boundingRectCenter = boundingRect.TopLeft;
        boundingRectCenter.X += boundingRect.Width / 2;
        boundingRectCenter.Y += boundingRect.Height / 2;
        Vector offset = (Vector)BoardMousePosition - (Vector)boundingRectCenter;

        // Move elements by offset to center to mouse
        foreach (var elementVM in elementVMs)
        {
            elementVM.SetStartingPosition(elementVM.ActualPosition + offset);
        }
    }

    [RelayCommand]
    private void Board_Dropped(RoutedEventArgs e)
    {
        DragEventArgs dragEvent = (DragEventArgs)e;
        object data = dragEvent.Data.GetData(typeof(ModuleType));
        if (data is ModuleType type)
        {
            ModuleVM module = ModuleVMFactory.Create(type);
            Point containerPosition = dragEvent.GetPosition((FrameworkElement)dragEvent.Source);
            module.SetCenterStartingPosition(MapContainerPositionToBoard(containerPosition));
            SelectBox.UnselectAll();
            AddElement(module);
            Workspace.CommitManager.CommitSeries();
        }
    }
}
