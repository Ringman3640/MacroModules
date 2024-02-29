using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class PercentPropertyEditor : BasePropertyEditor
{
    public double PercentProperty
    {
        get { return (double)GetValue(PercentPropertyProperty); }
        set { SetValue(PercentPropertyProperty, value); }
    }
    public static readonly DependencyProperty PercentPropertyProperty = DependencyProperty.Register(
        name: nameof(PercentProperty),
        propertyType: typeof(double),
        ownerType: typeof(PercentPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: default(double),
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public double SliderValue
    {
        get { return PercentProperty * 100; }
        set
        {
            if (PercentProperty != value)
            {
                PercentProperty = value / 100;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PercentDisplay));
            }
        }
    }

    public string PercentDisplay
    {
        get { return Math.Round(PercentProperty * 100).ToString() + "%"; }
    }

    public PercentPropertyEditor() : base()
    {
        InitializeComponent();
    }
}
