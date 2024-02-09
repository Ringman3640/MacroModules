using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace MacroModules.App.Behaviors;

public class DimensionsAwareBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty BoundDimensionsProperty = DependencyProperty.Register(
        name: "BoundDimensions",
        propertyType: typeof(Size),
        ownerType: typeof(DimensionsAwareBehavior),
        typeMetadata: new PropertyMetadata(default(Size)));

    public Size BoundDimensions
    {
        get { return (Size)GetValue(BoundDimensionsProperty); }
        set { SetValue(BoundDimensionsProperty, value); }
    }

    protected override void OnAttached()
    {
        AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
    }

    private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        BoundDimensions = e.NewSize;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
    }
}
