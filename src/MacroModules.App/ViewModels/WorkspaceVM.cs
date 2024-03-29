﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MacroModules.App.Managers;
using MacroModules.App.Messages;
using MacroModules.App.Views;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.ViewModels;

public partial class WorkspaceVM : ObservableObject
{
    public WorkspaceView ViewReference { get; private set; }

    public MouseInteractionManager MouseInteraction { get; private set; }

    public CommitManager CommitManager { get; private set; }

    public ModuleBoardVM ModuleBoard { get; private set; }

    public PropertiesPanelVM PropertiesPanel { get; private set; }

    public ModuleBarVM ModuleBar { get; private set; }

    public UtilitiesBarVM UtilitiesBar { get; private set; }

    public ProjectManager Project { get; private set; }

    public ExecutionManager Executor { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(Workspace_File_NewCommand))]
    [NotifyCanExecuteChangedFor(nameof(Workspace_File_OpenCommand))]
    [NotifyCanExecuteChangedFor(nameof(Workspace_File_SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(Workspace_File_SaveAsCommand))]
    private bool _executionRunning = false;

    public WorkspaceVM(WorkspaceView viewRef)
    {
        ViewReference = viewRef;
        MouseInteraction = new(this);
        CommitManager = new();
        ModuleBoard = new(this);
        PropertiesPanel = new(this);
        ModuleBar = new(this);
        UtilitiesBar = new(this);
        Project = new(this);
        Executor = new(this);

        WeakReferenceMessenger.Default.Register<ExecutionStateChangedMessage>(this, (recipient, message) =>
        {
            ExecutionRunning = message.Value;
        });
    }

    public void CaptureMouse()
    {
        Mouse.Capture(null);
        Mouse.Capture(ViewReference, CaptureMode.SubTree);
    }

    public void UncaptureMouse()
    {
        Mouse.Capture(null);
    }

    [RelayCommand]
    private void Workspace_RightMouseUp(RoutedEventArgs e)
    {
        MouseInteraction.ProcessMouseRightUp(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_LeftMouseUp(RoutedEventArgs e)
    {
        MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_MouseMove(RoutedEventArgs e)
    {
        MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.None);
        e.Handled = true;
    }

    [RelayCommand]
    private void Workspace_Undo()
    {
        CommitManager.Undo();
    }

    [RelayCommand]
    private void Workspace_Redo()
    {
        CommitManager.Redo();
    }

    [RelayCommand(CanExecute = nameof(Workspace_CanUseFileCommands))]
    private void Workspace_File_New()
    {
        Project.OpenNew();
    }

    [RelayCommand(CanExecute = nameof(Workspace_CanUseFileCommands))]
    private void Workspace_File_Open()
    {
        Project.PerformOpenExisting();
    }

    [RelayCommand(CanExecute = nameof(Workspace_CanUseFileCommands))]
    private void Workspace_File_Save()
    {
        Project.Save();
    }

    [RelayCommand(CanExecute = nameof(Workspace_CanUseFileCommands))]
    private void Workspace_File_SaveAs()
    {
        Project.PerformSaveAs();
    }

    private bool Workspace_CanUseFileCommands()
    {
        return !ExecutionRunning;
    }
}
