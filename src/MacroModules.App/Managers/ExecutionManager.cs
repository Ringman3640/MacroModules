using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MacroModules.App.Messages;
using MacroModules.App.ViewModels;
using MacroModules.App.ViewModels.Modules;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Execution;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.App.Managers;

public class ExecutionManager : ObservableObject
{
    public WorkspaceVM Workspace { get; private set; }

    public bool Running { get { return macroDispatcher.Running; } }

    public InputTrigger TerminateTrigger { get; set; } = new InputTrigger((ushort)InputCode.Escape);

    public ExecutionManager(WorkspaceVM workspace)
    {
        Workspace = workspace;
        macroDispatcher.RunningStateChanged += MacroDispatcher_RunningStateChanged;
    }

    public void Startup()
    {
        if (Running)
        {
            return;
        }

        macroDispatcher.ClearMacros();

        StartupEntryModuleVM? startupEntryModule = Workspace.ModuleBoard.StartupEntryModule;
        if (startupEntryModule != null)
        {
            macroDispatcher.SetStartupMacro((StartupEntryModule)startupEntryModule.ModuleData);
        }

        foreach (var triggerModule in Workspace.ModuleBoard.TriggerModulesList)
        {
            if (triggerModule.Trigger != null)
            {
                macroDispatcher.AddMacro((TriggerEntryModule)triggerModule.ModuleData);
            }
        }

        macroDispatcher.Startup();
        OnPropertyChanged(nameof(Running));
    }

    public void Terminate()
    {
        if (!Running)
        {
            return;
        }

        macroDispatcher.Terminate();
        OnPropertyChanged(nameof(Running));
    }

    private readonly MacroDispatcher macroDispatcher = new();

    private void MacroDispatcher_RunningStateChanged(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new ExecutionStateChangedMessage(Running));
    }
}
