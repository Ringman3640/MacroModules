using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Execution;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: null,
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            new PropertyChangedCallback(InputTriggerProperty_PropertyChanged)));

    public Brush Color
    {
        get { return (Brush)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
        name: nameof(Color),
        propertyType: typeof(Brush),
        ownerType: typeof(InputTriggerGraphic),
        typeMetadata: new PropertyMetadata(new SolidColorBrush(Colors.Black)));

    public bool IsKeyInput
    {
        get { return !InputTrigger?.IsMouseInput() ?? true; }
    }

    public string InputText
    {
        get
        {
            if (InputTrigger == null)
            {
                return "";
            }
            if (InputTrigger.IsMouseInput())
            {
                switch ((InputCode)InputTrigger.InputKeyCode)
                {
                    case InputCode.MouseLeft:
                        return "L";
                    case InputCode.MouseRight:
                        return "R";
                    case InputCode.MouseMiddle:
                        return "M";
                    case InputCode.MouseX1:
                        return "X1";
                    case InputCode.MouseX2:
                        return "X2";
                }
            }
            return InputCodeAbbreviation.GetAbbreviation(InputTrigger.InputKeyCode).ToUpper();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public InputTriggerGraphic()
    {
        InitializeComponent();
    }

    private void InputTriggerGraphic_Loaded(object sender, RoutedEventArgs e)
    {
        NotifyPropertyBindings();
    }

    private static void InputTriggerProperty_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (InputTriggerGraphic)d;
        self.NotifyPropertyBindings();
    }

    private void NotifyPropertyBindings()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsKeyInput)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputText)));
    }
}
