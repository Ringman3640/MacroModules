namespace MacroModules.App.Managers;

public abstract class Commit
{
    public Commit(ICommittable target)
    {
        this.target = target;
    }

    public void Undo()
    {
        target.PerformingCommitAction = true;
        UndoAction();
        target.PerformingCommitAction = false;
    }

    public void Redo()
    {
        target.PerformingCommitAction = true;
        RedoAction();
        target.PerformingCommitAction = false;
    }

    protected ICommittable target;

    protected abstract void UndoAction();

    protected abstract void RedoAction();
}
