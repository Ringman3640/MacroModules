namespace MacroModules.App.Behaviors;

public delegate void MousePositionRequestedHandler(object sender, EventArgs e);

public interface INotifyMousePositionRequested
{
    public event MousePositionRequestedHandler? MousePositionRequested;
}
