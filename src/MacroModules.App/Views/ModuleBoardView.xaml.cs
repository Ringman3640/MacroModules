using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class ModuleBoardView : UserControl
{
    public ModuleBoardView()
    {
        InitializeComponent();
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ModuleBoardAutomationPeer(this);
    }
}

public class ModuleBoardAutomationPeer : UserControlAutomationPeer
{
    public ModuleBoardAutomationPeer(UserControl owner) : base(owner) { }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Pane;
    }

    protected override string GetClassNameCore()
    {
        return nameof(ModuleBarView);
    }
}
