using MacroModules.App.ViewModels;

namespace MacroModules.App.Managers.Commits;

public class ElementRemovedCommit : Commit
{
    public ElementRemovedCommit(ICommittable target, BoardElementVM element) : base(target)
    {
        this.element = element;
    }

    protected override void UndoAction()
    {
        if (target is ModuleBoardVM moduleBoard)
        {
            moduleBoard.AddElement(element);
        }
    }

    protected override void RedoAction()
    {
        if (target is ModuleBoardVM moduleBoard)
        {
            moduleBoard.RemoveElement(element);
        }
    }

    private readonly BoardElementVM element;
}
