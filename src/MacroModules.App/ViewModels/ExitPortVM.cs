using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;

namespace MacroModules.App.ViewModels
{
    public partial class ExitPortVM : ObservableObject
    {
        public ExitPort ExitPortModel { get; set; }

        public ModuleVM AttachedModule { get; private set; }

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

        public ExitPortVM(ExitPort exitPort, int exitPortNumber, ModuleVM attachedModule)
        {
            ExitPortModel = exitPort;
            ExitPortNumber = exitPortNumber;
            AttachedModule = attachedModule;
        }
    }
}
