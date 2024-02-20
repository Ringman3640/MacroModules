using System.Windows;
using System.Windows.Controls;

namespace MacroModules.App.Views.Modules;

public partial class ModuleBodyView : UserControl
{
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        name: nameof(CornerRadius),
        propertyType: typeof(CornerRadius),
        ownerType: typeof(ModuleBodyView),
        typeMetadata: new PropertyMetadata(default(CornerRadius)));

    public ModuleBodyView()
    {
        InitializeComponent();
    }
}
