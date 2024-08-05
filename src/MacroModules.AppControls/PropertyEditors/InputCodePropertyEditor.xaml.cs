using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class InputCodePropertyEditor : BasePropertyEditor
{
    public ushort InputCodeProperty
    {
        get { return (ushort)GetValue(InputCodePropertyProperty); }
        set { SetValue(InputCodePropertyProperty, value); }
    }
    public static readonly DependencyProperty InputCodePropertyProperty = DependencyProperty.Register(
        name: nameof(InputCodeProperty),
        propertyType: typeof(ushort),
        ownerType: typeof(InputCodePropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: default(ushort),
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string InputCodeDisplay
    {
        get
        {
            if (InputCodeProperty == 0)
            {
                return "No Input";
            }
            if (Enum.IsDefined(typeof(InputCode), InputCodeProperty))
            {
                return ((InputCode)InputCodeProperty).ToString();
            }
            return "Vk_" + InputCodeProperty;
        }
    }

    public override UIElement LabelTarget { get; }

    public InputCodePropertyEditor() : base()
    {
        InitializeComponent();
        LabelTarget = labelTarget;
    }

    private void SetInput_Click(object sender, RoutedEventArgs e)
    {
        StartInputMonitor();
    }

    private void StartInputMonitor()
    {
        InputMonitor.FilterInjectedInputs = true;
        InputMonitor.FilterMouseMovements = true;
        InputMonitor.SetInputHandler(InputMonitor_HandleInput);
        InputMonitor.Install();
        InputMonitor.CollectInput = true;
    }

    private void StopInputMonitor()
    {
        InputMonitor.CollectInput = false;
        InputMonitor.Uninstall();
    }

    private bool InputMonitor_HandleInput(InputData input)
    {
        if (input.Type != InputType.KeyDown && input.Type != InputType.MouseDown)
        {
            return true;
        }
        InputCodeProperty = input.InputKeyCode;
        StopInputMonitor();
        OnPropertyChanged(nameof(InputCodeDisplay));
        return false;
    }
}
