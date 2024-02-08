using MacroModules.App.ViewModels;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class ModuleBoardView : UserControl
{
    public ModuleBoardView()
    {
        DataContext = new ModuleBoardViewModel(this);
        InitializeComponent();
    }

    public Canvas GetBoardCanvas()
    {
        return cvModuleBoard;
    }

    public Canvas GetContainerCanvas()
    {
        return cvBoardContainer;
    }
}
