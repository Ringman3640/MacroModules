using MacroModules.MacroLibrary.Types;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class StringPropertyEditor : BasePropertyEditor
{
    public string StringProperty
    {
        get { return (string)GetValue(StringPropertyProperty); }
        set { SetValue(StringPropertyProperty, value); }
    }
    public static readonly DependencyProperty StringPropertyProperty = DependencyProperty.Register(
        name: nameof(StringProperty),
        propertyType: typeof(string),
        ownerType: typeof(StringPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: "",
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string StringEntry
    {
        get { return StringProperty; }
        set
        {
            if (StringProperty != value)
            {
                StringProperty = value;
                OnPropertyChanged();
            }
        }
    }

    public override UIElement LabelTarget { get; }

    public StringPropertyEditor() : base()
    {
        InitializeComponent();
        LabelTarget = labelTarget;
    }
}
