using System.Reflection;

namespace MacroModules.App.Managers;

public class PropertyCommit : Commit
{
    public PropertyCommit(ICommittable target, string propertyName, object? initialValue, object? newValue) : base(target)
    {
        PropertyInfo? generatedPropInfo = target.GetType().GetProperty(propertyName);
        if (generatedPropInfo == null)
        {
            throw new ArgumentException("Could not obtain PropertyInfo from the given target and propertyName");
        }

        propInfo = generatedPropInfo;
        this.initialValue = initialValue;
        this.newValue = newValue;
    }

    protected override void UndoAction()
    {
        propInfo.SetValue(target, initialValue);
    }

    protected override void RedoAction()
    {
        propInfo.SetValue(target, newValue);
    }

    private readonly PropertyInfo propInfo;
    private readonly object? initialValue;
    private readonly object? newValue;
}
