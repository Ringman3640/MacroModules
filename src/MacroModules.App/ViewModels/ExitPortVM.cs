using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;
using System.Windows;
using System.Windows.Input;

namespace MacroModules.App.ViewModels
{
    public partial class ExitPortVM : MouseAwareVM
    {
        public ExitPort ExitPortData { get; set; }

        public ModuleVM AttachedModule { get; private set; }

        public ModuleVM? DestinationModule
        {
            get { return _destinationModule; }
            set
            {
                if (!ReferenceEquals(_destinationModule, value))
                {
                    if (_destinationModule != null)
                    {
                        _destinationModule.ElementMoved -= Module_ElementMoved;
                    }
                    _destinationModule = value;
                    if (value != null)
                    {
                        ExitPortData.Destination = value.ModuleData;
                        value.ElementMoved += Module_ElementMoved;
                    }
                    OnPropertyChanged();
                    ResetWire();
                }
            }
        }
        private ModuleVM? _destinationModule;

        public WorkspaceVM? Workspace
        {
            get { return AttachedModule.Workspace; }
        }

        [ObservableProperty]
        private Point _portModulePosition;

        [ObservableProperty]
        private bool _wireHitTestVisible = false;

        [ObservableProperty]
        private Visibility _wireVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Point _wireEndPoint = new(10, 10);

        public string? Name
        {
            get { return ExitPortData.Name; }
        }

        public string? Description
        {
            get { return ExitPortData.Description; }
        }

        public Point PortBoardPosition
        {
            get { return AttachedModule.Position + (Vector)PortModulePosition; }
        }

        public ExitPortVM(ExitPort exitPort, ModuleVM attachedModule)
        {
            ExitPortData = exitPort;
            AttachedModule = attachedModule;
            AttachedModule.ElementMoved += Module_ElementMoved;
        }

        public void ResetWire()
        {
            if (DestinationModule == null)
            {
                WireVisibility = Visibility.Collapsed;
                return;
            }
            WireEndPoint = DestinationModule.CenterPosition - (Vector)PortBoardPosition;
            WireVisibility = Visibility.Visible;
        }

        public void PreviewWireToMouse()
        {
            WireEndPoint = MousePosition;
            WireVisibility = Visibility.Visible;
        }

        public void PreviewWireToModule(ModuleVM module)
        {
            WireEndPoint = module.CenterPosition - (Vector)PortBoardPosition;
            WireVisibility = Visibility.Visible;
        }

        private void Module_ElementMoved(object sender, EventArgs e)
        {
            ResetWire();
        }

        [RelayCommand]
        private void Port_LeftMouseDown(MouseButtonEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Port_MouseMove(MouseEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Port_LeftMouseUp(MouseButtonEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Wire_LeftMouseDown(MouseButtonEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Wire_MouseMove(MouseEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Wire_LeftMouseUp(MouseButtonEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }
    }
}
