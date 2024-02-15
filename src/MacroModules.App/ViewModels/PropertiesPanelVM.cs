using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.ViewModels.Modules;
using System.Collections.Specialized;

namespace MacroModules.App.ViewModels;

public partial class PropertiesPanelVM : ObservableObject
{
    public WorkspaceVM Workspace { get; private set; }

    public ModuleVM? DisplayedModule
    {
        get { return _displayedModule; }
        set
        {
            ModuleVM? prevModule = _displayedModule;
            if (SetProperty(ref _displayedModule, value))
            {
                if (prevModule != null)
                {
                    prevModule.ElementRemoved -= DisplayedModule_ElementRemoved;
                }
                if (value != null)
                {
                    value.ElementRemoved += DisplayedModule_ElementRemoved;
                }
            }
        }
    }
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

    private void DisplayedModule_ElementRemoved(object? sender, EventArgs e)
    {
        DisplayedModule = null;
    }
}
