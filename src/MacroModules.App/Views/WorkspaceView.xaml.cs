using MacroModules.App.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class WorkspaceView : Window
{
    public WorkspaceView()
    {
        DataContext = new WorkspaceVM(this);
        InitializeComponent();
    }

    public Grid GetWorkspaceGrid()
    {
        return gdWorkspace;
    }
}
