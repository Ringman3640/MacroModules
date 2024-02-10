using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;
using System.Windows;

namespace MacroModules.App.ViewModels
{
    public partial class ExitPortVM : ObservableObject
    {
        public ExitPort ExitPortModel { get; set; }

        public ModuleVM AttachedModule { get; private set; }

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

        [ObservableProperty]
        private Point _wireEndPoint = new(10, 10);

        public string? Name
        {
            get { return ExitPortModel.Name; }
        }

        public string? Description
        {
            get { return ExitPortModel.Description; }
        }

        public ExitPortVM(ExitPort exitPort, ModuleVM attachedModule)
        {
            ExitPortModel = exitPort;
            AttachedModule = attachedModule;
        }
    }
}
