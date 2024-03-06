using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class PropertiesPanelView : UserControl
{
    public PropertiesPanelView()
    {
        InitializeComponent();
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new PropertiesPanelAutomationPeer(this);
    }
}

public class PropertiesPanelAutomationPeer : UserControlAutomationPeer
{
    public PropertiesPanelAutomationPeer(UserControl owner) : base(owner) { }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Pane;
    }

    protected override string GetClassNameCore()
    {
        return nameof(PropertiesPanelView);
    }
}
