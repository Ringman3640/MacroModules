using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MacroModules.App.ViewModels.Modules;
using System.Windows;
using System.Windows.Controls;

namespace MacroModules.App.ViewModels
{
    public partial class ModuleBarItemVM : ObservableObject
    {
        public ModuleVM Module { get; private set; }

        public ModuleBarItemVM(ModuleVM module)
        {
            Module = module;
        }

        [RelayCommand]
        private void ModuleBarItem_LeftMouseDown(RoutedEventArgs e)
        {
            DragDrop.DoDragDrop((UserControl)e.Source, Module.Type, DragDropEffects.Copy);
        }
    }
}
