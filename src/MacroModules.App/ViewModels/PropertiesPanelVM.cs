using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.ViewModels.Modules;
using System.Collections.Specialized;

namespace MacroModules.App.ViewModels;

public partial class PropertiesPanelVM : ObservableObject
{
    public WorkspaceVM Workspace { get; private set; }

    [ObservableProperty]
    private ModuleVM? _displayedModule = null;

    public PropertiesPanelVM(WorkspaceVM worksapce)
    {
        Workspace = worksapce;
        Workspace.ModuleBoard.SelectBox.SelectedElements.CollectionChanged += SelectedElements_CollectionChanged;
    }

    public void SelectedElements_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var selectedElements = Workspace.ModuleBoard.SelectBox.SelectedElements;
        if (selectedElements.Count == 1 && selectedElements[0] is ModuleVM selectedModule)
        {
            DisplayedModule = selectedModule;
        }
        else
        {
            DisplayedModule = null;
        }
    }
}
