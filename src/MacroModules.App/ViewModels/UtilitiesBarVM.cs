using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MacroModules.App.Messages;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Execution;
using MacroModules.Model.GolbalSystems;

namespace MacroModules.App.ViewModels;

public partial class UtilitiesBarVM : ObservableObject
{
    public WorkspaceVM Workspace { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartExecutionCommand))]
    [NotifyCanExecuteChangedFor(nameof(StopExecutionCommand))]
    [NotifyCanExecuteChangedFor(nameof(SetTerminateTriggerCommand))]
    private bool _executionRunning = false;

    public UtilitiesBarVM(WorkspaceVM workspace)
    {
        Workspace = workspace;

        WeakReferenceMessenger.Default.Register<ExecutionStateChangedMessage>(this, (recipient, message) =>
        {
            ExecutionRunning = message.Value;
        });
    }

    public InputTrigger TerminateTrigger { get; private set; } = new InputTrigger((ushort)InputCode.Escape, 0);

    [RelayCommand(CanExecute = nameof(CanStartExecution))]
    private void StartExecution()
    {
        Workspace.Executor.Startup();
    }
    private bool CanStartExecution()
    {
        return !ExecutionRunning;
    }

    [RelayCommand(CanExecute = nameof(CanStopExecution))]
    private void StopExecution()
    {
        Workspace.Executor.Terminate();
    }
    private bool CanStopExecution()
    {
        return ExecutionRunning;
    }

    [RelayCommand(CanExecute = nameof(CanSetTerminateTrigger))]
    private void SetTerminateTrigger()
    {
        TriggerInputObtainer.InputHandler = (InputTrigger trigger) => Workspace.Executor.TerminateTrigger = trigger;
        TriggerInputObtainer.Start();
    }
    private bool CanSetTerminateTrigger()
    {
        return !ExecutionRunning;
    }
}
