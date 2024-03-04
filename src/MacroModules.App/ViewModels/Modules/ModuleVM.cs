﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.Model.BoardElements;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Windows;

namespace MacroModules.App.ViewModels.Modules;

public abstract partial class ModuleVM : BoardElementVM
{
    public abstract ModuleType Type { get; }

    public override BoardElement ElementData { get; protected set; }

    public Module ModuleData { get; protected set; }

    public ObservableCollection<ExitPortVM> ExitPorts { get; private set; } = new();

    [ObservableProperty]
    private Point _entryPortModulePosition;

    public Point EntryPortBoardPosition
    {
        get { return VisualPosition + (Vector)EntryPortModulePosition; }
    }

    public bool IsConnectable
    {
        get { return ModuleData.IsConnectable; }
    }

    public ModuleVM()
    {
        ModuleData = ModuleFactory.Create(Type);
        ElementData = ModuleData;
    }

    public ModuleVM(Module moduleData)
    {
        if (moduleData.Type != Type)
        {
            throw new ArgumentException($"Cannot copy construct from Module of type {moduleData.Type} in ModuleVM of type {Type}");
        }
        ModuleData = moduleData;
        ElementData = ModuleData;
    }

    public override void Initialize(WorkspaceVM workspace)
    {
        base.Initialize(workspace);
        foreach (var exitPort in ModuleData.ExitPorts)
        {
            ExitPorts.Add(new ExitPortVM(exitPort, this));
        }
    }

    public void Initialize(WorkspaceVM workspace, Dictionary<Guid, ModuleVM> moduleIdMap)
    {
        base.Initialize(workspace);
        foreach (var exitPort in ModuleData.ExitPorts)
        {

            ExitPorts.Add(new ExitPortVM(exitPort, this, moduleIdMap));
        }
    }

    [RelayCommand]
    private void Body_LeftMouseDown(RoutedEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_LeftMouseUp(RoutedEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_MouseMove(RoutedEventArgs e)
    {
        Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Module);
        e.Handled = true;
    }

    [RelayCommand]
    private void Body_MouseEnter(RoutedEventArgs e)
    {
        Hovered = true;
    }

    [RelayCommand]
    private void Body_MouseLeave(RoutedEventArgs e)
    {
        Hovered = false;
    }
}
