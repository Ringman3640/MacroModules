using MacroModules.App.ViewModels;

namespace MacroModules.App.Managers.Commits;

public class ElementGroupAddedCommit : Commit
{
    public ElementGroupAddedCommit(ICommittable target, List<BoardElementVM> elements) : base(target)
    {
        this.elements = elements;
    }

    protected override void RedoAction()
    {
        if (target is ModuleBoardVM moduleBoard)
        {
            moduleBoard.AddElements(elements);
        }
    }

    protected override void UndoAction()
    {
        if (target is ModuleBoardVM moduleBoard)
        {
            foreach (var element in elements)
            {
                moduleBoard.RemoveElement(element);
            }
        }
    }

    private readonly List<BoardElementVM> elements;
}
