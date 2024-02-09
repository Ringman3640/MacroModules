namespace MacroModules.App.Behaviors;

public delegate void MousePositionRequestedHandler(object sender, EventArgs e);

public interface INotifyMousePositionRequested
{
    public MouseAwareBehavior MouseAwareBehaviorConnector { set; }

    public event MousePositionRequestedHandler? MousePositionRequested;
}
