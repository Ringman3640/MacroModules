using CommunityToolkit.Mvvm.Messaging;
using MacroModules.App.Managers.Commits;
using MacroModules.App.Messages;
using MacroModules.App.Types;

namespace MacroModules.App.Managers;

public class CommitManager
{
    public int MaxHistoryCount { get; set; } = 50;

    public bool PerformingUndoRedo { get; private set; } = false;

    public CommitManager()
    {
        undoStack = new(MaxHistoryCount);
        redoStack = new();
        commitSeries = [];
    }

    public void Undo()
    {
        WeakReferenceMessenger.Default.Send(new PerformingUndoRedoMessage());

        CommitSeries();
        if (undoStack.Count == 0)
        {
            return;
        }

        List<Commit> seriesToUndo = undoStack.Pop()!;
        PerformingUndoRedo = true;
        foreach (var commit in seriesToUndo)
        {
            commit.Undo();
        }
        PerformingUndoRedo = false;
        redoStack.Push(seriesToUndo);

        WeakReferenceMessenger.Default.Send(new FinishedUndoRedoMessage());
    }

    public void Redo()
    {
        WeakReferenceMessenger.Default.Send(new PerformingUndoRedoMessage());

        CommitSeries();
        if (redoStack.Count == 0)
        {
            return;
        }

        List<Commit> seriesToRedo = redoStack.Pop();
        PerformingUndoRedo = true;
        foreach (var commit in seriesToRedo)
        {
            commit.Redo();
        }
        PerformingUndoRedo = false;
        undoStack.Push(seriesToRedo);

        WeakReferenceMessenger.Default.Send(new FinishedUndoRedoMessage());
    }

    public void ClearCommits()
    {
        undoStack.Clear();
        redoStack.Clear();
        commitSeries.Clear();
    }

    public void PushToSeries(Commit commitItem)
    {
        if (!PerformingUndoRedo)
        {
            commitSeries.Add(commitItem);
        }
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
