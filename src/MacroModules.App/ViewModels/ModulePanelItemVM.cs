using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.ViewModels.Modules;
using System.Windows;
using System.Windows.Controls;

namespace MacroModules.App.ViewModels
{
    public partial class ModulePanelItemVM : ObservableObject
    {
        public ModuleVM Module { get; private set; }

        public ModulePanelItemVM(ModuleVM module)
        {
            Module = module;
        }

        [RelayCommand]
        private void ModulePanelItem_LeftMouseDown(RoutedEventArgs e)
        {
            DragDrop.DoDragDrop((UserControl)e.Source, Module.Type, DragDropEffects.Copy);
        }
    }
}
