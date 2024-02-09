using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;

namespace MacroModules.App.ViewModels
{
    public partial class ExitPortViewModel : ObservableObject
    {
        public ExitPort ExitPortModel { get; set; }

        public ModuleViewModel AttachedModule { get; private set; }

        public int ExitPortNumber { get; private set; }

        public Module? Destination
        {
            get { return ExitPortModel.Destination; }
            set
            {
                if (ReferenceEquals(ExitPortModel.Destination, value))
                {
                    return;
                }
                ExitPortModel.Destination = value;
                OnPropertyChanged();
            }
        }

        public string? Name
        {
            get { return ExitPortModel.Name; }
        }

        public string? Description
        {
            get { return ExitPortModel.Description; }
        }

        public ExitPortViewModel(ExitPort exitPort, int exitPortNumber, ModuleViewModel attachedModule)
        {
            ExitPortModel = exitPort;
            ExitPortNumber = exitPortNumber;
            AttachedModule = attachedModule;
        }
    }
}
