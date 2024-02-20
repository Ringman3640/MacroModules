using MacroModules.App.Managers;
using MacroModules.App.Managers.Commits;
using MacroModules.Model.Values;
using MacroModules.Model.Variables;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MacroModules.App.ViewModels;

public class VariableVM : ICommittable, INotifyPropertyChanged
{
    public WorkspaceVM Workspace { get; private set; }

    public Variable VariableData { get; private set; }

    public Guid Id
    {
        get { return VariableData.Id; }
    }

    public ValueDataType Type
    {
        get { return VariableData.Type; }
    }

    public string Name
    {
        get { return VariableData.Name; }
        set
        {
            if (VariableData.Name != value)
            {
                FullCommitPropertyChange(VariableData.Name, value);
                VariableData.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public Value InitialValue
    {
        get { return VariableData.InitialValue; }
        set
        {
            if (VariableData.InitialValue != value)
            {
                FullCommitPropertyChange(VariableData.InitialValue, value);
                VariableData.InitialValue = value;
                OnPropertyChanged();
            }
        }
    }

    public bool PerformingCommitAction { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public VariableVM(ValueDataType type, WorkspaceVM workspace)
    {
        VariableData = new(type);
        Workspace = workspace;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void FullCommitPropertyChange(object? originalValue, object newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!PerformingCommitAction)
        {
            Workspace.CommitManager.PushToSeries(new PropertyCommit(this, propertyName!, originalValue, newValue));
            Workspace.CommitManager.CommitSeries();
        }
    }
}
