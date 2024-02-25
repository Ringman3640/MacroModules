using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MacroModules.App.Messages;

public class ExecutionStateChangedMessage : ValueChangedMessage<bool>
{
    public ExecutionStateChangedMessage(bool value) : base(value) { }
}
