using MacroModules.Model.Modules;
using MacroModules.Model.Values;

namespace MacroModules.App.ViewModels.Modules;

public abstract class ValuedModuleVM : ModuleVM
{
    public ValueDataType ReturnValueType
    {
        get { return ((ValuedModule)ModuleData).ReturnValueType; }
    }

    public ValuedModuleVM() : base() { }

    public ValuedModuleVM(Module moduleData) : base(moduleData) { }
}
