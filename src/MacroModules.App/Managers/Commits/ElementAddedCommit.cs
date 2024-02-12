using MacroModules.App.ViewModels;

namespace MacroModules.App.Managers.Commits;

public class ElementAddedCommit : Commit
{
    public ElementAddedCommit(ICommittable target, BoardElementVM element) : base(target)
    {
        this.element = element;
    }

    protected override void UndoAction()
    {
        if (target is ModuleBoardVM moduleBoard)
        {
            moduleBoard.RemoveElement(element);
        }
    }

    protected override void RedoAction()
    {
        if (target is ModuleBoardVM moduleBoard)
        {
            moduleBoard.AddElement(element);
        }
    }

    private readonly BoardElementVM element;
}
