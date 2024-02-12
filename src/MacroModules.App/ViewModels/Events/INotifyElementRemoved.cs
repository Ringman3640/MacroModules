namespace MacroModules.App.ViewModels.Events;

public interface INotifyElementRemoved
{
    public event ElementRemovedHandler? ElementRemoved;
}

public delegate void ElementRemovedHandler(object sender, EventArgs e);
