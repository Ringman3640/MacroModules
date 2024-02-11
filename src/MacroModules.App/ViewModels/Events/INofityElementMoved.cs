namespace MacroModules.App.ViewModels.Events;

public interface INotifyElementMoved
{
    public event ElementMovedHandler? ElementMoved;
}

public delegate void ElementMovedHandler(object sender, EventArgs e);
