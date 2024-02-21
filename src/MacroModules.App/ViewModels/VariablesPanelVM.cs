using MacroModules.Model.Values;
using System.Collections.ObjectModel;

namespace MacroModules.App.ViewModels;

public class VariablesPanelVM
{
    public WorkspaceVM Workspace { get; private set; }

    public ObservableCollection<VariableVM> Variables { get; } = [];

    public VariablesPanelVM(WorkspaceVM workspace)
    {
        Workspace = workspace;

        // For testing only
        CreateVariable(ValueDataType.Bool).Name = "Variable 1";
        CreateVariable(ValueDataType.Color).Name = "Variable 2";
        CreateVariable(ValueDataType.Invalid).Name = "Variable 3";
    }

    public VariableVM CreateVariable(ValueDataType type)
    {
        VariableVM variable = new(type, Workspace);
        Variables.Add(variable);
        variableMap.Add(variable.Id, variable);
        return variable;
    }

    public VariableVM? GetVariable(Guid variableId)
    {
        variableMap.TryGetValue(variableId, out VariableVM? variable);
        return variable;
    }

    public bool RemoveVariable(Guid variableId)
    {
        if (!variableMap.TryGetValue(variableId, out VariableVM? variable))
        {
            return false;
        }

        Variables.Remove(variable);
        variableMap.Remove(variableId);
        variable.VariableData.IndicateDeleted();
        return true;
    }

    private readonly Dictionary<Guid, VariableVM> variableMap = [];
}
