using MacroModules.Model.Types;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class DurationPropertyEditor : BasePropertyEditor
{
    public TimeDuration DurationProperty
    {
        get { return (TimeDuration)GetValue(DurationPropertyProperty); }
        set { SetValue(DurationPropertyProperty, value); }
    }
    public static readonly DependencyProperty DurationPropertyProperty = DependencyProperty.Register(
        name: nameof(DurationProperty),
        propertyType: typeof(TimeDuration),
        ownerType: typeof(DurationPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: default(TimeDuration),
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public DurationGranularity SelectedGranularity
    {
        get { return DurationProperty.Granularity; }
        set
        {
            DurationProperty = new(DurationProperty.TimeUnits, value);
            OnPropertyChanged();
        }
    }

    public string TimeUnitsEntryText
    {
        get { return DurationProperty.TimeUnits.ToString(); }
        set
        {
            TimeDuration durationValue;
            try
            {
                double timeUnits = GetDoubleFromText(value);
                durationValue = new(timeUnits, SelectedGranularity);
            }
            catch
            {
                return;
            }
            if (DurationProperty != durationValue)
            {
                DurationProperty = durationValue;
                OnPropertyChanged();
            }
        }
    }

    public override UIElement LabelTarget { get; }

    public DurationPropertyEditor() : base()
    {
        InitializeComponent();
        LabelTarget = labelTarget;
    }
}
