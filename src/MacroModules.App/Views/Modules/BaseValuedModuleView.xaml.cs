using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MacroModules.App.Views.Modules;

public partial class BaseValuedModuleView : UserControl
{
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        name: nameof(CornerRadius),
        propertyType: typeof(CornerRadius),
        ownerType: typeof(BaseValuedModuleView),
        typeMetadata: new PropertyMetadata(default(CornerRadius)));

    public Brush BodyColor
    {
        get { return (Brush)GetValue(BodyColorProperty); }
        set { SetValue(BodyColorProperty, value); }
    }
    public static readonly DependencyProperty BodyColorProperty = DependencyProperty.Register(
        name: nameof(BodyColor),
        propertyType: typeof(Brush),
        ownerType: typeof(BaseValuedModuleView),
        typeMetadata: new PropertyMetadata(default(Brush)));

    public object BodyContent
    {
        get { return GetValue(BodyContentProperty); }
        set { SetValue(BodyContentProperty, value); }
    }
    public static readonly DependencyProperty BodyContentProperty = DependencyProperty.Register(
        name: nameof(BodyContent),
        propertyType: typeof(object),
        ownerType: typeof(BaseValuedModuleView),
        typeMetadata: new PropertyMetadata(default(object)));

    public BaseValuedModuleView()
    {
        InitializeComponent();
    }
}
