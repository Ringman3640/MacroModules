using MacroModules.AppControls.Buttons;
using MacroModules.MacroLibrary;
using MacroModules.Model.Execution;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MacroModules.AppControls.Graphics;

public partial class InputTriggerGraphic : UserControl, INotifyPropertyChanged
{
    public InputTrigger? InputTrigger
    {
        get { return (InputTrigger?)GetValue(InputTriggerProperty); }
        set { SetValue(InputTriggerProperty, value); }
    }
    public static readonly DependencyProperty InputTriggerProperty = DependencyProperty.Register(
        name: nameof(InputTrigger),
        propertyType: typeof(InputTrigger),
        ownerType: typeof(InputTriggerGraphic),
        typeMetadata: new PropertyMetadata(null));

    public bool IsKeyInput
    {
        get { return !InputTrigger?.IsMouseInput() ?? true; }
    }

    public string InputText
    {
        get { return (InputTrigger != null) ? InputCodeAbbreviation.GetAbbreviation(InputTrigger.InputKeyCode) : ""; }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public InputTriggerGraphic()
    {
        InitializeComponent();
    }

    private void InputTriggerGraphic_Loaded(object sender, RoutedEventArgs e)
    {
        OnPropertyChanged(nameof(IsKeyInput));
        OnPropertyChanged(nameof(InputText));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
