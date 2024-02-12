using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.Managers;
using MacroModules.App.ViewModels.Modules;
using MacroModules.Model.Modules;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MacroModules.App.ViewModels
{
    public partial class ExitPortVM : MouseAwareVM, ICommittable
    {
        public ExitPort ExitPortData { get; set; }

        public ModuleVM AttachedModule { get; private set; }

        public ModuleVM? DestinationModule
        {
            get { return _destinationModule; }
            set
            {
                ModuleVM? prevModule = _destinationModule;
                if (SetAndCommitProperty(ref _destinationModule, value))
                {
                    if (prevModule != null)
                    {
                        prevModule.ElementMoved -= Module_ElementMoved;
                    }
                    if (value != null)
                    {
                        ExitPortData.Destination = value.ModuleData;
                        value.ElementMoved += Module_ElementMoved;
                    }
                    else
                    {
                        ExitPortData.Destination = null;
                    }
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
            get { return AttachedModule.VisualPosition + (Vector)PortModulePosition; }
        }

        public bool PerformingCommitAction { get; set; }

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
            WireEndPoint = DestinationModule.CenterVisualPosition - (Vector)PortBoardPosition;
            WireVisibility = Visibility.Visible;
        }

        public void PreviewWireToMouse()
        {
            WireEndPoint = MousePosition;
            WireVisibility = Visibility.Visible;
        }

        public void PreviewWireToModule(ModuleVM module)
        {
            WireEndPoint = module.CenterVisualPosition - (Vector)PortBoardPosition;
            WireVisibility = Visibility.Visible;
        }

        protected bool SetAndCommitProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            if (!PerformingCommitAction)
            {
                Workspace?.CommitManager.PushToSeries(new PropertyCommit(this, propertyName!, field, newValue));
            }

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        private void Module_ElementMoved(object sender, EventArgs e)
        {
            ResetWire();
        }

        [RelayCommand]
        private void Port_LeftMouseDown(RoutedEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Port_MouseMove(RoutedEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Port_LeftMouseUp(RoutedEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Wire_LeftMouseDown(RoutedEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftDown(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Wire_MouseMove(RoutedEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseMove(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }

        [RelayCommand]
        private void Wire_LeftMouseUp(RoutedEventArgs e)
        {
            Workspace?.MouseInteraction.ProcessMouseLeftUp(this, MouseInteractionItemType.Wire);
            e.Handled = true;
        }
    }
}
