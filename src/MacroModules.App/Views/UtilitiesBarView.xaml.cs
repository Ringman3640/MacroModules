using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class UtilitiesBarView : UserControl
{
    public UtilitiesBarView()
    {
        InitializeComponent();
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new UtilitiesBarAutomationPeer(this);
    }
}

public class UtilitiesBarAutomationPeer : UserControlAutomationPeer
{
    public UtilitiesBarAutomationPeer(UserControl owner) : base(owner) { }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Pane;
    }

    protected override string GetClassNameCore()
    {
        return nameof(UtilitiesBarView);
    }
}
