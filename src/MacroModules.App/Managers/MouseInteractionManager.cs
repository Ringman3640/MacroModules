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
    public WorkspaceVM Workspace {  get; private set; }

    public ModuleBoardVM ModuleBoard
    {
        get { return Workspace.ModuleBoard; }
    }

    public MouseInteractionManager(WorkspaceVM workspace)
    {
        Workspace = workspace;
    }

    public void ProcessMouseLeftDown(object sender, MouseInteractionItemType senderType)
    {
        if (interactState != InteractionState.Idle)
        {
            return;
        }

        Workspace.CaptureMouse();
        startInteractItem = sender;
        startInteractItemType = senderType;
        startInteractMousePos = Mouse.GetPosition(null);

        switch (senderType)
        {
            case MouseInteractionItemType.Module:
                var module = (ModuleVM)sender;
                if (ModuleBoard.SelectBox.IsSelected(module))
                {
                    interactState = InteractionState.StartLeftHoldingSelected;
                }
                else
                {
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ModuleBoard.SelectBox.UnselectAll();
                    }
                    interactState = InteractionState.StartLeftHoldingModule;
                }
                break;

            case MouseInteractionItemType.Wire:
                interactState = InteractionState.DraggingWire;
                break;

            case MouseInteractionItemType.Board:
                ModuleBoard.SelectBox.LockSelectBoxPivotToMouse();
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

        Workspace.CaptureMouse();
        startInteractItem = sender;
        startInteractItemType = senderType;
        startInteractMousePos = Mouse.GetPosition(null);

        switch (senderType)
        {
            case MouseInteractionItemType.Module:
                if (sender is ModuleVM module)
                {
                    if (ModuleBoard.SelectBox.IsSelected(module))
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
                ModuleBoard.SelectBox.LockSelectedToMouse();
                interactState = InteractionState.DraggingSelection;
                goto case InteractionState.DraggingSelection;

            case InteractionState.StartLeftHoldingModule:
            {
                if (startInteractItem is ModuleVM module)
                {
                    ModuleBoard.SelectBox.Select(module);
                    ModuleBoard.SelectBox.LockSelectedToMouse();
                    interactState = InteractionState.DraggingSelection;
                }
                else
                {
                    interactState = InteractionState.Idle;
                    break;
                }
                goto case InteractionState.DraggingSelection;
            }
                

            case InteractionState.StartLeftHoldingBoard:
                if (MouseMovedPastThreshold())
                {
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ModuleBoard.SelectBox.UnselectAll();
                    }
                    ModuleBoard.SelectBox.StartSelectBox();
                    interactState = InteractionState.DraggingSelectBox;
                    goto case InteractionState.DraggingSelectBox;
                }
                break;

            case InteractionState.StartRightHoldingSelected:
                goto case InteractionState.StartRightHoldingBoard;

            case InteractionState.StartRightHoldingModule:
                goto case InteractionState.StartRightHoldingBoard;

            case InteractionState.StartRightHoldingBoard:
                ModuleBoard.LockCanvasToMouse();
                interactState = InteractionState.DraggingCanvas;
                break;

            case InteractionState.DraggingSelection:
                ModuleBoard.SelectBox.MoveSelectedWithMouse();
                break;

            case InteractionState.DraggingSelectBox:
                ModuleBoard.SelectBox.MoveSelectBoxWithMouse();
                break;

            case InteractionState.DraggingCanvas:
                ModuleBoard.MoveCanvasWithMouse();
                break;

            case InteractionState.DraggingWire:
            {
                if (startInteractItem is not ExitPortVM exitPort)
                {
                    break;
                }
                if (sender is ModuleVM module
                    && !ReferenceEquals(module, exitPort.AttachedModule))
                {
                    exitPort.PreviewWireToModule(module);
                }
                else
                {
                    exitPort.PreviewWireToMouse();
                }
                break;
            }
        }
    }

    public void ProcessMouseLeftUp(object sender, MouseInteractionItemType senderType)
    {
        switch (interactState)
        {
            case InteractionState.StartLeftHoldingSelected:
            {
                if (startInteractItem is not ModuleVM module)
                {
                    break;
                }
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoard.SelectBox.Unselect(module);
                }
                else
                {
                    ModuleBoard.SelectBox.UnselectAll();
                    ModuleBoard.SelectBox.Select(module);
                    // TODO: show single module on properties panel
                }
                break;
            }
                
            case InteractionState.StartLeftHoldingModule:
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoard.SelectBox.UnselectAll();
                }
                if (startInteractItem is ModuleVM module)
                {
                    ModuleBoard.SelectBox.Select(module);
                    // TODO: show module on properties if its the only one focused
                }
                break;
            }

            case InteractionState.StartLeftHoldingBoard:
                ModuleBoard.SelectBox.UnselectAll();
                break;

            case InteractionState.DraggingSelection:
                // TODO: Commit visual drag to model (which is also TODO)
                break;

            case InteractionState.DraggingSelectBox:
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ModuleBoard.SelectBox.UnselectAll();
                }
                ModuleBoard.SelectBox.ConfirmSelectBox();
                break;

            case InteractionState.DraggingWire:
                {
                    if (startInteractItem is not ExitPortVM exitPort)
                    {
                        break;
                    }
                    if (sender is ModuleVM module
                        && !ReferenceEquals(module, exitPort.AttachedModule))
                    {
                        exitPort.DestinationModule = module;
                    }
                    else
                    {
                        exitPort.DestinationModule = null;
                    }
                    exitPort.ResetWire();
                    break;
                }
        }

        Workspace.UncaptureMouse();
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
                ModuleVM module = new();
                ModuleBoard.AddElement(module);
                module.CenterToPoint(ModuleBoard.BoardMousePosition);
                break;

            case InteractionState.DraggingCanvas:
                // TODO
                break;
        }

        Workspace.UncaptureMouse();
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
        DraggingWire,
    }

    private static readonly double squaredMouseMoveThreshold = 10 * 10;

    private InteractionState interactState = InteractionState.Idle;
    private object? startInteractItem = null;
    private MouseInteractionItemType startInteractItemType = MouseInteractionItemType.None;
    private Point startInteractMousePos;

    private bool MouseMovedPastThreshold()
    {
        Point mousePos = Mouse.GetPosition(null);
        Vector offset = mousePos - startInteractMousePos;
        return offset.LengthSquared > squaredMouseMoveThreshold;
    }
}
