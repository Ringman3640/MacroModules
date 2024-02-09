using MacroModules.App.ViewModels;
using MacroModules.App.ViewModels.Modules;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.Managers;

public enum MouseInteractionItemType
{
    None,
    Module,
    Wire,
    Board
}

public sealed class MouseInteractionManager
{
    public static MouseInteractionManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MouseInteractionManager();
                    }
                }
            }
            return instance;
        }
    }

    public ModuleBoardViewModel ModuleBoardVM
    {
        get
        {
            if (_moduleBoardVM == null)
            {
                throw new InvalidOperationException("Attempting to get ModuleBoardVM before it was set.");
            }
            return _moduleBoardVM!;
        }
        set { _moduleBoardVM = value; }
    }
    private ModuleBoardViewModel? _moduleBoardVM = null;

    public void RegisterModuleBoard(ModuleBoardViewModel moduleBoard)
    {
        _moduleBoardVM = moduleBoard;
    }

    public void ProcessMouseLeftDown(object sender, MouseInteractionItemType senderType)
    {
        if (interactState != InteractionState.Idle)
        {
            return;
        }

        ModuleBoardVM.CaptureMouse();
        startInteractItem = sender;
        startInteractItemType = senderType;
        startInteractMousePos = Mouse.GetPosition(null);

        switch (senderType)
        {
            case MouseInteractionItemType.Module:
                var module = (ModuleViewModel)sender;
                if (ModuleBoardVM.IsSelected(module))
                {
                    interactState = InteractionState.StartLeftHoldingSelected;
                }
                else
                {
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ModuleBoardVM.UnselectAll();
                    }
                    interactState = InteractionState.StartLeftHoldingModule;
                }
                break;

            case MouseInteractionItemType.Wire:
                // TODO
                break;

            case MouseInteractionItemType.Board:
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoardVM.UnselectAll();
                }
                ModuleBoardVM.LockSelectBoxPivotToMouse();
                interactState = InteractionState.StartLeftHoldingBoard;
                break;
        }
    }

    public void ProcessMouseRightDown(object sender, MouseInteractionItemType senderType)
    {
        if (interactState != InteractionState.Idle)
        {
            return;
        }

        ModuleBoardVM.CaptureMouse();
        startInteractItem = sender;
        startInteractItemType = senderType;
        startInteractMousePos = Mouse.GetPosition(null);

        switch (senderType)
        {
            case MouseInteractionItemType.Module:
                if (sender is ModuleViewModel module)
                {
                    if (ModuleBoardVM.IsSelected(module))
                    {
                        interactState = InteractionState.StartRightHoldingSelected;
                    }
                    else
                    {
                        interactState = InteractionState.StartRightHoldingModule;
                    }
                }
                break;

            case MouseInteractionItemType.Wire:
                // TODO
                break;

            case MouseInteractionItemType.Board:
                interactState = InteractionState.StartRightHoldingBoard;
                break;
        }
    }

    public void ProcessMouseMove(object sender, MouseInteractionItemType senderType)
    {
        switch (interactState)
        {
            case InteractionState.StartLeftHoldingSelected:
                ModuleBoardVM.LockSelectedToMouse();
                interactState = InteractionState.DraggingSelection;
                goto case InteractionState.DraggingSelection;

            case InteractionState.StartLeftHoldingModule:
                if (startInteractItem is ModuleViewModel module)
                {
                    ModuleBoardVM.Select(module);
                    ModuleBoardVM.LockSelectedToMouse();
                    interactState = InteractionState.DraggingSelection;
                }
                else
                {
                    interactState = InteractionState.Idle;
                    break;
                }
                goto case InteractionState.DraggingSelection;

            case InteractionState.StartLeftHoldingBoard:
                if (MouseMovedPastThreshold())
                {
                    ModuleBoardVM.StartSelectBox();
                    interactState = InteractionState.DraggingSelectBox;
                    goto case InteractionState.DraggingSelectBox;
                }
                break;

            case InteractionState.StartRightHoldingSelected:
                goto case InteractionState.StartRightHoldingBoard;

            case InteractionState.StartRightHoldingModule:
                goto case InteractionState.StartRightHoldingBoard;

            case InteractionState.StartRightHoldingBoard:
                ModuleBoardVM.LockCanvasToMouse();
                interactState = InteractionState.DraggingCanvas;
                break;

            case InteractionState.DraggingSelection:
                ModuleBoardVM.MoveSelectedWithMouse();
                break;

            case InteractionState.DraggingSelectBox:
                ModuleBoardVM.MoveSelectBoxWithMouse();
                break;

            case InteractionState.DraggingCanvas:
                ModuleBoardVM.MoveCanvasWithMouse();
                break;
        }
    }

    public void ProcessMouseLeftUp(object sender, MouseInteractionItemType senderType)
    {
        switch (interactState)
        {
            case InteractionState.StartLeftHoldingSelected:
            {
                if (startInteractItem is not ModuleViewModel module)
                {
                    break;
                }
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoardVM.Unselect(module);
                }
                else
                {
                    ModuleBoardVM.UnselectAll();
                    ModuleBoardVM.Select(module);
                    // TODO: show single module on properties panel
                }
                break;
            }
                
            case InteractionState.StartLeftHoldingModule:
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoardVM.UnselectAll();
                }
                if (startInteractItem is ModuleViewModel module)
                {
                    ModuleBoardVM.Select(module);
                    // TODO: show module on properties if its the only one focused
                }
                break;
            }

            case InteractionState.StartLeftHoldingBoard:
                ModuleBoardVM.UnselectAll();
                break;

            case InteractionState.DraggingSelection:
                // TODO: Commit visual drag to model (which is also TODO)
                break;

            case InteractionState.DraggingSelectBox:
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoardVM.UnselectAll();
                }
                ModuleBoardVM.ConfirmSelectBox();
                break;
        }

        ModuleBoardVM.UncaptureMouse();
        startInteractItem = null;
        startInteractItemType = MouseInteractionItemType.None;
        interactState = InteractionState.Idle;
    }

    public void ProcessMouseRightUp(object sender, MouseInteractionItemType senderType)
    {
        switch (interactState)
        {
            case InteractionState.StartRightHoldingSelected:
                // TODO: Open options dropdown for selected
                break;

            case InteractionState.StartRightHoldingModule:
                // TODO: Open options dropdown for module
                break;

            case InteractionState.StartRightHoldingBoard:
                // TODO: Open options dropdown for board
                // TEMPORARY
                // This is just a way to spawn in modules
                ModuleViewModel module = new();
                ModuleBoardVM.AddElement(module);
                break;

            case InteractionState.DraggingCanvas:
                // TODO
                break;
        }

        ModuleBoardVM.UncaptureMouse();
        startInteractItem = null;
        startInteractItemType = MouseInteractionItemType.None;
        interactState = InteractionState.Idle;
    }

    private enum InteractionState
    {
        Idle,
        StartLeftHoldingSelected,
        StartLeftHoldingModule,
        StartLeftHoldingBoard,
        StartRightHoldingSelected,
        StartRightHoldingModule,
        StartRightHoldingBoard,
        DraggingSelection,
        DraggingSelectBox,
        DraggingCanvas,
    }

    private static MouseInteractionManager? instance = null;
    private static readonly object instanceLock = new object();
    private static readonly double squaredMouseMoveThreshold = 10 * 10;

    private InteractionState interactState = InteractionState.Idle;
    private object? startInteractItem = null;
    private MouseInteractionItemType startInteractItemType = MouseInteractionItemType.None;
    private Point startInteractMousePos;

    private MouseInteractionManager() { }

    private bool MouseMovedPastThreshold()
    {
        Point mousePos = Mouse.GetPosition(null);
        Vector offset = mousePos - startInteractMousePos;
        return offset.LengthSquared > squaredMouseMoveThreshold;
    }
}
