using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MacroModules.App.Behaviors;

public class CanvasAwareBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty BoundCanvasPositionProperty = DependencyProperty.Register(
        name: "BoundPosition",
        propertyType: typeof(Point),
        ownerType: typeof(CanvasAwareBehavior),
        typeMetadata: new PropertyMetadata(default(Point)));

    public Point BoundPosition
    {
        get { return (Point)GetValue(BoundCanvasPositionProperty); }
        set { SetValue(BoundCanvasPositionProperty, value); }
    }

    protected override void OnAttached()
    {
        AssociatedObject.Loaded += AssociatedObject_Loaded;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        double left = Canvas.GetLeft(AssociatedObject);
        double top = Canvas.GetTop(AssociatedObject);
        if (double.IsNaN(left))
        {
            left = 0;
        }
        if (double.IsNaN(top))
        {
            top = 0;
        }
        BoundPosition = new(left, top);
    }
}
