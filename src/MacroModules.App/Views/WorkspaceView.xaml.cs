using MacroModules.App.ViewModels;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class WorkspaceView : UserControl
{
    public WorkspaceView()
    {
        DataContext = new WorkspaceViewModel();
        InitializeComponent();
    }
}
