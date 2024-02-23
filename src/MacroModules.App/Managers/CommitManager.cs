using MacroModules.App.Managers.Commits;
using MacroModules.App.Types;

namespace MacroModules.App.Managers;

public class CommitManager
{
    public int MaxHistoryCount { get; set; } = 50;

    public CommitManager()
    {
        undoStack = new(MaxHistoryCount);
        redoStack = new();
        commitSeries = [];
    }

    public void Undo()
    {
        CommitSeries();
        if (undoStack.Count == 0)
        {
            return;
        }

        List<Commit> seriesToUndo = undoStack.Pop()!;
        foreach (var commit in seriesToUndo)
        {
            commit.Undo();
        }
        redoStack.Push(seriesToUndo);
    }

    public void Redo()
    {
        CommitSeries();
        if (redoStack.Count == 0)
        {
            return;
        }

        List<Commit> seriesToRedo = redoStack.Pop();
        foreach (var commit in seriesToRedo)
        {
            commit.Redo();
        }
        undoStack.Push(seriesToRedo);
    }

    public void ClearCommits()
    {
        undoStack.Clear();
        redoStack.Clear();
        commitSeries.Clear();
    }

    public void PushToSeries(Commit commitItem)
    {
        commitSeries.Add(commitItem);
    }

    public void CommitSeries()
    {
        if (commitSeries.Count == 0)
        {
            return;
        }

        undoStack.Push(commitSeries);
        commitSeries = [];
        redoStack.Clear();
    }

    private readonly DropoutStack<List<Commit>> undoStack;
    private readonly Stack<List<Commit>> redoStack;
    private List<Commit> commitSeries;
}
