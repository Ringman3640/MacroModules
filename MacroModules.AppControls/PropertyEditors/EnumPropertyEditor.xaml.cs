using MacroModules.Model.Types;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class EnumPropertyEditor : BasePropertyEditor
{
    public Type EnumType
    {
        get { return (Type)GetValue(EnumTypeProperty); }
        set { SetValue(EnumTypeProperty, value); }
    }
    public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register(
        name: nameof(EnumType),
        propertyType: typeof(Type),
        ownerType: typeof(EnumPropertyEditor),
        typeMetadata: new PropertyMetadata(default(Type)));

    public Enum EnumProperty
    {
        get { return (Enum)GetValue(EnumPropertyProperty); }
        set { SetValue(EnumPropertyProperty, value); }
    }
    public static readonly DependencyProperty EnumPropertyProperty = DependencyProperty.Register(
        name: nameof(EnumProperty),
        propertyType: typeof(Enum),
        ownerType: typeof(EnumPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: default(Enum),
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public Array? EnumArray
    {
        get { return (EnumType == null) ? null : EnumType.GetEnumValues(); }
    }

    public Enum SelectedEnum
    {
        get { return EnumProperty; }
        set
        {
            if (EnumProperty != value)
            {
                EnumProperty = value;
                OnPropertyChanged();
            }
        }
    }

    public EnumPropertyEditor() : base()
    {
        InitializeComponent();
    }
}
