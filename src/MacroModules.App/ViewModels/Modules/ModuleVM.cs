using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.Model.Modules;
using System.Collections.ObjectModel;
using System.Windows;

namespace MacroModules.App.ViewModels.Modules;

public abstract partial class ModuleVM : BoardElementVM
{
    public abstract ModuleType Type { get; }

    public Module ModuleData
    {
        get { return _moduleData!; }
        protected set
        {
            _moduleData = value;
            ElementData = value;
        }
    }
    private Module? _moduleData;

    public ObservableCollection<ExitPortVM> ExitPorts { get; private set; } = new();

    public bool IsConnectable
    {
        get { return ModuleData.IsConnectable; }
    }

    public ModuleVM()
    {
        ModuleData = ModuleFactory.Create(Type);
    }

    public ModuleVM(Module moduleData)
    {
        if (moduleData.Type != Type)
        {
            throw new ArgumentException($"Cannot copy construct from Module of type {moduleData.Type} in ModuleVM of type {Type}");
        }
        ModuleData = moduleData;
    }

    public override void Initialize(WorkspaceVM workspace)
    {
        base.Initialize(workspace);
        foreach (var exitPort in ModuleData.ExitPorts)
        {
            ExitPorts.Add(new ExitPortVM(exitPort, this));
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
}
