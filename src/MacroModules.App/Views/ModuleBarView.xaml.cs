using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace MacroModules.App.Views;

public partial class ModuleBarView : UserControl
{
    public ItemsControl ModuleBarItemsContainer { get; }

    public ModuleBarView()
    {
        InitializeComponent();
        ModuleBarItemsContainer = moduleBarItemsContainer;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ModuleBarAutomationPeer(this);
    }
}

public class ModuleBarAutomationPeer : UserControlAutomationPeer
{
    public ModuleBarAutomationPeer(UserControl owner) : base(owner) { }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Pane;
    }

    protected override string GetClassNameCore()
    {
        return nameof(ModuleBarView);
    }
}
